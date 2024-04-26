namespace OsuMultiplayerLobbyFinder.utils
{
    public class DateTimeConverter
    {
        public DateTime OsuApiTimeToDateTime(string value)
        {
            int year = Convert.ToInt32(value.Substring(0, 4));
            int month = Convert.ToInt32(value.Substring(5, 2));
            int day = Convert.ToInt32(value.Substring(8, 2));
            int hour = Convert.ToInt32(value.Substring(11, 2));
            int minute = Convert.ToInt32(value.Substring(14, 2));
            int second = Convert.ToInt32(value.Substring(17, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
