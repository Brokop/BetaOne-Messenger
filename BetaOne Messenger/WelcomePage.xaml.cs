using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace BetaOne_Messenger;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
    }

    private void ChangeServerButton_Clicked(object sender, EventArgs e)
    {
        GetIPAddress();
    }

    public async void GetIPAddress()
    {
        string result = await DisplayPromptAsync("Server setup", "Provide port and IP Address", "Ok", "Cancel", "IP:PORT");

        if (result != null)
        {
            string[] ipformat = result.Split(':');

            if(ipformat.Length != 2)
            {
                await DisplayAlert("Incorrect IP Format", "Please provide IP and Port", "Ok");
                return;
            }

            IPAddress ipAddress = null;

            bool success = IPAddress.TryParse(ipformat[0], out ipAddress);
                if(!success)
                {
                    await DisplayAlert("Incorrect IP Format", "Please try again.", "Ok");
                    return;
                }

            await SecureStorage.SetAsync("serv_address", ipAddress.ToString());
            await SecureStorage.SetAsync("serv_port", ipformat[1]);

            IPLabel.Text = (await SecureStorage.Default.GetAsync("serv_address")) + ":" + (await SecureStorage.Default.GetAsync("serv_port"));

        }
        
    
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        if (await (SecureStorage.Default.GetAsync("serv_address")) == null || (await SecureStorage.Default.GetAsync("serv_port")) == null)
        {
            await SecureStorage.Default.SetAsync("serv_address", "127.0.0.1");
            await SecureStorage.Default.SetAsync("serv_port", "7769");
        }

        IPLabel.Text = (await SecureStorage.Default.GetAsync("serv_address")) + ":" + (await SecureStorage.Default.GetAsync("serv_port"));
    }

    private void ButtonLogin_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginPage(), true);
    }
}