namespace OsuMultiplayerLobbyFinder.Feature.Finders;

public struct FindLobbyParameters(int startLobbyId, int timeToLive, string? namePattern, string? playerId)
{
    public int StartLobbyId { get; set; } = startLobbyId;
    public int TimeToLive { get; init; } = timeToLive;
    public string? NamePattern { get; init; } = namePattern;
    public string? PlayerId { get; init; } = playerId;
}