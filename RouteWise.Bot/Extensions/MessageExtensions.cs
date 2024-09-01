using RouteWise.Service.Helpers;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Extensions;

public static class MessageExtensions
{
    public static string GetBotCommand(this Message message)
    {
        if (message?.Entities is null) return null;

        if (message.Entities.First().Type is MessageEntityType.BotCommand)
            return message.EntityValues.First().Split("@").First();
        
        return null;
    }

    public static string[] GetBotCommandArgs(this Message message)
    {
        if (message?.Entities is null) return null;

        if (message.Entities.First().Type is MessageEntityType.BotCommand)
            return message.Text.Split().Skip(1).ToArray();

        return null;
    }

    public static string GetHtmlText(this Message message)
    {
        var text = message.Text ?? message.Caption;

        if (message?.Entities is null) return text;
        
        var htmlText = new StringBuilder(message.Text ?? message.Caption);
        for (int index = 0; index < message.Entities.Length; index++)
        {
            string currentSubString = message.EntityValues.ElementAt(index);
            string decoratedSubString = FormatText(currentSubString, message.Entities[index]);
            htmlText.Replace(currentSubString, decoratedSubString);
        }

        return htmlText.ToString();
    }

    private static string FormatText(string text, MessageEntity entity)
    {
        string formattedText = entity.Type switch
        {
            MessageEntityType.TextLink => HtmlDecoration.Link(text, entity.Url),
            MessageEntityType.Bold => HtmlDecoration.Bold(text),
            MessageEntityType.Italic => HtmlDecoration.Italic(text),
            MessageEntityType.Spoiler => HtmlDecoration.Spoiler(text),
            MessageEntityType.Code => HtmlDecoration.Code(text),
            MessageEntityType.Pre => entity.Language == null ? HtmlDecoration.Pre(text) : HtmlDecoration.PreLanguage(text, entity.Language),
            MessageEntityType.Underline => HtmlDecoration.Underline(text),
            MessageEntityType.Strikethrough => HtmlDecoration.Strikethrough(text),
            MessageEntityType.CustomEmoji => HtmlDecoration.CustomEmoji(text, entity.CustomEmojiId),
            _ => text,
        };
        return formattedText;
    }
}
