using System.Security.Cryptography.X509Certificates;

namespace BetaOne_Messenger
{

    using BetaOne;

    public static class MauiProgram
    {

        public static Client meClient = new Client();

        public static MauiApp CreateMauiApp()
        {

            meClient.Init("127.0.0.1", 7767);

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}