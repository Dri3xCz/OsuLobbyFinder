using OsuMultiplayerLobbyFinder.feature.api;
using OsuMultiplayerLobbyFinder.models;
using OsuMultiplayerLobbyFinder.utils;

namespace OsuMultiplayerLobbyFinder.feature.finders
{
    public class UserFinder : Finder
    {
        public UserFinder(IApi api) : base(api) {}
        
        public async Task<List<UserModel>> GetUsersFromLobby(LobbyModel lobby)
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

            var userModels = new List<UserModel>();

            foreach (string userId in knownUserIds)
            {
                var convertedUserId = Convert.ToInt32(userId);
                
                var exceptionOrUser = await GetUserModelById(convertedUserId);
                exceptionOrUser.Map(
                    (user) => { userModels.Add(user); }
                );
            }

            return userModels;
        }

        private async Task<Either<Exception, UserModel>> GetUserModelById(int id)
        {
            return await Api.UserById(id);
        }
    }
}
