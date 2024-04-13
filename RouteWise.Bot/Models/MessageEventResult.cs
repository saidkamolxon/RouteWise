namespace RouteWise.Bot.Interfaces;

public class MessageEventResult
{
    public string AnswerMessage { get; set; }

    public static implicit operator MessageEventResult(string answerMessage)
    {
        return new MessageEventResult
        {
            AnswerMessage = answerMessage
        };
    }
}