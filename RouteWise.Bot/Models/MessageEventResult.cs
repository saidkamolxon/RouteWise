namespace RouteWise.Bot.Interfaces;

public class MessageEventResult
{
    public string AnswerMessage { get; set; }
    public string PhotoUrl { get; set; }
    public IEnumerable<Uri> FileUrls { get; set; }

    public static implicit operator MessageEventResult(string answerMessage)
        => new() { AnswerMessage = answerMessage };
}
