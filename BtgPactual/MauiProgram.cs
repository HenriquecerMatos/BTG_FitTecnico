using BtgPactual.ViewModels;
using Microsoft.Extensions.Logging;

namespace BtgPactual
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ClientsViewModel>();
            builder.Services.AddSingleton<UpdateClientViewModel>();
            builder.Services.AddSingleton<AddClientViewModel>();
            builder.Services.AddSingleton<DeleteClientViewModel>();

            return builder.Build();
        }
    }
}
