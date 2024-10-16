using System.Net;
using OsuMultiplayerLobbyFinder.Feature.Api;
using OsuMultiplayerLobbyFinder.Feature.Matcher;
using OsuMultiplayerLobbyFinder.Models.Lobby;
using OsuMultiplayerLobbyFinder.Utils;

namespace OsuMultiplayerLobbyFinder.Feature.Finders
{
    // FIXME: Make this as a dependency injection and add service locator
    public class LobbyFinder(IApi api) : Finder(api)
    {
        private readonly ILobbyMatcher _lobbyMatcher = new LobbyMatcher();

        public async Task<int> FindLobbyUntilFound(FindLobbyParameters parameters)
        {
            while (true)
            {
                var resultId = await FindLobby(parameters);
                if (resultId != 0)
                    return resultId;

                Console.WriteLine($"Lobby not found, continue searching for {parameters.TimeToLive} more?");
                
                string input = (Console.ReadLine() ?? "n").Trim().ToLower();
                bool exit = input is "n" or "no";
                if (exit)
                {
                    Environment.Exit(0);
                }
                
                parameters.StartLobbyId -= parameters.TimeToLive;
            }    
        }

        private async Task<int> FindLobby(FindLobbyParameters parameters)
        {
            for (int i = 0; i < parameters.TimeToLive; i++)
            {
                int lobbyId = parameters.StartLobbyId - i;
                Console.Title = $"Fetching: {lobbyId}";
                
                Either<Exception, Lobby> response = await Api.LobbyById(lobbyId);

                Lobby? lobby = null;
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

        private static void HandleFindLobbyExceptions(Exception e, int lobbyId)
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
}
