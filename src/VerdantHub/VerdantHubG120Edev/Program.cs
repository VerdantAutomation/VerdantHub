using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Net;
using Microsoft.SPOT.Net.NetworkInformation;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using GHI.Pins;
using GHI.Networking;

using Verdant.Common;
using Verdant.HAL;

namespace VerdantHubG120Edev
{
    public class Program : Application
    {
        private static OutputPort _led1 = new OutputPort(G120E.Gpio.P3_27, false);
        private static OutputPort _led2 = new OutputPort(G120E.Gpio.P3_26, false);
        private static OutputPort _led3 = new OutputPort(G120E.Gpio.P3_28, false);
        private static OutputPort _led4 = new OutputPort(G120E.Gpio.P3_21, false);
        private static BaseInterface _netif;

        private IAnnunciator _annunciator;

        public static void Main()
        {
            var p = new Program();
            p.Initialize();
            p.Run();
        }

        private Program()
        {
            var eth = new EthernetBuiltIn();
            eth.Open();
            if (!eth.CableConnected)
            {
                eth.Close();
                var wifi = new WiFiRS9110(SPI.SPI_module.SPI2, G120E.Gpio.P3_30, G120E.Gpio.P2_30, G120E.Gpio.P4_31);
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

        private void Initialize()
        {
#if DEBUG
            GHI.Processor.Watchdog.Enable(120000);
#else
            GHI.Processor.WatchDog.Enable(5000);
#endif

            DiContainer.Instance.Install(
                new Verdant.Hub.Drivers.Installer(),
                new Verdant.HAL.HostEnvironmentInstaller()
            );
            GHI.Processor.Watchdog.ResetCounter();

            // initialize legacy drivers
            InitializeHal();
            GHI.Processor.Watchdog.ResetCounter();

            var display = (IDisplay)DiContainer.Instance.Resolve(typeof(IDisplay));
            display.Initialize(this);
            GHI.Processor.Watchdog.ResetCounter();

            //_annunciator = (IAnnunciator)DiContainer.Instance.Resolve(typeof(IAnnunciator));

            //var wifi = (IWirelessNetworking)DiContainer.Instance.Resolve(typeof(IWirelessNetworking));
            //wifi.Initialize();
        }

        private void Timer_Tick(Timer timer)
        {
            // Proof of life
            _annunciator.PulseDebug();
            GHI.Processor.Watchdog.ResetCounter();
        }

        private void InitializeHal()
        {


            Debug.Print("HAL initialization complete...");
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
    }
}
