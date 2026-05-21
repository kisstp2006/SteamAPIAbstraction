using System.Text;
using SteamOwnershipExample.Core;
using SteamOwnershipExample.Providers;

namespace SteamOwnershipExample.Samples;

public static class Sample08_CloudStorage
{
    private const string DemoFileName = "SteamOwnershipExample_demo.txt";

    public static async Task RunAsync()
    {
        Console.WriteLine("[8] Steam Cloud");
        Console.WriteLine("    Round-trip: write -> list -> read -> delete.");
        Console.WriteLine();

        await using ICloudStorageProvider cloud = new SteamworksCloudStorageProvider();

        if (!cloud.IsCloudEnabled)
        {
            Console.WriteLine("    Cloud is disabled for this app or account — nothing to do.");
            return;
        }

        var payload = $"hello from SteamOwnershipExample @ {DateTime.UtcNow:O}";
        var bytes = Encoding.UTF8.GetBytes(payload);

        Console.WriteLine($"    Writing  '{DemoFileName}' ({bytes.Length} bytes)...");
        var wrote = await cloud.WriteAsync(DemoFileName, bytes);
        Console.WriteLine($"    Write    : {(wrote ? "OK" : "FAILED")}");
        Console.WriteLine();

        var list = await cloud.ListAsync();
        Console.WriteLine($"    Cloud file count: {list.Count}");
        foreach (var entry in list.Take(5))
            Console.WriteLine($"      {entry.SizeBytes,8} bytes  {entry.Name}");
        if (list.Count > 5)
            Console.WriteLine($"      ... and {list.Count - 5} more");
        Console.WriteLine();

        var read = Encoding.UTF8.GetString(await cloud.ReadAsync(DemoFileName));
        Console.WriteLine($"    Read back: \"{read}\"");
        Console.WriteLine();

        var deleted = await cloud.DeleteAsync(DemoFileName);
        Console.WriteLine($"    Delete   : {(deleted ? "OK" : "FAILED")}");
    }
}
