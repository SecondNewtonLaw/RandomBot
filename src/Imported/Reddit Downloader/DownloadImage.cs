using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RandomBot;
using Reddit.PostDownloader.Extensions;
using Reddit.PostDownloader.Structures.Deep;

namespace Reddit.PostDownloader;

public partial class Downloader
{
    public struct ImageInformation
    {
        public Stream? ImageData { get; init; }
        public string ImageName { get; init; }
        public string ImageExtension { get; init; }
        public Uri Origin { get; init; }
    }
    /// <summary>
    /// Downloads a Reddit Image.
    /// </summary>
    /// <param name="post">The reddit post itself</param>
    /// <param name="token">Cancellation Token (Optional)</param>
    /// <returns>an ImageInformation struct/returns>
    public async Task<ImageInformation> DownloadImage(SubredditPost post, CancellationToken token = new())
    {
        string extension = post.Data.Children[0].Data.UrlOverriddenByDest?.ToString().Split('.').Last()!;
        Task<HttpResponseMessage> responseTask = MainActivity.HttpClient.GetAsync(post.Data.Children[0].Data.UrlOverriddenByDest, token);

        HttpResponseMessage response = await responseTask;


        return new()
        {
            ImageData = await response.Content.ReadAsStreamAsync(token),
            ImageName = await post.Data.Children[0].Data.Title.SanitizeString(token),
            ImageExtension = $".{extension}",
            Origin = post.Data.Children[0].Data.Url
        };
    }
}