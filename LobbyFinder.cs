using HtmlAgilityPack;
using System;

namespace OsuMultiplayerLobbyFinder
{
    public class LobbyFinder
    {
        public LobbyFinder() { }

        public async Task<int> FindLobby(FindLobbyParameters parameters)
        {
            var httpClient = new HttpClient();
            var htmlDocument = new HtmlDocument();
            for (int i = 0; i < parameters.timeToLive; i++)
            {
                int matchId = parameters.startLobbyId - i;
                try
                {
                    var html = await httpClient.GetStringAsync(
                        $"https://osu.ppy.sh/community/matches/{matchId}"
                    );
                    htmlDocument.LoadHtml(html);

                    string lobbyName = GetLobbyName(htmlDocument);

                    if (MatchPattern(lobbyName, parameters.namePattern))
                    {
                        return matchId;
                    }
                } catch
                {
                    Console.WriteLine("Fetch failed! MatchId: " + matchId);
                }

                // Just for safety, the delay between fetches should be enough, but if this gets blocked, increase the delay
                Thread.Sleep(500);
            }

            return 0;
        }

        private string GetLobbyName(HtmlDocument html)
        {
            return html.DocumentNode.SelectNodes("//title").First().InnerHtml;
        }

        private bool MatchPattern(string tested, string matcher)
        {
            return tested.Contains(matcher);
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
