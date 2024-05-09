using RouteWise.Bot.Interfaces;
using RouteWise.Bot.Services;
using RouteWise.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Handlers;

public partial class UpdateHandler
{
    private readonly ILogger<ConfigureWebhook> logger;
    private readonly ITelegramBotClient botClient;
    private readonly IUserService userService;
    private readonly IStateMachine stateMachine;

    public UpdateHandler(ILogger<ConfigureWebhook> logger,
        ITelegramBotClient botClient,
        IUserService userService,
        IStateMachine stateMachine)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.userService = userService;
        this.stateMachine = stateMachine;
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

        this.logger.LogInformation(ErrorMessage);

        return Task.CompletedTask;
    }

    private Task UnknownUpdateTypeHandler(Update update)
    {
        this.logger.LogInformation($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }
}