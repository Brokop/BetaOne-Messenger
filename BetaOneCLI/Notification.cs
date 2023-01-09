using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BetaOneCLI
{
    class Notification : Dialog
    {

        Label messageLabel = new Label()
        {
            X = Pos.Center(),
            Y = Pos.Center() - 2,
        };

        Button Okay = new Button("Ok", true);


        public Notification(string message, bool enableExit = true) : base("Connection", 50, 20)
        {
            // Set the window's size and position
            X = Pos.Center();
            Y = Pos.Center();

            Width = Dim.Percent(60);
            Height = Dim.Percent(40);

            messageLabel.Text = message;


            Okay.X= Pos.Center();
            Okay.Y= Pos.Center() + 2;
            
            this.Modal= true;
            this.EnsureFocus();

            // Add an "OK" button
            Add(messageLabel);

            if (enableExit)
            Add(Okay); 

            this.CanFocus= true;

            Okay.Clicked += () => {

                Application.MainLoop.Invoke(() => {
                    Application.Top.Remove(this);
                    Application.RequestStop();
                });
            };

            Okay.Height = 3;

            Okay.CanFocus= true;
            Okay.SetNeedsDisplay();

        }


        public void removeSelf()
        {
            this.Modal = false;
            Application.MainLoop.Invoke(() => {
                Application.Top.Remove(this);
                Application.RequestStop();
            });
        }


        public static Notification Show(string message, bool enableContol = true)
        {
            Notification a = null;
            bool wait = true;

            Application.MainLoop.Invoke(() => {

                // Create an instance of the NotificationWindow
                var notificationWindow = new Notification(message, enableContol);

                // Add the NotificationWindow to the top-level window
                Application.Top.Add(notificationWindow);

                // Run the notification window modally
                Application.Run(notificationWindow);

                a = notificationWindow;
                wait= false;
            });

            while(wait) { }

            return a;

        }


    }
}
