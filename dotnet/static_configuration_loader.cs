using Microsoft.Extensions.Configuration;

public static class StaticConfig
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetSetting(string key)
    {
        return _configuration[key];
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    var configuration = config.Build();
    StaticConfig.Initialize(configuration); // Initialize static config loader
});

builder.ConfigureServices(services =>
{
    services.AddHostedService<MyBackgroundService>();
});

await builder.Build().RunAsync();

public static class MyStaticClass
{
    public static void MyStaticFunction()
    {
        // Access configuration via the static loader
        var apiKey = StaticConfig.GetSetting("MySettings:ApiKey");
        var apiUrl = StaticConfig.GetSetting("MySettings:ApiUrl");

        Console.WriteLine($"API Key: {apiKey}");
        Console.WriteLine($"API URL: {apiUrl}");
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

public class MyBackgroundService : BackgroundService
{
    private readonly ILogger<MyBackgroundService> _logger;

    public MyBackgroundService(ILogger<MyBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background service is running.");

        // Call the static function (it retrieves settings from StaticConfig)
        MyStaticClass.MyStaticFunction();

        return Task.CompletedTask;
    }
}