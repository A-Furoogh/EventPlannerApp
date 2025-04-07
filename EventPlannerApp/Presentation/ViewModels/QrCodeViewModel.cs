using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class QrCodeViewModel : ObservableObject
    {
        public int? EventId;
        public int? UserId;
        public String? QrCode;

        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private View? _qrCodeView;

        public QrCodeViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void GenerateQrCode()
        {
            string qrCodeValue = null;

            if (EventId is not null)
            {
                qrCodeValue = $"event_{EventId}";
            }
            else if (UserId is not null)
            {
                qrCodeValue = $"user_{UserId}";
            }

            if (qrCodeValue != null)
            {
                QrCodeView = new BarcodeGeneratorView
                {
                    Format = BarcodeFormat.QrCode,
                    Value = qrCodeValue,
                    WidthRequest = 240,
                    HeightRequest = 240,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    ForegroundColor = Colors.DarkBlue,
                    BackgroundColor = Colors.DarkKhaki
                };
            }
            else
            {
                QrCodeView = null;
            }
        }
    }
}
