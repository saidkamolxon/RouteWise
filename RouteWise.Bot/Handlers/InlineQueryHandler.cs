using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace RouteWise.Bot.Handlers;

public partial class UpdateHandler
{
    public async Task BotOnInlineQueryReceived(InlineQuery query, CancellationToken cancellationToken = default)
    {
        if (!userService.IsPermittedUser(query.From.Id)) return;

        this.logger.LogInformation("Inline query received: {query}", query.Query);

        var queryText = query.Query;

        if (queryText.StartsWith("pick"))
        {
            await botClient.AnswerInlineQueryAsync(query.Id, []);
        }
        
    }
}