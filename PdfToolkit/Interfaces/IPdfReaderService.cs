namespace PdfToolkit.Interfaces
{
    public interface IPdfReaderService
    {
        /// <summary>
        /// Extract full text from a PDF file.
        /// </summary>
        string ExtractText(string filePath);

        /// <summary>
        /// Extract text from a specific page (1-based).
        /// Returns empty string for out-of-range pages.
        /// </summary>
        string ExtractPageText(string filePath, int pageNumber);
    }
}
