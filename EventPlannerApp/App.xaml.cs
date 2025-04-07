using EventPlannerApp.Presentation;
using EventPlannerApp.Presentation.ViewModels;

namespace EventPlannerApp
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
            var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
            loginPage.BindingContext = loginViewModel;
            MainPage = new NavigationPage(loginPage);
        }
    }
}
