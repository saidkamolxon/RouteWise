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
            InlineKeyboardButton.WithCallbackData(text: "✅ Accept", callbackData: "accept_the_request"),
            InlineKeyboardButton.WithCallbackData(text: "❌ Reject", callbackData: "reject_the_request"),
        }
    });

    public static readonly InlineKeyboardMarkup BroadcastMessageKeyboard = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(text: "Brokers", callbackData: "send_to_brokers"),
            InlineKeyboardButton.WithCallbackData(text: "Carriers", callbackData: "send_to_carriers"),
            InlineKeyboardButton.WithCallbackData(text: "Drivers", callbackData: "send_to_drivers"),
        ],
        [
            InlineKeyboardButton.WithCallbackData("❌ Cancel", callbackData: "cancel")
        ]
    ]);

    public static InlineKeyboardMarkup UnitKeyboard(Dictionary<string, string> units)
    {
        var keyboard = new InlineKeyboardMarkup();

        foreach (var unit in units)
            keyboard.AddButton(InlineKeyboardButton.WithCallbackData(text: unit.Key, callbackData: unit.Value));

        return keyboard;
    }
}
