using SteamOwnershipExample.Core;

namespace SteamOwnershipExample.Samples;

public static class Sample03_CustomProvider
{
    public static async Task RunAsync()
    {
        Console.WriteLine("[3] Custom IOwnershipProvider implementation");
        Console.WriteLine("    Mock provider with a hardcoded set — same pattern works for EGS / GOG / itch.io / your backend.");
        Console.WriteLine("    No Steam client required.");
        Console.WriteLine();

        await using IOwnershipProvider provider = new MockProvider(
            KnownApps.FiveNightsAtFreddys1,
            new AppId(620),
            new AppId(440));

        AppId[] toCheck =
        [
            KnownApps.FiveNightsAtFreddys1, // owned
            new AppId(620),                 // owned
            new AppId(99999)                // not owned
        ];

        foreach (var id in toCheck)
            SampleHelpers.PrintResult(await provider.CheckAsync(id));
    }

    private sealed class MockProvider(params AppId[] ownedAppIds) : IOwnershipProvider
    {
        private readonly HashSet<AppId> _owned = [..ownedAppIds];

        public string Name => "Mock";

        public Task<OwnershipResult> CheckAsync(AppId appId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(new OwnershipResult(
                appId, _owned.Contains(appId), Name, "in-memory mock"));
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
