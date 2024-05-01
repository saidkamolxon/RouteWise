using Microsoft.Extensions.Primitives;
using RouteWise.Bot.Constants;
using Telegram.Bot;
using Telegram.Bot.Requests;
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
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (message == null) return null;

        if(isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendMessageAsync(new SendMessageRequest
        {
            ChatId = message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
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
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendPhotoAsync(new SendPhotoRequest
        {
            ChatId = message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
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

    public static async Task<Message> AnswerMessageWithVenueAsync(this ITelegramBotClient botClient,
        Message message,
        float latitude,
        float longitude,
        string title,
        string address,
        string fourSquareId = null,
        string fourSquareType = null,
        string googlePlaceId = null,
        string googlePlaceType = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false)
    {
        if (isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendVenueAsync(new SendVenueRequest
        {
            ChatId = message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
            Address = address,
            Title = title,
            Latitude = latitude,
            Longitude = longitude,
            FoursquareId = fourSquareId,
            FoursquareType = fourSquareType,
            GooglePlaceId = googlePlaceId,
            GooglePlaceType = googlePlaceType,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
    }

    public static async Task<Message[]> AnswerMessageWithMediaGroupAsync(this ITelegramBotClient botClient,
        Message message,
        IEnumerable<IAlbumInputMedia> media,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        bool isReply = false
        )
    {
        if (isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendMediaGroupAsync(new SendMediaGroupRequest
        {
            ChatId = message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
            Media = media,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters
        });
    }

    public static async Task<Message> AnswerMessageWithStickerAsync(this ITelegramBotClient botClient,
        Message message,
        string stickerUrlOrFileId,
        string emoji = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendStickerAsync(new SendStickerRequest
        {
            ChatId= message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
            Sticker = InputFile.FromString(stickerUrlOrFileId),
            Emoji = emoji,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
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
        bool? hasSpoiler = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendAnimationAsync(new SendAnimationRequest
        {
            ChatId = message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
            Animation = InputFile.FromString(animationUrlOrFileId),
            Duration = duration,
            Width = width,
            Height = height,
            Thumbnail = InputFile.FromString(thumbnail),
            Caption = caption,
            ParseMode = parseMode,
            CaptionEntities = captionEntities,
            HasSpoiler = hasSpoiler,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
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
        bool? hasSpoiler = null,
        bool? supportsStreaming = null,
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendVideoAsync(new SendVideoRequest
        {
            ChatId= message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageThreadId : null,
            Video = InputFile.FromString(videoUrlOrFileId),
            Duration = duration,
            Width = width,
            Height = height,
            Thumbnail = InputFile.FromString(thumbnail),
            Caption = caption,
            ParseMode = parseMode,
            CaptionEntities = captionEntities,
            HasSpoiler = hasSpoiler,
            SupportsStreaming = supportsStreaming,
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
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
        bool? disableNotification = null,
        bool? protectContent = null,
        ReplyParameters replyParameters = null,
        IReplyMarkup replyMarkup = null,
        bool isReply = false
        )
    {
        if (isReply)
        {
            replyParameters = UpdateReplyParameters(replyParameters, message.MessageId);
        }

        return await botClient.SendAudioAsync(new SendAudioRequest
        {
            ChatId = message.Chat.Id,
            MessageThreadId = message.IsTopicMessage is true ? message.MessageId : null,
            Audio = InputFile.FromString(audioUrlOrFileId),
            Caption = caption,
            ParseMode = parseMode,
            CaptionEntities = captionEntities,
            Duration = duration,
            Performer = performer,
            Title = title,
            Thumbnail = InputFile.FromString(thumbnail),
            DisableNotification = disableNotification,
            ProtectContent = protectContent,
            ReplyParameters = replyParameters,
            ReplyMarkup = replyMarkup
        });
    }

    public static ReplyParameters UpdateReplyParameters(ReplyParameters replyParameters, int messageId)
    {
        replyParameters ??= new ReplyParameters();
        replyParameters.MessageId = messageId;
        return replyParameters;
    }
}