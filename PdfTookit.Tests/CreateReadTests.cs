using PdfToolkit.Services;

namespace PdfTookit.Tests
{
    public class CreateReadTests
    {
        [Fact]
        public void CreatePdf_ThenReadText_ShouldContainHeadingAndParagraph()
        {
            var svc = new MigraDocPdfDocumentService();
            var reader = new PdfPigReaderService();

            var tmp = Path.Combine(Path.GetTempPath(), "pdftoolkit_test.pdf");
            if (File.Exists(tmp)) File.Delete(tmp);

            svc.CreatePdf(tmp, builder =>
            {
                builder.AddHeading("Test Heading");
                builder.AddParagraph("Hello PdfToolkit. This is a test paragraph with number 123.");
            });

            Assert.True(File.Exists(tmp));

            var text = reader.ExtractText(tmp);
            Assert.False(string.IsNullOrWhiteSpace(text));

            // Normalize text for PDF whitespace quirks
            var normalized = new string(text.Where(c => !char.IsWhiteSpace(c)).ToArray());

            Assert.Contains("TestHeading", normalized, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("HelloPdfToolkit", normalized, StringComparison.OrdinalIgnoreCase);
        }
    }
}