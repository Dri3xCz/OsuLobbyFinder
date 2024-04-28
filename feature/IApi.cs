namespace OsuMultiplayerLobbyFinder.feature
{
    public interface IApi
    {
        public string ApiKey { get; set; }

        public Task<bool> ApiKeyIsValid(string apiKey);

        public Task<Either<Exception, LobbyModel>> LobbyById(int id);
    }
}
