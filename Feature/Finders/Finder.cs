using OsuMultiplayerLobbyFinder.Feature.Api;

namespace OsuMultiplayerLobbyFinder.Feature.Finders;

public abstract class Finder
{
    protected readonly IApi Api;

    protected Finder(IApi api)
    {
        Api = api;
    }
}