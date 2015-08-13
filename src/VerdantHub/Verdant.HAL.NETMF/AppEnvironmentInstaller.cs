using System;
using Microsoft.SPOT;
using Verdant.Common;

namespace Verdant.HAL
{
    public class AppEnvironmentInstaller : IContainerInstaller
    {
        public void Install(Container container)
        {
            //container.Register(typeof(IDisplay), typeof(Sharp128)).AsSingleton();
            //container.Register(typeof(IFileSystem), typeof(FlashFileSystem)).AsSingleton();
        }
    }
}
