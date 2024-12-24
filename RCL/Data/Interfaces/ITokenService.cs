using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface ITokenService
    {
        ValueTask SaveTokenAsync(string token);
        ValueTask<string> GetTokenAsync();
        ValueTask<string> GetUsernameAsync();
        ValueTask<string> GetFullNameAsync();
        Task SaveUserIdAsync(string userId);
        Task<string> GetUserIdAsync();
        ValueTask ClearTokenAsync();
        event Func<Task> OnUserLoggedIn;
    }
}
