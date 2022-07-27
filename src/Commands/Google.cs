using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using WebsiteScraper;

namespace RandomBot;

internal partial class Commands
{
    public static async Task GetGoogleResults(SocketSlashCommand cmdSock)
    {
        //const string BASE_URL = "https://www.google.com/search";
        await cmdSock.DeferAsync();
        string usrQuery = (string)cmdSock.Data.Options.ElementAt(0).Value;
        try
        {
            ScrapeGoogle scraper = new(usrQuery);


            Task<List<ScrapedSearchResult>> scraperTask = scraper.GetResultsAsync();

            await scraperTask;

            List<EmbedFieldBuilder> fields = new();
            List<ScrapedSearchResult> results = await scraperTask;
            results = results.OrderBy(x => x.ItemPosition).ToList(); // Order elements
            int addResults = 10;

            if (results.Count < 10) addResults = results.Count;

            for (int i = 0; i < addResults; i++)
            {
                fields.Add(new Discord.EmbedFieldBuilder() { Name = results[i].Title, Value = results[i].URL });
            }
            await cmdSock.FollowupAsync(embed: new EmbedBuilder() { Title = $"Searched \'{usrQuery}\' on google", Fields = fields, Footer = Extensions.GetTimeFooter() }.Build());
        }
        catch (NoResultsException ex)
        {
            Console.WriteLine(ex);
            await cmdSock.FollowupAsync(embed: new EmbedBuilder()
            {
                Title = "No search results :(",
                Description = $"No search results on google were found for query \'{usrQuery}\'",
                Footer = Extensions.GetTimeFooter()
            }.Build());

            return;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }
}