namespace OsuMultiplayerLobbyFinder.models
{
    public class ResponseModel<T>
    {
        public ResponseModel(T response)
        {
            Response = response;
        }
        public ResponseModel(Exception exception)
        {
            Exception = exception;
        }

        public T? Response { get; set; }
        public Exception? Exception { get; set; }
    }
}
