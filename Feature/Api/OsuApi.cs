using System.Text.Json;
using OsuMultiplayerLobbyFinder.Models;
using OsuMultiplayerLobbyFinder.Models.Lobby;
using OsuMultiplayerLobbyFinder.Utils;

namespace OsuMultiplayerLobbyFinder.Feature.Api
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
            var uri = new UriBuilder(OsuUrlConstants.GetUserRecent)
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

        public async Task<Either<Exception, Lobby>> LobbyById(int id)
        {
            var uriBuilder = new UriBuilder(OsuUrlConstants.GetMatch)
            {
                Query = $"k={ApiKey}&mp={id}"
            };
            var query = uriBuilder.ToString();

            return await GetAsync<Lobby>(query);
        }

        public async Task<Either<Exception, User>> UserById(int id)
        {
            var uriBuilder = new UriBuilder(OsuUrlConstants.GetUser)
            {
                Query = $"k={ApiKey}&u={id}"
            };
            var query = uriBuilder.ToString();

            return await GetAsync<User>(query);
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
