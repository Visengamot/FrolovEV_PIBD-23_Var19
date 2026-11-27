using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;

namespace ShoeStore.WinForms.Reports.Builders;

public sealed class PdfBuilder
{
    private readonly Document _document = new();
    private readonly Section _section;

    public PdfBuilder()
    {
        var normal = _document.Styles[StyleNames.Normal]!;
        normal.Font.Name = "Arial";
        normal.Font.Size = 11;
        normal.ParagraphFormat.SpaceAfter = Unit.FromPoint(5);
        _section = _document.AddSection();
        _section.PageSetup.PageFormat = PageFormat.A4;
    }

    public void AddHeading(string text)
    {
        var heading = _section.AddParagraph(text);
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = 17;
        heading.Format.SpaceAfter = Unit.FromPoint(10);
    }

    public void AddText(string text) => _section.AddParagraph(text);

    public void AddPieChart(string[] labels, double[] values)
    {
        if (labels.Length == 0 || labels.Length != values.Length
            || values.Any(value => !double.IsFinite(value) || value <= 0))
        {
            throw new ArgumentException("Для каждого сектора нужны название и положительное значение.");
        }

        var chart = _section.AddChart(ChartType.Pie2D);
        chart.Width = Unit.FromCentimeter(14);
        chart.Height = Unit.FromCentimeter(9);
        chart.Format.Font.Name = "Arial";
        chart.Format.Font.Size = 10;
        chart.SeriesCollection.AddSeries().Add(values);
        chart.XValues.AddXSeries().Add(labels);
        var legend = chart.BottomArea.AddLegend();
        legend.Format.Font.Name = "Arial";
        legend.Format.Font.Size = 10;
    }

    public void Save(string path)
    {
        var renderer = new PdfDocumentRenderer { Document = _document };
        renderer.RenderDocument();
        using var pdf = renderer.PdfDocument;
        pdf.Save(path);
    }
}
