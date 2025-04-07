using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Presentation;
using EventPlannerApp.Presentation.ViewModels;

namespace EventPlannerApp
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            CheckAuthentication();
        }

        private void CheckAuthentication()
        {
            var authenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();

            if (!authenticationService.IsUserAuthenticated())
            {
                var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
                loginPage.BindingContext = loginViewModel;
                CurrentItem = new ShellContent
                {
                    Content = loginPage
                };
            }
            else
            {
                ShowTabBar();
            }
        }
        private void ShowTabBar()
        {
            // Create the TabBar dynamically
            var tabBar = new TabBar();

            // MainPage-Tab
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            var mainPage = _serviceProvider.GetRequiredService<MainPage>();
            mainPage.BindingContext = mainViewModel;
            var mainShellContent = new ShellContent
            {
                Title = "Home",
                Icon = "home_icon.png",
                Content = mainPage
            };
            tabBar.Items.Add(mainShellContent);
            Console.WriteLine("MainPage added to TabBar.");

            // ChatsPage-Tab
            var chatsViewModel = _serviceProvider.GetRequiredService<ChatsViewModel>();
            var chatsPage = _serviceProvider.GetRequiredService<ChatsPage>();
            chatsPage.BindingContext = chatsViewModel;
            var ShatsShellContent = new ShellContent
            {
                Title = "Chats",
                Icon = "chat_icon.png",
                Content = chatsPage
            };
            tabBar.Items.Add(ShatsShellContent);
            Console.WriteLine("ChatsPage added to TabBar.");

            // MyEventsPage-Tab
            var myEventsPage = _serviceProvider.GetRequiredService<MyEventsPage>();
            var myEventsViewModel = _serviceProvider.GetRequiredService<MyEventsViewModel>();
            myEventsPage.BindingContext = myEventsViewModel;
            var myEventsShellContent = new ShellContent
            {
                Title = "My Events",
                Icon = "my_event_icon.png",
                Content = myEventsPage
            };
            tabBar.Items.Add(myEventsShellContent);

            // Set the TabBar as the current item
            CurrentItem = tabBar;
            Console.WriteLine("TabBar set as CurrentItem.");
        }

        private void OnSettingMenuButton_Clicked(object sender, EventArgs e)
        {
        }
        // SettingMenu class

    }
}
