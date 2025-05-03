using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Pawpal.ViewModel;
using Pawpal.Services;

namespace Pawpal;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Register services
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IFeedingTimeService, FeedingTimeService>();
        builder.Services.AddTransient<NotifyService>();

        // Register ViewModels
        builder.Services.AddSingleton<MainViewModel>();

        // Register Pages
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
	}
}
