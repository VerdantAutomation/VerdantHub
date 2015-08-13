using System;
using Microsoft.SPOT;

using Gadgeteer;
using GTM = Gadgeteer.Modules;
using GHIElectronics.Gadgeteer;
using Verdant.Common;
using Verdant.HAL;

namespace Verdant.Hub
{
    class HAL
    {
        /// <summary>The USB Client DP module using socket 1 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.USBClientDP usbClientDP;

        /// <summary>The Button module using socket 9 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Button button;

        /// <summary>The SD Card module using socket 5 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.SDCard sdCard;

        /// <summary>The USB-Serial module using socket 8 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.USBSerial usbSerial;

        public HAL()
        {
        }

        public void Initialize()
        { 
            InitializeModules();
        }

        private void InitializeModules()
        {
            this.usbClientDP = new GTM.GHIElectronics.USBClientDP(1);
            this.button = new GTM.GHIElectronics.Button(9);
            this.sdCard = new GTM.GHIElectronics.SDCard(5);
            this.usbSerial = new GTM.GHIElectronics.USBSerial(8);
        }
    }
}
