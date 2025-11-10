using PdfToolkit.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace PdfToolkit.Services
{
    public sealed class PdfPigReaderService : IPdfReaderService
    {
        public string ExtractText(string filePath)
        {
            if (filePath is null) throw new ArgumentNullException(nameof(filePath));
            var sb = new StringBuilder();
            using var doc = PdfDocument.Open(filePath);
            foreach (var page in doc.GetPages())
            {
                sb.AppendLine(page.Text);
            }
            return sb.ToString();
        }

        public string ExtractPageText(string filePath, int pageNumber)
        {
            if (filePath is null) throw new ArgumentNullException(nameof(filePath));
            if (pageNumber < 1) return string.Empty;
            using var doc = PdfDocument.Open(filePath);
            if (pageNumber > doc.NumberOfPages) return string.Empty;
            var page = doc.GetPage(pageNumber);
            return page.Text ?? string.Empty;
        }
    }
}
