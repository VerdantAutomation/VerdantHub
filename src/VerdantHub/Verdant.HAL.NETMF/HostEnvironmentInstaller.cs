using System;
using Microsoft.SPOT;

using Verdant.Common;

namespace Verdant.HAL
{
    public class HostEnvironmentInstaller : IContainerInstaller
    {
        public void Install(Container container)
        {
            //container.Register(typeof(IPeerChannel), typeof(PeerChannel)).AsSingleton();
            //container.Register(typeof(IFileSystem), typeof(FlashFileSystem)).AsSingleton();
        }
    }
}
