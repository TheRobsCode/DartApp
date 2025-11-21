using Microsoft.Extensions.Logging;
using Serilog;

namespace Dart
{
    public partial class App : Application
    {
        public static event Action AppActivated;

        public App()
        {
            Log.Information("App initializing");
            InitializeComponent();

            // Handle app action invocations
            AppActions.OnAppAction += HandleAppAction;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Log.Information("Creating application window");
            var window = new Window(new AppShell());

            // Window lifecycle events
            window.Activated += (s, e) => {
                Log.Information("Window activated");
                AppActivated?.Invoke();
            };

            window.Deactivated += (s, e) => {
                Log.Information("Window deactivated");
            };

            window.Destroying += (s, e) => {
                Log.Information("Window destroying");
            };

            return window;
        }

        private void HandleAppAction(object? sender, AppActionEventArgs e)
        {
            // Get the station name from the app action ID or title
            var station = e.AppAction.Title;
            Log.Information("App action received for station: {Station}", station ?? "(none)");

            if (!string.IsNullOrEmpty(station))
            {
                // Set the pending station so MainPage can handle it when it appears
                AppActionsService.SetPendingStation(station);

                // Navigate to main page if not already there
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (Windows.Count > 0)
                    {
                        var shell = Windows[0].Page as Shell;
                        if (shell != null)
                        {
                            Log.Debug("Navigating to MainPage for app action");
                            await shell.GoToAsync("//MainPage");
                        }
                        else
                        {
                            Log.Warning("Shell is null, cannot navigate to MainPage");
                        }
                    }
                    else
                    {
                        Log.Warning("No windows available, cannot handle app action");
                    }
                });
            }
        }
    }
}