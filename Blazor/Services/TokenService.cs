using Blazored.LocalStorage;
using RCL.Data.Interfaces;

public class WebTokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;

    public WebTokenService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public Task SaveTokenAsync(string token) => _localStorage.SetItemAsync("authToken", token).AsTask();

    public Task<string> GetTokenAsync() => _localStorage.GetItemAsync<string>("authToken").AsTask();

    public Task ClearTokenAsync() => _localStorage.RemoveItemAsync("authToken").AsTask();

    public Task<string> GetUsernameAsync() => _localStorage.GetItemAsync<string>("username").AsTask();
}
