using Microsoft.EntityFrameworkCore;
using RouteWise.Bot.Extensions;
using RouteWise.Bot.Models;
using RouteWise.Bot.Services;
using RouteWise.Data.Contexts;
using RouteWise.Service.Mappers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System.Collections.ObjectModel;
using Telegram.Bot;
using Telegram.Bot.Serialization;




var builder = WebApplication.CreateBuilder(args);

#region SerilogConfiguration
var connectionString = "Host=my_host;Port=5432;Database=my_db;Username=my_user;Password=my_password";

// Define the column options for the PostgreSQL sink
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("C:\\Users\\99894\\Source\\Repos\\saidkamolxon\\RouteWise\\RouteWise.Bot\\Logs\\log.txt", rollingInterval: RollingInterval.Minute)
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Bismillahir Rohmanir Rohiym");
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
#endregion

BotConfiguration botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    JsonSerializerOptionsProvider.Configure(options.JsonSerializerOptions);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => 
        new TelegramBotClient(botConfig.Token, httpClient));

builder.Services.AddScoped<UpdateHandlerService>();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"))
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.MapControllerRoute(name: "tgwebhook", pattern: $"bot/{botConfig.Token}",
    defaults: new { controller = "Webhook", action = "Post" });

app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();