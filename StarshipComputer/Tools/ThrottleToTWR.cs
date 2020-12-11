using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Throttle
    {
        public static float ThrottleToTWR(Vessel vessel, float twr)
        {
            float Mass = vessel.Mass * vessel.Orbit.Body.SurfaceGravity;
            float T = twr * Mass;
            float Throttle = (T - (330770 * Engines.RaptorSL.Count)) / (vessel.AvailableThrust - (330770 * Engines.RaptorSL.Count));
            return Throttle;
        }
    }
}
