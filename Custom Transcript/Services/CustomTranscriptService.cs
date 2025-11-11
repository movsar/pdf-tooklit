using Azure.Storage.Blobs;
using CornerstoneApiServices.Models.Core;
using CustomTranscript.App.Models;

namespace CustomTranscript.App.Services
{
    public class CustomTranscriptService
    {

        private readonly ApiReportingService _apiReportingService;
        private readonly PdfGenerationService _pdfService;

        public CustomTranscriptService(ApiReportingService apiBasedReportingService,
            PdfGenerationService pdfService)
        {
            _apiReportingService = apiBasedReportingService;
            _pdfService = pdfService;
        }

        public async Task<byte[]> CreatePdfReport(Employee employee, DateTime dateFrom, DateTime dateTo)
        {
            List<ReportRow> reportData = await _apiReportingService.GetReportDataAsync((int)employee.Id!, employee.ExternalId!, dateFrom, dateTo);

            try
            {
                var pdf = _pdfService.CreatePdfReport($"{employee.LastName}, {employee.FirstName}", dateFrom, dateTo, employee.Settings?.TimeZone, reportData);
                return pdf.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while generating or saving the PDF file", ex);
            }
        }
      
        public async Task UploadPdfToBlobAsync(string containerName, string blobName, MemoryStream pdfStream)
        {
            // ! Method is deprecated
            string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Storage connection is not set");
            }

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            // Upload the PDF from memory stream to Azure Blob Storage
            await blobClient.UploadAsync(pdfStream, overwrite: true);
        }
    }
}