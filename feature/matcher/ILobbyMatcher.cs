namespace OsuMultiplayerLobbyFinder.feature.matcher
{
    public interface ILobbyMatcher
    {
        public bool MatchLobby(LobbyModel lobby, string? lobbyName, string? playerId);
    }
}
