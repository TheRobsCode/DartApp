using Dart.Dtos;

namespace Dart
{
    public partial class TimeTable : ContentPage
    {
        private readonly IDartService _dartService;
        private readonly string _station;

        public TimeTable(IDartService dartService, string station)
        {
            InitializeComponent();
            _dartService = dartService;
            _station = station;

            Loaded += async (s, e) => await LoadTimeTableAsync();
            RefreshTimes.Pressed += async (s, e) => await LoadTimeTableAsync();
            ShowNorth.Pressed += (s, e) => ShowDirection(isNorth: true);
            ShowSouth.Pressed += (s, e) => ShowDirection(isNorth: false);

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += async (s, e) => await LoadTimeTableAsync();
            timer.Start();
        }

        private void ShowDirection(bool isNorth)
        {
            TimeTablesNorth.IsVisible = isNorth;
            TimeTablesSouth.IsVisible = !isNorth;
            ShowNorth.BackgroundColor = isNorth ? Colors.Gray : Colors.LightGray;
            ShowSouth.BackgroundColor = isNorth ? Colors.LightGray : Colors.Gray;
        }

        private async Task LoadTimeTableAsync()
        {
            SetRefreshText("Refreshing");
            var timeTables = await _dartService.GetTimeTable(_station);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                TimeTablesNorth.Clear();
                TimeTablesSouth.Clear();

                foreach (var item in timeTables ?? Array.Empty<TimeTableDto>())
                {
                    var label = CreateTimeTableLabel(item);
                    if (item.Direction == "Northbound")
                        TimeTablesNorth.Add(label);
                    else if (item.Direction == "Southbound")
                        TimeTablesSouth.Add(label);
                }

                SetRefreshText("Refresh");
            });
        }

        private void SetRefreshText(string text)
        {
            MainThread.BeginInvokeOnMainThread(() => RefreshTimes.Text = text);
        }

        private static Label CreateTimeTableLabel(TimeTableDto item)
        {
            var lastLocation = !string.IsNullOrWhiteSpace(item.Lastlocation)
                ? $"({item.Lastlocation})"
                : string.Empty;

            return new Label
            {
                Text = $"{item.EstimateTime} | {item.DueIn} mins - {item.Origin} - {item.Destination} {lastLocation}",
                FontSize = 14,
                Margin = new Thickness(0, 5)
            };
        }
    }
}