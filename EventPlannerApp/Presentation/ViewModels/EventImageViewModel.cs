using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.ViewModels
{
    public partial class EventImageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> images;

        public IRelayCommand<string> ImageClickedCommand => new RelayCommand<string>(OnImageClicked);

        public delegate void DataReturnedEventHandler(int? data);
        public event EventHandler<int?>? OnDataReturned;

        private readonly IServiceProvider _serviceProvider;
        public EventImageViewModel(IServiceProvider serviceProvider)
        {
            Images = new List<string>
            {
                "event0.png",
                "event1.png",
                "event2.png",
                "event3.png",
                "event4.png",
                "event5.png",
                "event6.png",
            };
            _serviceProvider = serviceProvider;
        }

        public void OnImageClicked(string? selectedImage)
        {
            if (selectedImage is not null)
            {
                int? ImageIndex = int.Parse(selectedImage.Substring(5, 1));
                OnDataReturned?.Invoke(this, ImageIndex);

                var navigationService = _serviceProvider.GetRequiredService<INavigation>();
                navigationService.PopAsync();
            }
        }
    }
}
