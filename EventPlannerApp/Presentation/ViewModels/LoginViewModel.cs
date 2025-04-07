using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;
        [ObservableProperty]
        private string? username;
        [ObservableProperty]
        private string? password;

        public IRelayCommand LoginCommand => new RelayCommand(OnLoginClicked);
        public IRelayCommand GotoSignupCommand => new RelayCommand(OnGotoSignupClicked);

        private readonly IServiceProvider _serviceProvider;
        public LoginViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (!string.IsNullOrEmpty(Preferences.Get("Username", "")) && !string.IsNullOrEmpty(Preferences.Get("Password", "")))
                AuthenticateUser(Preferences.Get("Username", ""), Preferences.Get("Password", ""));
        }

        private async void OnLoginClicked()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Einloggen Fehlgeschlagen", "Bitte geben Sie Benutzername und Passwort ein", "OK");
                return;
            }
            try
            {
                IsBusy = true;
                AuthenticateUser(Username, Password);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Einloggen Fehlgeschlagen", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnGotoSignupClicked()
        {
            var signupViewModel = _serviceProvider.GetRequiredService<SignupViewModel>();
            var signupPage = _serviceProvider.GetRequiredService<SignupPage>();
            signupPage.BindingContext = signupViewModel;
            await App.Current.MainPage.Navigation.PushAsync(signupPage);
        }

        private async void AuthenticateUser(string username, string password)
        {
            var authenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();
            bool authenticated = await authenticationService.AuthenticateUserAsync(username, password);
            if (authenticated)
            {
                Preferences.Set("Username", username);
                Preferences.Set("Password", password);
                var appShell = _serviceProvider.GetRequiredService<AppShell>();
                App.Current.MainPage = appShell;
            }
            else
            {
                Preferences.Remove("Username");
                Preferences.Remove("Password");
                await Shell.Current.DisplayAlert("Einloggen Fehlgeschlagen", "Benutzername oder Passwort ist falsch", "OK");
            }
        }
    }
}
