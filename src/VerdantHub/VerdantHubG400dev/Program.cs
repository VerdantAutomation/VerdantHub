using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Net;
using Microsoft.SPOT.Presentation.Media;

using GHI.Pins;
using GHI.Processor;
using GTM = Gadgeteer.Modules;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Touch;
using GHI.Networking;
using Microsoft.SPOT.Net.NetworkInformation;

namespace VerdantHubG400dev
{
    public class Program : Application
    {
        private static OutputPort _led1 = new OutputPort(G400.PC18, false);
        private static OutputPort _led2 = new OutputPort(G400.PD14, false);
        private static OutputPort _led3 = new OutputPort(G400.PD17, false);
        private static OutputPort _led4 = new OutputPort(G400.PD18, false);
        private static BaseInterface _netif;
        private static Bitmap _lcd;

        public static void Main()
        {
            new Program().Run();
        }

        private Program()
        {
            OneTimeConfig();

            _lcd = new Bitmap(SystemMetrics.ScreenWidth, SystemMetrics.ScreenHeight);
            Touch.Initialize(this);

            this.MainWindow = new Window();
            this.MainWindow.TouchDown += MainWindow_TouchDown;
            this.MainWindow.TouchUp += MainWindow_TouchUp;
            this.MainWindow.TouchMove += MainWindow_TouchMove;

            var eth = new EthernetBuiltIn();
            eth.Open();
            if (!eth.CableConnected)
            {
                eth.Close();
                var wifi = new WiFiRS9110(SPI.SPI_module.SPI1, G400.PD13, G400.PD12, G400.PD15);
                _netif = wifi;
                _netif.Open();
                wifi.Join("XXX", "XXX");
            }
            else
            {
                _netif = eth;
            }
            if (!_netif.IsDhcpEnabled)
                _netif.EnableDhcp();
            if (!_netif.IsDynamicDnsEnabled)
                _netif.EnableDynamicDns();

            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        private void OneTimeConfig()
        {
            Display.Populate(Display.GHIDisplay.DisplayT43);
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

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Debug.Print(" The network " + (e.IsAvailable ? "is " : "is not ") + "available");
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            var addr = _netif.IPAddress;
            Debug.Print("ip addr : " + addr);
        }

        private void MainWindow_TouchMove(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
        }

        private void MainWindow_TouchUp(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
            _led1.Write(false);
        }

        private void MainWindow_TouchDown(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
            _led1.Write(true);

            //_lcd.DrawLine(Colors.Green, 1, 20, 20, 40, 40);
            if (e.Touches.Length > 0)
            {
                _lcd.DrawEllipse(Colors.Blue, e.Touches[0].X, e.Touches[0].Y, 5, 5);
                _lcd.Flush();
            }
            //lcd.DrawText("Hello, World!", font, Colors.White, 0, 0);
            var addr = _netif.IPAddress;
            Debug.Print("ip addr : " + addr);
        }

    }
}
