using Microsoft.Extensions.Logging;
using SGERepasoMAUI.Services;

namespace SGERepasoMAUI
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

#if ANDROID || IOS || MACCATALYST
            builder.UseMauiMaps();
#endif

            builder.Services.AddMauiBlazorWebView();

            // Registrar servicios
            builder.Services.AddSingleton<CentrosService>();
            builder.Services.AddSingleton<WeatherService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
