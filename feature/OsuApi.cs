using OsuMultiplayerLobbyFinder.models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OsuMultiplayerLobbyFinder.feature
{
    public interface Api
    {
        public Task<bool> apiKeyIsValid(string apiKey);

        public Task<LobbyModel> lobbyById(int id);
    }

    public class OsuApi : Api
    {
        HttpClient client = new HttpClient();

        public async Task<bool> apiKeyIsValid(string apiKey)
        {
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_user_recent");
            uriBuilder.Query = $"k={apiKey}";
            var query = uriBuilder.ToString();

            ResponseModel<object> response = await _GetAsync<object>(query);

            if (response.Exception != null)
                return false;

            return true;
        }

        public async Task<LobbyModel> lobbyById(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<ResponseModel<T>> _GetAsync<T>(string query)
        {
            try
            {
                string response = await client.GetStringAsync(query);
                T? value = JsonSerializer.Deserialize<T>(response);

                if (value == null)
                    throw new JsonException();
                
                return new ResponseModel<T>(value);

            } catch (Exception ex)
            {
                return new ResponseModel<T>(ex);
            }
        }
    }
}
