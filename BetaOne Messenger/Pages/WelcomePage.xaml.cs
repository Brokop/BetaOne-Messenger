using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using BetaOne_Messenger.Pages;

namespace BetaOne_Messenger;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
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

            createClientConnection();

        }
        
    
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {

        if (await (SecureStorage.Default.GetAsync("serv_address")) == null || (await SecureStorage.Default.GetAsync("serv_port")) == null)
        {
            await SecureStorage.Default.SetAsync("serv_address", "127.0.0.1");
            await SecureStorage.Default.SetAsync("serv_port", "7769");
        }

        createClientConnection();
    }

    private void ButtonLogin_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginPage(), true);
    }

    private void ChangeServerButton_Clicked(object sender, EventArgs e)
    {
        GetIPAddress();
    }

    private async void createClientConnection()
    {

        var popup = new PopupProgress();
        this.ShowPopup(popup);
        

        IPContextLabel.Text = "Attempting to connect to:";
        
        ButtonLogin.IsEnabled = false;
        ButtonRegister.IsEnabled = false;

        RetryServerButton.IsVisible = false;
        ChangeServerButton.IsVisible = false;

        string ip = await SecureStorage.Default.GetAsync("serv_address");
        int port = int.Parse(await SecureStorage.Default.GetAsync("serv_port"));
        showIP();

        if (MauiProgram.client == null)
        {
            MauiProgram.client = new BetaOne.Client();

            if (!MauiProgram.client.Init(ip, port))
            {
                IPContextLabel.Text = "Failed connecting to: ";
                popup.Close();
                await Application.Current.MainPage.DisplayAlert("Server not available", $"Connection to {ip}:{port} could not be established.", "Return");
                MauiProgram.client = null;
                RetryServerButton.IsVisible = true;

            } else
            {
                IPContextLabel.Text = "Connected to: ";
                RetryServerButton.IsVisible = false;
                popup.Close();
                await Application.Current.MainPage.DisplayAlert("Server connected", $"You have succesfuly connected to {ip}:{port}.", "Ok");


                ButtonLogin.IsEnabled = true;
                ButtonRegister.IsEnabled = true;

            }

            
            ChangeServerButton.IsVisible = true;

        }
    }



    public async void showIP()
    {
        IPLabel.Text = (await SecureStorage.Default.GetAsync("serv_address")) + ":" + (await SecureStorage.Default.GetAsync("serv_port"));
    }

    private void RetryServerButton_Clicked(object sender, EventArgs e)
    {
        createClientConnection();
    }

    private void ButtonRegister_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegisterPage(), true);
    }
}