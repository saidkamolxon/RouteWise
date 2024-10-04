using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Interfaces;

public class MessageEventResult()
{
    public MessageType Type { get; set; }
    public bool IsMediaGroup { get; set; }

    public ICollection<string> Texts { get; set; } = [];
    public string Text
    {
        get => Texts.FirstOrDefault();
        set => Texts.Add(value);
    }

    public ICollection<FileBase> Files { get; set; } = [];
    public FileBase File
    { 
        get => Files.FirstOrDefault();
        set => Files.Add(value);
    }
    
    public static implicit operator MessageEventResult(string text)
        => new() { Type = MessageType.Text, Text = text };
}
