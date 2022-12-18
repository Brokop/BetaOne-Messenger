using System.Security.Cryptography.X509Certificates;

namespace BetaOne_Messenger
{

    using BetaOne;
    using CommunityToolkit.Maui;

    public static class MauiProgram
    {
        public static Client client;

        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}