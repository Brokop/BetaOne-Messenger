using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace BetaOne_Messenger;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        
    }

    private void ButtonReturn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}