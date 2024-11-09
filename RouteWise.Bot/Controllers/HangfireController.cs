﻿using Hangfire;
using Microsoft.AspNetCore.Mvc;
using RouteWise.Service.Interfaces;
using RouteWise.Service.Services;

namespace RouteWise.Bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HangfireController(IRecurringJobManagerV2 jobManager, ITrailerService trailerService, ILandmarkService landmarkService) : ControllerBase
{
    private readonly IRecurringJobManager jobManager = jobManager;
    private readonly ITrailerService trailerService = trailerService;
    private readonly ILandmarkService landmarkService = landmarkService;

    [HttpGet("job")]
    public void RunHangfire()
    {
        jobManager.AddOrUpdate(
        "UpdateTrailers",
        () => trailerService.UpdateTrailersStatesAsync(default),
        Cron.MinuteInterval(5));
    }

    [HttpGet("remove-redundant-landmarks")]
    public void RunRemoval()
    {
        jobManager.AddOrUpdate(
            "RemoveRedundantLandmarks",
            () => landmarkService.RemoveRedundantLandmarks(default),
            Cron.Daily);
    }
}
