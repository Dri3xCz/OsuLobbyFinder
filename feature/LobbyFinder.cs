using HtmlAgilityPack;
using System;
using System.Net;

namespace OsuMultiplayerLobbyFinder
{
    public class LobbyFinder
    {
        public LobbyFinder() { }

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
            var httpClient = new HttpClient();
            var htmlDocument = new HtmlDocument();
            for (int i = 0; i < parameters.timeToLive; i++)
            {
                int lobbyId = parameters.startLobbyId - i;
                try
                {
                    var html = await httpClient.GetStringAsync(
                        $"https://osu.ppy.sh/community/matches/{lobbyId}"
                    );
                    htmlDocument.LoadHtml(html);

                    string lobbyName = _GetLobbyName(htmlDocument);

                    if (_MatchPattern(lobbyName, parameters.namePattern))
                    {
                        return lobbyId;
                    }
                } catch (Exception e)
                {
                    _HandleFindLobbyExceptions(e, lobbyId, i);
                }
                Thread.Sleep(250);
            }
            return 0;
        }

        private string _GetLobbyName(HtmlDocument html)
        {
            return html.DocumentNode.SelectNodes("//title").First().InnerHtml;
        }

        private bool _MatchPattern(string tested, string matcher)
        {
            return tested.Contains(matcher);
        }

        private void _HandleFindLobbyExceptions(Exception e, int lobbyId, int iterator)
        {
            if (e.GetType() == typeof(HttpRequestException))
            {
                switch ((e as HttpRequestException).StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine($"Fetch failed - Unauthorized access, Lobby id: {lobbyId}");
                        break;
                    case HttpStatusCode.TooManyRequests:
                        Console.WriteLine($"Fetch failed - Too many requests, waiting for 5 seconds");
                        iterator--;
                        Thread.Sleep(5000);
                        Console.WriteLine("Resumed fetching");
                        break;
                    default:
                        Console.WriteLine($"Fetch failed - {e.Message} Lobby id: {lobbyId}");
                        break;
                }

                return;
            }

            Console.WriteLine($"Fetch failed! NotHttpRequestExcpetion, Error message: {e.Message} Lobby id: {lobbyId}");
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
