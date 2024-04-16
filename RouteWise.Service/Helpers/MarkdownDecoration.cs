using System.Text.RegularExpressions;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Helpers;

public class MarkdownDecoration : ITextDecoration
{
    private static readonly Regex _markdownQuotePattern = new Regex(@"([_*\[\]()~`>#+\-=|{}.!\\])");

    public static string Link(string text, string link)
        => $"[{text}]({link})";
    public static string Bold(string text)
        => $"*{text}*";
    public static string Italic(string text)
        => $"_\r{text}_\r";
    public static string Spoiler(string text)
        => $"||{text}||";
    public static string Code(string text)
        => $"`{text}`";
    public static string Pre(string text)
        => $"```\n{text}\n```";
    public static string PreLanguage(string text, string language)
        => $"```{language}\n{text}\n```";
    public static string Underline(string text)
        => $"__\r{text}__\r";
    public static string Strikethrough(string text)
        => $"~{text}~";
    public static string Quote(string text)
        => _markdownQuotePattern.Replace(text, @"\\$1"); // in python was like "\\\1"
    public static string CustomEmoji(string text, string customEmojiId)
        => Link(text: text, link: $"tg://emoji?id={customEmojiId}");
}
