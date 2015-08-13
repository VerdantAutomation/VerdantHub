using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

using Verdant.HAL;

namespace Verdant.Hub.Drivers
{
    class LedStripAnnunciator : IAnnunciator
    {
        /// <summary>The LED Strip module using socket 11 of the mainboard.</summary>
        private GTM.GHIElectronics.LEDStrip _ledStrip;
        private object[] _ledLock = new object[] { new object(), new object(), new object(), new object(), new object(), new object(), new object() };
        private long _netActivityLastSet;
        private long _debugLastSet;

        public LedStripAnnunciator()
        {
            _ledStrip = new GTM.GHIElectronics.LEDStrip(11);

            GT.Timer timer = new GT.Timer(125); // Create a timer
            timer.Tick += Timer_Tick;
            timer.Start(); // Start the timer
        }

        private void Timer_Tick(GT.Timer timer)
        {
            var now = DateTime.Now.Ticks;
            lock (_ledLock[0])
            {
                if (_debugLastSet != 0 && now - _debugLastSet > 125)
                {
                    _ledStrip.SetLed(0, false);
                    _debugLastSet = 0;
                }
            }

            lock (_ledLock[1])
            {
                if (_netActivityLastSet != 0 && now - _netActivityLastSet > 250)
                {
                    _ledStrip.SetLed(1, false);
                    _netActivityLastSet = 0;
                }
            }
        }

        public void NetworkActivity()
        {
            lock (_ledLock[1])
            {
                _ledStrip.SetLed(1, true);
                _netActivityLastSet = DateTime.Now.Ticks;
            }
        }

        private ConnectivityState _conectivityState = ConnectivityState.Unknown;
        public ConnectivityState ConnectivityState
        {
            get { return _conectivityState; }
            set
            {
                _conectivityState = value;
                lock (_ledLock[2]) // 2 will stand in as the lock for 2-5
                {
                    switch (_conectivityState)
                    {
                        case ConnectivityState.Unknown:
                            _ledStrip.SetLed(2, false);
                            _ledStrip.SetLed(3, false);
                            _ledStrip.SetLed(4, false);
                            _ledStrip.SetLed(5, false);
                            break;
                        case ConnectivityState.AwaitingConfig:
                            _ledStrip.SetLed(2, false);
                            _ledStrip.SetLed(3, false);
                            _ledStrip.SetLed(4, false);
                            _ledStrip.SetLed(5, true);
                            break;
                        case ConnectivityState.Configured:
                            _ledStrip.SetLed(2, false);
                            _ledStrip.SetLed(3, false);
                            _ledStrip.SetLed(4, true);
                            _ledStrip.SetLed(5, false);
                            break;
                        case ConnectivityState.HaveInternet:
                            _ledStrip.SetLed(2, false);
                            _ledStrip.SetLed(3, true);
                            _ledStrip.SetLed(4, true);
                            _ledStrip.SetLed(5, false);
                            break;
                        case ConnectivityState.HaveServices:
                            _ledStrip.SetLed(2, true);
                            _ledStrip.SetLed(3, true);
                            _ledStrip.SetLed(4, true);
                            _ledStrip.SetLed(5, false);
                            break;

                    }
                }
            }
        }

        public void PulseDebug()
        {
            lock (_ledLock[0])
            {
                _ledStrip.SetLed(0, true);
                _debugLastSet = DateTime.Now.Ticks;
            }
        }

        public void MediaAccess(bool state)
        {
            lock (_ledLock[6])
            {
                _ledStrip.SetLed(6, state);
            }
        }
    }
}
