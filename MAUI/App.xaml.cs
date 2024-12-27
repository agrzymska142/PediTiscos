using System.Diagnostics;

namespace MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Register global exception handlers
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    Debug.WriteLine($"Unhandled exception: {ex.Message}");
                    Debug.WriteLine(ex.StackTrace);
                }
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Debug.WriteLine($"Task exception: {e.Exception.Message}");
                Debug.WriteLine(e.Exception.StackTrace);
                e.SetObserved();
            };

            MainPage = new MainPage();
        }
    }
}
