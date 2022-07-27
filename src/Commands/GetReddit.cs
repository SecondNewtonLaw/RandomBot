using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace RandomBot;

internal partial class Commands
{
    public static async Task SubredditCommand(SocketSlashCommand cmdCtx)
    {
        await cmdCtx.DeferAsync();

        string subreddit = (string)cmdCtx.Data.Options.ElementAt(0).Value;
        bool allowNSFW = cmdCtx.Channel.GetGuild().GetTextChannel(cmdCtx.Channel.Id!).IsNsfw;

        RestFollowupMessage msg = await cmdCtx.FollowupAsync(embed: new EmbedBuilder()
        {
            Description = $"Obtaining post from subreddit \'**{subreddit}**\'... | NSFW Posts: {allowNSFW}",
            Footer = Extensions.GetTimeFooter()
        }.Build());

        new Thread(async () =>
        {
            try
            {
                List<string> ans = await Reddit.PostDownloader.Client.GetFromSubreddit(subreddit, allowNSFW, Reddit.PostDownloader.SubredditRequests.Random);

                await msg.ModifyAsync(x =>
                {
                    x.Embed = new EmbedBuilder()
                    {
                        Description = $"Post(s) found!\nLinks:\n\n{string.Join('\n', ans)}",
                        Footer = Extensions.GetTimeFooter(),
                        ImageUrl = ans[0]
                    }.Build();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await msg.ModifyAsync(x =>
                {
                    x.Embed = new EmbedBuilder()
                    {
                        Description = "No subreddit post matched the criterium or it was a non-existent subreddit.",
                        Footer = Extensions.GetTimeFooter()
                    }.Build();
                });
            }
        }).Start();
    }
}