using CustomTranscript.App.Models;
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
        builder.AddHeading("HDRU Professional Training History");
        builder.AddParagraph($"Professional Training History for: {userName}");
        builder.AddParagraph($"Certificate of completion for {dateFrom:d} - {dateTo:d}");
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
        var table = builder.AddTable(5, 2, 2, 2, 2, 1, 1, 1, 1, 1);

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
                r.CEUs.ToString("0.##"),
                r.CPDs.ToString("0.#"),
                r.HSWs.ToString("0.#"),
                r.LUs.ToString("0.#"),
                r.PDHs.ToString("0.#"));
        }

        table.AddMergedRow(5, "Total Credit Hours for this report:");

        table.AddRow(
            "",
            "",
            "",
            "",
            "",
            rows.Sum(r => r.CEUs).ToString("0.##"),
            rows.Sum(r => r.CPDs).ToString("0.#"),
            rows.Sum(r => r.HSWs).ToString("0.#"),
            rows.Sum(r => r.LUs).ToString("0.#"),
            rows.Sum(r => r.PDHs).ToString("0.#"));
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