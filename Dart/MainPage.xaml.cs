using MauiEx;

namespace Dart
{
    public partial class MainPage : ContentPage
    {
        private readonly IDartService _dartService;
        private readonly ICacheService _cacheService;

        private string[] _suggestions = [];
        private readonly List<string> _recent;

        public MainPage(IDartService dartService, ICacheService cacheService)
        {
            InitializeComponent();
            _dartService = dartService;
            _cacheService = cacheService;

            AutoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
            AutoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            _recent = _cacheService.Get<List<string>>("recent") ?? new List<string>();
            Task.Run(async ()=> await SetupSuggestionsAsync().ConfigureAwait(false));
            DrawRecentList();
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
            if (_recent == null || _recent.Count == 0)
                return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                RecentStations.Clear();
                foreach (var recent in _recent)
                {
                    var label = new Label { Text = recent, FontSize = 16, Margin = new Thickness(0, 5) };
                    var recentTap = new TapGestureRecognizer();
                    recentTap.Tapped += (s, e) => ShowStationTimeTable(((Label)s).Text);
                    label.GestureRecognizers.Add(recentTap);
                    RecentStations.Add(label);
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
