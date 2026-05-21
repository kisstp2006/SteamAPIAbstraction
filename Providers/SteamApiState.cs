using Steamworks;

namespace SteamOwnershipExample.Providers;

// Reference-counted SteamAPI lifetime. Multiple providers can be alive at once;
// SteamAPI.Init() runs on the first Acquire, SteamAPI.Shutdown() on the last Release.
internal static class SteamApiState
{
    private static int _refCount;
    private static readonly object _gate = new();

    public static void Acquire()
    {
        lock (_gate)
        {
            if (_refCount == 0)
            {
                if (!Packsize.Test())
                    throw new InvalidOperationException(
                        "Steamworks.NET pack-size mismatch — 32/64-bit DLL and build platform are not aligned.");

                if (!DllCheck.Test())
                    throw new InvalidOperationException(
                        "Steamworks.NET DLL check failed — steam_api64.dll is missing or has the wrong version.");

                if (!SteamAPI.Init())
                    throw new InvalidOperationException(
                        "SteamAPI.Init() failed. Is the Steam client running? Are you logged in? Is steam_appid.txt present in the working directory?");
            }
            _refCount++;
        }
    }

    public static void Release()
    {
        lock (_gate)
        {
            if (_refCount == 0)
                return;

            _refCount--;
            if (_refCount == 0)
                SteamAPI.Shutdown();
        }
    }

    public static void RunCallbacks() => SteamAPI.RunCallbacks();
}
