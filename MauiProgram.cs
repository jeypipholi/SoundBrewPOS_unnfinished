using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SoundBrewPOS.Data;
using SoundBrewPOS.ViewModels;

namespace SoundBrewPOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");

                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<OrdersViewModel>().AddSingleton<OrdersPage>()
                .AddTransient<ManageMenuItemsViewModel>()
                .AddTransient<ManageMenuItems>();
            builder.Services.AddSingleton<InventoryManagementViewModel>();
            builder.Services.AddSingleton<InventoryManagement>();




            return builder.Build();
        }
    }
}
