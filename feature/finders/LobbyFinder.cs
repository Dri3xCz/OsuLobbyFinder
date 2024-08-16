using System.Net;
using OsuMultiplayerLobbyFinder.feature.api;
using OsuMultiplayerLobbyFinder.utils;

namespace OsuMultiplayerLobbyFinder.feature.finders
{
    public class LobbyFinder : Finder
    {
        public LobbyFinder(IApi api) : base(api) {}
        
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

                if (lobby.match.name.Contains(parameters.NamePattern))
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
        public readonly string NamePattern;

        public FindLobbyParameters(int startLobbyId, int timeToLive, string namePattern)
        {
            this.StartLobbyId = startLobbyId;
            this.TimeToLive = timeToLive;
            this.NamePattern = namePattern;
        }
    }
}
