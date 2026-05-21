using System.Net.Http.Json;
using System.Text.Json.Serialization;
using SteamOwnershipExample.Core;

namespace SteamOwnershipExample.Providers;

public sealed class SteamWebApiOwnershipProvider : IOwnershipProvider
{
    private const string BaseUrl = "https://api.steampowered.com";

    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly ulong _steamId;
    private readonly bool _ownsHttp;

    public string Name => "Steam Web API";

    public SteamWebApiOwnershipProvider(string apiKey, ulong steamId64, HttpClient? http = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        _apiKey = apiKey;
        _steamId = steamId64;
        _http = http ?? new HttpClient();
        _ownsHttp = http is null;
    }

    public async Task<OwnershipResult> CheckAsync(AppId appId, CancellationToken ct = default)
    {
        var url =
            $"{BaseUrl}/IPlayerService/GetOwnedGames/v0001/" +
            $"?key={_apiKey}" +
            $"&steamid={_steamId}" +
            $"&include_appinfo=false" +
            $"&include_played_free_games=true" +
            $"&appids_filter[0]={appId.Value}" +
            $"&format=json";

        var resp = await _http.GetFromJsonAsync<GetOwnedGamesResponse>(url, ct).ConfigureAwait(false);

        // Empty response.games means the profile is private or the SteamID does not exist.
        var games = resp?.Response?.Games;
        var owned = games is not null && games.Any(g => g.AppId == appId.Value);

        string? notes = null;
        if (resp?.Response is null)
            notes = "Empty top-level response — wrong API key?";
        else if (games is null)
            notes = "Empty games list — private profile or invalid SteamID64.";

        return new OwnershipResult(appId, owned, Name, notes);
    }

    public ValueTask DisposeAsync()
    {
        if (_ownsHttp)
            _http.Dispose();
        return ValueTask.CompletedTask;
    }

    private sealed class GetOwnedGamesResponse
    {
        [JsonPropertyName("response")]
        public ResponseBody? Response { get; set; }
    }

    private sealed class ResponseBody
    {
        [JsonPropertyName("game_count")]
        public int GameCount { get; set; }

        [JsonPropertyName("games")]
        public List<GameEntry>? Games { get; set; }
    }

    private sealed class GameEntry
    {
        [JsonPropertyName("appid")]
        public uint AppId { get; set; }
    }
}
