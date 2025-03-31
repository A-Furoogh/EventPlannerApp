using EventPlannerApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Services
{
    public class NavigationService : INavigationService
    {
        private readonly INavigation _navigation;
        public NavigationService(INavigation navigation)
        {
            _navigation = navigation;
        }

        public Task PushAsync(Page page) => _navigation.PushAsync(page);
        public Task PopAsync() => _navigation.PopAsync();
        public Task PopToRootAsync() => _navigation.PopToRootAsync();
        public Task PushModalAsync(Page page) => _navigation.PushModalAsync(page);
        public Task PopModalAsync() => _navigation.PopModalAsync();
        public void RemovePage(Page page) => _navigation.RemovePage(page);
        public IReadOnlyList<Page> NavigationStack => _navigation.NavigationStack;
    }
}
