using Hangfire;
using Microsoft.Extensions.Logging;
using RouteWise.Service.Interfaces;

namespace RouteWise.Service.Services;

public class HangfireService(
    ILogger<HangfireService> logger,
    IRecurringJobManagerV2 jobManager,
    ITrailerService trailerService) : IHangfireService
{
    private readonly ILogger<HangfireService> _logger = logger;
    private readonly IRecurringJobManager _jobManager = jobManager;
    private readonly ITrailerService _trailerService = trailerService;
    
    public void Start(CancellationToken cancellationToken = default)
    {
        _jobManager.AddOrUpdate("UpdateTrailers",() => _trailerService.UpdateTrailersStatesAsync(default), "*/5 * * * *");
        _logger.LogInformation("Job for updating trailer states has been executed.");
    }
}