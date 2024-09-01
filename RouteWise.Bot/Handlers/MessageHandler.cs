using RouteWise.Bot.Constants.Keyboard;
using RouteWise.Bot.Constants;
using RouteWise.Bot.States;
using RouteWise.Service.Interfaces;
using Telegram.Bot.Types;
using RouteWise.Bot.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Handlers;

public partial class UpdateHandler
{
    private async Task BotOnMessageReceived(Message message)
    {
        if (!userService.IsPermittedUser(message.From.Id))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
                "You can't use the bot. Firstly, request an access from the owner 👇",
                replyMarkup: InlineKeyboards.RequestKeyboard);
            return;
        }

        if (!string.IsNullOrEmpty(message.MediaGroupId))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Sorry, but media groups are not supported!",
                                                 replyParameters: message.MessageId);
            Console.WriteLine("Hello world");
            return;
        }

        if (message.Type != MessageType.Text)
        {
            await botClient.CopyMessageAsync(message.Chat.Id, message.Chat.Id, message.MessageId, replyMarkup: InlineKeyboards.BroadcastMessageKeyboard);
            await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            return;
        }

        if (message.Text.StartsWith(BotCommands.Start))
        {
            await stateMachine.SetState(new Models.StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new InitialState(stateMachine));
        }

        using (var scope = stateMachine.ServiceProvider.CreateScope())
        {
            var truckService = scope.ServiceProvider.GetRequiredService<ITruckService>();
            var truckNumbers = await truckService.GetTruckNumbersAsync();

            var command = message.GetBotCommand();

            if (command != null && truckNumbers.Contains(command[1..]))
            {
                var truck = await truckService.GetByNameAsync(command[1..]);
                await botClient.AnswerMessageWithVenueAsync(message, truck.Coordinates.Latitude, truck.Coordinates.Longitude, $"{truck.Name} 🔥 {truck.LastEventAt} | {truck.Speed}", truck.Address, isReply: true);
                return;
            }
        }

        var result = await stateMachine.FireEvent(message);

        if (result.FileUrls is not null && result.FileUrls.Any())
        {
            var documents = result.FileUrls.Select(u => new InputMediaDocument(InputFile.FromUri(u)));
            await botClient.SendMediaGroupAsync(message.Chat.Id, documents);
            return;
        }

        if (!string.IsNullOrEmpty(result.PhotoUrl))
            await botClient.AnswerMessageWithPhotoAsync(message, result.PhotoUrl, result.AnswerMessage, isReply: true);
        else
            await botClient.AnswerMessageAsync(message, result.AnswerMessage, isReply: true);

        logger.LogInformation("Message received of type: {message.Type}", message.Type);
    }
}
