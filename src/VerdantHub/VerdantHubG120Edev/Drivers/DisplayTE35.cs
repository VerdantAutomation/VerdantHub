using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Presentation;

using GHI.Processor;

using Verdant.HAL;
using VerdantHubG120Edev;

namespace Verdant.Hub.Drivers
{
    class DisplayTE35 : IDisplay
    {
        private Bitmap _lcd;
        private Application _app;

        public void Initialize(object host)
        {
            _app = (Application)host;

            OneTimeConfig();

            _lcd = new Bitmap(SystemMetrics.ScreenWidth, SystemMetrics.ScreenHeight);
            Touch.Initialize(_app);

            _app.MainWindow = new Window();
            _app.MainWindow.TouchDown += MainWindow_TouchDown;
            _app.MainWindow.TouchUp += MainWindow_TouchUp;
            _app.MainWindow.TouchMove += MainWindow_TouchMove;
        }

        private void OneTimeConfig()
        {
            Display.Populate(Display.GHIDisplay.DisplayTE35);
            Display.ShowBootupMessages = false;
            if (Display.Save())
                PowerState.RebootDevice(false);

            Bitmap startupLogo = new Bitmap(Resources.GetBytes(Resources.BinaryResources.SpashScreen), Bitmap.BitmapImageType.Bmp);
            StartupLogo.Image = startupLogo;
            StartupLogo.Enabled = true;
            StartupLogo.X = 70;
            StartupLogo.Y = 30;
            if (StartupLogo.Save()) // save startup logo & reboot device, if necessary
                PowerState.RebootDevice(false);
        }

        private void MainWindow_TouchMove(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
        }

        private void MainWindow_TouchUp(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
            //_led1.Write(false);
        }

        private void MainWindow_TouchDown(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
            //_led1.Write(true);

            ////_lcd.DrawLine(Colors.Green, 1, 20, 20, 40, 40);
            //if (e.Touches.Length > 0)
            //{
            //    _lcd.DrawEllipse(Colors.Blue, e.Touches[0].X, e.Touches[0].Y, 5, 5);
            //    _lcd.Flush();
            //}
            ////lcd.DrawText("Hello, World!", font, Colors.White, 0, 0);
            //var addr = _netif.IPAddress;
            //Debug.Print("ip addr : " + addr);
        }
    }
}
