using System;
using Microsoft.SPOT;

using Gadgeteer;

namespace Verdant.HAL
{
    public interface IMainboard
    {
        Mainboard MainboardInstance { get; }
    }
}
