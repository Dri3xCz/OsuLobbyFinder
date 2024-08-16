using System.Text.Json;
using OsuMultiplayerLobbyFinder.models;
using OsuMultiplayerLobbyFinder.utils;

namespace OsuMultiplayerLobbyFinder.feature.api
{
    public class OsuApi : IApi
    {
        private string? _apiKey;

        public string ApiKey
        {
            get
            {
                if (_apiKey == null)
                    throw new NullReferenceException("ApiKey used before it was set");
                return _apiKey;
            }
            set => _apiKey = value;
        }

        private readonly HttpClient _client = new();

        public async Task<bool> ApiKeyIsValid(string apiKey)
        { 
            var uri = new UriBuilder("https://osu.ppy.sh/api/get_user_recent")
            {
                Query = $"k={apiKey}"
            };
            var query = uri.ToString();

            Either<Exception, object> response = await GetAsync<object>(query);

            var isValid = false;
            response.Fold(
                Console.WriteLine,
                _ => { isValid = true; }
            );

            return isValid;
        }

        public async Task<Either<Exception, LobbyModel>> LobbyById(int id)
        {
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_match")
            {
                Query = $"k={ApiKey}&mp={id}"
            };
            var query = uriBuilder.ToString();

            return await GetAsync<LobbyModel>(query);
        }

        public async Task<Either<Exception, UserModel>> UserById(int id)
        {
            var uriBuilder = new UriBuilder("https://osu.ppy.sh/api/get_user")
            {
                Query = $"k={ApiKey}&u={id}"
            };
            var query = uriBuilder.ToString();

            return await GetAsync<UserModel>(query);
        }

        private async Task<Either<Exception, T>> GetAsync<T>(string query)
        {
            try
            {
                var response = await _client.GetStringAsync(query);
                if (response[0] == '[' && response.Length > 2)
                {
                    response = response.Substring(1, response.Length - 2);
                }
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
