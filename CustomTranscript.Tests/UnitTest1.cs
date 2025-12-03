using CustomTranscript.App.Models;
using PdfToolkit.Services;
using System.Diagnostics;

namespace CustomTranscript.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CreatePdfReport_ShouldGeneratePdfFile()
        {
            // Arrange
            var service = new MigraDocPdfDocumentService();
            var pdfService = new PdfGenerationService(service);

            string userName = "John Doe";
            DateTime dateFrom = DateTime.Today.AddDays(-7);
            DateTime dateTo = DateTime.Today;
            string? employeeTimeZone = "UTC";

            // Generate 10 sample report rows
            var reportRows = new List<ReportRow>();
            for (int i = 1; i <= 100; i++)
            {
                reportRows.Add(new ReportRow
                {
                    DateCompleted = DateTime.Today.AddDays(-i),
                    CourseName = $"Course Course Course Course Course Course Course Course Course Course Course {i}",
                    ProviderName = $"Provider {i}",
                    Author = $"Author {i}",
                    DeliveryType = "Online",
                    CEUs = i * 0.5,
                    CPDs = i * 1.0,
                    LUs = i * 0.25,
                    PDHs = i * 0.75,
                    HSWs = i * 0.1
                });
            }

            // Act
            MemoryStream resultStream = pdfService.CreatePdfReport(userName, dateFrom, dateTo, employeeTimeZone, reportRows);

            // Assert
            Assert.NotNull(resultStream);
            Assert.True(resultStream.Length > 0, "MemoryStream should contain PDF content");

            // Optional: write to disk to check manually
            File.WriteAllBytes("test_report.pdf", resultStream.ToArray());
            Process.Start(new ProcessStartInfo("test_report.pdf") { UseShellExecute = true });
        }
    }
}