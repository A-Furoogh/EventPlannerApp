using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Interfaces
{
    public interface INavigationService
    {
        Task PushAsync(Page page);
        Task PopAsync();
        Task PopToRootAsync();
        Task PushModalAsync(Page page);
        Task PopModalAsync();
        void RemovePage(Page page);
        IReadOnlyList<Page> NavigationStack { get; }
    }
}
