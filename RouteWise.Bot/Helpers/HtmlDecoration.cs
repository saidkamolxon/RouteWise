using RouteWise.Bot.Interfaces;

namespace RouteWise.Bot.Helpers;

public class HtmlDecoration : ITextDecoration
{
    public static string Link(string text, string link)
        => $"<a href='{link}'>{text}</a>";
    public static string Bold(string text)
        => $"<b>{text}</b>";
    public static string Italic(string text)
        => $"<i>{text}</i>";
    public static string Spoiler(string text)
        => $"<span class='tg-spoiler'>{text}</span>";
    public static string Code(string text)
        => $"<code>{text}</code>";
    public static string Pre(string text)
        => $"<pre>{text}</pre>";
    public static string PreLanguage(string text, string language)
        => $"<pre><code class=\"language-{language}\">\n{text}\n</code></pre>";
    public static string Underline(string text)
        => $"<u>{text}</u>";
    public static string Strikethrough(string text)
        => $"<s>{text}</s>";
    public static string Quote(string text)
        => Escape(text, quote: false);
    public static string CustomEmoji(string text, string customEmojiId)
        => $"<tg-emoji emoji-id='{customEmojiId}'>{text}</tg-emoji>";
    private static string Escape(string text, bool quote = true)
    {
        text = text.Replace("&", "&amp;");
        text = text.Replace("<", "&lt;");
        text = text.Replace(">", "&gt;");
        if (quote)
        {
            text = text.Replace("\"", "&quot;");
            text = text.Replace("\'", "&#x27;");
        }
        return text;
    }
}
