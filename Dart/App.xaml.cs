namespace Dart
{
    public partial class App : Application
    {
        public static event Action AppActivated;
        public App()
        {
            InitializeComponent();

            // Handle app action invocations
            AppActions.OnAppAction += HandleAppAction;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Window lifecycle events
            window.Activated += (s, e) => {
                AppActivated?.Invoke();
            };
            return window;
        }

        private void HandleAppAction(object? sender, AppActionEventArgs e)
        {
            // Get the station name from the app action ID or title
            var station = e.AppAction.Title;

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
                            await shell.GoToAsync("//MainPage");
                        }
                    }
                });
            }
        }
    }
}