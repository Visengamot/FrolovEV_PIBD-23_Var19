using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using W = DocumentFormat.OpenXml.Wordprocessing;

namespace ShoeStore.WinForms.Reports.Builders;

public sealed class WordBuilder
{
    private readonly List<OpenXmlElement> _elements = new();

    public void AddHeading(string text)
    {
        _elements.Add(CreateParagraph(text, bold: true, heading: true));
    }

    public void AddText(string text)
    {
        _elements.Add(CreateParagraph(text));
    }

    public void AddTable(IReadOnlyList<IReadOnlyList<string>> rows, IReadOnlyList<int> widths)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(widths);
        if (widths.Count == 0 || widths.Any(width => width <= 0))
            throw new ArgumentException("Укажите положительную ширину каждой колонки.", nameof(widths));
        if (rows.Count == 0 || rows.Any(row => row == null || row.Count != widths.Count))
            throw new ArgumentException(
                "Таблица должна содержать заголовок, а число ячеек должно совпадать с числом колонок.",
                nameof(rows));

        var table = new W.Table(
            new W.TableProperties(
                new W.TableWidth
                {
                    Type = W.TableWidthUnitValues.Dxa,
                    Width = widths.Sum().ToString(CultureInfo.InvariantCulture)
                },
                new W.TableBorders(
                    SetBorder(new W.TopBorder()), SetBorder(new W.LeftBorder()),
                    SetBorder(new W.BottomBorder()), SetBorder(new W.RightBorder()),
                    SetBorder(new W.InsideHorizontalBorder()), SetBorder(new W.InsideVerticalBorder())),
                new W.TableLayout { Type = W.TableLayoutValues.Fixed }),
            new W.TableGrid(widths.Select(width => new W.GridColumn
            {
                Width = width.ToString(CultureInfo.InvariantCulture)
            })));

        table.AppendChild(CreateRow(rows.First(), widths, isHeader: true));
        table.Append(rows.Skip(1).Select(row => CreateRow(row, widths)));
        _elements.Add(table);
    }

    public void Save(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        using var file = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document);
        var part = file.AddMainDocumentPart();
        var body = new W.Body(_elements.Select(element => element.CloneNode(true)));
        body.AppendChild(new W.SectionProperties(
            new W.PageSize { Width = 11906, Height = 16838 },
            new W.PageMargin
            {
                Top = 1134, Right = 1134, Bottom = 1134, Left = 1134,
                Header = 567, Footer = 567, Gutter = 0
            }));
        part.Document = new W.Document(body);
        part.Document.Save();
    }

    private static T SetBorder<T>(T border) where T : W.BorderType
    {
        border.Val = W.BorderValues.Single;
        border.Size = 4;
        border.Color = "999999";
        return border;
    }

    private static W.TableRow CreateRow(IReadOnlyList<string> values, IReadOnlyList<int> widths, bool isHeader = false)
    {
        var row = new W.TableRow();
        if (isHeader)
            row.AppendChild(new W.TableRowProperties(new W.TableHeader()));

        row.Append(values.Select((value, index) => new W.TableCell(
            new W.TableCellProperties(new W.TableCellWidth
            {
                Type = W.TableWidthUnitValues.Dxa,
                Width = widths[index].ToString(CultureInfo.InvariantCulture)
            }),
            CreateParagraph(value, bold: isHeader, tableCell: true))));
        return row;
    }

    private static W.Paragraph CreateParagraph(
        string text, bool bold = false, bool heading = false, bool tableCell = false)
    {
        var paragraphProperties = new W.ParagraphProperties();
        if (heading)
            paragraphProperties.AppendChild(new W.KeepNext());
        paragraphProperties.AppendChild(new W.SpacingBetweenLines
        {
            Before = heading ? "180" : "0",
            After = tableCell ? "40" : "120"
        });

        var runProperties = new W.RunProperties(new W.RunFonts { Ascii = "Calibri", HighAnsi = "Calibri" });
        if (bold)
            runProperties.AppendChild(new W.Bold());
        runProperties.AppendChild(new W.FontSize { Val = heading ? "30" : "22" });

        var run = new W.Run(runProperties);
        var lines = (text ?? string.Empty).ReplaceLineEndings("\n").Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (i > 0)
                run.AppendChild(new W.Break());
            run.AppendChild(new W.Text(lines[i]) { Space = SpaceProcessingModeValues.Preserve });
        }
        return new W.Paragraph(paragraphProperties, run);
    }
}
