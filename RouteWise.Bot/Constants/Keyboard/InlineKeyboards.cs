using Telegram.Bot.Types.ReplyMarkups;

namespace RouteWise.Bot.Constants.Keyboard;

public static class InlineKeyboards
{
    public static readonly InlineKeyboardMarkup RequestKeyboard = new(new[]
    {
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Request an access", callbackData: "request_an_access")
        }
    });
    
    public static readonly InlineKeyboardMarkup ResponseKeyboard = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(text: "Accept ✅", callbackData: "accept_the_request"),
            InlineKeyboardButton.WithCallbackData(text: "Reject ❌", callbackData: "reject_the_request"),
        }
    });
}
