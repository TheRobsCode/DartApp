namespace Dart
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute("logs", typeof(LogsPage));
        }
    }
}
