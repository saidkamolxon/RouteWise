using Hangfire;
using Microsoft.EntityFrameworkCore;
using RouteWise.Bot.Extensions;
using RouteWise.Bot.Handlers;
using RouteWise.Bot.Models;
using RouteWise.Bot.Services;
using RouteWise.Data.Contexts;
using RouteWise.Service.Mappers;
using Serilog;
using Telegram.Bot;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.WebHost.ConfigureKestrel(options =>
    options.ListenAnyIP(47278));

BotConfiguration botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    JsonBotAPI.Configure(options.JsonSerializerOptions));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => 
        new TelegramBotClient(botConfig.Token, httpClient));

builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<NotificationHandler>();

builder.Services.AddHangfire(config =>
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseInMemoryStorage()
    );

builder.Services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(15));

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"))
);

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.UseRouting();

app.UseCors();

app.MapControllerRoute(name: "tgwebhook", pattern: $"bot/{botConfig.Token}",
    defaults: new { controller = "Webhook", action = "Post" });

app.MapControllerRoute(name: "samsarawebhook", pattern: "samsara-webhook",
    defaults: new { controller = "SamsaraWebhook", action = "Post" });

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.Run();