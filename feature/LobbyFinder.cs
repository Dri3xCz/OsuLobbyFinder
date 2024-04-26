using HtmlAgilityPack;
using OsuMultiplayerLobbyFinder.feature;
using OsuMultiplayerLobbyFinder.models;
using System.Net;

namespace OsuMultiplayerLobbyFinder
{
    public class LobbyFinder
    {
        private Api api;

        public LobbyFinder(Api api) 
        {
            this.api = api;
        }

        public async Task<int> FindLobbyUntilFound(FindLobbyParameters parameters)
        {
            while (true)
            {
                int res = await _FindLobby(parameters);
                if (res != 0)
                    return res;

                Console.WriteLine($"Lobby not found, continue searching for {parameters.timeToLive} more?");
                Console.ReadLine();
                parameters.startLobbyId -= parameters.timeToLive;
            }    
        }

        private async Task<int> _FindLobby(FindLobbyParameters parameters)
        {
            for (int i = 0; i < parameters.timeToLive; i++)
            {
                int lobbyId = parameters.startLobbyId - i;
                Console.Title = $"Fetching: {lobbyId}";
                
                Either<Exception, LobbyModel> response = await api.lobbyById(lobbyId);

                LobbyModel? lobby = null;
                response.Fold(
                    (ex) => { _HandleFindLobbyExceptions(ex, lobbyId); },
                    (value) => { lobby = value; }
                );

                if (lobby == null) continue;

                if (_MatchPattern(lobby.match.name, parameters.namePattern))
                    return lobbyId;

                Thread.Sleep(750);
            }
            return 0;
        }

        private bool _MatchPattern(string tested, string matcher)
        {
            return tested.Contains(matcher);
        }

        private void _HandleFindLobbyExceptions(Exception e, int lobbyId)
        {
            if (e.GetType() == typeof(HttpRequestException))
            {
                switch ((e as HttpRequestException).StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine($"Fetch failed - Unauthorized access, Lobby id: {lobbyId}");
                        break;
                    default:
                        Console.WriteLine($"Fetch failed - {e.Message} Lobby id: {lobbyId}");
                        break;
                }
                return;
            }
            Console.WriteLine($"Fetch failed! Lobby id: {lobbyId}");
        }
    }

    public struct FindLobbyParameters
    {
        public int startLobbyId;
        public int timeToLive;
        public string namePattern;

        public FindLobbyParameters(int startLobbyId, int timeToLive, string namePattern)
        {
            this.startLobbyId = startLobbyId;
            this.timeToLive = timeToLive;
            this.namePattern = namePattern;
        }
    }
}
