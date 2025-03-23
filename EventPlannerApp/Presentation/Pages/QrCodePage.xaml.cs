using System.ComponentModel;
using ZXing.Net.Maui.Controls;

namespace EventPlannerApp.Presentation;

public partial class QrCodePage : ContentPage
{
    public int? EventId;
    public int? UserId;
    public String? QrCode;

    public QrCodePage()
	{
        InitializeComponent();
        BindingContext = this;
    }

    public void CreateQrCodeInGrid()
    {
        base.OnAppearing();
            if (EventId is not null)
            {
                QrCode = $"event_{EventId}";
            }
            else if (UserId is not null)
            {
                QrCode = $"user_{UserId}";
            }

        var barcodeGeneratorView = new BarcodeGeneratorView
        {
            Format = ZXing.Net.Maui.BarcodeFormat.QrCode,
            Value = QrCode, 
            WidthRequest = 240,
            HeightRequest = 240,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            ForegroundColor = Colors.DarkBlue, BackgroundColor = Colors.DarkKhaki
        };

        // Add it to the Grid (QrField)
        QrField.Children.Clear(); // Clear any previous content
        QrField.Children.Add(barcodeGeneratorView);
    }
}