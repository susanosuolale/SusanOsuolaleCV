using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SusanOsuolaleCV.Services
{
    public class AzureBlobGalleryStorage : IGalleryStorage
    {
        private readonly BlobContainerClient? _container;

        public AzureBlobGalleryStorage(IConfiguration configuration)
        {
            var containerName = configuration["AzureStorage:ContainerName"];
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var accountName = configuration["AzureStorage:AccountName"];

            if (string.IsNullOrWhiteSpace(containerName))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _container = new BlobContainerClient(connectionString, containerName);
            }
            else if (!string.IsNullOrWhiteSpace(accountName))
            {
                var containerUri = new Uri($"https://{accountName}.blob.core.windows.net/{containerName}");
                _container = new BlobContainerClient(containerUri, new DefaultAzureCredential());
            }
        }

        public bool IsConfigured => _container is not null;

        public async Task UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
        {
            var container = GetContainer();
            await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

            var blob = container.GetBlobClient(blobName);
            await blob.UploadAsync(
                content,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
                },
                cancellationToken);
        }

        public async Task<Stream?> OpenReadAsync(string blobName, CancellationToken cancellationToken = default)
        {
            if (_container is null)
            {
                return null;
            }

            var blob = _container.GetBlobClient(blobName);
            if (!await blob.ExistsAsync(cancellationToken))
            {
                return null;
            }

            var response = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
            return response.Value.Content;
        }

        public async Task<bool> DeleteIfExistsAsync(string blobName, CancellationToken cancellationToken = default)
        {
            if (_container is null)
            {
                return false;
            }

            var blob = _container.GetBlobClient(blobName);
            var response = await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            return response.Value;
        }

        private BlobContainerClient GetContainer() =>
            _container ?? throw new InvalidOperationException(
                "Azure Blob Storage is not configured. Set AzureStorage:ContainerName and either AzureStorage:AccountName or AzureStorage:ConnectionString.");
    }
}
