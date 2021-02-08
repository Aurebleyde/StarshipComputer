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
        /*public static void LandingBurn(Vessel vessel)
        {
            vessel.Control.Throttle = 0;
            bool SuicideBurnText = false;
            float throt = Throttle.ThrottleToTWR(vessel, 0.0f, 3);
            bool SuicideBurn = false;

            double landedAltitude = 25;

            while (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 3000) { Thread.Sleep(500); }

            bool Engine3Cut = false;

            while (true)
            {
                if (SuicideBurnText == false) { throt = Throttle.ThrottleToTWR(vessel, 0.0f, 1); }

                double Speed = vessel.Flight(vessel.SurfaceReferenceFrame).TrueAirSpeed;
                if (Speed > 130) //130
                    Speed = 130;

                double vSpeed = vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed;

                double trueRadar = vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude - landedAltitude;
                double g = vessel.Orbit.Body.SurfaceGravity;
                double maxDecelThree = ((Engines.RaptorSL[0].MaxThrust * 2/*(Engines.RaptorSL.Count - 0)*//*) / vessel.Mass) - g;
                /*double stopDistThree = Math.Pow(Speed, 2) / (1.0 * maxDecelThree); 
                double impactTime = trueRadar / Speed;

                if ((trueRadar - (Speed * 1.0f) <= stopDistThree || trueRadar < 600) && SuicideBurnText == false)//1.0 | 1000
                {
                    Console.WriteLine("Landing Burn startup");
                    SuicideBurnText = true;

                    throt = 0.001f;
                    vessel.Control.Throttle = throt;
                    Engines.ActivateEngineSL(1);
                    Engines.ActivateEngineSL(2);
                    Engines.ActivateEngineSL(3);

                    while (vessel.Flight(vessel.SurfaceReferenceFrame).Pitch < 60) { } //45
                }

                if (Engines.RaptorSL[2 - 1].Thrust > 100000 && Engines.RaptorSL[3 - 1].Thrust > 100000 && Engine3Cut == false)
                {
                    Engines.CutoffEngineSL(1);
                    Engine3Cut = true;
                }

                if (SuicideBurnText)
                    throt = (float)(stopDistThree / trueRadar);

                if (trueRadar < 100 && vSpeed > -2)
                    throt = Throttle.ThrottleToTWR(vessel, 0.9f, 2); //1.18
                else if (trueRadar < 200 && vSpeed > -5)
                    throt = Throttle.ThrottleToTWR(vessel, 1.3f, 2); //0.90

                if (vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed > -0.1 && SuicideBurn == false && trueRadar < 5)
                {
                    vessel.Control.Throttle = 0;
                    throt = 0;
                    Console.WriteLine("Landing burn shutdown");
                    Console.WriteLine("TrueRadar : " + trueRadar);

                    break;
                }

                if (throt <= 0.001f && SuicideBurnText == true)
                    throt = 0.001f;

                vessel.Control.Throttle = throt;
            }
        }*/
    }
}
