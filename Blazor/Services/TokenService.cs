using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using RCL.Data.Interfaces;

public class WebTokenService : ITokenService
{
    private readonly ISessionStorageService _sessionStorage;
    public event Func<Task> OnUserLoggedIn;

    public WebTokenService(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async ValueTask SaveTokenAsync(string token)
    {
        await _sessionStorage.SetItemAsync("authToken", token);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var username = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var firstName = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        var lastName = jwt.Claims.FirstOrDefault(c => c.Type == "surname")?.Value;

        if (!string.IsNullOrEmpty(username)) await _sessionStorage.SetItemAsync("username", username);
        if (!string.IsNullOrEmpty(firstName)) await _sessionStorage.SetItemAsync("firstName", firstName);
        if (!string.IsNullOrEmpty(lastName)) await _sessionStorage.SetItemAsync("lastName", lastName);

        if (OnUserLoggedIn != null)
        {
            await OnUserLoggedIn.Invoke();
        }
    }

    public async ValueTask<string> GetTokenAsync()
    {
        return await _sessionStorage.GetItemAsync<string>("authToken");
    }

    public async ValueTask<string> GetUsernameAsync()
    {
        return await _sessionStorage.GetItemAsync<string>("username");
    }

    public ValueTask SaveUserIdAsync(string userId) => _sessionStorage.SetItemAsync("userId", userId);

    public ValueTask<string> GetUserIdAsync() => _sessionStorage.GetItemAsync<string>("userId");

    public async ValueTask<string> GetFullNameAsync()
    {
        var firstName = await _sessionStorage.GetItemAsync<string>("firstName");
        var lastName = await _sessionStorage.GetItemAsync<string>("lastName");
        return $"{firstName} {lastName}".Trim();
    }

    public async ValueTask ClearTokenAsync()
    {
        await _sessionStorage.RemoveItemAsync("authToken");
        await _sessionStorage.RemoveItemAsync("userId");
        await _sessionStorage.RemoveItemAsync("username");
        await _sessionStorage.RemoveItemAsync("firstName");
        await _sessionStorage.RemoveItemAsync("lastName");
    }
}