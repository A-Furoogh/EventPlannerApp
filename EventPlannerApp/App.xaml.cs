using EventPlannerApp.Presentation;

namespace EventPlannerApp
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            MainPage = new NavigationPage(_serviceProvider.GetRequiredService<LoginPage>());
        }
    }
}
