using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace RandomBot;

public struct CommandBuilder
{
    /// <summary>
    /// Build all bot commands for a specific Guild (Discord Server)
    /// </summary>
    /// <param name="guild">Socket of the Guild.</param>
    public async Task BuildFor(SocketGuild guild)
        => await guild.BulkOverwriteApplicationCommandAsync(this.BuildCommands().ToArray());
    /// <summary>
    /// Build all bot commands for the whole bot | Takes two hours to apply between iterations.
    /// </summary>
    /// <param name="client">Bot Client.</param>
    public async Task BuildApp(DiscordSocketClient client)
        => await client.BulkOverwriteGlobalApplicationCommandsAsync(BuildCommands().ToArray());

    private List<SlashCommandProperties> BuildCommands()
    {
        List<SlashCommandProperties> commands = new();

        SlashCommandBuilder pingCommand = new()
        {
            Name = "ping",
            Description = "Get the last heartbeat from the Gateway to our bot"
        };

        SlashCommandBuilder googleSearchCommand = new()
        {
            Name = "google",
            Description = "Searches for things on Google"
        };
        googleSearchCommand.AddOption("query", ApplicationCommandOptionType.String, "The text you want to search on google", true);

        SlashCommandBuilder bibleQuoteCommand = new()
        {
            Name = "biblequote",
            Description = "Gets a quote from the bible"
        };
        bibleQuoteCommand.AddOption("book", ApplicationCommandOptionType.String, "The bibles book you want to get", true);
        bibleQuoteCommand.AddOption("chapter", ApplicationCommandOptionType.Integer, "The bibles book chapter", true);
        bibleQuoteCommand.AddOption("versicle", ApplicationCommandOptionType.Integer, "The bibles versicle you want to get", true);

        SlashCommandBuilder redditGetterCommand = new()
        {
            Name = "reddit",
            Description = "Gets a random post from the specified subreddit"
        };
        redditGetterCommand.AddOption("subreddit", ApplicationCommandOptionType.String, "The subreddit to get a post from", true);

        SlashCommandBuilder passGenCommand = new()
        {
            Name = "passwordgenerator",
            Description = "Generates passwords with the specified parameters"
        };
        passGenCommand.AddOption("specialcharacters", ApplicationCommandOptionType.Boolean, "Use special characters in the password", true);
        passGenCommand.AddOption("numbers", ApplicationCommandOptionType.Boolean, "Use numbers in the password", true);
        passGenCommand.AddOption("maxcharacters", ApplicationCommandOptionType.Integer, "The maximum character count of the password max is 4088", true);

        /*        
            bool useSpecialCharacters = (bool)cmdCtx.Data.Options.ElementAt(0).Value,
            useNumbersCharacters = (bool)cmdCtx.Data.Options.ElementAt(1).Value;
            long maxCharacters = (long)cmdCtx.Data.Options.ElementAt(2).Value;
        */

        commands.Add(pingCommand.Build());
        commands.Add(googleSearchCommand.Build());
        commands.Add(bibleQuoteCommand.Build());
        commands.Add(redditGetterCommand.Build());
        commands.Add(passGenCommand.Build());

        return commands;
    }
}
