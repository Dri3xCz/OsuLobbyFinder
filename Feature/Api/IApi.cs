using OsuMultiplayerLobbyFinder.Models;
using OsuMultiplayerLobbyFinder.Models.Lobby;
using OsuMultiplayerLobbyFinder.Utils;

namespace OsuMultiplayerLobbyFinder.Feature.Api
{
    public interface IApi
    {
        public string ApiKey { get; set; }

        public Task<bool> ApiKeyIsValid(string apiKey);

        public Task<Either<Exception, Lobby>> LobbyById(int id);

        public Task<Either<Exception, User>> UserById(int id);
    }
}
