using System;
using Microsoft.SPOT;

namespace Verdant.HAL
{
    public enum ConnectivityState
    {
        Unknown,            
        AwaitingConfig,     
        Configured,         
        HaveInternet,       
        HaveServices          
    }

    public interface IAnnunciator
    {
        void PulseDebug();
        void MediaAccess(bool state);
        void NetworkActivity();

        ConnectivityState ConnectivityState { get; set; }
    }
}
