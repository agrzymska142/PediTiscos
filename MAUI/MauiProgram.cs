using MAUI.Services;
using Microsoft.Extensions.Logging;
using RCL.Data.Interfaces;

namespace MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7255") }); // Backend base URL
            builder.Services.AddSingleton<ITokenService, MauiTokenService>();
            builder.Services.AddSingleton<ISessionStorageService, MauiSessionStorageService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools(); 

            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
