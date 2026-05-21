namespace SteamOwnershipExample.Core;

public readonly record struct AppId(uint Value)
{
    public override string ToString() => Value.ToString();

    public static implicit operator uint(AppId id) => id.Value;
}
