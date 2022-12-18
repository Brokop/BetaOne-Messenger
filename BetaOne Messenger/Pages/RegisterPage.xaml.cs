using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace BetaOne_Messenger;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
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