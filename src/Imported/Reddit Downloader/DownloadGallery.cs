using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RandomBot;
using Reddit.PostDownloader.Structures.Deep;

namespace Reddit.PostDownloader;

public partial class Downloader
{
    /// <summary>
    /// Gathers metadata from a subreddit
    /// </summary>
    /// <param name="post">The data of the subreddit post containing the gallery</param>
    /// <param name="token">Cancellation Token (Optional)</param>
    /// <returns>ImageInformation containing Extensions, URLs and more</returns>
    public async Task<List<ImageInformation?>> GetGalleryInformation(SubredditPost post)
    {
        if (post.Data.Children[0].Data.IsGallery is false or null)
            throw new InvalidOperationException("Can not download gallery of a post when it is not a gallery!");

        List<ImageInformation?> imgInfo = new();
        SubredditPost.GalleryData[] galleryData = post.Data.Children[0].Data.GalleryData;

        StringBuilder strbld = new();

        // Assemble Image Data
        for (int i = 0; i < galleryData.Length; i++)
        {
            strbld
                .Append("https://i.redd.it/")
                .Append(galleryData[i].Id)
                .Append('.')
                .Append(galleryData[i].MIMEType!.Split('/')[1].ToLower());

            imgInfo.Add(new()
            {
                ImageName = post.Data.Children[0].Data.Title,
                Origin = new(strbld.ToString()),
                ImageExtension = galleryData[i].MIMEType!
            });
            strbld.Clear();
        }
        return imgInfo;
    }
}