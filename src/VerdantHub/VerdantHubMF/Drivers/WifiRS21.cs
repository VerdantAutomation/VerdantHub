using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Net.NetworkInformation;

using GHI.Networking;

using Verdant.HAL;
using Verdant.Common;

namespace Verdant.Hub.Drivers
{
    class WifiRS21 : IWirelessNetworking
    {
        private WiFiRS9110 _wifi;
        private IAnnunciator _annunciator;

        public WifiRS21()
        {
            _annunciator = (IAnnunciator)DiContainer.Instance.Resolve(typeof(IAnnunciator));

            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

            _wifi = new WiFiRS9110(
                GHI.Pins.FEZSpider.Socket6.SpiModule,
                GHI.Pins.FEZSpider.Socket6.Pin6,
                GHI.Pins.FEZSpider.Socket6.Pin3,
                GHI.Pins.FEZSpider.Socket6.Pin4,
                24000);
            _wifi.Open();
            _wifi.EnableDhcp();
            _wifi.EnableDynamicDns();
            _wifi.Join("XXX", "XXX");
        }

        public void Initialize()
        {
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
                _annunciator.ConnectivityState = ConnectivityState.HaveInternet;
            else
                _annunciator.ConnectivityState = ConnectivityState.Configured;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
        }
    }
}
