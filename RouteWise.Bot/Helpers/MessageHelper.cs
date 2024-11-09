namespace RouteWise.Bot.Helpers;

public static class MessageHelper
{
    public static ICollection<string> SplitMessage(string message, int maxLength = 4096, char separator = '\n')
    {
        ICollection<string> result = [];

        while (message.Length > maxLength)
        {
            int splitIndex = message.LastIndexOf(separator, maxLength);
            if (splitIndex == -1)
                splitIndex = maxLength;
            
            result.Add(message[..splitIndex]);
            message = message[(splitIndex + 1)..];
        }

        if (!string.IsNullOrEmpty(message))
            result.Add(message);

        return result;
    }
}
