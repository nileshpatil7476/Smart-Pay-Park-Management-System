using Microsoft.AspNetCore.Http;

namespace SmartPayPark.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string storageKey,
            CancellationToken cancellationToken = default);
    }
}
