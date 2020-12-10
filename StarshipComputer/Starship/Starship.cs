﻿using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using KRPC.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace StarshipComputer
{
    public class Starship
    {
        public static Vessel starship;

        public Starship(Connection connection, int number)
        {
            foreach (Vessel vessel in connection.SpaceCenter().Vessels)
            {
                if (vessel.Name.Contains("Starship") && vessel.Name.Contains("SN") && vessel.Type == VesselType.Probe)
                {
                    starship = vessel;
                    Console.WriteLine(starship.Name + " has been recovered.");
                    starship.Name = "Starship SN " + number;
                    Console.WriteLine("renamed in " + starship.Name);
                }
                /*else if (vessel.Name.Contains("Starhopper") && vessel.Type == VesselType.Probe)
                {
                    starship = vessel;
                    Console.WriteLine(starship.Name + " has been recovered.");
                    starship.Name = "Starhopper";
                }*/
            }

            Console.WriteLine("Engine setup...");
            Engines engine = new Engines(starship);

            Console.WriteLine("Wing setup...");
            Wings wing = new Wings(starship);

            Console.WriteLine("Tank setup...");
            Ressources tank = new Ressources(starship);

            Console.WriteLine("Legs setup...");
            Legs legs = new Legs(starship);
        }

        public void CommandControl()
        {
            while (true)
            {
                string command = Console.ReadLine();
                if (command[0] == '/')
                {
                    string[] items = command.Split('/', ' ');
                    // 
                    // maxspeed
                    // argument 1
                    // argument 2
                    string[] args = new string[items.Length - 2];

                    for (int i = 2; i < items.Length; i++)
                    {
                        args[i - 2] = items[i];
                    }

                    Console.WriteLine(items[1]);

                    if (!Command.Execute(items[1], args))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Syntax error in command : " + Command.Get(items[1]).Help);
                        Console.ResetColor();
                    }
                }

                command = null;
            }
        }

        public static bool GimbalTest()
        {
            Console.WriteLine("Starship Gimbal Test start...");

            foreach (Engine engine in Engines.RaptorSL)
            {
                engine.GimbalLocked = true;
            }
            foreach (Engine engine in Engines.RaptorVac)
            {
                engine.GimbalLocked = true;
            }

            Console.WriteLine("Gimbal of all engine locked.");
            int number = 0;
            foreach (Engine engine in Engines.RaptorSL)
            {
                number += 1;
                starship.Control.Throttle = 0;
                Console.WriteLine("Engine " + number);
                engine.GimbalLocked = false;
                engine.Active = true;
                starship.Control.Pitch = 1;
                Thread.Sleep(1000);
                starship.Control.Pitch = -1;
                Thread.Sleep(1000);
                starship.Control.Pitch = 0;
                starship.Control.Yaw = 1;
                Thread.Sleep(1000);
                starship.Control.Yaw = -1;
                Thread.Sleep(1000);
                starship.Control.Yaw = 0;
                starship.Control.Roll = 1;
                Thread.Sleep(1000);
                starship.Control.Roll = -1;
                Thread.Sleep(1000);
                starship.Control.Roll = 0;
                Thread.Sleep(1000);
                engine.GimbalLocked = true;
                engine.Active = false;
            }

            number = 0;
            foreach (Engine engine in Engines.RaptorSL)
            {
                number += 1;
                starship.Control.Throttle = 0;
                Console.WriteLine("Engine " + number);
                engine.GimbalLocked = false;
                engine.Active = true;
            }

            starship.Control.Pitch = 1;
            Thread.Sleep(1000);
            starship.Control.Pitch = -1;
            Thread.Sleep(1000);
            starship.Control.Pitch = 0;
            starship.Control.Yaw = 1;
            Thread.Sleep(1000);
            starship.Control.Yaw = -1;
            Thread.Sleep(1000);
            starship.Control.Yaw = 0;
            starship.Control.Roll = 1;
            Thread.Sleep(1000);
            starship.Control.Roll = -1;
            Thread.Sleep(1000);
            starship.Control.Roll = 0;
            Thread.Sleep(1000);

            foreach (Engine engine in Engines.RaptorSL)
            {
                number += 1;
                starship.Control.Throttle = 0;
                Console.WriteLine("Engine " + number);
                engine.GimbalLocked = true;
                engine.Active = false;
            }

            return true;
        }

        public static bool StaticFire(string[] args)
        {
            int number = 0;
            switch (args[0])
            {
                case "all":
                    number = 0;
                    foreach (Engine engine in Engines.RaptorSL)
                    {
                        number += 1;
                        starship.Control.Throttle = 0;
                        engine.Active = true;
                        Console.WriteLine("Engine " + number + " activate.");
                    }
                    Thread.Sleep(1000);
                    Console.WriteLine("Throttle 0.1");
                    starship.Control.Throttle = 0.1f;
                    Thread.Sleep(5000);
                    Console.WriteLine("Throttle up");
                    starship.Control.Throttle = 0.3f;
                    Thread.Sleep(7000);
                    Console.WriteLine("Shutdown");

                    number = 0;
                    foreach (Engine engine in Engines.RaptorSL)
                    {
                        number += 1;
                        engine.Active = false;
                        starship.Control.Throttle = 0;
                        Console.WriteLine("Engine " + number + " desactivate.");
                    }
                    break;

                default:
                    number = 0;
                    foreach (Engine engine in Engines.RaptorSL)
                    {
                        number += 1;
                        switch (args[0])
                        {
                            case "1":
                                starship.Control.Throttle = 0;
                                engine.Active = true;
                                Console.WriteLine("Engine " + number + " activate.");
                                Thread.Sleep(1000);
                                Console.WriteLine("Throttle 0.1");
                                starship.Control.Throttle = 0.1f;
                                Thread.Sleep(3000);
                                Console.WriteLine("Throttle up");
                                starship.Control.Throttle = 1;
                                Thread.Sleep(5000);
                                Console.WriteLine("Shutdown");
                                engine.Active = false;
                                starship.Control.Throttle = 0;
                                break;

                            case "2":
                                starship.Control.Throttle = 0;
                                engine.Active = true;
                                Console.WriteLine("Engine " + number + " activate.");
                                Thread.Sleep(1000);
                                Console.WriteLine("Throttle 0.1");
                                starship.Control.Throttle = 0.1f;
                                Thread.Sleep(3000);
                                Console.WriteLine("Throttle up");
                                starship.Control.Throttle = 1;
                                Thread.Sleep(5000);
                                Console.WriteLine("Shutdown");
                                engine.Active = false;
                                starship.Control.Throttle = 0;
                                break;

                            case "3":
                                starship.Control.Throttle = 0;
                                engine.Active = true;
                                Console.WriteLine("Engine " + number + " activate.");
                                Thread.Sleep(1000);
                                Console.WriteLine("Throttle 0.1");
                                starship.Control.Throttle = 0.1f;
                                Thread.Sleep(3000);
                                Console.WriteLine("Throttle up");
                                starship.Control.Throttle = 1;
                                Thread.Sleep(5000);
                                Console.WriteLine("Shutdown");
                                engine.Active = false;
                                starship.Control.Throttle = 0;
                                break;
                        }
                    }
                    break;
            }

            return true;
        }

        public static bool WingTest()
        {
            Console.WriteLine("Center of Mass set");
            CenterMass = starship.Flight(starship.SurfaceReferenceFrame).CenterOfMass;
            Console.WriteLine(CenterMass);
            Console.WriteLine("Center of Lift set");
            CenterLift = starship.Flight(starship.SurfaceReferenceFrame).LiftCoefficient;
            Console.WriteLine(CenterLift);

            Console.WriteLine("Top Wing");
            foreach (Part hinge in Wings.WingUp)
            {
                Console.WriteLine("New hinge");
                Console.WriteLine("Hinge to 0.");
                Module HingeModule = hinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                HingeModule.SetFieldFloat("Target Angle", 0);
                Thread.Sleep(5000);
                Console.WriteLine("Hinge to 90.");
                HingeModule.SetFieldFloat("Target Angle", 90);
                Thread.Sleep(5000);
            }

            Console.WriteLine("Down Wing");
            foreach (Part hinge in Wings.WingDown)
            {
                Console.WriteLine("New hinge");
                Console.WriteLine("Hinge to 0.");
                Module HingeModule = hinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                HingeModule.SetFieldFloat("Target Angle", 0);
                Thread.Sleep(5000);
                Console.WriteLine("Hinge to 90.");
                HingeModule.SetFieldFloat("Target Angle", 90);
                Thread.Sleep(5000);
            }

            int i = 0;

            Console.WriteLine("Stability Pitch placement test");
            while (true)
            {
                CenterLift = starship.Flight(starship.SurfaceReferenceFrame).LiftCoefficient;

                float CenterDiff = (float)(CenterLift - CenterMass.Item2);
                Console.WriteLine(CenterDiff);

                foreach (Part hinge in Wings.WingUp)
                {
                    Module HingeModule = hinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                    HingeModule.SetFieldFloat("Target Angle", CenterDiff);
                }
                foreach (Part hinge in Wings.WingDown)
                {
                    Module HingeModule = hinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                    HingeModule.SetFieldFloat("Target Angle", CenterDiff);
                }

                i += 1;
                if (i == 60)
                    break;
            }

            Console.WriteLine("Reset Wings position");
            foreach (Part hinge in Wings.WingUp)
            {
                Module HingeModule = hinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                HingeModule.SetFieldFloat("Target Angle", 90);
            }
            foreach (Part hinge in Wings.WingDown)
            {
                Module HingeModule = hinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                HingeModule.SetFieldFloat("Target Angle", 90);
            }

            return true;
        }

        public static bool KnowPosition()
        {
            Console.WriteLine($"{starship.Position(starship.Orbit.Body.ReferenceFrame).Item1} | {starship.Position(starship.Orbit.Body.ReferenceFrame).Item2} | {starship.Position(starship.SurfaceReferenceFrame).Item3}");
            return true;
        }

        public static bool KnowRotation()
        {
            Console.WriteLine($"{starship.Rotation(starship.Orbit.Body.ReferenceFrame).Item1} | {starship.Rotation(starship.Orbit.Body.ReferenceFrame).Item2} | {starship.Rotation(starship.Orbit.Body.ReferenceFrame).Item3} | {starship.Rotation(starship.Orbit.Body.ReferenceFrame).Item4}");
            return true;
        }

        public static bool WingWontrolLaunch()
        {
            Thread Wing = new Thread(WingControl);
            Wing.Start();

            return true;
        }


        public static Tuple<double, double, double> CenterMass;
        public static float CenterLift;

        public static void WingControl()
        {
            Part Hinge1 = Wings.WingUp[0];
            Part Hinge2 = Wings.WingUp[1];
            Part Hinge3 = Wings.WingDown[0];
            Part Hinge4 = Wings.WingDown[1];

            Module HingeUp1 = Hinge1.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeUp2 = Hinge2.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeDown1 = Hinge3.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeDown2 = Hinge4.Modules.First(m => m.Name == "ModuleRoboticServoHinge");

            CenterMass = starship.Flight(starship.SurfaceReferenceFrame).CenterOfMass;

            while (starship.Thrust < 1000000)
            {
                CenterLift = starship.Flight(starship.SurfaceReferenceFrame).LiftCoefficient;

                float CenterDiff = (float)(CenterLift - CenterMass.Item2);
                Console.WriteLine(CenterDiff);

                //float pitch = starship.Flight(starship.SurfaceReferenceFrame).Pitch;

                float pitch = ((CenterDiff + 1) * 90) / 2;

                float pitchUp = -pitch + 90 - 20;
                if (starship.Flight(starship.SurfaceReferenceFrame).Pitch < -5)
                {
                    pitchUp = pitchUp + 15;
                }
                else if(starship.Flight(starship.SurfaceReferenceFrame).Pitch > 5)
                {
                    pitchUp = pitchUp - 50;
                }

                float pitchDown = pitch + 40;
                if (starship.Flight(starship.SurfaceReferenceFrame).Pitch < -5)
                {
                    pitchDown = pitchDown - 15;
                }
                else if (starship.Flight(starship.SurfaceReferenceFrame).Pitch > 5)
                {
                    pitchDown = pitchDown + 50;
                }


                ReferenceFrame body_ref_frame = starship.Orbit.Body.NonRotatingReferenceFrame;
                Tuple<double, double, double> angvel = starship.AngularVelocity(body_ref_frame);
                Tuple<double, double, double> angvel_corrected = starship.connection.SpaceCenter().TransformDirection(angvel, body_ref_frame, starship.ReferenceFrame);
                Console.WriteLine("AngularVelocity : " + angvel_corrected);

                Console.WriteLine(pitch);
                HingeUp1.SetFieldFloat("Target Angle", pitchUp);
                HingeUp2.SetFieldFloat("Target Angle", pitchUp);
                HingeDown1.SetFieldFloat("Target Angle", pitchDown);
                HingeDown2.SetFieldFloat("Target Angle", pitchDown);
            }

            Console.WriteLine("Go To Vertical");

            HingeUp1.SetFieldFloat("Target Angle", 90);
            HingeUp2.SetFieldFloat("Target Angle", 90);
            HingeDown1.SetFieldFloat("Target Angle", 0);
            HingeDown2.SetFieldFloat("Target Angle", 0);

            starship.Control.SAS = false;
            starship.AutoPilot.Disengage();
            Thread.Sleep(500);
            starship.Control.SAS = true;
            Thread.Sleep(2000);
            starship.Control.SASMode = SASMode.Retrograde;

            while (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed > 20) { }

            Console.WriteLine("Landing legs deployed");
            foreach (Part legs in Legs.landingLegs)
            {
                Module legModule = legs.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                legModule.SetFieldFloat("Target Angle", 90);
                Thread.Sleep(100);
            }

            starship.Control.SAS = false;
            starship.AutoPilot.Engage();
            starship.AutoPilot.TargetPitch = 90;

            Console.WriteLine("Wings full opened");
            HingeDown1.SetFieldFloat("Target Angle", 90);
            HingeDown2.SetFieldFloat("Target Angle", 90);
        }

        public static void Landing()
        {
            starship.Control.Throttle = 0;
            bool SuicideBurnText = false;
            float throt = ThrottleToTWR(0.0f);
            bool SuicideBurn = false;

            double landedAltitude = 20;

            while (starship.Flight(starship.SurfaceReferenceFrame).SurfaceAltitude > 8000) { Thread.Sleep(500); }

            while(true)
            {
                if (SuicideBurnText == false) { throt = ThrottleToTWR(0.0f); }

                double Speed = starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed;
                if (Speed > 110)
                    Speed = 110;

                double trueRadar = starship.Flight(starship.SurfaceReferenceFrame).SurfaceAltitude - landedAltitude;
                double g = starship.Orbit.Body.SurfaceGravity;
                double maxDecelThree = ((Engines.RaptorSL[0].AvailableThrust * Engines.RaptorSL.Count) / starship.Mass) - g;
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
                    throt = ThrottleToTWR(0.90f);

                if (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -0.1 && SuicideBurn == false)
                {
                    starship.Control.Throttle = 0;
                    throt = 0;
                    Console.WriteLine("Landing burn shutdown");

                    break;
                }

                if (throt <= 0.001f && SuicideBurnText == true)
                    throt = 0.001f;

                starship.Control.Throttle = throt;
            }
        }

        public static bool DesentControl()
        {
            Console.WriteLine("Wings control started");
            Thread WingsControl = new Thread(WingControl);
            WingsControl.Start();

            Console.WriteLine("Landing control started");
            Landing();

            return true;
        }

        public static float ThrottleToTWR(float twr)
        {
            float Mass = starship.Mass * starship.Orbit.Body.SurfaceGravity;
            float T = twr * Mass;
            float Throttle = (T - (330770 * Engines.RaptorSL.Count)) / (starship.AvailableThrust - (330770 * Engines.RaptorSL.Count));
            return Throttle;
        }
    }
}