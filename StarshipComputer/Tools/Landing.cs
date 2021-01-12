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

            while (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 3000) { Thread.Sleep(500); }

            while (true)
            {
                if (SuicideBurnText == false) { throt = Throttle.ThrottleToTWR(vessel, 0.0f); }

                double Speed = vessel.Flight(vessel.SurfaceReferenceFrame).TrueAirSpeed;
                if (Speed > 130) //130
                    Speed = 130;

                double trueRadar = vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude - landedAltitude;
                double g = vessel.Orbit.Body.SurfaceGravity;
                double maxDecelThree = ((Engines.RaptorSL[0].MaxThrust * 2/*(Engines.RaptorSL.Count - 0)*/) / vessel.Mass) - g;
                double stopDistThree = Math.Pow(Speed, 2) / (1.0 * maxDecelThree); 
                double impactTime = trueRadar / Speed;

                if (trueRadar - (Speed * 1.0f) <= stopDistThree && SuicideBurnText == false || trueRadar < 800)//1.0
                {
                    Console.WriteLine("Landing Burn startup");
                    SuicideBurnText = true;

                    throt = 0.1f;
                    vessel.Control.Throttle = throt;

                    while (vessel.Flight(vessel.SurfaceReferenceFrame).Pitch < 45) { }
                }

                if (SuicideBurnText)
                    throt = (float)(stopDistThree / trueRadar);

                if (trueRadar < 200 && Speed < 5)
                    throt = Throttle.ThrottleToTWR(vessel, 0.99f); //1.18
                else if (trueRadar < 100 && Speed < 2)
                    throt = Throttle.ThrottleToTWR(vessel, 0.85f); //0.90

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
