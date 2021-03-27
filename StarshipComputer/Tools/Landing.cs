using KRPC.Client.Services.SpaceCenter;
using KRPC.Client.Services.Trajectories;
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
            Console.WriteLine("Confirmed");
            //vessel.Control.Throttle = 0;
            bool SuicideBurnText = false;
            float throt = Throttle.ThrottleToTWR(vessel, 0.0f, 3);
            bool SuicideBurn = false;
            bool legs = false;

            double landedAltitude = 25; //25

            while (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 3000) { Thread.Sleep(500); }

            bool Engine3Cut = false;
            bool Engine2Cut = false;

            int EngineNumber = 1;

            while (true)
            {
                if (SuicideBurnText == false) { throt = Throttle.ThrottleToTWR(vessel, 0.0f, 1); }

                double Speed = vessel.Flight(vessel.SurfaceReferenceFrame).TrueAirSpeed;
                if (Speed > 130) //130
                    Speed = 130;

                double vSpeed = vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed;

                double trueRadar = vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude - landedAltitude;
                double g = vessel.Orbit.Body.SurfaceGravity;
                double maxDecelThree = ((Engines.RaptorSL[0].MaxThrust() * EngineNumber/*(Engines.RaptorSL.Count - 0)*/) / vessel.Mass) - g;
                double stopDistThree = Math.Pow(Speed, 2) / (1.0 * maxDecelThree);
                double impactTime = trueRadar / Speed;

                if ((trueRadar - (Speed * 1.0f) <= stopDistThree * 1.5 || trueRadar < 1000) && SuicideBurnText == false)//1.0 | 1000
                {
                    Console.WriteLine("Landing Burn startup");
                    SuicideBurnText = true;

                    throt = 0.001f; //0.01f
                    vessel.Control.Throttle = throt;

                    //Engines.RaptorSL[1].Activate();

                    //while (Engines.RaptorSL[1].Thrust() < 1000) { }
                    Thread.Sleep(1000);

                    //Engines.RaptorSL[0].Activate();
                    //Engines.RaptorSL[2].Activate();

                    throt = 0.3f; //0.01f
                    vessel.Control.Throttle = throt;

                    while (vessel.Flight(vessel.SurfaceReferenceFrame).Pitch < 60) { } //45
                }

                /*if (Engines.RaptorSL[1].Thrust() > 30000 && Engines.RaptorSL[2].Thrust() > 30000 && Engine3Cut == false && (Speed < 20 || (vessel.Control.Throttle < 0.01 && Throttle.TWR(vessel) > 0.95f)))
                {
                    Thread.Sleep(1000);
                    Engines.RaptorSL[0].Shutdown();
                    Engine3Cut = true;
                    EngineNumber = 2;
                }*/

                /*if (Engine3Cut == true && Engine2Cut == false && stopDistThree > trueRadar - (Speed * impactTime) && (Speed < 10 || (vessel.Control.Throttle < 0.01 && Throttle.TWR(vessel) > 0.95f)))
                {
                    Engines.RaptorSL[2].Shutdown();
                    Engine2Cut = true;
                    EngineNumber = 1;
                    Engines.RaptorSL[1].Trim(3, -4);
                }*/

                if (SuicideBurnText)
                    throt = (float)(stopDistThree / trueRadar);

                if (trueRadar < 100 && vSpeed > -3)
                    throt = Throttle.ThrottleToTWR(vessel, 0.9f, EngineNumber); //1.18
                else if (trueRadar < 200 && vSpeed > -5)
                    throt = Throttle.ThrottleToTWR(vessel, 1.3f, EngineNumber); //0.90
                else if (vSpeed > 0)
                    throt = Throttle.ThrottleToTWR(vessel, 0.5f, EngineNumber); //0.90

                if (vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed > -0.1 && SuicideBurn == false && trueRadar < 5)
                {
                    vessel.Control.Throttle = 0;
                    throt = 0;
                    Console.WriteLine("Landing burn shutdown");
                    Console.WriteLine("TrueRadar : " + trueRadar);

                    foreach (var engine in Engines.RaptorSL)
                    {
                        engine.Shutdown();
                    }

                    Console.WriteLine("Distance to zone : " + Starship.Distance(vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, Starship.InitPos.Item1, vessel.Flight(vessel.SurfaceReferenceFrame).Longitude, Starship.InitPos.Item2));

                    break;
                }

                if (legs == false && vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude < 50 && vessel.Flight(vessel.SurfaceReferenceFrame).TrueAirSpeed < 20)
                {
                    vessel.Control.Gear = true;
                    legs = true;
                    Console.WriteLine("Landing Legs deployed");
                }

                if (throt <= 0.001f && SuicideBurnText == true)
                    throt = 0.001f;

                vessel.Control.Throttle = throt;
            }
        }

        public static void EngineGuidanceKOS(Vessel vessel, Tuple<double, double> LZ)
        {
            try
            {
                Console.WriteLine("Start of Engine Guidance");
                //Console.WriteLine("Pitch PID Gains : " + vessel.AutoPilot.PitchPIDGains);
                //Console.WriteLine("Yaw PID Gains : " + vessel.AutoPilot.YawPIDGains);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Start of Engine Guidance 2");
            //vessel.AutoPilot.Engage();
            //vessel.AutoPilot.TargetRoll = 180;
            Console.WriteLine("Start of Engine Guidance 3");
            //vessel.AutoPilot.AutoTune = false;

            vessel.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(0.01, 360, 45);
            vessel.AutoPilot.StoppingTime = new Tuple<double, double, double>(3, 3, 3);
            vessel.AutoPilot.DecelerationTime = new Tuple<double, double, double>(10, 10, 10);
            vessel.AutoPilot.RollThreshold = 360;
            vessel.AutoPilot.PitchPIDGains = new Tuple<double, double, double>(50, 50, 0);
            vessel.AutoPilot.YawPIDGains = new Tuple<double, double, double>(0, 0, 0);
            vessel.AutoPilot.RollPIDGains = new Tuple<double, double, double>(50, 50, 0);
            Console.WriteLine("Start of Engine Guidance 4");
            double ImpactLat = vessel.connection.Trajectories().ImpactPos().Item1;
            double ImpactLon = vessel.connection.Trajectories().ImpactPos().Item2;

            float Pitch = 90;
            float Head = 0;

            double prevDistance = Starship.Distance(LZ.Item1, ImpactLat, LZ.Item2, ImpactLon);

            try
            {
                while (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 45)
                {
                    while (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 80)
                    {
                        ImpactLat = vessel.connection.Trajectories().ImpactPos().Item1;
                        ImpactLon = vessel.connection.Trajectories().ImpactPos().Item2;

                        if (/*prevDistance + 2 > Starship.Distance(LZ.Item1, ImpactLat, LZ.Item2, ImpactLon) && */vessel.Flight(vessel.SurfaceReferenceFrame).Speed < 40 && vessel.Flight(vessel.SurfaceReferenceFrame).HorizontalSpeed < 5)
                        {
                            /*if (Starship.Distance(LZ.Item1, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, LZ.Item2, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude) - 10 > Starship.Distance(ImpactLat, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, ImpactLon, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude))
                            {*/
                            vessel.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(0.01, 360, 45);
                            Console.WriteLine("1");
                            vessel.AutoPilot.TargetRoll = 180;
                            Pitch = Calculs.Clamp<float>(90 - ((float)Starship.Distance(ImpactLat, LZ.Item1, ImpactLon, LZ.Item2) / 1f), 70, 90); // / 15
                            Head = (float)ZoneHeading(vessel);
                            /*}
                            else
                            {
                                Console.WriteLine("2");
                                /*vessel.AutoPilot.TargetRoll = 0;
                                Pitch = Calculs.Clamp<float>(90 + ((float)Starship.Distance(ImpactLat, LZ.Item1, ImpactLon, LZ.Item2) / 15), 90, 95);
                                Head = (float)ZoneHeading(vessel) + 180;*/
                            /*vessel.AutoPilot.TargetRoll = 0;
                            Pitch = 90 - (90 - (float)RetrogradePitch(vessel)) / 2;
                            Head = (float)RetrogradeHeading(vessel);
                            vessel.AutoPilot.TargetPitchAndHeading(Pitch, Head);
                        }*/
                        }
                        else
                        {
                            vessel.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(0.01, 360, 90);
                            vessel.AutoPilot.TargetRoll = 0;
                            Pitch = 90 - (Math.Abs(90 - (float)RetrogradePitch(vessel))) / 0.5f;
                            Head = (float)RetrogradeHeading(vessel);
                            vessel.AutoPilot.TargetPitchAndHeading(Pitch, Head);
                            while ((vessel.AutoPilot.PitchError > 10 && vessel.AutoPilot.HeadingError > 25) || (vessel.AutoPilot.PitchError < -10 && vessel.AutoPilot.HeadingError < -25))
                            {
                                Console.WriteLine("Retro");
                                vessel.AutoPilot.TargetRoll = 0;
                                Pitch = 90 - (Math.Abs(90 - (float)RetrogradePitch(vessel))) / 0.5f;
                                Head = (float)RetrogradeHeading(vessel);
                                vessel.AutoPilot.TargetPitchAndHeading(Calculs.Clamp(Pitch - 18, 75, 115), Head);
                                Console.WriteLine("Error : " + vessel.AutoPilot.Error);
                                Console.WriteLine("Pitch Target = " + vessel.AutoPilot.TargetPitch);
                                Console.WriteLine("Pitch Head = " + vessel.AutoPilot.TargetHeading);
                                Console.WriteLine("Pitch Roll = " + vessel.AutoPilot.TargetRoll);

                                vessel.Control.Pitch += 0.5f;
                            }
                            Console.WriteLine("Retro End");
                        }

                        vessel.AutoPilot.TargetPitchAndHeading(Calculs.Clamp(Pitch - 18, 70, 115), Head);
                        Console.WriteLine("Error : " + vessel.AutoPilot.Error);
                        Console.WriteLine("Pitch Target = " + vessel.AutoPilot.TargetPitch);
                        Console.WriteLine("Pitch Head = " + vessel.AutoPilot.TargetHeading);
                        Console.WriteLine("Pitch Roll = " + vessel.AutoPilot.TargetRoll);

                        prevDistance = Starship.Distance(LZ.Item1, ImpactLat, LZ.Item2, ImpactLon);
                    }

                    Console.WriteLine("Retro 2");
                    vessel.AutoPilot.TargetRoll = 0;
                    Pitch = 90 - (Math.Abs(90 - (float)RetrogradePitch(vessel))) / 0.5f;
                    Head = (float)RetrogradeHeading(vessel);
                    vessel.AutoPilot.TargetPitchAndHeading(Calculs.Clamp(Pitch - 18, 75, 115), Head);
                    Console.WriteLine("Error : " + vessel.AutoPilot.Error);
                    Console.WriteLine("Pitch Target = " + vessel.AutoPilot.TargetPitch);
                    Console.WriteLine("Pitch Head = " + vessel.AutoPilot.TargetHeading);
                    Console.WriteLine("Pitch Roll = " + vessel.AutoPilot.TargetRoll);
                }
            }
            catch { }

            try
            {
                while (vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed < -0.1)
                {
                    vessel.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(0.01, 360, 90);
                    Console.WriteLine("Decel for TD");
                    vessel.AutoPilot.TargetRoll = 0;
                    Pitch = 90 - (90 - (float)RetrogradePitch(vessel)) / 2; //4
                    Head = 90; // (float)RetrogradeHeading(vessel);
                    vessel.AutoPilot.TargetPitchAndHeading(Calculs.Clamp(Pitch + 0, 80, 130), Head);
                    Console.WriteLine("Error : " + vessel.AutoPilot.Error);
                    Console.WriteLine("Pitch Target = " + vessel.AutoPilot.TargetPitch);
                    Console.WriteLine("Pitch Head = " + vessel.AutoPilot.TargetHeading);
                    Console.WriteLine("Pitch Roll = " + vessel.AutoPilot.TargetRoll);
                }
            }
            catch
            {

            }

            Console.WriteLine("Good Place to Vertical");
            vessel.AutoPilot.TargetPitch = 90;
        }

        public static void EngineGuidance(Vessel vessel, Tuple<double, double> LZ)
        {
            Console.WriteLine("Start of Engine Guidance");
            vessel.AutoPilot.Engage();
            vessel.AutoPilot.TargetRoll = 0;

            float Pitch = 90;
            float Head = 0;

            double ImpactLat = vessel.connection.Trajectories().ImpactPos().Item1;
            double ImpactLon = vessel.connection.Trajectories().ImpactPos().Item2;

            double prevDistance = Starship.Distance(LZ.Item1, ImpactLat, LZ.Item2, ImpactLon);

            vessel.AutoPilot.Engage();
            vessel.AutoPilot.TargetRoll = 180;

            vessel.AutoPilot.AutoTune = false;

            vessel.AutoPilot.TimeToPeak = new Tuple<double, double, double>(0.1, 0.1, 0.1);
            vessel.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(0.01, 0.01, 0.01);
            vessel.AutoPilot.Overshoot = new Tuple<double, double, double>(0.2, 0.2, 0.2);
            vessel.AutoPilot.StoppingTime = new Tuple<double, double, double>(2, 2, 2);
            vessel.AutoPilot.DecelerationTime = new Tuple<double, double, double>(10, 10, 10);
            vessel.AutoPilot.RollThreshold = 360;
            vessel.AutoPilot.PitchPIDGains = new Tuple<double, double, double>(50, 50, 0);
            vessel.AutoPilot.YawPIDGains = new Tuple<double, double, double>(30, 30, 0);
            vessel.AutoPilot.RollPIDGains = new Tuple<double, double, double>(30, 30, 0);

            try
            {

                while (vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed < -1 || vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude > 30)
                {
                    ImpactLat = vessel.connection.Trajectories().ImpactPos().Item1;
                    ImpactLon = vessel.connection.Trajectories().ImpactPos().Item2;

                    //Calcul l'inclinaison nécessaire pour freinder la vitesse horizontale
                    double g = vessel.Orbit.Body.SurfaceGravity;
                    double maxDecel = ((Engines.RaptorSL[0].MaxThrust() * 1/*(Engines.RaptorSL.Count - 0)*/) / vessel.Mass) - g;
                    double stopDist = Math.Pow(vessel.Flight(vessel.SurfaceReferenceFrame).HorizontalSpeed, 2) / (1.0 * maxDecel);
                    double DecelPitch = (100 * stopDist) / 1 * maxDecel; //Starship.Distance(LZ.Item1, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, LZ.Item2, vessel.Flight(vessel.SurfaceReferenceFrame).Longitude);
                    DecelPitch = 90 - ((DecelPitch * 2) * 1.1f);
                    Console.WriteLine("Decel Pitch = " + DecelPitch);

                    if (DecelPitch < 70)
                    {
                        DecelPitch = 70;
                    }

                    if (prevDistance > Starship.Distance(LZ.Item1, ImpactLat, LZ.Item2, ImpactLon))
                    {
                        if (Starship.Distance(ImpactLat, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, ImpactLon, vessel.Flight(vessel.SurfaceReferenceFrame).Longitude) < Starship.Distance(LZ.Item1, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, LZ.Item2, vessel.Flight(vessel.SurfaceReferenceFrame).Longitude))
                        {
                            if (DecelPitch >= 70)
                            {
                                Console.WriteLine("Continue");
                                Pitch = (float)Starship.Distance(LZ.Item1, vessel.Flight(vessel.SurfaceReferenceFrame).Latitude, LZ.Item2, vessel.Flight(vessel.SurfaceReferenceFrame).Longitude) / 5;
                                if (Pitch < 60) { Pitch = 60; }
                            }
                            else
                            {
                                Console.WriteLine("Too fast");
                                Pitch = (float)(90 + Math.Abs(90 - (DecelPitch * 2)));
                            }

                            Head = (float)ZoneHeading(vessel);
                        }
                        else
                        {
                            Console.WriteLine("Decel");
                            Pitch = (float)(90 + Math.Abs(90 - (DecelPitch * 2)));

                            Head = (float)ZoneHeading(vessel);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Retro");
                        Pitch = (float)RetrogradePitch(vessel) / 3;
                        Head = (float)Math.Round((float)RetrogradeHeading(vessel));
                    }

                    if (vessel.Flight(vessel.SurfaceReferenceFrame).SurfaceAltitude < 33) { Pitch = 90; }

                    vessel.AutoPilot.TargetPitch = Pitch;
                    vessel.AutoPilot.TargetHeading = Head;

                    prevDistance = Starship.Distance(LZ.Item1, ImpactLat, LZ.Item2, ImpactLon);

                    Console.WriteLine("Error : " + vessel.AutoPilot.Error);
                    Console.WriteLine("Pitch Target = " + vessel.AutoPilot.TargetPitch);
                    Console.WriteLine("Pitch Head = " + vessel.AutoPilot.TargetHeading);
                    Console.WriteLine("Pitch Roll = " + vessel.AutoPilot.TargetRoll);
                }

                vessel.AutoPilot.TargetPitch = 90;

                Console.WriteLine("pre landing");
            }
            catch
            {
                Console.WriteLine("Place to Vertical");
                vessel.AutoPilot.TargetPitch = 90;
            }
        }

        public static double ZoneHeading(Vessel vessel)
        {
            double LZVectorY = Starship.ImpactPoint().Item1 - Starship.InitPos.Item1;
            double LZVectorX = Starship.ImpactPoint().Item2 - Starship.InitPos.Item2;

            double LZAngle = 90;
            if (LZVectorX == 0)
            {
            }
            else
            {
                LZAngle = Math.Atan(LZVectorY / LZVectorX);
            }

            LZAngle = RadToDeg(LZAngle);

            double LZHeading = 0;
            if (LZVectorY >= 0 && LZVectorX < 0)
            {
                LZHeading = 90 - LZAngle;
            }
            else if (LZVectorY < 0 && LZVectorX < 0)
            {
                LZHeading = 90 - LZAngle;
            }
            else if (LZVectorX >= 0 && LZVectorY >= 0)

            {
                LZHeading = 270 - LZAngle;

            }
            else

            {
                LZHeading = 270 - LZAngle;

            }

            return LZHeading;
        }

        public static double RetrogradeHeading(Vessel vessel)
        {
            double RetroVectorY = (vessel.connection.Trajectories().ImpactPos().Item1 - vessel.Flight(vessel.SurfaceReferenceFrame).Latitude);
            double RetroVectorX = (vessel.connection.Trajectories().ImpactPos().Item2 - vessel.Flight(vessel.SurfaceReferenceFrame).Longitude);

            double RetroAngle = 90;
            if (RetroVectorX == 0)
            {
            }
            else
            {
                RetroAngle = Math.Atan(RetroVectorY / RetroVectorX);
            }

            RetroAngle = RadToDeg(RetroAngle);

            double RetroHeading = 0;
            if (RetroVectorY >= 0 && RetroVectorX < 0)
            {
                RetroHeading = 90 - RetroAngle;
            }
            else if (RetroVectorY < 0 && RetroVectorX < 0)
            {
                RetroHeading = 90 - RetroAngle;
            }
            else if (RetroVectorX >= 0 && RetroVectorY >= 0)

            {
                RetroHeading = 270 - RetroAngle;

            }
            else

            {
                RetroHeading = 270 - RetroAngle;

            }

            return RetroHeading;
        }

        public static double RetrogradePitch(Vessel vessel)
        {
            double RetroPitch = Math.Atan(Math.Abs(vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed) / Math.Abs((vessel.Flight(vessel.SurfaceReferenceFrame).TrueAirSpeed - Math.Abs(vessel.Flight(vessel.Orbit.Body.ReferenceFrame).VerticalSpeed))));
            Console.WriteLine("retro pitch : " + RetroPitch);
            RetroPitch = RadToDeg(RetroPitch);
            Console.WriteLine("retro pitch corrigé : " + RetroPitch);
            return RetroPitch;
        }

        public static double RadToDeg(double valor)
        {
            return valor * (180 / Math.PI);
        }
    }
}
