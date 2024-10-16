using OsuMultiplayerLobbyFinder.Feature.Api;
using OsuMultiplayerLobbyFinder.Models;
using OsuMultiplayerLobbyFinder.Models.Lobby;
using OsuMultiplayerLobbyFinder.Utils;

namespace OsuMultiplayerLobbyFinder.Feature.Finders;

public class UserFinder(IApi api) : Finder(api)
{
    public async Task<List<User>> GetUsersFromLobby(Lobby lobby)
    {
        var users = new Dictionary<string, User>();

        foreach (var game in lobby.games)
        {
            foreach (string userId in game.scores.Select(score => score.user_id))
            {
                if (users.ContainsKey(userId))
                {
                    continue;
                }

                var exceptionOrUser = await Api.UserById(userId);
                exceptionOrUser.Fold(
                    (ex) => { Console.WriteLine($"Unable to get user {userId}: {ex.Message}"); },
                    (user) => { users.Add(userId, user); }
                );
            }
        }

        return users.Values.ToList();
    }
}