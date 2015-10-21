using System;
using Microsoft.SPOT;

using Gadgeteer;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GHIElectronics.Gadgeteer;


using Verdant.Common;
using Verdant.HAL;
using Verdant.Hub.Drivers;

namespace Verdant.Hub
{
    public partial class Program : Gadgeteer.Program
    {
        private HAL _hal;
        private IAnnunciator _annunciator;

        public static void Main()
        {
            Debug.EnableGCMessages(true);

            // As much as I would like to bury this in the dicontainer setup in Initialize(), this has to happen before
            //   the Program() instance can be created
            DiContainer.Instance.Register(typeof(IMainboard), typeof(Spider1MainBoard)).AsSingleton();
            var mb = (IMainboard)DiContainer.Instance.Resolve(typeof(IMainboard));
            Program.Mainboard = mb.MainboardInstance;

            var p = new Program();
            p.Initialize();
            p.Run();
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

            _annunciator = (IAnnunciator)DiContainer.Instance.Resolve(typeof(IAnnunciator));

            var wifi = (IWirelessNetworking)DiContainer.Instance.Resolve(typeof(IWirelessNetworking));
            wifi.Initialize();

            GT.Timer timer = new GT.Timer(500); // Create a timer
            timer.Tick += Timer_Tick;
            timer.Start(); // Start the timer

        }

        private void Timer_Tick(Timer timer)
        {
            // Proof of life
            _annunciator.PulseDebug();
            GHI.Processor.Watchdog.ResetCounter();
        }

        private void InitializeHal()
        {

            // legacy gadgeteer drivers
            _hal = new HAL();
            _hal.Initialize();

            Debug.Print("HAL initialization complete...");
        }


        private static Dispatcher _dispatcher = null;
        internal static Dispatcher Dispatcher
        {
            get
            {
                if (_dispatcher == null)
                    _dispatcher = Dispatcher.CurrentDispatcher;
                return _dispatcher;
            }
        }

    }
}
