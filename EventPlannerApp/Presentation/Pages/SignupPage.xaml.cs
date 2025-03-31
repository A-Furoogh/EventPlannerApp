using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation.ViewModels;

namespace EventPlannerApp.Presentation;

public partial class SignupPage : ContentPage
{
    public SignupPage(SignupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private async void OnProfileImageTapped(object sender, EventArgs e)
    {
        try
        {
#pragma warning disable CA1416 // Validate platform compatibility
            var status = await Permissions.RequestAsync<Permissions.Photos>();
#pragma warning restore CA1416 // Validate platform compatibility

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Zugriff verweigert", "nicht möglich Bild zu wählen.", "OK");
                return;
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
            await DisplayAlert("Error", $"Fehler aufgetreten: {ex.Message}", "OK");
        }
    }
}