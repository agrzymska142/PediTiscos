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
        // Metoda do ustawiania elementu w pamięci lokalnej
        public ValueTask SetItemAsync<T>(string key, T value)
        {
            return _localStorage.SetItemAsync(key, value);
        }
        // Metoda do pobierania elementu z pamięci lokalnej
        public ValueTask<T> GetItemAsync<T>(string key)
        {
            return _localStorage.GetItemAsync<T>(key);
        }
        // Metoda do usuwania elementu z pamięci lokalnej
        public ValueTask RemoveItemAsync(string key)
        {
            return _localStorage.RemoveItemAsync(key);
        }
    }
}
