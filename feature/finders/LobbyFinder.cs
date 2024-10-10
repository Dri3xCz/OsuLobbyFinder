using System.Net;
using OsuMultiplayerLobbyFinder.feature.api;
using OsuMultiplayerLobbyFinder.feature.matcher;
using OsuMultiplayerLobbyFinder.utils;

namespace OsuMultiplayerLobbyFinder.feature.finders
{
    public class LobbyFinder : Finder
    {
        private ILobbyMatcher _lobbyMatcher;

        public LobbyFinder(IApi api) : base(api) 
        {
            // FIXME: Make this as a dependency injection and add service locator
            _lobbyMatcher = new LobbyMatcher();
        }
        
        public async Task<int> FindLobbyUntilFound(FindLobbyParameters parameters)
        {
            while (true)
            {
                var resultId = await FindLobby(parameters);
                if (resultId != 0)
                    return resultId;

                Console.WriteLine($"Lobby not found, continue searching for {parameters.TimeToLive} more?");
                Console.ReadLine();
                parameters.StartLobbyId -= parameters.TimeToLive;
            }    
        }

        private async Task<int> FindLobby(FindLobbyParameters parameters)
        {
            for (int i = 0; i < parameters.TimeToLive; i++)
            {
                int lobbyId = parameters.StartLobbyId - i;
                Console.Title = $"Fetching: {lobbyId}";
                
                Either<Exception, LobbyModel> response = await Api.LobbyById(lobbyId);

                LobbyModel? lobby = null;
                response.Fold(
                    (ex) => { HandleFindLobbyExceptions(ex, lobbyId); },
                    (value) => { lobby = value; }
                );

                if (lobby == null) continue;

                if (_lobbyMatcher.MatchLobby(lobby, parameters.NamePattern, parameters.PlayerId))
                    return lobbyId;

                Thread.Sleep(750);
            }
            return 0;
        }

        private void HandleFindLobbyExceptions(Exception e, int lobbyId)
        {
            if (e is HttpRequestException httpRequestException)
            {
                switch (httpRequestException.StatusCode)
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
        public int StartLobbyId;
        public readonly int TimeToLive;
        public readonly string? NamePattern;
        public readonly string? PlayerId;

        public FindLobbyParameters(int startLobbyId, int timeToLive, string? namePattern, string? playerId)
        {
            this.StartLobbyId = startLobbyId;
            this.TimeToLive = timeToLive;
            this.NamePattern = namePattern;
            this.PlayerId = playerId;
        }
    }
}
