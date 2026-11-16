namespace SusanOsuolaleCV.Services
{
    public interface IGalleryStorage
    {
        bool IsConfigured { get; }
        Task UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
        Task<Stream?> OpenReadAsync(string blobName, CancellationToken cancellationToken = default);
        Task<bool> DeleteIfExistsAsync(string blobName, CancellationToken cancellationToken = default);
    }
}
