using Microsoft.Extensions.Configuration;
using SmartPayPark.Application.Common.Interfaces;

namespace SmartPayPark.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _storagePath;

    private const long MaxFileSize = 5 * 1024 * 1024;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _storagePath = configuration["FileStorage:Path"]
            ?? Path.Combine(
                AppContext.BaseDirectory,
                "App_Data",
                "OperatorDocuments");

        Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (extension is not ".pdf" and not ".jpg" and not ".jpeg" and not ".png")
            throw new ArgumentException("Unsupported file format.");

        var storageKey = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(_storagePath, storageKey);

        await using var output = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None);

        var buffer = new byte[81920];
        long totalBytes = 0;

        try
        {
            int bytesRead;

            while ((bytesRead = await fileStream.ReadAsync(
                buffer.AsMemory(0, buffer.Length),
                cancellationToken)) > 0)
            {
                totalBytes += bytesRead;

                if (totalBytes > MaxFileSize)
                    throw new ArgumentException("Maximum file size is 5 MB.");

                await output.WriteAsync(
                    buffer.AsMemory(0, bytesRead),
                    cancellationToken);
            }

            if (totalBytes == 0)
                throw new ArgumentException("The uploaded file is empty.");

            return storageKey;
        }
        catch
        {
            await output.DisposeAsync();

            if (File.Exists(filePath))
                File.Delete(filePath);

            throw;
        }
    }

    public Task DeleteAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storageKey) ||
            Path.GetFileName(storageKey) != storageKey)
        {
            throw new ArgumentException("Invalid storage key.");
        }

        var filePath = Path.Combine(_storagePath, storageKey);

        cancellationToken.ThrowIfCancellationRequested();

        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }

    public async Task<Stream?> OpenReadAsync(
        string storageKey, 
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // check storage key
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            return null;
        }

        // get file name 
        string filename = Path.GetFileName(storageKey);

        if(!string.Equals(
            filename,
            storageKey,
            StringComparison.Ordinal)){
            return null;
        }

        string filepath = Path.Combine(_storagePath, filename);

        if (!File.Exists(filepath))
        {
            return null;
        }

        Stream stream = new FileStream(
            filepath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 81920,
            useAsync: true
            );

        return stream;
    }
}