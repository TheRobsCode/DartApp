using MauiEx;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Extensions.Logging;

namespace Dart
{
    public partial class MainPage : ContentPage
    {
        private readonly ILogger<MainPage> _logger;
        private readonly IDartService _dartService;
        private readonly ICacheService _cacheService;
        private readonly IAppActionsService _appActionsService;

        private string[] _suggestions = [];
        private readonly List<string> _recent;

        public MainPage(ILogger<MainPage> logger, IDartService dartService, ICacheService cacheService, IAppActionsService appActionsService)
        {
            _logger = logger;
            _logger.LogInformation("MainPage initializing");

            InitializeComponent();
            _dartService = dartService;
            _cacheService = cacheService;
            _appActionsService = appActionsService;

            AutoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
            AutoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            _recent = _cacheService.Get<List<string>>("recent") ?? new List<string>();
            _logger.LogInformation("Loaded {RecentCount} recent stations from cache", _recent.Count);

            Task.Run(async ()=> await SetupSuggestionsAsync().ConfigureAwait(false));
            DrawRecentList();

            // Update app shortcuts on startup with existing recent stations
            if (_recent.Count > 0)
            {
                _logger.LogDebug("Updating app shortcuts with {Count} recent stations", _recent.Count);
                Task.Run(async () => await _appActionsService.UpdateRecentStationsAsync(_recent));
            }
        }
        private bool _isSubscribedToLifecycle = false;
        protected override void OnAppearing()
        {
            _logger.LogDebug("MainPage appearing");
            base.OnAppearing();

            // Subscribe to app lifecycle events
            if (!_isSubscribedToLifecycle)
            {
                _logger.LogDebug("Subscribing to app lifecycle events");
                App.AppActivated += OnAppActivated;
                _isSubscribedToLifecycle = true;
            }
        }
        protected void OnAppActivated()
        {
            _logger.LogInformation("App activated, checking for pending station");
            base.OnAppearing();

            // Check if there's a pending station from app shortcut
            var pendingStation = _appActionsService.GetPendingStationAsync();
            if (!string.IsNullOrEmpty(pendingStation))
            {
                _logger.LogInformation("Navigating to pending station: {Station}", pendingStation);
                ShowStationTimeTable(pendingStation);
            }
        }

        private async Task SetupSuggestionsAsync()
        {
            try
            {
                _suggestions = _cacheService.Get<string[]>("stations");
                if (_suggestions != null && _suggestions.Length > 0)
                {
                    _logger.LogDebug("Using cached station suggestions, count: {Count}", _suggestions.Length);
                    return;
                }

                _logger.LogInformation("No cached stations found, fetching from API");
                var stations = await _dartService.GetStations();
                _suggestions = stations.Select(x => x.StationDesc).ToArray();
                _cacheService.Set("stations", _suggestions);
                _logger.LogInformation("Cached {Count} station suggestions", _suggestions.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to setup station suggestions");
            }
        }

        private void DrawRecentList()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RecentStations.Clear();

                if (_recent == null || _recent.Count == 0)
                {
                    EmptyState.IsVisible = true;
                    RecentStationsHeader.IsVisible = false;
                    return;
                }

                EmptyState.IsVisible = false;
                RecentStationsHeader.IsVisible = true;

                foreach (var recent in _recent)
                {
                    var border = new Border
                    {
                        StrokeThickness = 0,
                        Padding = new Thickness(16, 12),
                        Margin = new Thickness(0, 2),
                        BackgroundColor = Application.Current.RequestedTheme == AppTheme.Dark
                            ? Color.FromArgb("#2b2b2b")
                            : Color.FromArgb("#f5f5f5")
                    };
                    border.StrokeShape = new RoundRectangle { CornerRadius = 10 };

                    var grid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = GridLength.Auto }
                        }
                    };

                    var label = new Label
                    {
                        Text = recent,
                        FontSize = 16,
                        VerticalOptions = LayoutOptions.Center
                    };

                    var arrow = new Label
                    {
                        Text = "›",
                        FontSize = 24,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Application.Current.RequestedTheme == AppTheme.Dark
                            ? Color.FromArgb("#888888")
                            : Color.FromArgb("#666666")
                    };

                    grid.Add(label, 0, 0);
                    grid.Add(arrow, 1, 0);
                    border.Content = grid;

                    var recentTap = new TapGestureRecognizer();
                    recentTap.Tapped += (s, e) => ShowStationTimeTable(recent);
                    border.GestureRecognizers.Add(recentTap);

                    RecentStations.Add(border);
                }
            });
        }

        private void AutoSuggestBox_TextChanged(object? sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput || sender is not AutoSuggestBox box) return;
            var filtered = _suggestions
                .Where(x => x.Contains(box.Text, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            box.ItemsSource = filtered;
        }

        private void AutoSuggestBox_SuggestionChosen(object? sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var station = args.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(station))
                return;

            _logger.LogInformation("User selected station: {Station}", station);

            if (!_recent.Contains(station))
            {
                _recent.Add(station);
                _recent.Sort(StringComparer.CurrentCultureIgnoreCase);
                _cacheService.Set("recent", _recent);
                _logger.LogDebug("Added station to recent list: {Station}", station);
                DrawRecentList();

                // Update app shortcuts
                Task.Run(async () => await _appActionsService.UpdateRecentStationsAsync(_recent));
            }

            ShowStationTimeTable(station);
        }

        private void ShowStationTimeTable(string station)
        {
            _logger.LogInformation("Navigating to timetable for station: {Station}", station);
            try
            {
                Navigation.PushAsync(new TimeTable(_dartService, station));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to navigate to timetable for station: {Station}", station);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object? sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            // Implementation can be added if needed
        }

        private async void ViewLogsButton_Clicked(object? sender, EventArgs e)
        {
            _logger.LogInformation("User clicked View Logs button");
            try
            {
                var loggerFactory = Handler?.MauiContext?.Services.GetService<ILoggerFactory>();
                var logsPageLogger = loggerFactory?.CreateLogger<LogsPage>();
                await Navigation.PushAsync(new LogsPage(logsPageLogger));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to navigate to logs page");
            }
        }
    }
}
