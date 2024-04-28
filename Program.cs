using Microsoft.Toolkit.Uwp.Notifications;
using OsuMultiplayerLobbyFinder.feature;
using OsuMultiplayerLobbyFinder.models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OsuMultiplayerLobbyFinder;

class Program
{
    const string CONFIG_PATH = "./config.json";

    public async static Task Main()
    {
        InputHandler inputHandler = new InputHandler();
        IApi api = new OsuApi();
        string apiKey = await HandleAPIKeyConfig(inputHandler, api);
        api.ApiKey = apiKey;


        Console.Write("ID of first lobby: ");
        int id = inputHandler.ReadInt();
        Console.Write("Time to live: ");
        int ttl = inputHandler.ReadInt();
        Console.Write("Name of lobby: ");
        string name = inputHandler.ReadString();

        var lobbyFinder = new LobbyFinder(api);
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
        OpenBrowser(url);

        Console.ReadLine();
    }

    static async Task<string> HandleAPIKeyConfig(InputHandler inputHandler, IApi api)
    {
        if (!File.Exists(CONFIG_PATH))
        {
            return await HandleNoConfig(inputHandler, api);
        } else
        {
            string json = File.ReadAllText(CONFIG_PATH);
            string? key = JsonSerializer.Deserialize<ConfigModel>(json)?.ApiKey;
            if (key != null)
                return key;

            throw new Exception("API key in config file is not working");
        }   
    }

    static async Task<string> HandleNoConfig(InputHandler inputHandler, IApi api)
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

    static void WriteKeyToConfig(string key)    
    {
        var fs = File.Create(CONFIG_PATH);
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
