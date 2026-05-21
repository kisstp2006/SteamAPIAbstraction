namespace SteamOwnershipExample.Core;

public sealed record CloudFileEntry(string Name, int SizeBytes);

public interface ICloudStorageProvider : IAsyncDisposable
{
    string Name { get; }
    bool IsCloudEnabled { get; }

    Task<bool> ExistsAsync(string fileName, CancellationToken ct = default);
    Task<byte[]> ReadAsync(string fileName, CancellationToken ct = default);
    Task<bool> WriteAsync(string fileName, byte[] data, CancellationToken ct = default);
    Task<bool> DeleteAsync(string fileName, CancellationToken ct = default);
    Task<IReadOnlyList<CloudFileEntry>> ListAsync(CancellationToken ct = default);
}
