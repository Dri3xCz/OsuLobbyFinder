using OsuMultiplayerLobbyFinder.feature.api;

namespace OsuMultiplayerLobbyFinder.feature.finders;

public abstract class Finder
{
    protected readonly IApi Api;

    protected Finder(IApi api)
    {
        this.Api = api;
    }
}