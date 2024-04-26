using OsuMultiplayerLobbyFinder.models;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OsuMultiplayerLobbyFinder.feature
{
    public interface Api
    {
        public string ApiKey { get; set; }

        public Task<bool> apiKeyIsValid(string apiKey);

        public Task<Either<Exception, LobbyModel>> lobbyById(int id);
    }

    public class OsuApi : Api
    {
        public string ApiKey { get; set; } = "";

        HttpClient client = new HttpClient();

        public async Task<bool> apiKeyIsValid(string apiKey)
        { 
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_user_recent");
            uriBuilder.Query = $"k={apiKey}";
            var query = uriBuilder.ToString();

            Either<Exception, object> response = await _GetAsync<object>(query);

            bool isValid = false;
            response.Fold(
                (_) => { },
                (_) => { isValid = true; }
            );

            return isValid;
        }

        public async Task<Either<Exception, LobbyModel>> lobbyById(int id)
        {
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_match");
            uriBuilder.Query = $"k={ApiKey}&mp={id}";
            var query = uriBuilder.ToString();

            return await _GetAsync<LobbyModel>(query);
        }

        private async Task<Either<Exception, T>> _GetAsync<T>(string query)
        {
            try
            {
                string response = await client.GetStringAsync(query);
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
