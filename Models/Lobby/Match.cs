namespace OsuMultiplayerLobbyFinder.Models.Lobby;

public class Match
{
    public string match_id { get; set; }
    public string name { get; set; }
    public string start_time { get; set; }
    public string? end_time { get; set; }
}