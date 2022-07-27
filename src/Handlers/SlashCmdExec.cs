using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Spectre.Console;

namespace RandomBot;

public partial class Handlers
{
    public static async Task HandleSlashCommandAsync(SocketSlashCommand cmdSket)
    {
        AnsiConsole.MarkupLine($"[green][[INFO]][/] Slash Command Executed '[green]{cmdSket.Data.Name}[/]' on guild {cmdSket.Channel.GetGuild().Name} ({cmdSket.Channel.GetGuild().Id})");
        try
        {
            switch (cmdSket.Data.Name)
            {
                case "ping":
                    await Commands.PingCommand(cmdSket);
                    break;
                case "google":
                    await Commands.GetGoogleResults(cmdSket);
                    break;
                case "biblequote":
                    await Commands.BibleSearchCommand(cmdSket);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception Occured!!! \n\n{ex}");
        }
    }
}