using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Reddit.PostDownloader.Exceptions;

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
            catch (NoPostsException)
            {
                await msg.ModifyAsync(x =>
                {
                    x.Embed = new EmbedBuilder()
                    {
                        Description = $"The bot could not find a post in the subreddit in subreddit **`{subreddit}`** that contained a gif, a gallery or an image.",
                        Footer = Extensions.GetTimeFooter()
                    }.Build();
                });
            }
            catch (InvalidSubredditException)
            {
                await msg.ModifyAsync(x =>
                {
                    x.Embed = new EmbedBuilder()
                    {
                        Description = $"The bot evaluated the subreddit **`{subreddit}`** to be non-existant.",
                        Footer = Extensions.GetTimeFooter()
                    }.Build();
                });
            }
            catch (Exception ex)
            {
                await msg.ModifyAsync(x =>
                {
                    x.Embed = new EmbedBuilder()
                    {
                        Description = $"The bot suffered an unknown error... Strange, Stack Trace attached below\n\n```prolog\n{ex}\n```",
                        Footer = Extensions.GetTimeFooter()
                    }.Build();
                });
            }
        }).Start();
    }
}