namespace OsuMultiplayerLobbyFinder.feature.matcher
{
    public class LobbyMatcher : ILobbyMatcher
    {
        public bool MatchLobby(LobbyModel lobby, string? lobbyName, string? playerId)
        {
            if (lobbyName != null)
            {
                return MatchByLobbyName(lobby, lobbyName);
            }

            if (playerId != null)
            {
                return MatchByPlayerId(lobby, playerId);
            }

            throw new Exception("LobbyMatcher: MatchLobby called without lobbyName and playerId");
        }

        private bool MatchByLobbyName(LobbyModel lobby, string lobbyName) 
        {
            return lobby.match.name.Contains(lobbyName);
        }

        private bool MatchByPlayerId(LobbyModel lobby, string playerId)
        {
            var knownUserIds = new List<string>();

            foreach (Game game in lobby.games)
            {
                foreach (Score score in game.scores)
                {
                    if (!knownUserIds.Contains(score.user_id))
                    {
                        knownUserIds.Add(score.user_id);
                    }
                }
            }

            return knownUserIds.Contains(playerId);
        }
    }
}
