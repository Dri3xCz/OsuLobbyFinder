using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMultiplayerLobbyFinder.models
{
    public class LobbyModel
    {
        public LobbyModel(Lobby lobby)
        {
            Lobby = lobby;
        }

        public Lobby Lobby { get; set; }
    }

    public class Lobby
    {
        public Lobby(int matchId, string name, DateTime startTime, DateTime endTime)
        {
            MatchId = matchId;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int MatchId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
