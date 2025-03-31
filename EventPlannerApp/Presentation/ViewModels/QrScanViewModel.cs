using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventPlannerApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;


namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class QrScanViewModel: ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private BarcodeReaderOptions _qrCodeScannerOptions;

        public QrScanViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _qrCodeScannerOptions = new BarcodeReaderOptions
            {
                AutoRotate = true,
                TryInverted = true,
                TryHarder = true,
                Formats = BarcodeFormat.QrCode,
                Multiple = false,
            };
        }

        public async Task HandleBarcodesDetected(ZXing.Net.Maui.BarcodeDetectionEventArgs e, Page currentPage)
        {
            var result = e.Results.FirstOrDefault();
            if (result == null)
            {
                return;
            }
            var navigation = _serviceProvider.GetRequiredService<INavigation>();

            if (int.TryParse(string.Concat(result.Value.Where(char.IsDigit)), out int id))
            {
                try
                {
                    var eventService = _serviceProvider.GetRequiredService<IEventService>();
                    var ev = await eventService.GetEventById(id);
                    if (ev != null)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            var eventPage = _serviceProvider.GetRequiredService<EventPage>();
                            eventPage.InitializeAsync(ev);
                            await navigation.PushAsync(eventPage);
                            navigation.RemovePage(currentPage);
                        });
                    }
                    else
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            if (App.Current?.MainPage != null)
                            {
                                await App.Current.MainPage.DisplayAlert("Error", "Falsches Qr-Code vielleicht?!", "OK");
                            }
                            navigation.RemovePage(currentPage);
                        });
                    }
                }
                catch (Exception ex)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        if (App.Current?.MainPage != null)
                        {
                            await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                        }
                        navigation.RemovePage(currentPage);
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (App.Current?.MainPage != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Invalid QR code format.", "OK");
                    }
                    navigation.RemovePage(currentPage);
                });
            }
        }
    }
}
