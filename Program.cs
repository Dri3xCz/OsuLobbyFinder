namespace OsuMultiplayerLobbyFinder;

class Program
{
    public async static Task Main()
    {
        var LobbyFinder = new LobbyFinder();
        Console.Write("ID of first lobby: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.Write("Time to live: ");
        int ttl = Convert.ToInt32(Console.ReadLine());
        Console.Write("Name of lobby: ");
        string name = Console.ReadLine();

        FindLobbyParameters parameters = new FindLobbyParameters(
            id,
            ttl,
            name
        );

        int result = await LobbyFinder.FindLobby(parameters);
        Console.WriteLine("Succesfull find!: " + result);
        // 0 znamená že to nic nenašlo
    }
}
