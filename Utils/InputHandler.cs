namespace OsuMultiplayerLobbyFinder.Utils
{
    public class InputHandler
    {
        public InputHandler() { }

        public int ReadInt()
        {
            while (true)
            {
                try
                {
                    return Convert.ToInt32(Console.ReadLine());
                } catch
                {
                    Console.WriteLine("Incorrect value, try again!");
                }
            }
        }

        public string ReadString()
        {
            string text = Console.ReadLine() ?? "";
            return text.Trim();
        }

        public string? ReadStringOrNull()
        {
            string? text = Console.ReadLine() ?? "";

            return text != "" ? text.Trim() : null;
        }
    }
}
