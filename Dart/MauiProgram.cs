using MauiEx;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Dart
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Configure Serilog with async file writing for better performance
            var logPath = Path.Combine(FileSystem.AppDataDirectory, "logs", "dart-.txt");

            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Maui", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(a => a.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                    buffered: true))
                .WriteTo.Debug()
                .CreateLogger();

            Log.Information("Starting Dart application");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseMauiEx();

            // Add Serilog to the logging infrastructure
            builder.Logging.AddSerilog(Log.Logger, dispose: true);

            builder.Services.AddTransient<IDartService,DartService>();
            builder.Services.AddTransient<ICacheService, CacheService>();
            builder.Services.AddSingleton<IAppActionsService, AppActionsService>();

            Log.Information("Dart application configured successfully");

            return builder.Build();
        }
    }
}
