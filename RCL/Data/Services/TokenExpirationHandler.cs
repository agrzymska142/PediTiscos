using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RCL.Data.Interfaces;

namespace RCL.Data.Services
{
    public class TokenExpirationHandler
    {
        private readonly ITokenService _tokenService;
        private readonly NavigationManager _navigation;

        public TokenExpirationHandler(ITokenService tokenService, NavigationManager navigation)
        {
            _tokenService = tokenService;
            _navigation = navigation;
        }

        public async Task HandleTokenErrorAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _tokenService.ClearTokenAsync(); // Clear the stored token
                _navigation.NavigateTo("/login", true); // Redirect to login
            }
        }
    }
}
