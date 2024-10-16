using System.Text.Json;

namespace OsuMultiplayerLobbyFinder.Utils
{
    public class JsonSerializerUtil
    {
        public static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}
