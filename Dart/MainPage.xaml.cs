using MauiEx;
using Microsoft.Maui.Controls.Shapes;

namespace Dart
{
    public partial class MainPage : ContentPage
    {
        private readonly IDartService _dartService;
        private readonly ICacheService _cacheService;
        private readonly IAppActionsService _appActionsService;

        private string[] _suggestions = [];
        private readonly List<string> _recent;

        public MainPage(IDartService dartService, ICacheService cacheService, IAppActionsService appActionsService)
        {
            InitializeComponent();
            _dartService = dartService;
            _cacheService = cacheService;
            _appActionsService = appActionsService;

            AutoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
            AutoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            _recent = _cacheService.Get<List<string>>("recent") ?? new List<string>();
            Task.Run(async ()=> await SetupSuggestionsAsync().ConfigureAwait(false));
            DrawRecentList();

            // Update app shortcuts on startup with existing recent stations
            if (_recent.Count > 0)
            {
                Task.Run(async () => await _appActionsService.UpdateRecentStationsAsync(_recent));
            }
        }
        private bool _isSubscribedToLifecycle = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Subscribe to app lifecycle events
            if (!_isSubscribedToLifecycle)
            {
                App.AppActivated += OnAppActivated;
                _isSubscribedToLifecycle = true;
            }
        }
        protected void OnAppActivated()
        {
            base.OnAppearing();

            // Check if there's a pending station from app shortcut
            var pendingStation = _appActionsService.GetPendingStationAsync();
            if (!string.IsNullOrEmpty(pendingStation))
            {
                ShowStationTimeTable(pendingStation);
            }
        }

        private async Task SetupSuggestionsAsync()
        {
            _suggestions = _cacheService.Get<string[]>("stations");
            if (_suggestions != null && _suggestions.Length > 0)
                return;

            var stations = await _dartService.GetStations();
            _suggestions = stations.Select(x => x.StationDesc).ToArray();
            _cacheService.Set("stations", _suggestions);
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

            if (!_recent.Contains(station))
            {
                _recent.Add(station);
                _recent.Sort(StringComparer.CurrentCultureIgnoreCase);
                _cacheService.Set("recent", _recent);
                DrawRecentList();

                // Update app shortcuts
                Task.Run(async () => await _appActionsService.UpdateRecentStationsAsync(_recent));
            }

            ShowStationTimeTable(station);
        }

        private void ShowStationTimeTable(string station)
        {
            Navigation.PushAsync(new TimeTable(_dartService, station));
        }

        private void AutoSuggestBox_QuerySubmitted(object? sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            // Implementation can be added if needed
        }
    }
}
