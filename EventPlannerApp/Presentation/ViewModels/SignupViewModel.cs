using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class SignupViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? username;
        [ObservableProperty]
        private string? password;
        [ObservableProperty]
        private string? confirmPassword;
        [ObservableProperty]
        private string? email;

        public IRelayCommand OnSignupClickedCommand => new RelayCommand(async () => await OnSignupClicked());
        public IRelayCommand OnProfileImageTappedCommand => new RelayCommand(async () => await OnProfileImageTapped());

        private readonly IServiceProvider _serviceProvider;
        public SignupViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        public async Task OnSignupClicked()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                if (App.Current?.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Fehlgeschlagen", "Benutzername oder Passwort leer", "OK");
                }
                return;
            }
            else if (Password != ConfirmPassword)
            {
                if (App.Current?.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Fehlergeschlagen", "Passwörter stimmen nicht", "OK");
                }
                return;
            }
            var userServiceProvider = _serviceProvider.GetRequiredService<IUserRepository>();
            string email = Email ?? "";
            if (string.IsNullOrEmpty(email))
            {
                email = " ";
            }
            User user = new User
            {
                Id = await GenerateUserId(),
                Name = Username,
                Password = Password,
                Email = email,
                EventsIds = new List<int>()
            };
            try
            {
                await userServiceProvider.AddUserAsync(user);
                if (App.Current?.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Erfolgreich", "Benutzer erfolgreich angelegt", "OK");
                }
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                if (App.Current?.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Anmeldefehler", ex.Message, "OK");
                }
            }
        }

        [RelayCommand]
        public async Task OnProfileImageTapped()
        {
            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
                var status = await Permissions.RequestAsync<Permissions.Photos>();
#pragma warning restore CA1416 // Validate platform compatibility

                if (status != PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Zugriff verweigert", "nicht möglich Bild zu wählen.", "OK");
                }

                // Pick a photo
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "wähle ein Bild aus",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    //ProfileImage.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Fehler aufgetreten: {ex.Message}", "OK");
            }
        }
        Task<int> GenerateUserId()
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var users = userService.Users;
            Random random = new Random();
            int userId = random.Next(1000, 9999);
            while (users.Any(u => u.Id == userId))
            {
                userId = random.Next(1000, 9999);
            }
            return Task.FromResult(userId);
        }
    }
}
