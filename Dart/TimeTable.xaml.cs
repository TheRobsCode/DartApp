using Dart.Dtos;
using Microsoft.Maui.Controls.Shapes;

namespace Dart
{
    public partial class TimeTable : ContentPage
    {
        private readonly IDartService _dartService;
        private readonly string _station;
        private bool _isNorthVisible = true;

        public TimeTable(IDartService dartService, string station)
        {
            InitializeComponent();
            _dartService = dartService;
            _station = station;

            StationNameLabel.Text = station;
            Title = station;

            Loaded += async (s, e) => await LoadTimeTableAsync();
            RefreshTimes.Pressed += async (s, e) => await LoadTimeTableAsync();
            ShowNorth.Pressed += (s, e) => ShowDirection(isNorth: true);
            ShowSouth.Pressed += (s, e) => ShowDirection(isNorth: false);

            UpdateDirectionButtons();

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += async (s, e) => await LoadTimeTableAsync();
            timer.Start();
        }

        private void ShowDirection(bool isNorth)
        {
            _isNorthVisible = isNorth;
            TimeTablesNorth.IsVisible = isNorth;
            TimeTablesSouth.IsVisible = !isNorth;
            EmptyStateNorth.IsVisible = isNorth && TimeTablesNorth.Children.Count == 0;
            EmptyStateSouth.IsVisible = !isNorth && TimeTablesSouth.Children.Count == 0;
            UpdateDirectionButtons();
        }

        private void UpdateDirectionButtons()
        {
            var selectedColor = Application.Current.RequestedTheme == AppTheme.Dark
                ? Color.FromArgb("#512BD4")
                : Color.FromArgb("#512BD4");
            var unselectedColor = Colors.Transparent;

            ShowNorth.BackgroundColor = _isNorthVisible ? selectedColor : unselectedColor;
            ShowSouth.BackgroundColor = _isNorthVisible ? unselectedColor : selectedColor;
        }

        private async Task LoadTimeTableAsync()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
            });

            var timeTables = await _dartService.GetTimeTable(_station);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                TimeTablesNorth.Clear();
                TimeTablesSouth.Clear();

                foreach (var item in timeTables ?? Array.Empty<TimeTableDto>())
                {
                    var trainCard = CreateTimeTableCard(item);
                    if (item.Direction == "Northbound")
                        TimeTablesNorth.Add(trainCard);
                    else if (item.Direction == "Southbound")
                        TimeTablesSouth.Add(trainCard);
                }

                // Show empty states if no trains
                EmptyStateNorth.IsVisible = _isNorthVisible && TimeTablesNorth.Children.Count == 0;
                EmptyStateSouth.IsVisible = !_isNorthVisible && TimeTablesSouth.Children.Count == 0;

                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;

                LastUpdatedLabel.Text = $"Last updated: {DateTime.Now:HH:mm:ss}";
            });
        }

        private static Border CreateTimeTableCard(TimeTableDto item)
        {
            var lastLocation = !string.IsNullOrWhiteSpace(item.Lastlocation)
                ? $"Last seen: {item.Lastlocation}"
                : string.Empty;

            var border = new Border
            {
                StrokeThickness = 1,
                Stroke = Application.Current.RequestedTheme == AppTheme.Dark
                    ? Color.FromArgb("#3a3a3a")
                    : Color.FromArgb("#e0e0e0"),
                Padding = new Thickness(16, 12),
                Margin = new Thickness(0, 2),
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
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                RowSpacing = 6
            };

            // Due time (large, prominent)
            var dueLabel = new Label
            {
                Text = $"{item.DueIn}m",
                FontSize = 28,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                TextColor = item.DueIn <= 5
                    ? Color.FromArgb("#ff6b6b")
                    : Application.Current.RequestedTheme == AppTheme.Dark
                        ? Color.FromArgb("#4ecdc4")
                        : Color.FromArgb("#2a9d8f")
            };
            grid.Add(dueLabel, 0, 0);
            Grid.SetRowSpan(dueLabel, 2);

            // Scheduled time
            var timeLabel = new Label
            {
                Text = item.EstimateTime,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(15, 0, 0, 0)
            };
            grid.Add(timeLabel, 1, 0);

            // Route information
            var routeLabel = new Label
            {
                Text = $"{item.Origin} → {item.Destination}",
                FontSize = 14,
                Margin = new Thickness(15, 0, 0, 0),
                TextColor = Application.Current.RequestedTheme == AppTheme.Dark
                    ? Color.FromArgb("#b0b0b0")
                    : Color.FromArgb("#666666")
            };
            grid.Add(routeLabel, 1, 1);

            // Last location (if available)
            if (!string.IsNullOrWhiteSpace(lastLocation))
            {
                var locationLabel = new Label
                {
                    Text = lastLocation,
                    FontSize = 12,
                    Margin = new Thickness(15, 0, 0, 0),
                    TextColor = Application.Current.RequestedTheme == AppTheme.Dark
                        ? Color.FromArgb("#888888")
                        : Color.FromArgb("#999999")
                };
                grid.Add(locationLabel, 1, 2);
            }

            border.Content = grid;
            return border;
        }
    }
}