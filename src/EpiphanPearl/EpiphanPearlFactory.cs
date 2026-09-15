using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using PepperDash.Essentials.Core;

namespace PepperDash.Essentials.Plugins
{
    public class EpiphanPearlFactory: EssentialsPluginDeviceFactory<EpiphanPearlController>
    {
        public EpiphanPearlFactory()
        {
            TypeNames = new List<string> { "epiphanpearl2" };
            MinimumEssentialsFrameworkVersion = "3.0.0";
        }

        public override EssentialsDevice BuildDevice(PepperDash.Essentials.Core.Config.DeviceConfig dc)
        {
            return new EpiphanPearlController(dc);
        }
    }
}