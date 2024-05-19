using System.Text.Json;

namespace OsuMultiplayerLobbyFinder.utils
{
    public class JsonSerializerUtil
    {
        public static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}
