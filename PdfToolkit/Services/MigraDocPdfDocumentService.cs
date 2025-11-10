using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;

namespace PdfToolkit.Services
{
    public sealed class MigraDocPdfDocumentService : IPdfDocumentService
    {
        public void CreatePdf(string filePath, Action<PdfBuilder> build)
        {
            if (filePath is null) throw new ArgumentNullException(nameof(filePath));
            if (build is null) throw new ArgumentNullException(nameof(build));

            var builder = new PdfBuilder();
            build(builder);

            SaveDocument(builder.Document, filePath);
        }

        void SaveDocument(Document doc, string filePath)
        {
            // PdfDocumentRenderer can render MigraDoc Document into PDF.
            var renderer = new PdfDocumentRenderer()
            {
                Document = doc
            };
            renderer.RenderDocument();
            // ensure directory exists
            var dir = System.IO.Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            renderer.PdfDocument.Save(filePath);
        }
    }
}
