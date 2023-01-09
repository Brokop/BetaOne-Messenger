namespace BetaOneCLI
{
    using Terminal.Gui;
    using BetaOne;
    internal class Program
    {

        public static Client client;

        static void Main(string[] args)
        {
            Application.Init();

            Application.Run(new BetaOneCLI.WelcomeScreen());

        }
    }
}