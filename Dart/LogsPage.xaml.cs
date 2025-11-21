using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Shapes;
using System.Text;
using Path = System.IO.Path;

namespace Dart
{
    public partial class LogsPage : ContentPage
    {
        private readonly ILogger<LogsPage> _logger;
        private string _logsDirectory;

        public LogsPage(ILogger<LogsPage> logger)
        {
            _logger = logger;
            InitializeComponent();

            _logsDirectory = Path.Combine(FileSystem.AppDataDirectory, "logs");
            LogPathLabel.Text = $"Log location: {_logsDirectory}";

            RefreshButton.Clicked += async (s, e) => await LoadLogsAsync();
            ShareButton.Clicked += async (s, e) => await ShareLogsAsync();
            ClearButton.Clicked += async (s, e) => await ClearOldLogsAsync();

            _logger.LogInformation("LogsPage initialized");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadLogsAsync();
        }

        private async Task LoadLogsAsync()
        {
            try
            {
                _logger.LogDebug("Loading log files from: {LogsDirectory}", _logsDirectory);
                LogsContainer.Clear();

                if (!Directory.Exists(_logsDirectory))
                {
                    _logger.LogWarning("Logs directory does not exist: {LogsDirectory}", _logsDirectory);
                    ShowNoLogsMessage();
                    return;
                }

                var logFiles = Directory.GetFiles(_logsDirectory, "dart-*.txt")
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .ToArray();

                if (logFiles.Length == 0)
                {
                    _logger.LogInformation("No log files found");
                    ShowNoLogsMessage();
                    return;
                }

                _logger.LogInformation("Found {LogFileCount} log files", logFiles.Length);

                foreach (var logFile in logFiles)
                {
                    await AddLogFileCard(logFile);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load log files");
                await DisplayAlert("Error", "Failed to load log files: " + ex.Message, "OK");
            }
        }

        private async Task AddLogFileCard(string logFilePath)
        {
            var fileInfo = new FileInfo(logFilePath);
            var fileName = fileInfo.Name;
            var fileSize = FormatFileSize(fileInfo.Length);
            var lastModified = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

            var border = new Border
            {
                StrokeThickness = 1,
                Stroke = Application.Current.RequestedTheme == AppTheme.Dark
                    ? Color.FromArgb("#3a3a3a")
                    : Color.FromArgb("#e0e0e0"),
                Padding = new Thickness(16, 12),
                BackgroundColor = Application.Current.RequestedTheme == AppTheme.Dark
                    ? Color.FromArgb("#2b2b2b")
                    : Color.FromArgb("#ffffff")
            };
            border.StrokeShape = new RoundRectangle { CornerRadius = 12 };

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                RowSpacing = 4
            };

            var fileNameLabel = new Label
            {
                Text = fileName,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };

            var detailsLabel = new Label
            {
                Text = $"Size: {fileSize} | Modified: {lastModified}",
                FontSize = 12,
                TextColor = Application.Current.RequestedTheme == AppTheme.Dark
                    ? Color.FromArgb("#b0b0b0")
                    : Color.FromArgb("#666666")
            };

            var buttonsLayout = new HorizontalStackLayout
            {
                Spacing = 8
            };

            var viewButton = new Button
            {
                Text = "View",
                FontSize = 12,
                Padding = new Thickness(12, 4),
                BackgroundColor = Color.FromArgb("#512BD4"),
                TextColor = Colors.White,
                CornerRadius = 6
            };
            viewButton.Clicked += async (s, e) => await ViewLogFileAsync(logFilePath);

            var shareButton = new Button
            {
                Text = "Share",
                FontSize = 12,
                Padding = new Thickness(12, 4),
                BackgroundColor = Color.FromArgb("#4caf50"),
                TextColor = Colors.White,
                CornerRadius = 6
            };
            shareButton.Clicked += async (s, e) => await ShareSingleLogAsync(logFilePath);

            buttonsLayout.Add(viewButton);
            buttonsLayout.Add(shareButton);

            grid.Add(fileNameLabel, 0, 0);
            grid.Add(detailsLabel, 0, 1);
            grid.Add(buttonsLayout, 0, 2);

            border.Content = grid;
            LogsContainer.Add(border);
        }

        private async Task ViewLogFileAsync(string logFilePath)
        {
            try
            {
                _logger.LogInformation("Viewing log file: {LogFile}", logFilePath);

                // Read last 1000 lines to avoid memory issues with large files
                var lines = File.ReadLines(logFilePath).Reverse().Take(1000).Reverse().ToList();
                var content = string.Join(Environment.NewLine, lines);

                var scrollView = new ScrollView
                {
                    Content = new Label
                    {
                        Text = content,
                        FontFamily = "Courier New",
                        FontSize = 10,
                        Padding = new Thickness(10)
                    }
                };

                var contentPage = new ContentPage
                {
                    Title = Path.GetFileName(logFilePath),
                    Content = scrollView
                };

                await Navigation.PushAsync(contentPage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to view log file: {LogFile}", logFilePath);
                await DisplayAlert("Error", "Failed to view log file: " + ex.Message, "OK");
            }
        }

        private async Task ShareSingleLogAsync(string logFilePath)
        {
            try
            {
                _logger.LogInformation("Sharing log file: {LogFile}", logFilePath);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share Log File",
                    File = new ShareFile(logFilePath)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to share log file: {LogFile}", logFilePath);
                await DisplayAlert("Error", "Failed to share log file: " + ex.Message, "OK");
            }
        }

        private async Task ShareLogsAsync()
        {
            try
            {
                _logger.LogInformation("Sharing all log files");

                if (!Directory.Exists(_logsDirectory))
                {
                    await DisplayAlert("No Logs", "No log files found to share.", "OK");
                    return;
                }

                var logFiles = Directory.GetFiles(_logsDirectory, "dart-*.txt");

                if (logFiles.Length == 0)
                {
                    await DisplayAlert("No Logs", "No log files found to share.", "OK");
                    return;
                }

                // Create a combined log file
                var combinedLogPath = Path.Combine(FileSystem.CacheDirectory, "dart-combined-logs.txt");
                using (var writer = new StreamWriter(combinedLogPath, false))
                {
                    foreach (var logFile in logFiles.OrderBy(f => f))
                    {
                        await writer.WriteLineAsync($"========== {Path.GetFileName(logFile)} ==========");
                        await writer.WriteLineAsync(await File.ReadAllTextAsync(logFile));
                        await writer.WriteLineAsync();
                    }
                }

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share Dart Logs",
                    File = new ShareFile(combinedLogPath)
                });

                _logger.LogInformation("Shared combined log file with {FileCount} files", logFiles.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to share logs");
                await DisplayAlert("Error", "Failed to share logs: " + ex.Message, "OK");
            }
        }

        private async Task ClearOldLogsAsync()
        {
            try
            {
                var result = await DisplayAlert(
                    "Clear Old Logs",
                    "This will delete all log files older than 2 days. Continue?",
                    "Yes",
                    "No");

                if (!result)
                    return;

                _logger.LogInformation("Clearing old log files");

                if (!Directory.Exists(_logsDirectory))
                {
                    await DisplayAlert("No Logs", "No logs directory found.", "OK");
                    return;
                }

                var logFiles = Directory.GetFiles(_logsDirectory, "dart-*.txt");
                var cutoffDate = DateTime.Now.AddDays(-2);
                var deletedCount = 0;

                foreach (var logFile in logFiles)
                {
                    var fileInfo = new FileInfo(logFile);
                    if (fileInfo.LastWriteTime < cutoffDate)
                    {
                        File.Delete(logFile);
                        deletedCount++;
                        _logger.LogInformation("Deleted old log file: {LogFile}", logFile);
                    }
                }

                await DisplayAlert("Success", $"Deleted {deletedCount} old log file(s).", "OK");
                await LoadLogsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clear old logs");
                await DisplayAlert("Error", "Failed to clear old logs: " + ex.Message, "OK");
            }
        }

        private void ShowNoLogsMessage()
        {
            var label = new Label
            {
                Text = "No log files found.\n\nLogs will appear here after the app has been running.",
                FontSize = 14,
                TextColor = Colors.Gray,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 50, 0, 0)
            };

            LogsContainer.Add(label);
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
