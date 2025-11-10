namespace PdfToolkit.Services
{
    public interface IPdfDocumentService
    {
        /// <summary>
        /// Build and save a PDF file.
        /// </summary>
        /// <param name="filePath">Target file path to create/overwrite.</param>
        /// <param name="build">Use builder to describe document content and styles.</param>
        void CreatePdf(string filePath, Action<PdfBuilder> build);
    }
}
