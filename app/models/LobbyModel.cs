public class LobbyModel
{
    public MatchInfo match { get; set; }
    public Game[] games { get; set; }
}

public class MatchInfo
{
    public string match_id { get; set; }
    public string name { get; set; }
    public string start_time { get; set; }
    public string? end_time { get; set; }
}

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

public class Score
{
    public string slot { get; set; }
    public string team { get; set; }
    public string user_id { get; set; }
    public string score { get; set; }
    public string maxcombo { get; set; }
    public string rank { get; set; }
    public string count50 { get; set; }
    public string count100 { get; set; }
    public string count300 { get; set; }
    public string countmiss { get; set; }
    public string countgeki { get; set; }
    public string countkatu { get; set; }
    public string perfect { get; set; }
    public string pass { get; set; }
    public string enabled_mods { get; set; }
}
