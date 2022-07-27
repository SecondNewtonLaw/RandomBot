using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Reddit.PostDownloader.Structures.Deep;

/* 
 * |---------------------------------------------------------------------------------------------------|
 * | Disable non nullable property must contain a non nullable value before exiting the ctor. (CS8618) |
 * | Disable Possible null reference assignment. (CS8601)                                              |
 * |---------------------------------------------------------------------------------------------------|
 */
#pragma warning disable CS8618, CS8601

#region Implementations 

public partial class SubredditPost
{
    public bool IsGif()
    {
        if (this.Data.Children[0].Data.Video is not null && this.Data.Children[0].Data.Video?.VideoInformation is not null)
        {
            try
            {
                return this.Data.Children[0].Data.Video?.VideoInformation.IsGif != null && this.Data.Children[0].Data.Video.VideoInformation.IsGif;
            }
            catch { return false; }
        }
        return false;
    }
    public bool IsGallery()
    {
        bool maybeGallery = !(this.Data.Children[0].Data.IsGallery is null or false);

        try
        {
            if (this.Data.Children[0].Data.GalleryData is null) return false;

            return maybeGallery;
        }
        catch { return maybeGallery; }
    }

    public bool IsNSFW()
            => !(this.Data.Children[0].Data.Over18 is null or false);

    public bool IsImage()
        =>
            this.Data.Children[0].Data.IsVideo is not null &&       // Check that the Video is not null
            this.Data.Children[0].Data.IsVideo is false &&          // Check If it is video
            this.Data.Children[0].Data.Domain is not null &&        // Check that the Domain is not null
            this.Data.Children[0].Data.Domain == "i.redd.it";       // Check that the domain is 'i.redd.it'

    public static SubredditPost[]? FromJson(string json)
        => JsonConvert.DeserializeObject<SubredditPost[]>(json, Converter.Settings);
}
public static class Serialize
{
    public static string ToJson(this SubredditPost[] self) => JsonConvert.SerializeObject(self, SubredditPost.Converter.Settings);
}

#endregion

public partial class SubredditPost
{
    [JsonProperty("kind")]
    public string Kind { get; set; }

    [JsonProperty("data")]
    public SubredditPostData Data { get; set; }

    public partial class SubredditPostData
    {
        [JsonProperty("dist")]
        public long? Dist { get; set; }

        [JsonProperty("modhash")]
        public string Modhash { get; set; }

        [JsonProperty("geo_filter")]
        public string GeoFilter { get; set; }

        [JsonProperty("children")]
        public PurpleChild[] Children { get; set; }
    }

    public partial class PurpleChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public PurpleData Data { get; set; }
    }
    public class SecureMedia
    {
        [JsonProperty("reddit_video")]
        public RedditVideo VideoInformation;

        public class RedditVideo
        {
            [JsonProperty("bitrate_kbps")]
            public long BitrateKbps { get; set; }

            [JsonProperty("fallback_url")]
            public Uri FallbackUrl { get; set; }

            [JsonProperty("height")]
            public long? Height { get; set; }

            [JsonProperty("width")]
            public long? Width { get; set; }

            [JsonProperty("scrubber_media_url")]
            public Uri ScrubberMediaUrl { get; set; }

            [JsonProperty("dash_url")]
            public Uri DashUrl { get; set; }

            [JsonProperty("duration")]
            public long? Duration { get; set; }

            [JsonProperty("hls_url")]
            public Uri HlsUrl { get; set; }

            [JsonProperty("is_gif")]
            public bool IsGif { get; set; }

            [JsonProperty("transcoding_status")]
            public string TranscodingStatus { get; set; }
        }
    }
    public partial class PurpleData
    {
        [JsonProperty("secure_media")]
        public SecureMedia? Video;
        [JsonProperty("is_gallery")]
        public bool? IsGallery { get; set; }

        [JsonProperty("media_metadata"), JsonConverter(typeof(SingleOrArrayConverter<GalleryData>))]
        public GalleryData[] GalleryData { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("selftext", NullValueHandling = NullValueHandling.Ignore)]
        public string Selftext { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }
        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("clicked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Clicked { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("hidden", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Hidden { get; set; }

        [JsonProperty("pwls", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pwls { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("thumbnail_height", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailHeight { get; set; }

        [JsonProperty("parent_whitelist_status", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentWhitelistStatus { get; set; }

        [JsonProperty("hide_score", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HideScore { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quarantine", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Quarantine { get; set; }

        [JsonProperty("link_flair_text_color", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkFlairTextColor { get; set; }

        [JsonProperty("upvote_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public double? UpvoteRatio { get; set; }

        [JsonProperty("author_flair_background_color")]
        public string AuthorFlairBackgroundColor { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }

        [JsonProperty("total_awards_received")]
        public long TotalAwardsReceived { get; set; }

        [JsonProperty("thumbnail_width", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailWidth { get; set; }

        [JsonProperty("is_original_content", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOriginalContent { get; set; }

        [JsonProperty("author_fullname", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorFullname { get; set; }

        [JsonProperty("is_reddit_media_domain", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRedditMediaDomain { get; set; }

        [JsonProperty("is_meta", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMeta { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("is_created_from_ads_ui", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCreatedFromAdsUi { get; set; }

        [JsonProperty("author_premium", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPremium { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? Thumbnail { get; set; }

        [JsonProperty("edited")]
        public bool Edited { get; set; }

        [JsonProperty("post_hint", NullValueHandling = NullValueHandling.Ignore)]
        public string? PostHint { get; set; }

        [JsonProperty("is_self", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSelf { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("link_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkFlairType { get; set; }

        [JsonProperty("wls", NullValueHandling = NullValueHandling.Ignore)]
        public long? Wls { get; set; }
        [JsonProperty("author_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFlairType { get; set; }

        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string? Domain { get; set; }

        [JsonProperty("allow_live_comments", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowLiveComments { get; set; }
        [JsonProperty("url_overridden_by_dest", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? UrlOverriddenByDest { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("no_follow")]
        public bool NoFollow { get; set; }

        [JsonProperty("is_crosspostable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCrosspostable { get; set; }

        [JsonProperty("pinned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Pinned { get; set; }

        [JsonProperty("over_18", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Over18 { get; set; }
        [JsonProperty("media_only", NullValueHandling = NullValueHandling.Ignore)]
        public bool? MediaOnly { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("spoiler", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Spoiler { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("visited", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Visited { get; set; }

        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("author_is_blocked")]
        public bool AuthorIsBlocked { get; set; }

        [JsonProperty("link_flair_background_color", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_robot_indexable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRobotIndexable { get; set; }

        [JsonProperty("num_duplicates", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumDuplicates { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("num_comments", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumComments { get; set; }

        [JsonProperty("send_replies")]
        public bool SendReplies { get; set; }

        [JsonProperty("contest_mode", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ContestMode { get; set; }

        [JsonProperty("author_patreon_flair", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPatreonFlair { get; set; }

        [JsonProperty("author_flair_text_color")]
        public string AuthorFlairTextColor { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("whitelist_status", NullValueHandling = NullValueHandling.Ignore)]
        public string WhitelistStatus { get; set; }

        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("subreddit_subscribers", NullValueHandling = NullValueHandling.Ignore)]
        public long? SubredditSubscribers { get; set; }

        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        [JsonProperty("num_crossposts", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumCrossposts { get; set; }

        [JsonProperty("is_video", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVideo { get; set; }

        [JsonProperty("replies", NullValueHandling = NullValueHandling.Ignore)]
        public StickyReplies? Replies { get; set; }

        [JsonProperty("collapsed_reason_code")]
        public string CollapsedReasonCode { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("collapsed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Collapsed { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        [JsonProperty("is_submitter", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSubmitter { get; set; }

        [JsonProperty("body_html", NullValueHandling = NullValueHandling.Ignore)]
        public string BodyHtml { get; set; }

        [JsonProperty("score_hidden", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ScoreHidden { get; set; }

        [JsonProperty("link_id", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkId { get; set; }

        [JsonProperty("controversiality", NullValueHandling = NullValueHandling.Ignore)]
        public long? Controversiality { get; set; }

        [JsonProperty("depth", NullValueHandling = NullValueHandling.Ignore)]
        public long? Depth { get; set; }
    }

    public partial class ResizedIcon
    {
        [JsonProperty("url")]
        public Uri? Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public partial class Preview
    {
        [JsonProperty("images")]
        public Image[]? Images { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("source")]
        public ResizedIcon? Source { get; set; }

        [JsonProperty("resolutions")]
        public ResizedIcon[]? Resolutions { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }
    }

    public partial class PostReplies
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }

        [JsonProperty("data")]
        public FluffyData? Data { get; set; }
    }

    public partial class FluffyData
    {
        [JsonProperty("modhash")]
        public string Modhash { get; set; }

        [JsonProperty("geo_filter")]
        public string GeoFilter { get; set; }

        [JsonProperty("children")]
        public FluffyChild[] Children { get; set; }
    }
    public partial class FluffyChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public TentacledData Data { get; set; }
    }

    public partial class TentacledData
    {
        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("author_is_blocked")]
        public bool AuthorIsBlocked { get; set; }

        [JsonProperty("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonProperty("total_awards_received")]
        public long TotalAwardsReceived { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("replies")]
        public TentacledReplies Replies { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("no_follow")]
        public bool NoFollow { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        [JsonProperty("send_replies")]
        public bool SendReplies { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("edited")]
        public Edited Edited { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_submitter")]
        public bool IsSubmitter { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("author_patreon_flair")]
        public bool AuthorPatreonFlair { get; set; }

        [JsonProperty("body_html")]
        public string BodyHtml { get; set; }

        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("author_premium")]
        public bool IsAuthorPremium { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("link_id")]
        public string LinkId { get; set; }

        [JsonProperty("score_hidden")]
        public bool HiddenScore { get; set; }

        [JsonProperty("permalink")]
        public string Permanentlink { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("controversiality")]
        public long Controversiality { get; set; }

        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }
    }

    public partial class FluffyReplies
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public StickyData Data { get; set; }
    }

    public partial class StickyData
    {
        [JsonProperty("modhash")]
        public string Modhash { get; set; }

        [JsonProperty("geo_filter")]
        public string GeoFilter { get; set; }

        [JsonProperty("children")]
        public TentacledChild[] Children { get; set; }
    }

    public partial class TentacledChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public IndigoData Data { get; set; }
    }

    public partial class IndigoData
    {
        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("author_is_blocked")]
        public bool AuthorIsBlocked { get; set; }

        [JsonProperty("total_awards_received")]
        public long TotalAwardsReceived { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("replies")]
        public DataRepliesClass Replies { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("no_follow")]
        public bool NoFollow { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("send_replies")]
        public bool SendReplies { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("edited")]
        public bool Edited { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_submitter")]
        public bool IsSubmitter { get; set; }

        [JsonProperty("collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty("author_patreon_flair")]
        public bool AuthorPatreonFlair { get; set; }

        [JsonProperty("body_html")]
        public string BodyHtml { get; set; }

        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("author_premium")]
        public bool AuthorPremium { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("link_id")]
        public string LinkId { get; set; }

        [JsonProperty("score_hidden")]
        public bool ScoreHidden { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("controversiality")]
        public long Controversiality { get; set; }

        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }
    }

    public partial class DataRepliesClass
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public IndecentData Data { get; set; }
    }

    public partial class IndecentData
    {
        [JsonProperty("modhash")]
        public string Modhash { get; set; }

        [JsonProperty("geo_filter")]
        public string GeoFilter { get; set; }

        [JsonProperty("children")]
        public StickyChild[] Children { get; set; }
    }

    public partial class StickyChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public HilariousData Data { get; set; }
    }

    public partial class HilariousData
    {
        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("author_is_blocked")]
        public bool AuthorIsBlocked { get; set; }

        [JsonProperty("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonProperty("total_awards_received")]
        public long TotalAwardsReceived { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("replies")]
        public string Replies { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }
        [JsonProperty("no_follow")]
        public bool NoFollow { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("send_replies")]
        public bool SendReplies { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonProperty("collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("edited")]
        public bool Edited { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_submitter")]
        public bool IsSubmitter { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("author_patreon_flair")]
        public bool AuthorPatreonFlair { get; set; }

        [JsonProperty("body_html")]
        public string BodyHtml { get; set; }
        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("author_premium")]
        public bool AuthorPremium { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("link_id")]
        public string LinkId { get; set; }

        [JsonProperty("score_hidden")]
        public bool ScoreHidden { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("controversiality")]
        public long Controversiality { get; set; }

        [JsonProperty("depth")]
        public long Depth { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }
    }

    public partial struct Edited
    {
        public bool? Bool;
        public long? Integer;

        public static implicit operator Edited(bool Bool) => new() { Bool = Bool };
        public static implicit operator Edited(long Integer) => new() { Integer = Integer };
    }

    public partial struct TentacledReplies
    {
        public FluffyReplies FluffyReplies;
        public string String;

        public static implicit operator TentacledReplies(FluffyReplies FluffyReplies) => new() { FluffyReplies = FluffyReplies };
        public static implicit operator TentacledReplies(string String) => new() { String = String };
    }

    public partial struct StickyReplies
    {
        public PostReplies PostReplies;
        public string String;

        public static implicit operator StickyReplies(PostReplies PostReplies) => new() { PostReplies = PostReplies };
        public static implicit operator StickyReplies(string String) => new() { String = String };
    }

    public struct GalleryData
    {
        [JsonProperty("status")]
        public string? Status;

        [JsonProperty("e")]
        public string? MediaType;

        [JsonProperty("m")]
        public string? MIMEType;

        [JsonProperty("p")]
        public PreviewGalleryInformation[]? Previews;

        [JsonProperty("s")]
        public PreviewGalleryInformation? BestPreview;

        [JsonProperty("id")]
        public string[]? Id;

        public class PreviewGalleryInformation
        {
            [JsonProperty("y")]
            public int Y;
            [JsonProperty("x")]
            public int X;
            [JsonProperty("u")]
            public Uri? Uri;
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                StickyRepliesConverter.Singleton,
                EditedConverter.Singleton,
                TentacledRepliesConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    #region Converters

    internal class StickyRepliesConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(StickyReplies) || t == typeof(StickyReplies?);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new StickyReplies { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<PostReplies>(reader);
                    return new StickyReplies { PostReplies = objectValue };
            }
            throw new Exception("Cannot unmarshal type StickyReplies");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            var value = (StickyReplies)untypedValue!;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.PostReplies != null)
            {
                serializer.Serialize(writer, value.PostReplies);
                return;
            }
            throw new Exception("Cannot marshal type StickyReplies");
        }

        public static readonly StickyRepliesConverter Singleton = new();
    }

    internal class EditedConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Edited) || t == typeof(Edited?);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Edited { Integer = integerValue };
                case JsonToken.Boolean:
                    var boolValue = serializer.Deserialize<bool>(reader);
                    return new Edited { Bool = boolValue };
            }
            throw new Exception("Cannot unmarshal type Edited");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            var value = (Edited)untypedValue!;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.Bool != null)
            {
                serializer.Serialize(writer, value.Bool.Value);
                return;
            }
            throw new Exception("Cannot marshal type Edited");
        }

        public static readonly EditedConverter Singleton = new();
    }

    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objecType)
        {
            return objecType == typeof(Array);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objecType,
            object? existingValue,
            JsonSerializer serializer
            )
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<T[]>()!;
            }
            return new T[] { token.ToObject<T>()! };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    internal class TentacledRepliesConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TentacledReplies) || t == typeof(TentacledReplies?);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new TentacledReplies { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<FluffyReplies>(reader);
                    return new TentacledReplies { FluffyReplies = objectValue };
            }
            throw new Exception("Cannot unmarshal type TentacledReplies");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            var value = (TentacledReplies)untypedValue!;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.FluffyReplies != null)
            {
                serializer.Serialize(writer, value.FluffyReplies);
                return;
            }
            throw new Exception("Cannot marshal type TentacledReplies");
        }

        public static readonly TentacledRepliesConverter Singleton = new();
    }

    #endregion Converters
}