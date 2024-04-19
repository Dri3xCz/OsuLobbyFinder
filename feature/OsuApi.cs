using OsuMultiplayerLobbyFinder.models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OsuMultiplayerLobbyFinder.feature
{
    public interface Api
    {
        public Task<bool> apiKeyIsValid(string apiKey);
    }

    public class OsuApi : Api
    {
        public async Task<bool> apiKeyIsValid(string apiKey)
        {
            var client = new HttpClient();
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_user_recent");
            uriBuilder.Query = $"k={apiKey}";
            var query = uriBuilder.ToString();
     
            try
            {
                string result = await client.GetStringAsync(query);
            } catch
            {
                return false;
            }

            return true;
        }
    }
}
