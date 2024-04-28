using OsuMultiplayerLobbyFinder.models;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OsuMultiplayerLobbyFinder.feature
{
    public class OsuApi : IApi
    {
        public string ApiKey { get; set; } = "";

        private HttpClient _client = new HttpClient();

        public async Task<bool> ApiKeyIsValid(string apiKey)
        { 
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_user_recent");
            uriBuilder.Query = $"k={apiKey}";
            var query = uriBuilder.ToString();

            Either<Exception, object> response = await GetAsync<object>(query);

            bool isValid = false;
            response.Fold(
                (_) => { },
                (_) => { isValid = true; }
            );

            return isValid;
        }

        public async Task<Either<Exception, LobbyModel>> LobbyById(int id)
        {
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_match");
            uriBuilder.Query = $"k={ApiKey}&mp={id}";
            var query = uriBuilder.ToString();

            return await GetAsync<LobbyModel>(query);
        }

        private async Task<Either<Exception, T>> GetAsync<T>(string query)
        {
            try
            {
                string response = await _client.GetStringAsync(query);
                T? value = JsonSerializer.Deserialize<T>(response);

                if (value == null)
                    throw new JsonException();
                
                return new Right<Exception, T>(value);

            } catch (Exception ex)
            {
                return new Left<Exception, T>(ex);
            }
        }
    }
}
