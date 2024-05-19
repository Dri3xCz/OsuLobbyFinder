using OsuMultiplayerLobbyFinder.models;

namespace OsuMultiplayerLobbyFinder.feature
{
    public class UserFinder
    {
        private readonly IApi _api;

        public UserFinder(IApi api)
        {
            this._api = api;
        }

        public async Task<List<UserModel>> GetUsersFromLobby(LobbyModel lobby)
        {
            List<string> knownUserIds = new List<string>();

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

            List<UserModel> userModels = new List<UserModel>();

            foreach (string userId in knownUserIds)
            {
                var ExceptionOrUser = await GetUserModelById(Convert.ToInt32(userId));
                ExceptionOrUser.Fold(
                    (_) => { },
                    (user) => { userModels.Add(user); }
                );
            }

            return userModels;
        }

        private async Task<Either<Exception, UserModel>> GetUserModelById(int id)
        {
            return await _api.UserById(id);
        }
    }
}
