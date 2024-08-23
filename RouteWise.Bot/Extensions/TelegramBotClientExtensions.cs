using RouteWise.Bot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if(isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            text: text,
            parseMode: parseMode,
            entities: entities,
            linkPreviewOptions: linkPreviewOptions,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithPhotoAsync(this ITelegramBotClient botClient,
        Message message,
        string photoUrlOrFileId,
        string caption,
        ParseMode parseMode = Defaults.DefaultParseMode,
        bool hasSpoiler = default,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false)
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendPhotoAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            photo: InputFile.FromString(photoUrlOrFileId),
            caption: caption,
            parseMode: parseMode,
            hasSpoiler: hasSpoiler,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithVenueAsync(this ITelegramBotClient botClient,
        Message message,
        double latitude,
        double longitude,
        string title,
        string address,
        string fourSquareId = null,
        string fourSquareType = null,
        string googlePlaceId = null,
        string googlePlaceType = null,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false)
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendVenueAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            address: address,
            title: title,
            latitude: latitude,
            longitude: longitude,
            foursquareId: fourSquareId,
            foursquareType: fourSquareType,
            googlePlaceId: googlePlaceId,
            googlePlaceType: googlePlaceType,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message[]> AnswerMessageWithMediaGroupAsync(this ITelegramBotClient botClient,
        Message message,
        IEnumerable<IAlbumInputMedia> media,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendMediaGroupAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            media: media,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters
        );
    }

    public static async Task<Message> AnswerMessageWithStickerAsync(this ITelegramBotClient botClient,
        Message message,
        string stickerUrlOrFileId,
        string emoji = null,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendStickerAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            sticker: InputFile.FromString(stickerUrlOrFileId),
            emoji: emoji,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithAnimationAsync(this ITelegramBotClient botClient,
        Message message,
        string animationUrlOrFileId,
        int? duration = null,
        int? width = null,
        int? height = null,
        string thumbnail = null,
        string caption = null,
        ParseMode parseMode = Defaults.DefaultParseMode,
        IEnumerable<MessageEntity> captionEntities = null,
        bool hasSpoiler = default,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendAnimationAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            animation: InputFile.FromString(animationUrlOrFileId),
            duration: duration,
            width: width,
            height: height,
            thumbnail: InputFile.FromString(thumbnail),
            caption: caption,
            parseMode: parseMode,
            captionEntities: captionEntities,
            hasSpoiler: hasSpoiler,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithVideoAsync(this ITelegramBotClient botClient,
        Message message,
        string videoUrlOrFileId,
        int? duration = null,
        int? width = null,
        int? height = null,
        string thumbnail = null,
        string caption = null,
        ParseMode parseMode = Defaults.DefaultParseMode,
        IEnumerable<MessageEntity> captionEntities = null,
        bool hasSpoiler = default,
        bool supportsStreaming = default,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendVideoAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageThreadId : null,
            video: InputFile.FromString(videoUrlOrFileId),
            duration: duration,
            width: width,
            height: height,
            thumbnail: InputFile.FromString(thumbnail),
            caption: caption,
            parseMode: parseMode,
            captionEntities: captionEntities,
            hasSpoiler: hasSpoiler,
            supportsStreaming: supportsStreaming,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithAudioAsync(this ITelegramBotClient botClient,
        Message message,
        string audioUrlOrFileId,
        string caption = null,
        ParseMode parseMode = Defaults.DefaultParseMode,
        IEnumerable<MessageEntity> captionEntities = null,
        int? duration = null,
        string performer = null,
        string title = null,
        string thumbnail = null,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendAudioAsync(
            chatId: message.Chat.Id,
            messageThreadId: message.IsTopicMessage is true ? message.MessageId : null,
            audio: InputFile.FromString(audioUrlOrFileId),
            caption: caption,
            parseMode: parseMode,
            captionEntities: captionEntities,
            duration: duration,
            performer: performer,
            title: title,
            thumbnail: InputFile.FromString(thumbnail),
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithDocumentAsync(this ITelegramBotClient botClient,
        Message message,
        string documentUrlOrFileId,
        string thumbnail = null,
        string caption = null,
        ParseMode parseMode = Defaults.DefaultParseMode,
        IEnumerable<MessageEntity> captionEntities = null,
        bool disableContentTypeDetection = default,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendDocumentAsync(
            chatId: message.Chat.Id,
            document: InputFile.FromString(documentUrlOrFileId),
            thumbnail: InputFile.FromString(thumbnail),
            caption: caption,
            parseMode: parseMode,
            captionEntities: captionEntities,
            disableContentTypeDetection: disableContentTypeDetection,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> AnswerMessageWithVoiceAsync(this ITelegramBotClient botClient,
        Message message,
        string voiceUrlOrFileId,
        string caption = null,
        ParseMode parseMode = Defaults.DefaultParseMode,
        IEnumerable<MessageEntity> captionEntities = null,
        int? duration = null,
        bool disableNotification = default,
        bool protectContent = default,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply) replyParameters = GetUpdatedReplyParameters(replyParameters, message.MessageId);

        return await botClient.SendVoiceAsync(
            chatId: message.Chat.Id,
            voice: InputFile.FromString(voiceUrlOrFileId),
            caption: caption,
            parseMode: parseMode,
            captionEntities: captionEntities,
            duration: duration,
            disableNotification: disableNotification,
            protectContent: protectContent,
            replyParameters: replyParameters,
            replyMarkup: replyMarkup
        );
    }

    public static async Task<Message> EditMessageTextOrCaptionAsync(this ITelegramBotClient botClient,
        Message message,
        string textOrCaption,
        ParseMode parseMode = Defaults.DefaultParseMode,
        InlineKeyboardMarkup replyMarkup = null)
    {
        if (!string.IsNullOrEmpty(message.Text))
        {
            return await botClient.EditMessageTextAsync(
                chatId: message.Chat.Id,
                messageId: message.MessageId,
                text: textOrCaption,
                parseMode: parseMode,
                replyMarkup: replyMarkup
            );
        }
        
        return await botClient.EditMessageCaptionAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            caption: textOrCaption,
            parseMode: parseMode,
            replyMarkup: replyMarkup
        );
    }

    public static ReplyParameters GetUpdatedReplyParameters(ReplyParameters replyParameters, int messageId)
    {
        replyParameters ??= new ReplyParameters();
        replyParameters.MessageId = messageId;
        return replyParameters;
    }
}