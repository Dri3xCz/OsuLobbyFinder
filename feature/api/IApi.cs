using OsuMultiplayerLobbyFinder.models;
using OsuMultiplayerLobbyFinder.utils;

namespace OsuMultiplayerLobbyFinder.feature.api
{
    public interface IApi
    {
        public string ApiKey { get; set; }

        public Task<bool> ApiKeyIsValid(string apiKey);

        public Task<Either<Exception, LobbyModel>> LobbyById(int id);

        public Task<Either<Exception, UserModel>> UserById(int id);
    }
}
