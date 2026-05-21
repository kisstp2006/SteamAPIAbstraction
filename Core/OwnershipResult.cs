namespace SteamOwnershipExample.Core;

public sealed record OwnershipResult(
    AppId AppId,
    bool IsOwned,
    string ProviderName,
    string? Notes = null);
