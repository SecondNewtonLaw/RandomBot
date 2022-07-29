// ABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789 !@#$%^&*(){}[]:;,.<>/?\

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace RandomBot;

internal partial class Commands
{
    public static async Task PasswordGeneratorCommand(SocketSlashCommand cmdCtx)
    {
        char[] ASCII_NON_SPECIAL_NON_NUMERIC = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
            'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        char[] ASCII_NON_SPECIAL_NUMERIC = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        char[] ASCII_SPECIAL_NON_NUMERIC = new char[]{
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
            '{', '}', '[', ']', ':', ';', ',', '.', '<', '>',
            '/', '?', '\\', '\'', '\"', '|', '+', '-', '=',
            '_', '`', '~'
        };

        const int trueMaxCharacterLimit = 4088;
        await cmdCtx.DeferAsync();

        bool useSpecialCharacters = (bool)cmdCtx.Data.Options.ElementAt(0).Value,
        useNumbersCharacters = (bool)cmdCtx.Data.Options.ElementAt(1).Value;

        long maxCharacters = (long)cmdCtx.Data.Options.ElementAt(2).Value;

        // lIMIT MAX CHARACTERS.
        if (maxCharacters > trueMaxCharacterLimit)
        {
            await cmdCtx.FollowupAsync(embed: new EmbedBuilder()
            {
                Title = "Invalid Password Length!",
                Description = $"ERROR! :x:\nThe max password length is of **`{trueMaxCharacterLimit}`**; this is not allowed, as the maximum limit is of **`{maxCharacters}`** characters.",
                Footer = Extensions.GetTimeFooter()
            }.Build());
            return;
        }

        new Thread(async () =>
        {
            async Task<string> GetUsedCharacters()
            {
                return ("Normal ASCII Characters" + (useNumbersCharacters ? ", Numbers" : "")) + (useSpecialCharacters ? ", Special ASCII Characters" : "");
            }

            var msg = await cmdCtx.FollowupAsync(embed: new EmbedBuilder()
            {
                Title = "Generating Password!",
                Description = $"Your password of {maxCharacters} characters is being generated using **{await GetUsedCharacters()}**",
                Footer = Extensions.GetTimeFooter()
            }.Build());

            // RandomNumberGenerator cryptoRNG = RandomNumberGenerator.Create();

            StringBuilder stringBuilder = new();
            int currArrayPos, currArraySelector;
            bool useCapital = false;

            #region Password Generator Logic

            for (int i = 0; i < maxCharacters; i++)
            {
                // 0 => ASCII_NON_SPECIAL_NON_NUMERIC
                // 1 => ASCII_NON_SPECIAL_NUMERIC
                // 2 => ASCII_SPECIAL_NON_NUMERIC
                currArraySelector = RandomNumberGenerator.GetInt32(0, 3);
                if (currArraySelector is 0)
                {
                    currArrayPos = RandomNumberGenerator.GetInt32(0, ASCII_NON_SPECIAL_NON_NUMERIC.Length);
                    useCapital = RandomNumberGenerator.GetInt32(0, 1) is 1;

                    if (useCapital) stringBuilder.Append(ASCII_NON_SPECIAL_NON_NUMERIC[currArrayPos]);
                    else stringBuilder.Append(ASCII_NON_SPECIAL_NON_NUMERIC[currArrayPos].ToString().ToLower());
                }
                else if (currArraySelector is 1 && useNumbersCharacters)
                {
                    currArrayPos = RandomNumberGenerator.GetInt32(0, ASCII_NON_SPECIAL_NUMERIC.Length);
                    stringBuilder.Append(ASCII_NON_SPECIAL_NUMERIC[currArrayPos]);
                }
                else if (currArraySelector is 2 && useSpecialCharacters)
                {
                    currArrayPos = RandomNumberGenerator.GetInt32(0, ASCII_SPECIAL_NON_NUMERIC.Length);
                    stringBuilder.Append(ASCII_SPECIAL_NON_NUMERIC[currArrayPos]);
                }
                else
                {
                    currArrayPos = RandomNumberGenerator.GetInt32(0, ASCII_NON_SPECIAL_NON_NUMERIC.Length);
                    stringBuilder.Append(ASCII_NON_SPECIAL_NON_NUMERIC[currArrayPos]);
                }
            }

            #endregion Password Generator Logic

            await msg.ModifyAsync(async x =>
            {
                x.Content = $"**Password with {await GetUsedCharacters()}** with a length of **`{maxCharacters}`** has been generated for user {cmdCtx.User.Mention}!";
                x.Embed = new EmbedBuilder()
                {
                    Title = "Password Generated!",
                    Description = "```\n" + stringBuilder.ToString() + "\n```",
                    Footer = Extensions.GetTimeFooter()
                }.Build();
            });

        }).Start();
    }
}