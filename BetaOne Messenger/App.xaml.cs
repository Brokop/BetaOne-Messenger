namespace BetaOne_Messenger
{

    using BetaOne;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());

        }
    }
}