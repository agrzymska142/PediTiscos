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
            var firstName = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var lastName = jwt.Claims.FirstOrDefault(c => c.Type == "surname")?.Value;

            if (!string.IsNullOrEmpty(username)) Preferences.Set("username", username);
            if (!string.IsNullOrEmpty(firstName)) Preferences.Set("firstName", firstName);
            if (!string.IsNullOrEmpty(lastName)) Preferences.Set("lastName", lastName);

            if (OnUserLoggedIn != null)
            {
                return new ValueTask(OnUserLoggedIn.Invoke());
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask<string> GetTokenAsync()
        {
            var token = Preferences.Get("authToken", string.Empty);
            return new ValueTask<string>(token);
        }

        public ValueTask<string> GetUsernameAsync()
        {
            var username = Preferences.Get("username", string.Empty);
            return new ValueTask<string>(username);
        }

        public ValueTask SaveUserIdAsync(string userId)
        {
            Preferences.Set("userId", userId);
            return ValueTask.CompletedTask;
        }

        public ValueTask<string> GetUserIdAsync()
        {
            var userId = Preferences.Get("userId", string.Empty);
            return new ValueTask<string>(userId);
        }

        public ValueTask<string> GetFullNameAsync()
        {
            var firstName = Preferences.Get("firstName", string.Empty);
            var lastName = Preferences.Get("lastName", string.Empty);
            var fullName = $"{firstName} {lastName}".Trim();
            return new ValueTask<string>(fullName);
        }

        public async ValueTask<bool> IsTokenValidAsync(string token)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                if (!jwtHandler.CanReadToken(token))
                {
                    return false;
                }

                var jwtToken = jwtHandler.ReadJwtToken(token);
                var expiration = jwtToken.ValidTo;

                // Check if the token is expired
                return expiration > DateTime.UtcNow;
            }
            catch
            {
                return false; // Treat any exception as an invalid token
            }
        }


        public ValueTask ClearTokenAsync()
        {
            Preferences.Remove("authToken");
            Preferences.Remove("userId");
            Preferences.Remove("username");
            Preferences.Remove("firstName");
            Preferences.Remove("lastName");

            return ValueTask.CompletedTask;
        }
    }
}
