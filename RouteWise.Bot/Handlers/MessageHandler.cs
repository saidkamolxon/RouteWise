using RouteWise.Bot.Constants.Keyboard;
using RouteWise.Bot.Constants;
using RouteWise.Bot.States;
using RouteWise.Service.Interfaces;
using Telegram.Bot.Types;
using RouteWise.Bot.Extensions;

namespace RouteWise.Bot.Services;

public partial class UpdateHandlerService
{
    private async Task BotOnMessageReceived(Message message)
    {
        if (!_userService.IsPermittedUser(message.From.Id))
        {
            await _botClient.AnswerMessageAsync(message,
                text: "You can't use the bot. Firstly, request an access from the owner 👇",
                replyMarkup: InlineKeyboards.RequestKeyboard);
            return;
        }

        if (message.Text.StartsWith(BotCommands.Start))
        {
            await _stateMachine.SetState(new Models.StateValuesDto { ChatId = message.Chat.Id, UserId = message.From.Id }, new InitialState(_stateMachine));
        }

        using (var scope = _stateMachine.ServiceProvider.CreateScope())
        {
            var truckService = scope.ServiceProvider.GetRequiredService<ITruckService>();
            var truckNumbers = await truckService.GetTruckNumbersAsync();

            var command = message.GetBotCommand();

            if (command != null && truckNumbers.Contains(command[1..]))
            {
                var truck = await truckService.GetAsync(command[1..]);
                await _botClient.AnswerMessageWithVenueAsync(message, (float)truck.Coordinates.Latitude, (float)truck.Coordinates.Longitude, $"{truck.Name} -> {truck.LastEventAt}", truck.Address);
            }
        }

        var result = await _stateMachine.FireEvent(message);

        if (!string.IsNullOrEmpty(result.PhotoUrl))
            await _botClient.AnswerMessageWithPhotoAsync(message, result.PhotoUrl, result.AnswerMessage, isReply: true);
        else
            await _botClient.AnswerMessageAsync(message, result.AnswerMessage, isReply: true);

        _logger.LogInformation("Message received of type: {message.Type}", message.Type);
    }
}
