using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BibleAPI;

public class API
{
    public static async Task<CallInformation> GetVersicle(string book, long chapter, long verse)
    {
        Stream jsonutf8 = await RandomBot.MainActivity.HttpClient.GetStreamAsync($"https://bible-api.com/{book}+{chapter}:{verse}");

        BibleJson biblicalQuote = (await System.Text.Json.JsonSerializer.DeserializeAsync<BibleJson>(jsonutf8))!;

        return new()
        {
            TranslationInformation = $"Translation Origin: {biblicalQuote.TranslationName} | Note: {biblicalQuote.TranslationNote}",
            QuoteOrigin = biblicalQuote.Reference,
            BiblicalQuote = biblicalQuote.Text.Remove(biblicalQuote.Text.Length - 1, 1)
        };
    }
    public class CallInformation
    {
        public string? TranslationInformation { get; init; }
        public string? QuoteOrigin { get; init; }
        public string? BiblicalQuote { get; set; }
    }
}
