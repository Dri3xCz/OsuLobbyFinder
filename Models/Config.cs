namespace OsuMultiplayerLobbyFinder.Models
{
    public class Config(string apiKey)
    {
        public string ApiKey { get; set; } = apiKey;
    }
}
