using OsuMultiplayerLobbyFinder.Models.Lobby;

namespace OsuMultiplayerLobbyFinder.Feature.Matcher
{
    public interface ILobbyMatcher
    {
        public bool MatchLobby(Lobby lobby, string? lobbyName, string? playerId);
    }
}
