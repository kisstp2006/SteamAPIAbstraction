using SteamOwnershipExample.Core;
using Steamworks;

namespace SteamOwnershipExample.Providers;

public sealed class SteamworksCloudStorageProvider : SteamworksProviderBase, ICloudStorageProvider
{
    public string Name => "Steam Cloud";

    public bool IsCloudEnabled
    {
        get
        {
            EnsureAcquired();
            return SteamRemoteStorage.IsCloudEnabledForApp() &&
                   SteamRemoteStorage.IsCloudEnabledForAccount();
        }
    }

    public Task<bool> ExistsAsync(string fileName, CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();
        return Task.FromResult(SteamRemoteStorage.FileExists(fileName));
    }

    public Task<byte[]> ReadAsync(string fileName, CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();

        if (!SteamRemoteStorage.FileExists(fileName))
            throw new FileNotFoundException($"Steam Cloud file '{fileName}' not found.");

        var size = SteamRemoteStorage.GetFileSize(fileName);
        var buf = new byte[size];
        var read = SteamRemoteStorage.FileRead(fileName, buf, size);
        if (read != size)
            throw new IOException($"Read {read} bytes, expected {size} for '{fileName}'.");
        return Task.FromResult(buf);
    }

    public Task<bool> WriteAsync(string fileName, byte[] data, CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();
        return Task.FromResult(SteamRemoteStorage.FileWrite(fileName, data, data.Length));
    }

    public Task<bool> DeleteAsync(string fileName, CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();
        return Task.FromResult(SteamRemoteStorage.FileDelete(fileName));
    }

    public Task<IReadOnlyList<CloudFileEntry>> ListAsync(CancellationToken ct = default)
    {
        EnsureAcquired();
        ct.ThrowIfCancellationRequested();

        var count = SteamRemoteStorage.GetFileCount();
        var list = new List<CloudFileEntry>(count);
        for (var i = 0; i < count; i++)
        {
            var name = SteamRemoteStorage.GetFileNameAndSize(i, out var size);
            list.Add(new CloudFileEntry(name, size));
        }
        return Task.FromResult<IReadOnlyList<CloudFileEntry>>(list);
    }
}
