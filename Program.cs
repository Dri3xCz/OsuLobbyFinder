using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;

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
        await STATask.Run(() => Clipboard.SetText(result.ToString()));

        new ToastContentBuilder()
            .AddText("OsuLobbyFinder")
            .AddText("Lobby found: " + result)
            .Show(toast =>
            {
                toast.ExpirationTime = DateTime.Now.AddSeconds(5);
            });

        // Platform dependent as fuck
        string url = $"https://osu.ppy.sh/community/matches/{result}";
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));

        Console.ReadLine();
    }
}
