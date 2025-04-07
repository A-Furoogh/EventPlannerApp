using EventPlannerApp.Application.Interfaces;
using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation;
using EventPlannerApp.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation
{
    public partial class MainPage : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;
        public MainPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }
    }

}
