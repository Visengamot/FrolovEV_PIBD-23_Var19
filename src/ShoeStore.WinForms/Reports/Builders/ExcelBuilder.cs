using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using X = DocumentFormat.OpenXml.Spreadsheet;

namespace ShoeStore.WinForms.Reports.Builders;

public sealed class ExcelBuilder
{
    private const int MaxColumns = 16384;
    private const int MaxRows = 1048576;
    private const uint HeadingStyle = 1;
    private const uint HeaderStyle = 2;
    private const uint TableStyle = 3;
    private const uint TotalStyle = 4;

    private readonly List<X.Row> _rows = new();
    private readonly List<double> _widths = new();

    public void AddHeading(string text)
    {
        AddRow(new[] { text }, HeadingStyle);
    }

    public void AddText(string text)
    {
        AddRow(new[] { text });
    }

    public void AddTable(
        IReadOnlyList<string> headers,
        IReadOnlyList<IReadOnlyList<object>> rows,
        IReadOnlyList<object> total)
    {
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(total);
        if (headers.Count == 0 || headers.Count > MaxColumns)
            throw new ArgumentException("Таблица должна содержать от 1 до 16384 колонок.", nameof(headers));
        if (rows.Count > MaxRows - _rows.Count - 2)
            throw new ArgumentException("Превышено допустимое число строк листа Excel.", nameof(rows));
        if (rows.Any(row => row == null || row.Count != headers.Count))
            throw new ArgumentException("Число ячеек каждой строки должно совпадать с числом колонок.", nameof(rows));
        if (total.Count != headers.Count)
            throw new ArgumentException("Число ячеек строки «Итого» должно совпадать с числом колонок.", nameof(total));

        AddRow(headers, HeaderStyle, adjustWidths: true);
        foreach (var row in rows)
            AddRow(row, TableStyle, adjustWidths: true);
        AddRow(total, TotalStyle, adjustWidths: true);
    }

    public void Save(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        using var file = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
        var workbookPart = file.AddWorkbookPart();
        var sheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
        stylesPart.Stylesheet = CreateStyles();
        stylesPart.Stylesheet.Save();

        var worksheet = new X.Worksheet();
        if (_widths.Count > 0)
        {
            worksheet.AppendChild(new X.Columns(_widths.Select((width, index) => new X.Column
            {
                Min = (uint)index + 1,
                Max = (uint)index + 1,
                Width = width,
                CustomWidth = true
            })));
        }
        worksheet.AppendChild(new X.SheetData(_rows.Select(row => row.CloneNode(true))));
        sheetPart.Worksheet = worksheet;
        worksheet.Save();

        workbookPart.Workbook = new X.Workbook(new X.Sheets(new X.Sheet
        {
            Id = workbookPart.GetIdOfPart(sheetPart),
            SheetId = 1,
            Name = "Отчет"
        }));
        workbookPart.Workbook.Save();
    }

    private void AddRow(IReadOnlyList<object> values, uint style = 0, bool adjustWidths = false)
    {
        if (_rows.Count >= MaxRows)
            throw new InvalidOperationException("Превышено допустимое число строк листа Excel.");

        uint rowIndex = (uint)_rows.Count + 1;
        var row = new X.Row { RowIndex = rowIndex };
        for (int i = 0; i < values.Count; i++)
        {
            var cell = CreateCell(values[i]);
            cell.StyleIndex = style;
            cell.CellReference = GetColumnName(i + 1) + rowIndex.ToString(CultureInfo.InvariantCulture);
            row.AppendChild(cell);

            if (adjustWidths)
            {
                if (i == _widths.Count)
                    _widths.Add(12);
                var length = cell.InnerText.ReplaceLineEndings("\n").Split('\n').Max(line => line.Length);
                _widths[i] = Math.Max(_widths[i], Math.Min(56, length + 3));
            }
        }
        _rows.Add(row);
    }

    private static X.Cell CreateCell(object? value)
    {
        switch (value)
        {
            case null:
                return new X.Cell();
            case bool boolean:
                return new X.Cell { DataType = X.CellValues.Boolean, CellValue = new X.CellValue(boolean ? "1" : "0") };
            case byte or sbyte or short or ushort or int or uint or long or ulong or decimal:
            case float single when float.IsFinite(single):
            case double number when double.IsFinite(number):
                return new X.Cell
                {
                    DataType = X.CellValues.Number,
                    CellValue = new X.CellValue(Convert.ToString(value, CultureInfo.InvariantCulture)!)
                };
            default:
                return new X.Cell
                {
                    DataType = X.CellValues.InlineString,
                    InlineString = new X.InlineString(new X.Text(value.ToString() ?? string.Empty)
                    {
                        Space = SpaceProcessingModeValues.Preserve
                    })
                };
        }
    }

    private static string GetColumnName(int index)
    {
        var name = string.Empty;
        while (index > 0)
        {
            index--;
            name = (char)('A' + index % 26) + name;
            index /= 26;
        }
        return name;
    }

    private static X.Stylesheet CreateStyles()
    {
        return new X.Stylesheet(
            new X.Fonts(
                CreateFont(),
                CreateFont(size: 16, bold: true),
                CreateFont(bold: true)),
            new X.Fills(
                new X.Fill(new X.PatternFill { PatternType = X.PatternValues.None }),
                new X.Fill(new X.PatternFill { PatternType = X.PatternValues.Gray125 }),
                new X.Fill(new X.PatternFill(
                    new X.ForegroundColor { Rgb = "FFE3EDF7" },
                    new X.BackgroundColor { Indexed = 64 }) { PatternType = X.PatternValues.Solid })),
            new X.Borders(
                new X.Border(new X.LeftBorder(), new X.RightBorder(), new X.TopBorder(),
                    new X.BottomBorder(), new X.DiagonalBorder()),
                new X.Border(
                    new X.LeftBorder { Style = X.BorderStyleValues.Thin },
                    new X.RightBorder { Style = X.BorderStyleValues.Thin },
                    new X.TopBorder { Style = X.BorderStyleValues.Thin },
                    new X.BottomBorder { Style = X.BorderStyleValues.Thin },
                    new X.DiagonalBorder())),
            new X.CellStyleFormats(new X.CellFormat { NumberFormatId = 0, FontId = 0, FillId = 0, BorderId = 0 }),
            new X.CellFormats(
                CreateFormat(0, 0, 0),
                CreateFormat(1, 0, 0),
                CreateFormat(2, 2, 1, center: true),
                CreateFormat(0, 0, 1),
                CreateFormat(2, 2, 1)),
            new X.CellStyles(new X.CellStyle { Name = "Normal", FormatId = 0, BuiltinId = 0 }));
    }

    private static X.Font CreateFont(double size = 11, bool bold = false)
    {
        var font = new X.Font();
        if (bold)
            font.AppendChild(new X.Bold());
        font.Append(new X.FontSize { Val = size }, new X.FontName { Val = "Calibri" });
        return font;
    }

    private static X.CellFormat CreateFormat(uint font, uint fill, uint border, bool center = false)
    {
        var format = new X.CellFormat
        {
            NumberFormatId = 0,
            FontId = font,
            FillId = fill,
            BorderId = border,
            FormatId = 0,
            ApplyFont = true,
            ApplyFill = fill != 0,
            ApplyBorder = border != 0,
            ApplyAlignment = true,
            Alignment = new X.Alignment { Vertical = X.VerticalAlignmentValues.Center, WrapText = border != 0 }
        };
        if (center)
            format.Alignment.Horizontal = X.HorizontalAlignmentValues.Center;
        return format;
    }
}
