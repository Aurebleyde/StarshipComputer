using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Landing
    {
        public static void LandingBurn(Vessel vessel)
        {
            vessel.Control.Throttle = 0;
            bool SuicideBurnText = false;
            float throt = Throttle.ThrottleToTWR(vessel, 0.0f);
            bool SuicideBurn = false;

            double landedAltitude = 25;

            while (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 8000) { Thread.Sleep(500); }

            while (true)
            {
                if (SuicideBurnText == false) { throt = Throttle.ThrottleToTWR(vessel, 0.0f); }

                double Speed = vessel.Flight(vessel.SurfaceReferenceFrame).TrueAirSpeed;
                if (Speed > 110)
                    Speed = 110;

                double trueRadar = vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude - landedAltitude;
                double g = vessel.Orbit.Body.SurfaceGravity;
                double maxDecelThree = ((Engines.RaptorSL[0].MaxThrust * (Engines.RaptorSL.Count - 0)) / vessel.Mass) - g;
                double stopDistThree = Math.Pow(Speed, 2) / (1.0 * maxDecelThree);
                double impactTime = trueRadar / Speed;

                if (trueRadar - (Speed * 1.0f) <= stopDistThree && SuicideBurnText == false)
                {
                    Console.WriteLine("Landing Burn startup");
                    SuicideBurnText = true;

                    throt = 1;
                }

                if (SuicideBurnText)
                    throt = (float)(stopDistThree / trueRadar);

                if (trueRadar < 200 && Speed < 5)
                    throt = Throttle.ThrottleToTWR(vessel, 1.05f);
                else if (trueRadar < 100 && Speed < 2)
                    throt = Throttle.ThrottleToTWR(vessel, 0.90f);

                if (vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed > -0.1 && SuicideBurn == false)
                {
                    vessel.Control.Throttle = 0;
                    throt = 0;
                    Console.WriteLine("Landing burn shutdown");

                    break;
                }

                if (throt <= 0.001f && SuicideBurnText == true)
                    throt = 0.001f;

                vessel.Control.Throttle = throt;
            }
        }
    }
}
