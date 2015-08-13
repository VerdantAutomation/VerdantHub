using System;
using Microsoft.SPOT;

using Gadgeteer;
using GHIElectronics.Gadgeteer;

using Verdant.HAL;

namespace Verdant.Hub.Drivers
{
    class Spider1MainBoard : IMainboard
    {
        private FEZSpider _mb;

        public Spider1MainBoard()
        {
            _mb = new FEZSpider();
        }

        public Mainboard MainboardInstance
        {
            get { return _mb; }
        }

    }
}
