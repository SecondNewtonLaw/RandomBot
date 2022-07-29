using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RandomBot;
using Reddit.PostDownloader.Exceptions;
using Reddit.PostDownloader.Structures.Deep;

namespace Reddit.PostDownloader;

partial class Downloader
{
    public static async Task<SubredditPost[]> DownloadPost(string subreddit, SubredditRequests searchParam, int limit = 1)
    {
        string json = await MainActivity.HttpClient.GetStringAsync($"https://reddit.com/r/{subreddit}/{searchParam.ToString().ToLower()}.json?limit={limit}");
        try
        {
            if (json is "{\"kind\": \"Listing\", \"data\": {\"after\": null, \"dist\": 0, \"modhash\": \"\", \"geo_filter\": \"\", \"children\": [], \"before\": null}}")
                throw new InvalidSubredditException("Invalid Subreddit.");

            try
            {
                return JsonConvert.DeserializeObject<SubredditPost[]>(json);
            }
            catch
            {
                return new SubredditPost[] { JsonConvert.DeserializeObject<SubredditPost>(json) };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            Console.WriteLine($"\r\nA JSON failed to be processed by Newtonsoft.Json -> {json}");
            throw;
        }
    }
    //! Reference URL   |
    //!                 V 
    //!      https://reddit.com/r/unixporn/random/.json?limit=1
}

public enum SubredditRequests
{
    Hot = 69,
    Random = 255
}