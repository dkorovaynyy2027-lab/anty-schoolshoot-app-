using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace CollegeAlert;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseLocalNotification()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                // Додаємо Bold, якщо він є у файлах, або використовуємо Semibold як заміну
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansBold"); 
			});

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<ActiveAlertPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
