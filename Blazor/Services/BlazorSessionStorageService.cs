using Blazored.LocalStorage;
using RCL.Data.Interfaces;
using System.Threading.Tasks;
namespace Blazor.Services


{
    public class BlazorSessionStorageService : ISessionStorageService
    {
        private readonly ILocalStorageService _localStorage;

        public BlazorSessionStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public ValueTask SetItemAsync<T>(string key, T value)
        {
            return _localStorage.SetItemAsync(key, value);
        }

        public ValueTask<T> GetItemAsync<T>(string key)
        {
            return _localStorage.GetItemAsync<T>(key);
        }

        public ValueTask RemoveItemAsync(string key)
        {
            return _localStorage.RemoveItemAsync(key);
        }
    }
}
