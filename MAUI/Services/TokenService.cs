using Microsoft.Maui.Storage;
using System.IdentityModel.Tokens.Jwt;
using RCL.Data.Interfaces;

namespace MAUI.Services
{
    public class MauiTokenService : ITokenService
    {

        public event Func<Task> OnUserLoggedIn;
        public ValueTask SaveTokenAsync(string token)
        {
            Preferences.Set("authToken", token);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var username = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var firstName = jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var lastName = jwt.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;

            if (!string.IsNullOrEmpty(username)) Preferences.Set("username", username);
            if (!string.IsNullOrEmpty(firstName)) Preferences.Set("firstName", firstName);
            if (!string.IsNullOrEmpty(lastName)) Preferences.Set("lastName", lastName);

            return ValueTask.CompletedTask;
        }

        public ValueTask<string> GetTokenAsync()
        {
            var token = Preferences.Get("authToken", string.Empty);
            return ValueTask.FromResult(token);
        }

        public ValueTask<string> GetUsernameAsync()
        {
            var username = Preferences.Get("username", string.Empty);
            return ValueTask.FromResult(username);
        }

        public ValueTask<string> GetFullNameAsync()
        {
            var firstName = Preferences.Get("firstName", string.Empty);
            var lastName = Preferences.Get("lastName", string.Empty);
            var fullName = $"{firstName} {lastName}".Trim();
            return ValueTask.FromResult(fullName);
        }

        public ValueTask ClearTokenAsync()
        {
            Preferences.Remove("authToken");
            Preferences.Remove("username");
            Preferences.Remove("firstName");
            Preferences.Remove("lastName");

            return ValueTask.CompletedTask;
        }
    }
}
