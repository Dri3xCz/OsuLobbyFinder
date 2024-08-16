using Microsoft.Toolkit.Uwp.Notifications;
using OsuMultiplayerLobbyFinder.models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using OsuMultiplayerLobbyFinder.feature.api;
using OsuMultiplayerLobbyFinder.feature.finders;

namespace OsuMultiplayerLobbyFinder;

class Program
{
    const string ConfigPath = "./config.json";

    public static async Task Main()
    {
        var inputHandler = new InputHandler();
        IApi api = new OsuApi();
        string apiKey = await HandleApiKeyConfig(inputHandler, api);
        api.ApiKey = apiKey;


        Console.Write("ID of first lobby: ");
        var id = inputHandler.ReadInt();
        Console.Write("Time to live: ");
        var ttl = inputHandler.ReadInt();
        Console.Write("Name of lobby: ");
        var name = inputHandler.ReadString();

        var lobbyFinder = new LobbyFinder(api);
        FindLobbyParameters parameters = new FindLobbyParameters(
            id,
            ttl,
            name
        );
  
        int result = await lobbyFinder.FindLobbyUntilFound(parameters);
        Console.WriteLine("Successful find!: " + result);

        Console.WriteLine("\nPlayers:\n");

        var lobbyOrException = await api.LobbyById(result);
        lobbyOrException.Fold(
            Console.WriteLine,
            (lobby) => { HandleUsers(lobby, api); }
        );

        new ToastContentBuilder()
            .AddText("OsuLobbyFinder")
            .AddText("Lobby found: " + result)
            .Show(toast =>
            {
                toast.ExpirationTime = DateTime.Now.AddSeconds(5);
            });

        // Platform dependent as fuck
        string url = $"https://osu.ppy.sh/community/matches/{result}";
        OpenBrowser(url);

        Console.ReadLine();
    }

    static async void HandleUsers(LobbyModel lobby, IApi api)
    { 
        UserFinder userFinder = new UserFinder(api);
        List<UserModel> users = await userFinder.GetUsersFromLobby(lobby);
        List<string> ids = new List<string>();
        List<string> usernames = new List<string>();

        foreach (UserModel user in users)
        {
            Console.WriteLine($"{user.user_id} {user.username}");
            ids.Add(user.user_id);
            usernames.Add(user.username);
        }

        Console.WriteLine("\nDifferent format:\n");

        foreach (string id in ids)
        {
            Console.WriteLine(id);
        }
        foreach (string username in usernames)
        {
            Console.WriteLine(username);
        }
    }

    private static async Task<string> HandleApiKeyConfig(InputHandler inputHandler, IApi api)
    {
        if (!File.Exists(ConfigPath))
        {
            return await HandleNoConfig(inputHandler, api);
        } else
        {
            string json = await File.ReadAllTextAsync(ConfigPath);
            string? key = JsonSerializer.Deserialize<ConfigModel>(json)?.ApiKey;
            if (key != null)
                return key;

            throw new Exception("API key in config file is not working");
        }   
    }

    private static async Task<string> HandleNoConfig(InputHandler inputHandler, IApi api)
    {
        Console.WriteLine("It seems you didn't set API key yet!");
        Console.WriteLine("Enter your api key or type GETAPI to open osu api page");

        string key;

        while (true)
        {
            string input = inputHandler.ReadString();

            if (input == "GETAPI")
            {
                string url = "https://osu.ppy.sh/p/api";
                OpenBrowser(url);
            }
            else
            {
                if (await api.ApiKeyIsValid(input))
                {
                    WriteKeyToConfig(input);
                    key = input;
                    break;
                }
                else
                {
                    Console.WriteLine("API key doesn't seem to work, try again.");
                }
            }
        }

        Console.Clear();
        return key;
    }

    private static void WriteKeyToConfig(string key)    
    {
        var fs = File.Create(ConfigPath);
        var sw = new StreamWriter(fs);
        var jsonObject = new JsonObject
        {
            { "ApiKey", key }
        };
        sw.Write(jsonObject);

        sw.Close();
        fs.Close();
    }

    static void OpenBrowser(string url)
    {
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
    }
}
