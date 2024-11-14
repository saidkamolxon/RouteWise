namespace RouteWise.Bot.Models;

public class StateValuesDto
{
    public StateValuesDto()
    {
    }

    public StateValuesDto(long chatId, long userId, string distanceOrigin)
    {
        ChatId = chatId;
        UserId = userId;
        DistanceOrigin = distanceOrigin;
    }

    public long ChatId { get; set; }
    public long UserId { get; set; }
    public string DistanceOrigin { get; set; }
    public string DistanceDestination { get; set; }
}
