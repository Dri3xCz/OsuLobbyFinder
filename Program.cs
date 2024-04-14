namespace OsuMultiplayerLobbyFinder;


class Program
{
    
    public async static Task Main()
    {
        InputHandeler inputHandeler = new InputHandeler();

        Console.Write("ID of first lobby: ");
        int id = inputHandeler.ReadInt();
        Console.Write("Time to live: ");
        int ttl = inputHandeler.ReadInt();
        Console.Write("Name of lobby: ");
        string name = inputHandeler.ReadString();

        var lobbyFinder = new LobbyFinder();
        FindLobbyParameters parameters = new FindLobbyParameters(
            id,
            ttl,
            name
        );
  
        int result = await lobbyFinder.FindLobbyUntilFound(parameters);
        Console.WriteLine("Succesfull find!: " + result);
        
        Console.ReadLine();
    }
}
