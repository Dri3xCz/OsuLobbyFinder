namespace OsuMultiplayerLobbyFinder.Models.Lobby;

public class Game
{
    public string game_id { get; set; }
    public string start_time { get; set; }
    public string end_time { get; set; }
    public string beatmap_id { get; set; }
    public string play_mode { get; set; }
    public string match_type { get; set; }
    public string scoring_type { get; set; }
    public string team_type { get; set; }
    public string mods { get; set; }
    public Score[] scores { get; set; }
}