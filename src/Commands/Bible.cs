using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BibleAPI;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace RandomBot;

internal partial class Commands
{
    public static async Task BibleSearchCommand(SocketSlashCommand cmdCtx)
    {
        await cmdCtx.DeferAsync();

        string book = (string)cmdCtx.Data.Options.ElementAt(0).Value;
        long chapter = (long)cmdCtx.Data.Options.ElementAt(1).Value;
        long versicle = (long)cmdCtx.Data.Options.ElementAt(2).Value;

        Discord.Rest.RestFollowupMessage msg = await cmdCtx.FollowupAsync("Obtaining Book, Chapter and Versicle of the Bible...");
        API.CallInformation bibleQuoteInfo = null!;
        try
        {
            bibleQuoteInfo = await BibleAPI.API.GetVersicle(book, versicle, chapter);
        }
        catch
        {
            await msg.ModifyAsync(x => x.Content = "The Versicle/Book/Chapter did not exist.");
            return;
        }

        await msg.ModifyAsync(x =>
            {
                x.Content = "";
                x.Embed = new EmbedBuilder()
                {
                    Title = $"Book: {book} | Chapter: {chapter} | Versicle:{versicle}",
                    Description = $"Quote -> ***{bibleQuoteInfo.BiblicalQuote}***"
                }.Build();
            });

    }
}