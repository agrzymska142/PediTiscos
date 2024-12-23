using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string token);
        Task<string> GetTokenAsync();
        Task ClearTokenAsync();
    }
}
