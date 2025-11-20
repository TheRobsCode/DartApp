namespace Dart
{
    public interface IAppActionsService
    {
        Task UpdateRecentStationsAsync(List<string> recentStations);
        string? GetPendingStationAsync();
    }

    public class AppActionsService : IAppActionsService
    {
        private const int MaxShortcuts = 5;
        private static string? _pendingStation;

        public async Task UpdateRecentStationsAsync(List<string> recentStations)
        {
            try
            {
                // Take only the last 5 stations
                var topStations = recentStations.Take(MaxShortcuts).ToList();

                // Clear existing app actions
                await AppActions.SetAsync(Array.Empty<AppAction>());

                // Create new app actions for recent stations
                var appActions = topStations.Select((station, index) => new AppAction(
                    id: $"station_{index}",
                    title: station,
                    subtitle: station
                )).ToArray();

                await AppActions.SetAsync(appActions);
            }
            catch (Exception)
            {
                // App actions may not be supported on all platforms
                // Silently fail to avoid crashes
            }
        }

        public string? GetPendingStationAsync()
        {
            var station = _pendingStation;
            _pendingStation = null;
            return station;
        }

        public static void SetPendingStation(string station)
        {
            _pendingStation = station;
        }
    }
}
