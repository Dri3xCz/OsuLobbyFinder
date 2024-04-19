namespace OsuMultiplayerLobbyFinder.models
{
    public class ConfigModel
    {
        public ConfigModel(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; set; }
    }
}
