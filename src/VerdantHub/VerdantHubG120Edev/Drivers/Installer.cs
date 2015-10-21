using System;
using Microsoft.SPOT;

using Verdant.Common;
using Verdant.HAL;

namespace Verdant.Hub.Drivers
{
    public class Installer : IContainerInstaller
    {
        public void Install(Container container)
        {
//            container.Register(typeof(IAnnunciator), typeof(LedStripAnnunciator)).AsSingleton();
            container.Register(typeof(IDisplay), typeof(DisplayTE35)).AsSingleton();
//            container.Register(typeof(IWirelessNetworking), typeof(WifiRS21)).AsSingleton();
            //container.Register(typeof(IPeerChannel), typeof(PeerChannel)).AsSingleton();
            //container.Register(typeof(IFileSystem), typeof(FlashFileSystem)).AsSingleton();
        }
    }
}
