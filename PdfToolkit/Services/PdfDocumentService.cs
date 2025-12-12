using System.Text;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using UglyToad.PdfPig;

namespace PdfToolkit.Services
{
    public sealed class PdfDocumentService
    {
        public void SavePdf(string filePath, Action<PdfBuilder> build)
        {
            if (filePath is null) throw new ArgumentNullException(nameof(filePath));
            if (build is null) throw new ArgumentNullException(nameof(build));

            var builder = new PdfBuilder();
            build(builder);

            var document = builder.Build();
            SaveDocument(document, filePath);
        }

        public byte[] GeneratePdf(Action<PdfBuilder> build)
        {
            if (build is null) throw new ArgumentNullException(nameof(build));

            var builder = new PdfBuilder();
            build(builder);

            var document = builder.Build();
            return GeneratePdfBytes(document);
        }

        public byte[] GeneratePdfBytes(Document document)
        {
            var renderer = new PdfDocumentRenderer()
            {
                Document = document
            };
            renderer.RenderDocument();

            using var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            return stream.ToArray();
        }

        void SaveDocument(Document doc, string filePath)
        {
            var renderer = new PdfDocumentRenderer()
            {
                Document = doc
            };
            renderer.RenderDocument();

            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(filePath))
                File.Delete(filePath);

            renderer.PdfDocument.Save(filePath);
        }

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