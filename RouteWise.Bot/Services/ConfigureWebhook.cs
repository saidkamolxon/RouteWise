using RouteWise.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;

namespace RouteWise.Bot.Services;

public class ConfigureWebhook : IHostedService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfiguration _botConfig;

    public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var botClient = scope.ServiceProvider.GetService<ITelegramBotClient>();

        var webhookAddress = $@"{_botConfig.HostAddress}/bot/{_botConfig.Token}";

        _logger.LogInformation("Setting webhook...");

        await botClient.SendMessageAsync(new() {
            ChatId = _botConfig.OwnerId,
            Text = "Bot has been started."
        });

        await botClient.SetWebhookAsync(new SetWebhookRequest()
        {
            Url = webhookAddress,
            AllowedUpdates = Array.Empty<UpdateType>(),
            DropPendingUpdates = true
        }, cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var botClient = scope.ServiceProvider.GetService<ITelegramBotClient>();

        _logger.LogInformation("Webhook removing...");

        await botClient.SendMessageAsync(new()
        {
            ChatId = _botConfig.OwnerId,
            Text = "Bot has been stopped."
        });
    }
}
