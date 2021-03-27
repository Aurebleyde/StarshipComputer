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
        public static float ThrottleToTWR(Vessel vessel, float twr, int Number)
        {
            float Mass = vessel.Mass * vessel.Orbit.Body.SurfaceGravity;
            float T = twr * Mass;
            float Throttle = (T - (330770 * Number)) / ((Engines.RaptorSL[0].MaxThrust() * Number) - (330770 * Number));

            if (Throttle < 0.0001)
                Throttle = 0.0001f;

            return Throttle;
        }

        public static float TWR(Vessel vessel)
        {
            float Mass = vessel.Mass * vessel.Orbit.Body.SurfaceGravity;
            float twr = vessel.Thrust / Mass;

            return twr;
        }
    }
}
