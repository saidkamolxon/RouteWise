using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using RouteWise.Bot.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace RouteWise.Bot.Extensions;

public static class TelegramBotClientExtensions
{
    public static async Task<Message> AnswerMessageAsync(this ITelegramBotClient botClient,
        Message message,
        string text,
        ParseMode parseMode = Defaults.DefaultParseMode,
        List<MessageEntity> entities = null,
        LinkPreviewOptions linkPreviewOptions = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (message == null) return null;

        if (isReply)
        {
            if (replyParameters != null)
                replyParameters.MessageId = message.MessageId;
            else
                replyParameters = new ReplyParameters { MessageId = message.MessageId };
        }

        return await botClient.SendMessageAsync(new SendMessageRequest
        {
            ChatId = message.Chat.Id,
            Text = text,
            ParseMode = parseMode,
            Entities = entities,
            LinkPreviewOptions = linkPreviewOptions,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
    }

    public static async Task<Message> AnswerMessageWithPhotoAsync(this ITelegramBotClient botClient,
        Message message,
        string photoUrlOrFileId,
        string caption,
        ParseMode parseMode = Defaults.DefaultParseMode,
        bool? hasSpoiler = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false)
    {
        if (isReply)
        {
            if (replyParameters != null)
                replyParameters.MessageId = message.MessageId;
            else
                replyParameters = new ReplyParameters { MessageId = message.MessageId };
        }

        return await botClient.SendPhotoAsync(new SendPhotoRequest
        {
            ChatId = message.Chat.Id,
            Photo = InputFile.FromString(photoUrlOrFileId),
            Caption = caption,
            ParseMode = parseMode,
            HasSpoiler = hasSpoiler,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
    }
}

/*
 self,
        photo: Union[InputFile, str],
        caption: Optional[str] = None,
        parse_mode: Optional[Union[str, Default]] = Default("parse_mode"),
        caption_entities: Optional[List[MessageEntity]] = None,
        has_spoiler: Optional[bool] = None,
        disable_notification: Optional[bool] = None,
        protect_content: Optional[Union[bool, Default]] = Default("protect_content"),
        reply_parameters: Optional[ReplyParameters] = None,
        reply_markup: Optional[
            Union[InlineKeyboardMarkup, ReplyKeyboardMarkup, ReplyKeyboardRemove, ForceReply]
        ] = None,
        allow_sending_without_reply: Optional[bool] = None,
        **kwargs: Any,
 */