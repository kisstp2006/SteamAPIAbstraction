namespace SteamOwnershipExample.Core;

public interface IOwnershipProvider : IAsyncDisposable
{
    string Name { get; }

    Task<OwnershipResult> CheckAsync(AppId appId, CancellationToken ct = default);
}
