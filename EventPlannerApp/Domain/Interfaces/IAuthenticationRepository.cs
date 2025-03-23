using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        int GetUserIdAsync();
    }
}
