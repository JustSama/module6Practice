using System;
using System.Collections.Generic;
using System.IO;

public interface IReportBuilder
{
    void SetHeader(string header);
    void SetContent(string content);
    void SetFooter(string footer);
    void AddSection(string sectionName, string sectionContent);
    void SetStyle(ReportStyle style);
    Report GetReport();
}

public class TextReportBuilder : IReportBuilder
{
    private Report report;

    public TextReportBuilder()
    {
        report = new Report("Text");
    }

    public void SetHeader(string header) { report.Header = header; }
    public void SetContent(string content) { report.Content = content; }
    public void SetFooter(string footer) { report.Footer = footer; }
    public void AddSection(string sectionName, string sectionContent)
    {
        report.Sections.Add(sectionName, sectionContent);
    }

    public void SetStyle(ReportStyle style) { report.Style = style; }
    public Report GetReport() { return report; }
}

public class HtmlReportBuilder : IReportBuilder
{
    private Report report;

    public HtmlReportBuilder()
    {
        report = new Report("HTML");
    }

    public void SetHeader(string header) { report.Header = $"<h1>{header}</h1>"; }
    public void SetContent(string content) { report.Content = $"<p>{content}</p>"; }
    public void SetFooter(string footer) { report.Footer = $"<footer>{footer}</footer>"; }
    public void AddSection(string sectionName, string sectionContent)
    {
        report.Sections.Add(sectionName, $"<h2>{sectionName}</h2><p>{sectionContent}</p>");
    }

    public void SetStyle(ReportStyle style) { report.Style = style; }
    public Report GetReport() { return report; }
}

public class PdfReportBuilder : IReportBuilder
{
    private Report report;

    public PdfReportBuilder()
    {
        report = new Report("PDF");
    }

    public void SetHeader(string header) { report.Header = header; }
    public void SetContent(string content) { report.Content = content; }
    public void SetFooter(string footer) { report.Footer = footer; }
    public void AddSection(string sectionName, string sectionContent)
    {
        report.Sections.Add(sectionName, sectionContent);
    }

    public void SetStyle(ReportStyle style) { report.Style = style; }
    public Report GetReport() { return report; }
}

public class Report
{
    public string Header { get; set; }
    public string Content { get; set; }
    public string Footer { get; set; }
    public Dictionary<string, string> Sections { get; }
    public ReportStyle Style { get; set; }

    public Report(string type)
    {
        Sections = new Dictionary<string, string>();
    }

    public void Export(string format)
    {
        string filePath = $"Report.{format.ToLower()}";
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(Header);
            writer.WriteLine(Content);
            foreach (var section in Sections)
            {
                writer.WriteLine($"{section.Key}: {section.Value}");
            }
            writer.WriteLine(Footer);
        }
    }
}

public class ReportStyle
{
    public string BackgroundColor { get; set; }
    public string FontColor { get; set; }
    public float FontSize { get; set; }
}

public class ReportDirector
{
    public void ConstructReport(IReportBuilder builder, ReportStyle style)
    {
        builder.SetStyle(style);
        builder.SetHeader("Report Header");
        builder.SetContent("This is the main content of the report.");
        builder.SetFooter("Report Footer");
        builder.AddSection("Introduction", "This is the introduction section.");
        builder.AddSection("Conclusion", "This is the conclusion section.");
    }
}

class Program
{
    static void Main()
    {
        var director = new ReportDirector();
        var textBuilder = new TextReportBuilder();
        var htmlBuilder = new HtmlReportBuilder();
        var pdfBuilder = new PdfReportBuilder();
        var style = new ReportStyle { BackgroundColor = "White", FontColor = "Black", FontSize = 12 };

        director.ConstructReport(textBuilder, style);
        var textReport = textBuilder.GetReport();
        textReport.Export("text");

        director.ConstructReport(htmlBuilder, style);
        var htmlReport = htmlBuilder.GetReport();
        htmlReport.Export("html");

        director.ConstructReport(pdfBuilder, style);
        var pdfReport = pdfBuilder.GetReport();
        pdfReport.Export("pdf");
    }
}
