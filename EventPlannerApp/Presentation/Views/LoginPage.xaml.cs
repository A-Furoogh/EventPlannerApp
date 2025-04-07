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
    }


}