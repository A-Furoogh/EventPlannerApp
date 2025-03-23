using EventPlannerApp.Application.Interfaces;
using Microsoft.Extensions.Logging;
using ZXing;

namespace EventPlannerApp.Presentation;

public partial class QrScanPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
	public QrScanPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        QrCodeScanner.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
            
        };
    }

    private async void QrCodeScanner_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var result = e.Results.FirstOrDefault();
        if (result == null)
        {
            return;
        }
        //Dispatcher.DispatchAsync(() =>
        //{
        //    DisplayAlert("Barcode", result.Value, "OK");
        //});
        int id = int.Parse(String.Concat(result.Value.Where(Char.IsDigit)));
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        var ev = eventService.GetEventById(id).Result;
        if (ev != null)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var eventPage = _serviceProvider.GetRequiredService<EventPage>();
                eventPage.InitializeAsync(ev);
                await Navigation.PushAsync(eventPage);

                Navigation.RemovePage(this); 
            });
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Error", "Event nicht gefunden!", "OK");

                Navigation.RemovePage(this);
            });
        }
    }
}