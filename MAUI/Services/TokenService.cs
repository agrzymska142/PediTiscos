using Microsoft.Maui.Storage;
using RCL.Data.Interfaces;

namespace MAUI.Services
{
    public class MobileTokenService : ITokenService
    {
        public Task SaveTokenAsync(string token) => SecureStorage.SetAsync("authToken", token);

        public Task<string> GetTokenAsync() => Task.FromResult(SecureStorage.GetAsync("authToken").Result);

        public Task ClearTokenAsync()
        {
            SecureStorage.Remove("authToken");
            return Task.CompletedTask;
        }
    }
}
