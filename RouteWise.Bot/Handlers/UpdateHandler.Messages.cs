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
                await botClient.SendPhotoAsync(message.Chat.Id, InputFile.FromString(truck.PhotoUrl), caption: truck.ToString(), parseMode: ParseMode.Html);
                return;
            }
        }

        var result = await stateMachine.FireEvent(message);

        if (result.IsMediaGroup)
        {
            var docs = result.Files.Select(f => new InputMediaDocument(InputFile.FromString(f.FileId)));
            await botClient.SendMediaGroupAsync(message.Chat.Id, docs);
            return;
        }
        
        for (int i = 0; i < Math.Max(result.Texts.Count, result.Files.Count); i++)
        {
            switch (result.Type)
            {
                case MessageType.Photo:
                    await botClient.AnswerMessageWithPhotoAsync(message, result.Files.ElementAt(i).FileId, result.Texts.ElementAt(i), isReply: true);
                    break;

                case MessageType.Text:
                    await botClient.AnswerMessageAsync(message, result.Texts.ElementAt(i), isReply: i == 0);
                    break;
            }
        }

        logger.LogInformation("[{userId}] [{userName}] Message received of type: {messageType}", message.From.Id, message.From.GetFullName(), message.Type);
    }
}
