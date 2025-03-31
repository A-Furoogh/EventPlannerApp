using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Presentation.ViewModels;

namespace EventPlannerApp.Presentation;

public partial class LoginPage : ContentPage
{
    private bool _busy;
    public bool Busy
    {
        get => _busy;
        set
        {
            _busy = value;
            OnPropertyChanged();
        }
    }
    private readonly IServiceProvider _serviceProvider;
    public LoginPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);
        if (!string.IsNullOrEmpty(Preferences.Get("Username", "")) && !string.IsNullOrEmpty(Preferences.Get("Password", "")))
            AuthenticateUser(Preferences.Get("Username", ""), Preferences.Get("Password", ""));
        BindingContext = this;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("Einloggen Fehlgeschlagen", "Bitte geben Sie Benutername und Passwordt ein", "OK");
            return;
        }
        try
        {
            IsBusy = true;
            AuthenticateUser(UsernameEntry.Text, PasswordEntry.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Einloggen Fehlgeschlagen", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async void OnGotoSignupClicked(object sender, EventArgs e)
    {
        var signupViewModel = _serviceProvider.GetRequiredService<SignupViewModel>();
        await Navigation.PushAsync(new SignupPage(signupViewModel));
    }

    private async void AuthenticateUser(string username, string password)
    {
        var authenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();

        bool authenticated = await authenticationService.AuthenticateUserAsync(username, password);

        if (authenticated)
        {
            int userId = authenticationService.GetUserIdAsync();
            if (userId > 0)
            {
                Preferences.Set("Username", username);
                Preferences.Set("Password", password);
                var appShell = _serviceProvider.GetRequiredService<AppShell>();
                App.Current.MainPage = appShell;
            }
        }
        else
        {
            await DisplayAlert("Einloggen Fehlgeschlagen", "Benutername oder Passwordt ist falsch", "OK");
        }
    }
}