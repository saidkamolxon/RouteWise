using AutoMapper;
using RouteWise.Bot.Constants;
using RouteWise.Domain.Enums;
using RouteWise.Service.DTOs.User;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Services;

public class UpdateHandlerService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IUserService _userService;
    private readonly ILandmarkService _landmarkService;
    private readonly IMapper _mapper;

    public UpdateHandlerService(ILogger<ConfigureWebhook> logger,
        ITelegramBotClient botClient,
        IGoogleMapsService googleMapsService,
        IUserService userService,
        ILandmarkService landmarkService,
        IMapper mapper)
    {
        _logger = logger;
        _botClient = botClient;
        _googleMapsService = googleMapsService;
        _userService = userService;
        _landmarkService = landmarkService;
        _mapper = mapper;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message),
            UpdateType.CallbackQuery => BotOnCallBackQueryReceived(update.CallbackQuery),
            _ => UnknownUpdateTypeHandler(update)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            await HandlerErrorAsync(ex);
        }
    }

    private Task HandlerErrorAsync(Exception ex)
    {
        var ErrorMessage = ex switch
        {
            ApiRequestException exception => $"Telegram API Error:\n{exception.ErrorCode}",
            _ => ex.ToString()
        };

        _logger.LogInformation(ErrorMessage);

        return Task.CompletedTask;
    }

    private Task UnknownUpdateTypeHandler(Update update)
    {
        _logger.LogInformation($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }

    private async Task BotOnCallBackQueryReceived(CallbackQuery callbackQuery)
    {
        _logger.LogInformation($"CallbackQuery received: {callbackQuery}");

        await _botClient.AnswerCallbackQueryAsync(new AnswerCallbackQueryRequest()
        {
            CallbackQueryId = callbackQuery.Id,
            Text = "Hey you ... "
        });
    }

    private async Task BotOnMessageReceived(Message message)
    {
        if (!_userService.IsPermittedUser(message.From.Id))
        {
            //TODO Request an access
            return;
        }

        _logger.LogInformation($"Message received: {message.Type}");

        var reply = message.Type switch
        {
            MessageType.Text => BotOnTextMessageReceived(message),

            _ => _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = "something else",
                        ReplyParameters = { MessageId = message.MessageId }
                    })
        };

        try
        {
            await reply;
        }
        catch (Exception ex)
        {
            await HandlerErrorAsync(ex);
        }
    }

    private async Task BotOnTextMessageReceived(Message message)
    {
        var msg = message.Text ?? "";
        var user = await _userService.GetByTelegramIdAsync(message.From.Id);
        var command = msg.Split()[0];

        if (!BotCommands.Contains(command))
        {
            switch (user.CurrentStep)
            {
                case Step.Initial:
                    //TODO implement  method for sending messages to groups
                    break;

                case Step.DistanceOrigin:
                    SetOrigin(user, msg);
                    await _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = "Destination"
                    });
                    break;

                case Step.DistanceDestination:
                    SetDistance(user, msg);
                    await _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = await GetDistanceResult(user)
                    });
                    break;

                case Step.LandmarkStatus:
                    ReturnToInitialStep(user);
                    await _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = await GetLandmarkStatusResult(msg)
                    });
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            };
        }
        else
        {
            ReturnToInitialStep(user);

            switch(command)
            {
                case BotCommands.Start:
                    await _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = "Assalamu alaykum"
                    });
                    break;

                case BotCommands.MeasureDistance:
                    user.CurrentStep = Step.DistanceOrigin;
                    await _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = "Origin"
                    });
                    break;

                case BotCommands.GetLandmarkStatus:
                    user.CurrentStep = Step.LandmarkStatus;
                    await _botClient.SendMessageAsync(new()
                    {
                        ChatId = message.Chat.Id,
                        Text = "Enter the landmark name"
                    });
                    break;

                default:
                    break;
            }
        }

        await _userService.UpdateAsync(_mapper.Map<UserUpdateDto>(user));
    }

    private async Task<string> GetLandmarkStatusResult(string name)
    {
        var status = await _landmarkService.GetLandmarksByNameAsync(name);
        return status.ToString();
    }

    private static void ReturnToInitialStep(UserResultDto user)
    {
        user.CurrentStep = Step.Initial;
        user.StepValue.Origin = null;
        user.StepValue.Destination = null;
    }

    private async Task<string> GetDistanceResult(UserResultDto user)
    {
        var distance = await _googleMapsService.GetDistanceAsync(user.StepValue.Origin, user.StepValue.Destination);
        return distance;
    }

    private static void SetDistance(UserResultDto user, string destination)
    {
        user.StepValue.Destination = destination;
        user.CurrentStep = Step.Initial;
    }

    private static void SetOrigin(UserResultDto user, string origin)
    {
        user.StepValue.Origin = origin;
        user.CurrentStep = Step.DistanceDestination;
    }
}
