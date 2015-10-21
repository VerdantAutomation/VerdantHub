using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

using GTM = Gadgeteer.Modules;

using Verdant.HAL;

namespace Verdant.Hub.Drivers
{
    class DisplayTE35 : IDisplay
    {
        private GTM.GHIElectronics.DisplayTE35 _display;

        public DisplayTE35()
        {
            _display = new GTM.GHIElectronics.DisplayTE35(14, 13, 12, 10);
        }

        public void Initialize(object unused)
        {
            _display.WPFWindow.Background = new SolidColorBrush(Colors.Black);
            _display.WPFWindow.TouchDown += WPFWindow_TouchDown;
            _display.WPFWindow.TouchUp += WPFWindow_TouchUp;
        }

        private void WPFWindow_TouchUp(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
        }

        private void WPFWindow_TouchDown(object sender, Microsoft.SPOT.Input.TouchEventArgs e)
        {
        }

        //public GTM.Module.DisplayModule DisplayInstance
        //{
        //    get { return _display;  }
        //}

    }
}
