using CustomTranscript.App.Models;
using MigraDocCore.DocumentObjectModel;
using PdfToolkit.Interfaces;
using PdfToolkit.Services;
using System.Reflection;

public sealed class PdfGenerationService
{
    private const string NO_DATA = "N/A";

    private readonly IPdfDocumentService _pdfService;

    public PdfGenerationService(IPdfDocumentService pdfService)
    {
        _pdfService = pdfService;
    }

    public MemoryStream CreatePdfReport(string userName, DateTime dateFrom, DateTime dateTo, string? employeeTimeZone, List<ReportRow> reportRows)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"report_{Guid.NewGuid():N}.pdf");

        _pdfService.CreatePdf(tempPath, builder =>
        {
            SetFooter(builder, employeeTimeZone);
            AddLogos(builder);
            AddMainText(builder, userName, dateFrom, dateTo);
            AddTable(builder, reportRows);
        });

        // Read the file into a byte array first
        byte[] pdfBytes = File.ReadAllBytes(tempPath);

        // Now it's safe to delete
        File.Delete(tempPath);
        
        // Return as MemoryStream
        return new MemoryStream(pdfBytes);
    }

    private void AddMainText(PdfBuilder builder, string userName, DateTime dateFrom, DateTime dateTo)
    {
        var historyParagraph = builder.AddCenteredParagraph("HDRU Professional Training History", 16, bold: true);
        historyParagraph.Format.SpaceAfter = Unit.FromCentimeter(0.3);
        var trainingHistoryParagraph = builder.AddParagraphSized($"Professional Training History for: {userName}", 11, bold: true);
        trainingHistoryParagraph.Format.SpaceAfter = Unit.FromCentimeter(0.2);
        var certificateParagraph = builder.AddParagraphSized($"Certificate of completion for {dateFrom:d} - {dateTo:d}", 11, bold: false);
        certificateParagraph.Format.SpaceAfter = Unit.FromCentimeter(0.1);
    }

    private void AddLogos(PdfBuilder builder)
    {
        var logos = new List<(string name, string text, double width)>
            {
                ("hdr-logo.png", "HDR, Inc.\r\n1917 S. 67th Street\r\nOmaha, NE 68106\r\n(402) 399-1000\r\nHDRUniversityRegistrar@hdrinc.com\r\n", 2.5),
                ("aia-logo.png", "AIA Provider: C093", 1.5),
                ("cpd-logo.png", "CPD Number: 15890", 2.0),
                ("iacet-logo.png", "IACET Provider: 1001157-3", 2.5)
            };

        foreach (var (name, text, width) in logos)
        {
            var bytes = GetImageResourceAsBytes(name);
            builder.AddImage(bytes, width);
            builder.AddParagraph(text);
        }
    }

    private void AddTable(PdfBuilder builder, List<ReportRow> rows)
    {
        var table = builder.AddTable(6, 3, 3, 3, 3.5, 1.8, 1.8, 1.8, 1.8, 1.8);

        table.AddHeader(
            "Course Name", "Date Completed", "Provider Name",
            "Delivery Type", "Author / Instructor", "CEUs",
            "CPDs", "HSWs", "LUs", "PDHs");

        foreach (var r in rows)
        {
            table.AddRow(
                r.CourseName,
                r.DateCompleted?.ToString("yyyy-MM-dd") ?? NO_DATA,
                r.ProviderName,
                r.DeliveryType,
                r.Author,
                r.CEUs.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture),
                r.CPDs.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture),
                r.HSWs.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture),
                r.LUs.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture),
                r.PDHs.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture));
        }

        table.AddMergedRow(
            5,
            "Total Credit Hours for this report:",
            rows.Sum(r => r.CEUs).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.CPDs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.HSWs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.LUs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.PDHs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
        );
    }

    private void SetFooter(PdfBuilder builder, string? employeeTimeZone)
    {
        var culture = new System.Globalization.CultureInfo("en-US");
        var now = DateTime.Now;
        var month = now.ToString("MMMM", culture);
        var day = now.Day;
        var time = now.ToString("h:mm tt", culture);
        var footerText = $"{month} {day}, {time}, Eastern Time (US & Canada)";

        builder.SetFooter(footerText);
    }

    private static byte[] GetImageResourceAsBytes(string resourceName)
    {
        var asm = Assembly.GetExecutingAssembly();
        var path = $"CustomTranscript.App.Images.{resourceName}";

        using var stream = asm.GetManifestResourceStream(path)
            ?? throw new FileNotFoundException("Embedded image not found", path);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}