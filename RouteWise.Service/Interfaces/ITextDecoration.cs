namespace RouteWise.Service.Interfaces;

public interface ITextDecoration
{
    static abstract string Link(string text, string link);
    static abstract string Bold(string text);
    static abstract string Italic(string text);
    static abstract string Spoiler(string text);
    static abstract string Code(string text);
    static abstract string Pre(string text);
    static abstract string PreLanguage(string text, string language);
    static abstract string Underline(string text);
    static abstract string Strikethrough(string text);
    static abstract string Quote(string text);
    static abstract string CustomEmoji(string text, string customEmojiId);
}
