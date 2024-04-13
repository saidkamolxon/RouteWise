using Telegram.Bot.Types;

namespace RouteWise.Bot.Models;

public class MessageEvent
{
    public long ChatId { get; set; }
    public Message Message { get; set; }
}