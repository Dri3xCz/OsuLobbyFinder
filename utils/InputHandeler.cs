﻿namespace OsuMultiplayerLobbyFinder
{
    public class InputHandeler
    {
        public InputHandeler() { }

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
    }
}
