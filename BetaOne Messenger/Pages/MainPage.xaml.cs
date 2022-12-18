namespace BetaOne_Messenger
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // this is needed because calling this before page load yields error
        private async void Page_Loaded(object sender, EventArgs e)
        {

            // If first launch
            if (!(CheckloadConfig()))
            {
                await Navigation.PushAsync(new WelcomePage(), false);
            }
        }


        public bool CheckloadConfig()
        {

            // If we are not a user, go to welcome screen
            if (SecureStorage.Default.GetAsync("user_id").Result == null)
            {
                return false;
            }
            else
                return true;

        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";


            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}