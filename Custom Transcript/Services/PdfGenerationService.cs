using System.Reflection;
using CustomTranscript.App.Models;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using PdfToolkit.Services;

public sealed class PdfGenerationService
{
    private const string NO_DATA = "N/A";
    private readonly PdfDocumentService _pdfService;

    public PdfGenerationService(PdfDocumentService pdfService)
    {
        _pdfService = pdfService;
    }

    public MemoryStream CreatePdfReport(string userName, DateTime dateFrom, DateTime dateTo, string? employeeTimeZone, List<ReportRow> reportRows)
    {
        var pdfBytes = _pdfService.GeneratePdf(builder =>
        {
            SetFooter(builder, employeeTimeZone);
            AddLogos(builder);
            AddMainText(builder, userName, dateFrom, dateTo);
            AddTable(builder, reportRows);
        });

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
        var document = builder.Build();
        var currentSection = document.LastSection;

        var logoTable = currentSection.AddTable();
        logoTable.Borders.Visible = false;

        logoTable.AddColumn(Unit.FromCentimeter(10.5));
        logoTable.AddColumn(Unit.FromCentimeter(10.0));

        var row = logoTable.AddRow();
        row.VerticalAlignment = VerticalAlignment.Top;

        var leftCell = row.Cells[0];
        var hdrBytes = GetImageResourceAsBytes("hdr-logo.png");
        var hdrImg = leftCell.AddImage(new ByteArrayImageSource(hdrBytes));
        hdrImg.Width = Unit.FromCentimeter(2.8);
        hdrImg.LockAspectRatio = true;

        var address = leftCell.AddParagraph(
            "HDR, Inc.\r\n" +
            "1917 S. 67th Street\r\n" +
            "Omaha, NE 68106\r\n" +
            "(402) 399-1000\r\n" +
            "HDRUniversityRegistrar@hdrinc.com");
        address.Format.Font.Size = 9;
        address.Format.SpaceBefore = Unit.FromPoint(8);

        var rightCell = row.Cells[1];

        var smallTable = rightCell.Elements.AddTable();
        smallTable.Borders.Visible = false;

        smallTable.AddColumn(Unit.FromCentimeter(4.0));
        smallTable.AddColumn(Unit.FromCentimeter(3.6));
        smallTable.AddColumn(Unit.FromCentimeter(1.5));
        smallTable.AddColumn(Unit.FromCentimeter(3.6));
        smallTable.AddColumn(Unit.FromCentimeter(1.5));
        smallTable.AddColumn(Unit.FromCentimeter(4.2));

        var smallRow = smallTable.AddRow();

        smallRow.Cells[0].AddParagraph();
        smallRow.Cells[2].AddParagraph();
        smallRow.Cells[4].AddParagraph();

        var logos = new[]
        {
         (CellIndex: 1, FileName: "aia-logo.png", Width: 3.4, Text: "AIA Provider: C093"),
         (CellIndex: 3, FileName: "cpd-logo.png", Width: 3.0, Text: "CPD Number: 15890"),
         (CellIndex: 5, FileName: "iacet-logo.png", Width: 3.4, Text: "IACET Provider: 1001157-3")
        };
        foreach (var logo in logos)
        {
            var img = smallRow.Cells[logo.CellIndex].AddImage(new ByteArrayImageSource(GetImageResourceAsBytes(logo.FileName)));
            img.Width = Unit.FromCentimeter(logo.Width);
            img.LockAspectRatio = true;
            smallRow.Cells[logo.CellIndex].AddParagraph(logo.Text).Format.Font.Size = 9;
        }

        currentSection.AddParagraph().Format.SpaceAfter = Unit.FromCentimeter(1.2);
    }

    private void AddTable(PdfBuilder builder, List<ReportRow> rows)
    {
        var tableBuilder = new TableBuilder(6, 3, 3, 3, 3.5, 1.8, 1.8, 1.8, 1.8, 1.8);
        var table = tableBuilder.AddHeader(
            "Course Name", "Date Completed", "Provider Name",
            "Delivery Type", "Author / Instructor", "CEUs",
            "CPDs", "HSWs", "LUs", "PDHs")
            .Build();

        foreach (var r in rows)
        {
            tableBuilder.AddRow(
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

        tableBuilder.AddMergedRow(
            5,
            "Total Credit Hours for this report:",
            rows.Sum(r => r.CEUs).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.CPDs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.HSWs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.LUs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
            rows.Sum(r => r.PDHs).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
        );

        builder.AddTable(table);
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