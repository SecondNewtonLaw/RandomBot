using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Spectre.Console;


namespace RandomBot;

class MainActivity
{
    internal static HttpClient HttpClient = new(new HttpClientHandler()
    {
        UseCookies = false,
        AllowAutoRedirect = true,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        UseProxy = false,
        SslProtocols = SslProtocols.Tls12
    });
    internal static DiscordSocketClient BotClient = new(new DiscordSocketConfig()
    {
        GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages,
        LogLevel = LogSeverity.Debug // YES BOY GIMME THAT FALLING TEXT!
    });
    private static string? Token { get; } = File.ReadLines($"{Environment.CurrentDirectory}/token.IGNORE").ElementAt(0);

    internal static async Task Main()
    {
        RandomBot.Modules.WebServer pingSRV = new(new(80, IPAddress.Any));
        Thread Listener = new(async () => await ListenConsole());
        AnsiConsole.MarkupLine($"[yellow underline bold]Loaded Token[/][white]:[/] [red]{Token?.FixMarkup()}[/]");
        BotClient.Log += async logInfo
        => await Task.Run(() => AnsiConsole.MarkupLine($"[yellow underline bold][[Discord.Net Library]][/] -> [grey underline italic]{logInfo.Message.FixMarkup()}[/]"));
        BotClient.SlashCommandExecuted += Handlers.HandleSlashCommandAsync;

        AnsiConsole.MarkupLine("[green][[INFO]] [italic]Starting[/] HTTP Server, [yellow bold]awaiting pings[/]...[/]");
        _ = pingSRV.StartServer();

        AnsiConsole.MarkupLine("[yellow][[WARN]] Replit Spams the Discord gateway, and [red underline bold]will cause a ratelimit[/], avoid this by adding an [red bold]artificial time-out[/] of [red]~3 Minutes[/][/]");
        await Task.Delay(3 * 1000 * 60); // Wait 3 Minutes
        AnsiConsole.MarkupLine("[green][[INFO]] [bold underline]Time-out completed![/] Bot runtime starting up...[/]");

        // Login & Startup
        await BotClient.LoginAsync(TokenType.Bot, Token, true);
        await BotClient.StartAsync();
        Listener.Start();
        await Task.Delay(-1); // Not Closing Moment.
    }

    internal static async Task ListenConsole()
    {
        CommandBuilder builder = new();
        while (true)
        {
            string? str = Console.ReadLine();

            // Skip iteration if null.
            if (str is null || str.Length <= 0)
                continue;

            switch (str)
            {
                case "buildcommand":
                    const ulong srId = 1001635282912809000;
                    AnsiConsole.MarkupLine($"[yellow bold underline][[Command Line Interface]][/] -> [green bold underline]Building commands [[SLASH]] for [yellow underline bold]{BotClient.GetGuild(srId).Name} ({srId})[/][/]");
                    await builder.BuildFor(BotClient.GetGuild(srId));
                    AnsiConsole.MarkupLine($"[yellow bold underline][[Command Line Interface]][/] -> [green bold underline]Command Building Completed![/] ✅");
                    break;
            }
        }
    }
}