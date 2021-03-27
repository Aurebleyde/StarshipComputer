using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public static class Hop
    {
        public static double TargetAltitude = 10000;

        public static bool Small()
        {
            Console.WriteLine("Set TargetAltitude in meter :");
            TargetAltitude = Double.Parse(Console.ReadLine());
            Console.WriteLine("Target Altitude set to " + TargetAltitude + "m");

            Wings.WingUpR[0].Deployed();
            Wings.WingUpR[0].Orientation(80);
            Wings.WingUpL[0].Deployed();
            Wings.WingUpL[0].Orientation(80);
            Wings.WingDownR[0].Deployed();
            Wings.WingDownR[0].Orientation(80);
            Wings.WingDownL[0].Deployed();
            Wings.WingDownL[0].Orientation(80);

            if (Startup.Igntion(3) == true)
            {
                Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.25f, 3);
                Thread.Sleep(2500);
                Starship.starship.AutoPilot.Engage();
                Engines.RaptorSL[0].Trim(4, 0);
                Starship.starship.AutoPilot.TargetPitch = 86;
                Starship.starship.AutoPilot.TargetHeading = (float)Landing.ZoneHeading(Starship.starship);
                Starship.starship.AutoPilot.TargetRoll = 0;

                Starship.starship.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(1, 1, 1);
                Starship.starship.AutoPilot.StoppingTime = new Tuple<double, double, double>(3, 3, 3);
                Starship.starship.AutoPilot.DecelerationTime = new Tuple<double, double, double>(10, 10, 10);
                Starship.starship.AutoPilot.RollThreshold = 1;
                Starship.starship.AutoPilot.PitchPIDGains = new Tuple<double, double, double>(50, 50, 0);
                Starship.starship.AutoPilot.YawPIDGains = new Tuple<double, double, double>(30, 30, 0);
                Starship.starship.AutoPilot.RollPIDGains = new Tuple<double, double, double>(30, 30, 0);

                while (Starship.starship.Orbit.ApoapsisAltitude - 97.6 < TargetAltitude - (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed * 1.6f))
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.15f, 3);
                }
                Console.WriteLine("Apogee");
                while (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -5)
                {
                    if (Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).SurfaceAltitude - 20 < TargetAltitude - 5)
                    {
                        Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 0.6f, 3);
                    }
                    else
                    {
                        Starship.starship.Control.Throttle = 0.001f;
                    }
                }

                Thread LBC = new Thread(Starship.Tamere);
                LBC.Start();

                Console.WriteLine("Max altitude : " + Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).SurfaceAltitude + "m");

                while (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -6) { }

                Console.WriteLine("Landing Burn control Started");
                Landing.LandingBurn(Starship.starship);

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Atmospheric()
        {
            Console.WriteLine("Set TargetAltitude in meter :");
            TargetAltitude = Double.Parse(Console.ReadLine());
            Console.WriteLine("Target Altitude set to " + TargetAltitude + "m");

            if (Startup.Igntion(3) == true)
            {
                Thread Enginess = new Thread(ThrottleGuidance);
                Enginess.Start();
                /*AscentGuidance();

                //Starship.starship.AutoPilot.Engage();
                Starship.starship.AutoPilot.TargetRoll = 0;

                Enginess.Abort();

                Thread WingsControl = new Thread(Starship.PIDcontrolNewStarship);
                WingsControl.Start();

                Console.WriteLine("Landing control started");*/
                Thread.Sleep(40000);
                Confirmed = true;
                Thread.Sleep(1000);
                Thread LBC = new Thread(Starship.Tamere);
                LBC.Start();
                Console.WriteLine("Landing Burn control Started");
                Landing.LandingBurn(Starship.starship);

                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool Confirmed = false;
        private static int ActiveEngine = 3;
        public static void ThrottleGuidance()
        {
            Thread engineControl = new Thread(EngineControl);
            //engineControl.Start();

            /*double Tosc = 0.5;
            PIDLoop ThrottlePID = new PIDLoop(0.01, 0, 0, 0.8, 1.1);
            ThrottlePID.Ki = 0.1 * ThrottlePID.Kp / Tosc;
            ThrottlePID.Kd = 0.01 * Tosc;*/

            while (Starship.starship.Orbit.ApoapsisAltitude < TargetAltitude - 500 && Confirmed == false)
            {
                double TargetSpeed = ActiveEngine * 25;

                /*ThrottlePID.MaxOutput = Throttle.ThrottleToTWR(Starship.starship, 1.1f, ActiveEngine);
                ThrottlePID.Update(Starship.starship.connection.SpaceCenter().UT, 50 - Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed);
                Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, (float)ThrottlePID.Output, ActiveEngine);*/

                if (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > TargetSpeed)
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 0.9f, ActiveEngine);
                }
                else if (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > TargetSpeed / 1.1)
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.01f, ActiveEngine);
                }
                else
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.15f, ActiveEngine);
                }
            }

            while ((Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 17/* || Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Pitch > 70*/) && Confirmed == false)
            {
                /*ThrottlePID.MaxOutput = Throttle.ThrottleToTWR(Starship.starship, 1.1f, ActiveEngine);
                ThrottlePID.Update(Starship.starship.connection.SpaceCenter().UT, TargetAltitude - Starship.starship.Orbit.ApoapsisAltitude);
                Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, (float)ThrottlePID.Output, ActiveEngine);*/

                if (Starship.starship.Orbit.ApoapsisAltitude > TargetAltitude + 100)
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 0.80f, ActiveEngine);
                }
                else if (Starship.starship.Orbit.ApoapsisAltitude > TargetAltitude)
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 0.95f, ActiveEngine);
                }
                else
                {
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.15f, ActiveEngine);
                }
            }

            Starship.starship.Control.Throttle = 0;
        }
        public static void EngineControl()
        {
            bool Engine1 = true;
            bool Engine2 = true;
            bool Engine3 = true;

            Thread.Sleep(10000);

            while(Starship.starship.Control.Throttle != 0 && Confirmed == false)
            {
                if (Engine2 == true && Starship.starship.Control.Throttle < 0.1)
                {
                    Starship.starship.AutoPilot.TargetPitchAndHeading(91, 0);
                    Engine2 = false;
                    Engines.RaptorSL[1].Shutdown();
                    ActiveEngine -= 1;
                    Thread.Sleep(2000);
                }
                if (Engine2 == false && Engine3 == true && Starship.starship.Control.Throttle < 0.15)
                {
                    Engine3 = false;
                    Engines.RaptorSL[2].Shutdown();
                    ActiveEngine -= 1;
                    Starship.starship.AutoPilot.TargetPitchAndHeading(95, 90);
                    //Starship.starship.AutoPilot.RollPIDGains = new Tuple<double, double, double>(2, 2, 0);
                }
            }

            Engine1 = false;
            Engines.RaptorSL[0].Shutdown();
            ActiveEngine -= 1;
        }

        public static void AscentGuidance()
        {
            while (Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).SurfaceAltitude < 1000 && Confirmed == false) { /*Starship.starship.AutoPilot.TargetPitchAndHeading(90, 90);*/ }

            Starship.starship.AutoPilot.AutoTune = false;
            //Starship.starship.AutoPilot.RollPIDGains = new Tuple<double, double, double>(0.5, 0.5, 0);

            Starship.starship.AutoPilot.TargetPitchAndHeading(91, 90);
            Starship.starship.AutoPilot.TargetRoll = 0;

            while (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed < 40 && Confirmed == false) { }

            while (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 20 && Confirmed == false)
            {

            }

            Starship.starship.AutoPilot.Disengage();
            Starship.starship.Control.Pitch = -1;

            while (Starship.starship.Flight(Starship.starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 0 && Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Pitch > 70 && Confirmed == false)
            {
                Starship.starship.Control.Pitch = -1;
            }

            Starship.starship.Control.Pitch = 0;
        }
    }
}
