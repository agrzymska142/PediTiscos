using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using RCL.Data.Interfaces;

public class WebTokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;
    public event Func<Task> OnUserLoggedIn;

    public WebTokenService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }


    public async ValueTask SaveTokenAsync(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var username = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var firstName = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        var lastName = jwt.Claims.FirstOrDefault(c => c.Type == "surname")?.Value;

        if (!string.IsNullOrEmpty(username)) await _localStorage.SetItemAsync("username", username);
        if (!string.IsNullOrEmpty(firstName)) await _localStorage.SetItemAsync("firstName", firstName);
        if (!string.IsNullOrEmpty(lastName)) await _localStorage.SetItemAsync("lastName", lastName);

        if (OnUserLoggedIn != null)
        {
            await OnUserLoggedIn.Invoke();
        }
    }

    public async ValueTask<string> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }

    public async ValueTask<string> GetUsernameAsync()
    {
        return await _localStorage.GetItemAsync<string>("username");
    }

    public Task SaveUserIdAsync(string userId) => _localStorage.SetItemAsync("userId", userId).AsTask();

    public Task<string> GetUserIdAsync() => _localStorage.GetItemAsync<string>("userId").AsTask();

    public async ValueTask<string> GetFullNameAsync()
    {
        var firstName = await _localStorage.GetItemAsync<string>("firstName");
        var lastName = await _localStorage.GetItemAsync<string>("lastName");
        return $"{firstName} {lastName}".Trim();
    }

    public async ValueTask ClearTokenAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("userId");
        await _localStorage.RemoveItemAsync("username");
        await _localStorage.RemoveItemAsync("firstName");
        await _localStorage.RemoveItemAsync("lastName");
    }
}
