using OsuMultiplayerLobbyFinder.Feature.Api;
using OsuMultiplayerLobbyFinder.Models;
using OsuMultiplayerLobbyFinder.Models.Lobby;
using OsuMultiplayerLobbyFinder.Utils;

namespace OsuMultiplayerLobbyFinder.Feature.Finders
{
    public class UserFinder : Finder
    {
        public UserFinder(IApi api) : base(api) {}
        
        public async Task<List<User>> GetUsersFromLobby(Lobby lobby)
        {
            var knownUserIds = new List<string>();

            foreach (Game game in lobby.games)
            {
                foreach (Score score in game.scores)
                {
                    if (!knownUserIds.Contains(score.user_id)) 
                    {
                        knownUserIds.Add(score.user_id);
                    }
                }
            }

            var userList = new List<User>();

            foreach (string userId in knownUserIds)
            {
                var convertedUserId = Convert.ToInt32(userId);
                
                var exceptionOrUser = await GetUserById(convertedUserId);
                exceptionOrUser.Map(
                    (user) => { userList.Add(user); }
                );
            }

            return userList;
        }

        private async Task<Either<Exception, User>> GetUserById(int id)
        {
            return await Api.UserById(id);
        }
    }
}
