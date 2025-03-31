using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Presentation.ViewModels;
using Microsoft.Extensions.Logging;
using ZXing;

namespace EventPlannerApp.Presentation;

public partial class QrScanPage : ContentPage
{
    private readonly QrScanViewModel _viewModel;

    private bool _isScanning = true;
    public QrScanPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _viewModel = new QrScanViewModel(serviceProvider);
        BindingContext = _viewModel;
    }

    private async void QrCodeScanner_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (_isScanning)
        {
            _isScanning = false; // Set flag to false immediately
            QrCodeScanner.IsEnabled = false; //disable the scanner

            await _viewModel.HandleBarcodesDetected(e, this);
        }
    }
}