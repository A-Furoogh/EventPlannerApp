using EventPlannerApp.Presentation.ViewModels;
using System.ComponentModel;
using ZXing.Net.Maui.Controls;

namespace EventPlannerApp.Presentation;

public partial class QrCodePage : ContentPage
{

    public QrCodePage(QrCodeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}