using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Reddit.PostDownloader;
using Reddit.PostDownloader.Structures.Deep;
using Spectre.Console;

namespace Reddit.PostDownloader;

class Client
{
    public static Downloader dlCtx = new();
    public static async Task<List<string>> GetFromSubreddit(string subreddit, bool allowNSFW, SubredditRequests searchParam)
    {
        for (int i = 0; i < 25; i++)
        {
            SubredditPost post = (await Downloader.DownloadPost(subreddit, searchParam, 1))[0];

            #region 'Safe-Guards'

            if (post.IsNSFW() && !allowNSFW)
            {
                continue;
            }
            #endregion 'Safe-Guards'

            #region Do when finding X

            if (post.IsImage() || post.IsGif())
            {
                return new() { post.Data.Children[0].Data.UrlOverriddenByDest!.ToString() };
            }

            if (post.IsGallery())
            {
                List<string> strs = new();
                List<Downloader.ImageInformation?> imgInfo = await dlCtx.GetGalleryInformation(post);

                for (int j = 0; j < imgInfo.Count; j++)
                {
                    strs.Add(imgInfo[j]!.Value.Origin.ToString());
                }

                return strs;
            }
            #endregion
        }
        throw new Exception("No Posts with Images or Galleries Encountered!");
    }
}