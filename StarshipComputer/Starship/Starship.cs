using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using KRPC.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KRPC.Client.Services.Trajectories;
using System.IO;
using System.Numerics;

namespace StarshipComputer
{
    public class Starship
    {
        public static Vessel starship;
        public static Connection Connection;

        public static int apoTarget = 926000;
        public static int periTarget = 926000;
        public static int headTarget = 90;
        public static int incli = 25;

        public Starship(Connection connection, int number)
        {
            Connection = connection;

            foreach (Vessel vessel in connection.SpaceCenter().Vessels)
            {
                if (vessel.Name.Contains("Starship") && vessel.Name.Contains("SN") && vessel.Type == VesselType.Probe)
                {
                    starship = vessel;
                    Console.WriteLine(starship.Name + " has been recovered.");
                    starship.Name = "Starship SN " + number;
                    Console.WriteLine("renamed in " + starship.Name);
                }
                else if (vessel.Name.Contains("Super Heavy") && vessel.Name.Contains("SN") && vessel.Type == VesselType.Probe)
                {
                    starship = vessel;
                    Console.WriteLine(starship.Name + " has been recovered.");
                    starship.Name = "Starhopper";
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

        public static void CommandControl()
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

        public static bool StaticFire(string[] arg)
        {
            Engines.RaptorSL[0].Activate();
            Engines.RaptorSL[1].Activate();
            Engines.RaptorSL[2].Activate();

            starship.Control.Throttle = Throttle.ThrottleToTWR(starship, 0.8f, 3);

            while (Engines.RaptorSL[0].Thrust() < 330770 && Engines.RaptorSL[1].Thrust() < 330770 && Engines.RaptorSL[2].Thrust() < 330770) { }

            for (int i = 0; i < 1000; i++)
            {
                starship.Control.Throttle = Throttle.ThrottleToTWR(starship, 0.8f, 3);
            }

            Engines.RaptorSL[0].Shutdown();
            Engines.RaptorSL[1].Shutdown();
            Engines.RaptorSL[2].Shutdown();

            starship.Control.Throttle = 0;

            return true;
        }

        /*public static bool GimbalTest()
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
        }*/

        /*public static bool StaticFire(string[] args)
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
        }*/

        /*public static bool WingTest()
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
        }*/

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

        /*public static bool WingWontrolLaunch()
        {
            Thread Wing = new Thread(WingControl);
            Wing.Start();

            return true;
        }*/

        public static bool TakeCoords()
        {
            //InitPos = new Tuple<double, double>(starship.Flight(starship.SurfaceReferenceFrame).Latitude, starship.Flight(starship.SurfaceReferenceFrame).Longitude); //Liftoff Position
            //InitPos = new Tuple<double, double>(25.912118, -97.140131); //Landing Pad Boca Chica
            //InitPos = new Tuple<double, double>(28.471535, -80.527918); //LZ-1
            InitPos = new Tuple<double, double>(28.494832274862347, -80.51891733604505); //LZ-2

            return true;
        }

        public static Tuple<double, double> InitPos;

        public static Tuple<double, double, double> CenterMass;
        public static float CenterLift;

        public static double Distance(double lat1, double lat2, double lon1, double lon2) //Lat1 & 2 and Lon1 & 2 = Pose Initial and Final. Eli1 & 2 = Altitude Init and Final.
        {

            int R = 6371; // Radius of the earth

            double latDistance = ToRadians(lat2 - lat1);
            double lonDistance = ToRadians(lon2 - lon1);
            double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                    + Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2))
                    * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c * 1000; // convert to meters

            /*double height = el1 - el2;*/

            distance = Math.Pow(distance, 2)/* + Math.Pow(height, 2)*/;

            return Math.Sqrt(distance);
        }

        public static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }

        public static double ToDegree(double val)
        {
            return 180 * val / Math.PI;
        }

        public static void Record()
        {
            using (StreamWriter Record = new StreamWriter($@"D:\Users\Utilisateur\Documents\KSPRP\Flight\SpaceX\Starship\StarshipRecord_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.csv", true))
            {
                Record.WriteLine("'Time', 'Output Pitch', 'Output Roll', 'Output Yaw', 'Speed', 'Altitude', 'Pitch', 'Roll', 'Yaw', 'Thrust', 'Throttle', 'Target Pitch', 'Target Heading', 'Autopilot Pitch', 'Autopilot Heading', 'Autopilot Roll'");

                while(true)
                {
                    Record.WriteLine($"'{Hour(starship.connection.SpaceCenter().UT)}', '{pidController.Output}', '{PidRoll.Output}', '{PidYaw.Output}','{starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed}', '{starship.Flight(starship.SurfaceReferenceFrame).SurfaceAltitude}', '{starship.Flight(starship.SurfaceReferenceFrame).Pitch}', '{starship.Flight(starship.SurfaceReferenceFrame).Roll}', '{starship.Flight(starship.SurfaceReferenceFrame).Heading}', '{starship.Thrust}', '{starship.Control.Throttle}', '{PitchTarget}', '{HeadingTarget}', '{starship.AutoPilot.TargetPitch}', '{starship.AutoPilot.TargetHeading}', '{starship.AutoPilot.TargetRoll}'");
                    Thread.Sleep(100);
                }
            }
        }
        public static string Hour(double uT)
        {
            return $"{uT}";
        }

        public static PIDLoop pidController;
        public static PIDLoop PidRoll;
        public static PIDLoop PidYaw;
        public static TimeSpan deltaTime;

        public static double PitchTarget = 0;
        public static double HeadingTarget = 0;

        public static Tuple<double, double> ImpactPoint()
        {
            return starship.connection.Trajectories().ImpactPos();
        }

        public static double TargetedHeading()
        {
            double TargetVectorY = /*ImpactPoint().Item1*/ starship.Flight(starship.SurfaceReferenceFrame).Latitude - InitPos.Item1;
            double TargetVectorX = /*ImpactPoint().Item2*/ starship.Flight(starship.SurfaceReferenceFrame).Longitude - InitPos.Item2;

            double TargetAngle = 90;

            if (TargetVectorX == 0)
            {
            }
            else
            {
                TargetAngle = Math.Atan(TargetVectorY / TargetVectorX);
            }

            TargetAngle = ToDegree(TargetAngle);

            double TheTargetHeading = 0;

            if (TargetVectorY >= 0 && TargetVectorX < 0)

            {
                //Console.WriteLine("1");

                TheTargetHeading = 90 - TargetAngle;

            }

            else if (TargetVectorY < 0 && TargetVectorX < 0)

            {
                //Console.WriteLine("2");

                TheTargetHeading = 90 - TargetAngle;

            }

            else if (TargetVectorX >= 0 && TargetVectorY >= 0)

            {
                //Console.WriteLine("3");

                TheTargetHeading = 270 - TargetAngle;

            }

            else

            {
                //Console.WriteLine("4");

                TheTargetHeading = 270 - TargetAngle;

            }

            Console.WriteLine("Targeted Heading" + TheTargetHeading);

            return TheTargetHeading;
        }

        /*public static void EngineCutoffAscent()
        {
            bool Engine1 = false;
            bool Engine2 = false;

            while (starship.Control.Throttle != 0)
            {
                if (Engine1 == false && (starship.Control.Throttle < 0.002 || starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 120))
                {
                    Engines.CutoffEngineSL(2);
                    Engines.MoveEngineToOff(2);
                    Engine1 = true;
                    Thread.Sleep(2000);
                }

                if (Engine1 == true && Engine2 == false && (starship.Control.Throttle < 0.002 || starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 150))
                {
                    Engines.CutoffEngineSL(3);
                    Engines.MoveEngineToOff(3);
                    Engine2 = true;
                }
            }

            Engines.CutoffEngineSL(1);
            Engines.MoveEngineToOff(1);
        }*/

        public static bool Launch()
        {
            SuperHeavy SH = new SuperHeavy(Connection);

            SH.Liftoff();

            while(starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed < 70) { }

            Thread gt = new Thread(SH.GravityTurn);
            gt.Start();
            SH.stageSep();

            Thread.Sleep(3000);

            SES1();

            SECO();

            return true;
        }
        public static void SES1()
        {
            starship = Connection.SpaceCenter().ActiveVessel;

            starship.Control.Throttle = 0.001f;
            foreach (var item in Engines.RaptorVac)
            {
                item.Active = true;
            }

            Thread.Sleep(5000);

            starship.Control.Throttle = 1;
        }

        public static void SECO()
        {
            Connection.SpaceCenter().PhysicsWarpFactor = 1;
            starship.Control.Throttle = 1;
            while (true)
            {
                if (starship.Flight(starship.SurfaceReferenceFrame).MeanAltitude > 150 && starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 0)
                {
                    starship.AutoPilot.TargetPitch = 25;
                }
                else
                {
                    starship.AutoPilot.TargetPitch = 35;
                }


                if (starship.Orbit.ApoapsisAltitude >= periTarget - 30000)
                {
                    Connection.SpaceCenter().PhysicsWarpFactor = 0;
                    starship.Control.Throttle = 0.001f;
                }

                if (starship.Orbit.ApoapsisAltitude >= periTarget)
                {
                    starship.Control.Throttle = 0;
                    Console.WriteLine("STAGE 2 : Second engine cutoff.");
                    //starship.Parts.WithTag("Second")[0].Engine.GimbalLimit = 0;
                    Console.WriteLine("STAGE 2 : Apoapsis = " + starship.Orbit.ApoapsisAltitude);
                    Console.WriteLine("STAGE 2 : Periapsis = " + starship.Orbit.PeriapsisAltitude);
                    Console.WriteLine("STAGE 2 : Inclination = " + (starship.Orbit.Inclination * 180) / Math.PI);

                    Circularisation();
                    break;
                }
            }
        }

        public static double ForceVive(double periAlt, double a)
        {
            return Math.Sqrt(starship.Orbit.Body.GravitationalParameter * ((2 / (periAlt + starship.Orbit.Body.EquatorialRadius)) - (1 / a)));
        }

        public static double DvIncli()
        {
            double SpeedOnGoodOrbit = ForceVive(apoTarget, (apoTarget + periTarget + starship.Orbit.Body.EquatorialRadius) / 2);
            double dv = 2 * SpeedOnGoodOrbit * Math.Sin(incli - starship.Orbit.Inclination / 2);
            return dv;
        }

        public static void Circularisation()
        {
            Thread.Sleep(10000);

            double SpeedApo = ForceVive(starship.Orbit.ApoapsisAltitude, starship.Orbit.SemiMajorAxis);
            double SpeedToNewApo = ForceVive(starship.Orbit.ApoapsisAltitude, ((starship.Orbit.ApoapsisAltitude + periTarget) / 2) + starship.Orbit.Body.EquatorialRadius);
            double dV = SpeedToNewApo - SpeedApo;

            foreach (var DeleteNode in starship.Control.Nodes)
            {
                DeleteNode.Remove();
            }

            Node node = starship.Control.AddNode(Connection.SpaceCenter().UT + starship.Orbit.TimeToApoapsis, (float)dV, -(float)DvIncli() / 8, 0);

            starship.AutoPilot.Engage();
            starship.AutoPilot.ReferenceFrame = starship.Orbit.Body.ReferenceFrame;
            starship.AutoPilot.TargetDirection = node.Direction(starship.Orbit.Body.ReferenceFrame);
            starship.AutoPilot.TargetRoll = 0;
            starship.Control.RCS = true;


            double m = starship.Mass;
            double isp = Engines.RaptorVac[0].SpecificImpulse;
            double F = Engines.RaptorVac[0].AvailableThrust * 3;
            double BurnTime = (m - (m / Math.Exp(node.DeltaV / (isp * starship.Orbit.Body.GravitationalParameter)))) / (F / (isp * starship.Orbit.Body.GravitationalParameter));

            Console.WriteLine("BurnTime : " + BurnTime);

            while (node.TimeTo - (BurnTime / 2) > 30) { }
            while (node.TimeTo - (BurnTime / 2) > 5) { starship.Control.Forward = 1; }
            DateTime StartTime = DateTime.Now;
            starship.Control.Forward = 0;

            starship.Control.Throttle = 0.001f;
            //starship.Parts.WithTag("Second")[0].Engine.GimbalLimit = 1;
            Console.WriteLine("STAGE2 : Second Engine Startup.");

            var dTime = DateTime.Now - StartTime;

            while (/*(dTime.TotalSeconds < BurnTime || starship.Orbit.PeriapsisAltitude < ) && */(starship.Orbit.ApoapsisAltitude < apoTarget || starship.Orbit.PeriapsisAltitude < periTarget - 15000))
            {
                dTime = DateTime.Now - StartTime;
                starship.AutoPilot.TargetDirection = node.Direction(starship.Orbit.Body.ReferenceFrame);
            }

            Console.WriteLine("dTime : " + dTime.ToString());

            starship.Control.Throttle = 0;
            Console.WriteLine("STAGE2 : Second Engine Cutoff.");
            //starship.Parts.WithTag("Second")[0].Engine.GimbalLimit = 0;

            Console.WriteLine("STAGE 2 : Apoapsis = " + starship.Orbit.ApoapsisAltitude);
            Console.WriteLine("STAGE 2 : Periapsis = " + starship.Orbit.PeriapsisAltitude);
            Console.WriteLine("STAGE 2 : Inclination = " + (starship.Orbit.Inclination * 180) / Math.PI);
        }

        public static TrackingCam cam;
        public static bool CreateCam()
        {
            cam = new TrackingCam(starship.connection.SpaceCenter().ActiveVessel);

            return true;
        }

        public static void NewLandingGuidanceLatitude()
        {
            Atmospheric.CreateTargetedTrajectory(starship, InitPos);

            starship.AutoPilot.Engage();
            starship.AutoPilot.TargetRoll = 0;

            while (starship.Control.Throttle == 0) //starship.Thrust < 100
            {
                Atmospheric.GuidanceByLatitude();

                double ZoneDistance = Distance(ImpactPoint().Item1, InitPos.Item1, ImpactPoint().Item2, InitPos.Item2);

                double Pitch = ZoneDistance / 1000;
                if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 45 && ZoneDistance > 500)
                {
                    Math.Round(Pitch *= 15 * Calculs.Clamp((ZoneDistance / 1000), 1, 15));
                }
                else if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 45 && ZoneDistance <= 500 && ZoneDistance > 100)
                {
                    Math.Round(Pitch *= 5 * Calculs.Clamp((ZoneDistance / 1000), 0.8, 15)); //1
                }
                else
                {
                    Math.Round(Pitch = -(Pitch * 5)); //10
                }

                double Head = Atmospheric.BaseHeading;
                Console.WriteLine("Base Heading : " + Head);
                double angle = Calculs.Clamp((Distance(InitPos.Item2, ImpactPoint().Item2, 0, 0) / 100) * 1, 0, 60);
                if (Atmospheric.teta < Math.PI / 2 && Atmospheric.teta > 0)
                {
                    Pitch = -(Pitch * 1.1);
                    Head += angle;
                    Console.WriteLine("Dir 1");
                }
                else if (Atmospheric.teta > -(Math.PI / 2) && Atmospheric.teta < 0)
                {
                    Pitch = -(Pitch * 1.1);
                    Head -= angle;
                    Console.WriteLine("Dir 2");
                }
                else if (Atmospheric.teta > Math.PI / 2 && Atmospheric.teta > 0)
                {
                    if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 45)
                    {
                        Pitch = Pitch * 5.5; //3.5 //1.1
                    }
                    else
                    {
                        Pitch = -Pitch * 5.5; //3.5 //1.1
                    }
                    Head -= angle;
                    Console.WriteLine("Dir 3");
                }
                else if (Atmospheric.teta < -(Math.PI / 2) && Atmospheric.teta < 0)
                {
                    if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 45)
                    {
                        Pitch = Pitch * 5.5; //2.5 //1.1
                    }
                    else
                    {
                        Pitch = -Pitch * 5.5; //2.5 //1.1
                    }
                    Head += angle;
                    Console.WriteLine("Dir 4");
                }

                if (Pitch > 25)
                    Pitch = 25;
                else if (Pitch < -25)
                    Pitch = -25;

                HeadingTarget = Math.Round(Head);
                PitchTarget = Pitch;
                starship.AutoPilot.TargetPitchAndHeading((float)PitchTarget, (float)HeadingTarget);
                Console.WriteLine("Head target : " + HeadingTarget);
                Console.WriteLine("Pitch Target : " + PitchTarget);
                Console.WriteLine("Angle : " + angle);

                if (((starship.AutoPilot.HeadingError > 2 || starship.AutoPilot.PitchError > 1)/* || (starship.AutoPilot.HeadingError < -2 || starship.AutoPilot.PitchError < -1)*/) && starship.AutoPilot.RollError < 90 && starship.Flight(starship.SurfaceReferenceFrame).Pitch < 20 && starship.Flight(starship.SurfaceReferenceFrame).Pitch > -20)
                    starship.Control.RCS = true;
                else
                    starship.Control.RCS = false;
            }
        }
        public static void NewLandingGuidance()
        {
            Atmospheric.CreateTargetedTrajectory(starship, InitPos);

            starship.AutoPilot.Engage();
            starship.AutoPilot.TargetRoll = 0;

            while (starship.Thrust < 100)
            {
                Atmospheric.FoundIntersection(starship, InitPos);

                double ZoneDistance = Distance(ImpactPoint().Item1, InitPos.Item1, ImpactPoint().Item2, InitPos.Item2);

                double Pitch = ZoneDistance / 1000;
                if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 30 && ZoneDistance > 500)
                {
                    Math.Round(Pitch *= 15);
                }
                else if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 30 && ZoneDistance <= 500 && ZoneDistance > 100)
                {
                    Math.Round(Pitch *= 5); //1
                }

                double Head = (360 - (ToDegree(Calculs.VectorAngleWithNegative(Atmospheric.targetedTrajectory) - (Math.PI / 2)))) - 98;
                Console.WriteLine("Base Heading : " + Head);
                Console.WriteLine("Distance for Angle : " + Distance(Atmospheric.intersection.Y, starship.Flight(starship.SurfaceReferenceFrame).Latitude, Atmospheric.intersection.X, starship.Flight(starship.SurfaceReferenceFrame).Longitude));
                double angle = Calculs.Clamp(Distance(Atmospheric.intersection.Y, starship.Flight(starship.SurfaceReferenceFrame).Latitude, Atmospheric.intersection.X, starship.Flight(starship.SurfaceReferenceFrame).Longitude) / 10000000, 0, 20);
                if (Atmospheric.teta < Math.PI / 2 && Atmospheric.teta > 0)
                {
                    Pitch = -Pitch;
                    Head -= angle;
                }
                else if (Atmospheric.teta > -(Math.PI / 2) && Atmospheric.teta < 0)
                {
                    Pitch = -Pitch;
                    Head += angle;
                }
                else if (Atmospheric.teta < Math.PI / 2 && Atmospheric.teta > 0)
                {
                    Head += angle;
                }
                else if (Atmospheric.teta > -(Math.PI / 2) && Atmospheric.teta < 0)
                {
                    Head -= angle;
                }

                if (Pitch > 15)
                    Pitch = 15;
                else if (Pitch < -15)
                    Pitch = -15;

                HeadingTarget = Head;
                PitchTarget = Pitch;
                starship.AutoPilot.TargetPitchAndHeading((float)PitchTarget, (float)HeadingTarget);
                Console.WriteLine("Head target : " + HeadingTarget);
                Console.WriteLine("Pitch Target : " + PitchTarget);
                Console.WriteLine("Angle : " + angle);

                if ((starship.AutoPilot.HeadingError > 2 || starship.AutoPilot.PitchError > 1) && starship.AutoPilot.RollError < 90 && starship.Flight(starship.SurfaceReferenceFrame).Pitch < 20 && starship.Flight(starship.SurfaceReferenceFrame).Pitch > -20)
                    starship.Control.RCS = true;
                else
                    starship.Control.RCS = false;
            }
        }

        public static void LandingGuidance()
        {
            double ImpactLat = 0;
            double ImpactLon = 0;

            Console.WriteLine("Gravity Center : " + starship.Flight(starship.SurfaceReferenceFrame).CenterOfMass);

            double Tosc = 3.55f; //9.27f
            PIDLoop PitchPid = new PIDLoop(0.01, 0, 0, 0, 25);
            PitchPid.Ki = 2 * PitchPid.Kp / Tosc;
            PitchPid.Kd = 2 * Tosc;

            /*starship.Control.SAS = false;
            starship.AutoPilot.Engage();
            starship.AutoPilot.AutoTune = false;

            starship.AutoPilot.TargetPitchAndHeading(88, 0);*/

           /* while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed < 40) 
            {
                starship.Control.Throttle = Throttle.ThrottleToTWR(starship, 1.2f, 3);
                starship.AutoPilot.TargetPitchAndHeading(88, 0);
            }

            starship.AutoPilot.AutoTune = true;*/

            double a = 0;

            /*Thread EngineCutoff = new Thread(EngineCutoffAscent);
            EngineCutoff.Start();*/

            /*while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 10)
            {
                a = TargetedHeading()/* - starship.Flight(starship.SurfaceReferenceFrame).Heading*///;
                /*a = Calculs.Mod((a + 180), 360) - 180;*/
                /*HeadingTarget = Math.Round(a);

                double Variable = starship.Flight().VerticalSpeed;

                if (starship.Orbit.ApoapsisAltitude < 20000) //12000
                {
                    float Throt = 0;
                    if (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed < 100)
                    {
                        Throt = Throttle.ThrottleToTWR(starship, 1.02f, Engines.ActiveEngines());
                    }
                    else
                    {
                        Throt = Throttle.ThrottleToTWR(starship, 0.97f, Engines.ActiveEngines());
                    }

                    if (Throt < 0.001)
                        Throt = 0.001f;
                    starship.Control.Throttle = Throt;
                    starship.AutoPilot.TargetPitchAndHeading(82, 90/*(float)HeadingTarget*///);
                    /*starship.AutoPilot.TargetRoll = 0;
                }
                else
                {
                    starship.Control.Throttle = 0.001f;
                    starship.AutoPilot.TargetPitchAndHeading(88, (float)HeadingTarget);
                    starship.AutoPilot.TargetRoll = 0;
                    //Console.WriteLine(HeadingTarget);
                }
            }*/

            /*starship.AutoPilot.TargetPitchAndHeading(0, (float)HeadingTarget);
            starship.AutoPilot.TargetRoll = 0;
            starship.Control.Pitch = -1;
            while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > 0) { starship.Control.Pitch = -1; }*/
                
                
            starship.Control.Throttle = 0;
            //Thread.Sleep(2000);
            starship.Control.Pitch = 0;

            while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -30) { }

            starship.AutoPilot.Engage();

            double mode = 1;

            ImpactLat = ImpactPoint().Item1;
            ImpactLon = ImpactPoint().Item2;

            double ZoneDistance = Distance(ImpactLat, InitPos.Item1, ImpactLon, InitPos.Item2);

            //PitchPid.Update(starship.connection.SpaceCenter().UT, 0 - ZoneDistance / 1000);

            a = 0;

            if (ZoneDistance > 10 && mode == 1)
            {
                a = TargetedHeading()/* - starship.Flight(starship.SurfaceReferenceFrame).Heading*/;
            }
            else
                a = starship.Flight(starship.SurfaceReferenceFrame).Heading;
            a = Calculs.Mod((a + 180), 360) - 180;

            HeadingTarget = Math.Round(a);

            while (starship.Thrust < 5000)
            {
                ImpactLat = ImpactPoint().Item1;
                ImpactLon = ImpactPoint().Item2;

                ZoneDistance = Distance(ImpactLat, InitPos.Item1, ImpactLon, InitPos.Item2);

                //PitchPid.Update(starship.connection.SpaceCenter().UT, 0 - ZoneDistance / 1000);

                if (ZoneDistance > 10 && mode == 1)
                {
                    a = TargetedHeading()/* - starship.Flight(starship.SurfaceReferenceFrame).Heading*/;
                }
                else
                    a = starship.Flight(starship.SurfaceReferenceFrame).Heading;
                a = Calculs.Mod((a + 180), 360) - 180;

                double Pitch = ZoneDistance / 1000;
                if ((ZoneDistance < 500 && ZoneDistance > 20 && mode == 2)) //<
                {
                    /*Math.Round(Pitch *= 1 * starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed / 10); //*15 //45 //60
                    Pitch = Calculs.Clamp(Pitch, 0, 30);*/
                    //Pitch += 5;
                    Pitch = Pitch;
                }
                else if (mode == 2)
                {
                    /*Math.Round(Pitch *= 1 * (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed / 10) * 2); //*60
                    Pitch = Calculs.Clamp(Pitch, 0, 30);*/
                    Pitch = Pitch * 2;
                }
                else if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 30 && ZoneDistance > 500 && mode == 1)
                {
                    Math.Round(Pitch *= 15);
                }
                else if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 30 && ZoneDistance <= 500 && ZoneDistance > 20 && mode == 1)
                {
                    Math.Round(Pitch *= 5); //5 //1
                }
                /*else if (starship.Flight(starship.Orbit.Body.ReferenceFrame).HorizontalSpeed < 40 && ZoneDistance <= 100 && mode == 1)
                {
                    Math.Round(Pitch *= 10); //1
                    Pitch -= Pitch;
                }*/
                else if (mode == 1 && starship.Flight(starship.SurfaceReferenceFrame).SurfaceAltitude < 10000)
                    Pitch = -3.5;

                if (Pitch > 15 && mode == 1)
                    Pitch = 15;
                else if (Pitch < -15 && mode == 1)
                    Pitch = -15;
                else if (Pitch > 35 && mode == 2)
                    Pitch = 35;
                else if (Pitch < -35 && mode == 2)
                    Pitch = -35;

                Vector2 ShipZone = new Vector2((float)(InitPos.Item1 - starship.Flight(starship.SurfaceReferenceFrame).Latitude), (float)(InitPos.Item2 - starship.Flight(starship.SurfaceReferenceFrame).Longitude));
                Vector2 ShipImpact = new Vector2((float)(ImpactLat - starship.Flight(starship.SurfaceReferenceFrame).Latitude), (float)(ImpactLon - starship.Flight(starship.SurfaceReferenceFrame).Longitude));
                Vector2 ShipMode = ShipZone - ShipImpact;

                //if (Distance(ImpactLat, starship.Flight(starship.SurfaceReferenceFrame).Latitude, ImpactLon, starship.Flight(starship.SurfaceReferenceFrame).Longitude) < Distance(starship.Flight(starship.SurfaceReferenceFrame).Latitude, InitPos.Item1, starship.Flight(starship.SurfaceReferenceFrame).Longitude, InitPos.Item2) - 50 || (Distance(ImpactLat, InitPos.Item1, ImpactLon, InitPos.Item2) > Distance(starship.Flight(starship.SurfaceReferenceFrame).Latitude, InitPos.Item1, starship.Flight(starship.SurfaceReferenceFrame).Longitude, InitPos.Item2) && Distance(starship.Flight(starship.SurfaceReferenceFrame).Latitude, ImpactLat, starship.Flight(starship.SurfaceReferenceFrame).Longitude, ImpactLat) < Distance(starship.Flight(starship.SurfaceReferenceFrame).Latitude, InitPos.Item1, starship.Flight(starship.SurfaceReferenceFrame).Longitude, InitPos.Item2))) //+10
                if (ShipZone.Length() > ShipImpact.Length() / 1 /* * 1 */ || (ShipZone.Length() < ShipImpact.Length() * 1 && Calculs.VectorAngle(ShipMode, ShipZone) < 90))
                { 
                    HeadingTarget = Math.Round(a);
                    PitchTarget = -Pitch; //PitchPid.Output
                    Console.WriteLine("1");
                    Console.WriteLine(ShipZone.Length());
                    mode = 1;
                }
                else
                {
                    PitchTarget = Pitch * 50; //PitchPid.Output
                    Console.WriteLine("2");

                    mode = 2;
                    //HeadingTarget = Math.Round(a);

                    /*if (HeadingTarget > 180)
                        HeadingTarget = a - 180;
                    else
                        HeadingTarget = a + 180;*/
                }

                if ((starship.AutoPilot.HeadingError > 2 || starship.AutoPilot.PitchError > 1) && starship.AutoPilot.RollError < 90 && starship.Flight(starship.SurfaceReferenceFrame).Pitch < 20 && starship.Flight(starship.SurfaceReferenceFrame).Pitch > -20)
                    starship.Control.RCS = true;
                else
                    starship.Control.RCS = false;

                /*if (HeadingTarget - starship.Flight(starship.SurfaceReferenceFrame).Heading >= 30)
                    HeadingTarget = starship.Flight(starship.SurfaceReferenceFrame).Heading - 30;
                else if (HeadingTarget - starship.Flight(starship.SurfaceReferenceFrame).Heading <= -30)
                    HeadingTarget = starship.Flight(starship.SurfaceReferenceFrame).Heading + 30;*/

                starship.AutoPilot.TargetPitchAndHeading((float)PitchTarget, (float)HeadingTarget);
                starship.AutoPilot.TargetRoll = 0;

                Console.WriteLine("After Correction : " + HeadingTarget);
                Console.WriteLine("Pitch Target : " + PitchTarget);
            }
        }

        public static void PIDcontrol()
        {
            //Part Hinge1 = Wings.WingUpR[0];
            /*Part Hinge2 = Wings.WingUpL[0];
            Part Hinge3 = Wings.WingDownR[0];
            Part Hinge4 = Wings.WingDownL[0];*/

            //Module HingeUp1 = Hinge1.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            /*Module HingeUp2 = Hinge2.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeDown1 = Hinge3.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeDown2 = Hinge4.Modules.First(m => m.Name == "ModuleRoboticServoHinge");*/

            float Tosc = 9.64f;
            float ToscRoll = 7;
            float ToscYaw = 9.55f;
            pidController = new PIDLoop(2, 0, 0, 90, 0);
            pidController.Ki = 4 * pidController.Kp / Tosc;
            pidController.Kd = 0.5 * Tosc;

            PidRoll = new PIDLoop(2, 0, 0, 25, -25);
            PidRoll.Ki = 2 * PidRoll.Kp / Tosc;
            PidRoll.Kd = 2 * ToscRoll;

            PidYaw = new PIDLoop(2, 0, 0, 25, -25);
            PidYaw.Ki = 4 * PidYaw.Kp / ToscYaw;
            PidYaw.Kd = 2 * ToscYaw;

            Thread Recorder = new Thread(Record);
            Recorder.Start();

            while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -10) { Thread.Sleep(1000); }

            while (starship.Thrust < 10000)
            {
                DateTime date1 = DateTime.Now;

                double a = HeadingTarget - starship.Flight(starship.SurfaceReferenceFrame).Heading;
                a = Calculs.Mod((a + 180), 360) - 180; //((a + 180) % 360) - 180; //
                //Console.WriteLine(HeadingTarget);
                Console.WriteLine(a);

                pidController.Update(starship.connection.SpaceCenter().UT, PitchTarget - starship.Flight(starship.SurfaceReferenceFrame).Pitch);
                PidYaw.Update(starship.connection.SpaceCenter().UT, a);
                //PidRoll.Setpoint = PidYaw.Output;
                PidRoll.Update(starship.connection.SpaceCenter().UT, 0 - starship.Flight(starship.SurfaceReferenceFrame).Roll);

                //HingeUp1.SetFieldFloat("Target Angle", (float)pidController.Output + (float)PidRoll.Output - (float)PidYaw.Output); //90-Le reste
                /*HingeUp2.SetFieldFloat("Target Angle", (float)pidController.Output - (float)PidRoll.Output + (float)PidYaw.Output); //90-Le reste
                HingeDown1.SetFieldFloat("Target Angle", 90-(float)pidController.Output + (float)PidRoll.Output + (float)PidYaw.Output); //Rien de spécial
                HingeDown2.SetFieldFloat("Target Angle", 90-(float)pidController.Output - (float)PidRoll.Output - (float)PidYaw.Output); //Rien de spécial*/
                DateTime date2 = DateTime.Now;

                deltaTime = date2 - date1;
            }

            Console.WriteLine("Go To Vertical");

            //HingeUp1.SetFieldFloat("Target Angle", 0); //90
            /*HingeUp2.SetFieldFloat("Target Angle", 0); //90
            HingeDown1.SetFieldFloat("Target Angle", 80); //0
            HingeDown2.SetFieldFloat("Target Angle", 80); //0*/

            starship.AutoPilot.Disengage();
            /*starship.AutoPilot.Engage();
            starship.AutoPilot.ReferenceFrame = starship.SurfaceVelocityReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(0, -3.14f, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);
            starship.AutoPilot.AutoTune = true;*/

            starship.Control.Pitch = 1;
            while (starship.AutoPilot.PitchError > 45 || starship.AutoPilot.HeadingError > 180) { starship.Control.Pitch = 1; }
            starship.Control.Pitch = 0;
            starship.AutoPilot.Disengage();

            while (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed > 20) { }

            Console.WriteLine("Landing legs deployed");
            /*foreach (Part legs in Legs.landingLegs)
            {
                Module legModule = legs.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                legModule.SetFieldFloat("Target Angle", 90);
            }*/
            starship.Control.Gear = true;

            starship.AutoPilot.ReferenceFrame = starship.SurfaceReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(3.14f, 0, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);

            Console.WriteLine("Wings full opened");
            /*HingeDown1.SetFieldFloat("Target Angle", 90); //90
            HingeDown2.SetFieldFloat("Target Angle", 90); //90*/
        }


        public static PIDLoop EngineRoll;
        public static PIDLoop EngineYaw;
        public static void PIDcontrolNewStarship()
        {
            starship.Control.Throttle = 0;

            TakeCoords();

            /*Part Hinge1 = Wings.WingUpR[0];
            Part Hinge2 = Wings.WingUpL[0];
            Part Hinge3 = Wings.WingDownR[0];
            Part Hinge4 = Wings.WingDownL[0];*/

            //Module HingeUp1 = Hinge1.Modules.First(m => m.Name == "ModuleControlSurface");
            /*Module HingeUp2 = Hinge2.Modules.First(m => m.Name == "ModuleControlSurface");
            Module HingeDown1 = Hinge3.Modules.First(m => m.Name == "ModuleControlSurface");
            Module HingeDown2 = Hinge4.Modules.First(m => m.Name == "ModuleControlSurface");*/
            //Hinge1.ControlSurface.Deployed = true;
            /*Hinge2.ControlSurface.Deployed = true;
            Hinge3.ControlSurface.Deployed = true;
            Hinge4.ControlSurface.Deployed = true;*/

            Wings.WingDownL[0].Deployed();
            Wings.WingDownR[0].Deployed();
            Wings.WingUpL[0].Deployed();
            Wings.WingUpR[0].Deployed();

            while (starship.Flight(starship.SurfaceReferenceFrame).Pitch > 25)
            {
                Wings.WingUpR[0].Orientation(0); //90-Le reste
                Wings.WingUpL[0].Orientation(0); //90-Le reste

                Wings.WingDownR[0].Orientation(70); //Rien de spécial
                Wings.WingDownL[0].Orientation(70); //Rien de spécial
            }

            float Tosc = 3f;
            float ToscRoll = 2.3f; //3.5f //2.1722
            float ToscYaw = 10.79f; //3.55 //10.78
            pidController = new PIDLoop(2, 0, 0, 35, -40); //1ère valeur : Puissance //2 //80, 0
            pidController.Ki = 8 * pidController.Kp / Tosc; //Anticipe //8
            pidController.Kd = 8 * Tosc; //Amorti //2

            PidRoll = new PIDLoop(1, 0, 0, 55, -55);//8 //15 -15
            PidRoll.Ki = 32 * PidRoll.Kp / ToscRoll; //16
            PidRoll.Kd = 8 * ToscRoll; //8

            PidYaw = new PIDLoop(1, 0, 0, 30, -30); //25 -25
            PidYaw.Ki = 2 * PidYaw.Kp / ToscYaw; //2
            PidYaw.Kd = 1 * ToscYaw; //0.1

            //Starship.starship.AutoPilot.RollPIDGains = new Tuple<double, double, double>(10, 10, 0);

            Thread Recorder = new Thread(Starship.Record);
            Recorder.Start();

            while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -10) { Thread.Sleep(1000); }

            starship.Control.Throttle = 0;

            while (starship.Thrust > 6000) { starship.Control.Throttle = 0; }

            double a = 0;

            while (starship.Flight(starship.SurfaceReferenceFrame).Pitch > 85)
            {
                Wings.WingUpR[0].Orientation(0); //90-Le reste
                Wings.WingUpL[0].Orientation(0); //90-Le reste

                Wings.WingDownR[0].Orientation(80); //Rien de spécial
                Wings.WingDownL[0].Orientation(80); //Rien de spécial
            }

            Thread Guidance = new Thread(LandingGuidance); //LandingGuidance
            Guidance.Start();

            starship.AutoPilot.RollThreshold = 0.001;

            starship.AutoPilot.AttenuationAngle = new Tuple<double, double, double>(0.01, 0.01, 360);
            starship.AutoPilot.YawPIDGains = new Tuple<double, double, double>(0, 0, 0);

            while(starship.Flight(starship.SurfaceReferenceFrame).Pitch > 0)
            {
                starship.Control.Pitch = -1;
            }
            starship.Parts.WithTag("Probe")[0].Modules.First(m => m.Name == "ModuleReactionWheel").SetFieldFloat("Autorité de la roue", 30);
            starship.Control.Pitch = 0;
            Thread.Sleep(2000);
            starship.Parts.WithTag("Probe")[0].Modules.First(m => m.Name == "ModuleReactionWheel").SetFieldFloat("Autorité de la roue", 100);

            while (starship.Thrust < 10000)
            {
                DateTime date1 = DateTime.Now;

                a = HeadingTarget; //Test
                //a = HeadingTarget - starship.Flight(starship.SurfaceReferenceFrame).Heading;
                a = Calculs.Mod((a + 180), 360) - 180; //((a + 180) % 360) - 180; //
                //Console.WriteLine(HeadingTarget);

                if (a < 0)
                {
                    a += 360;
                }

                Console.WriteLine("new Head : " + a);

                if (a > 280)
                {
                    if (Math.Abs(a - starship.Flight(starship.SurfaceReferenceFrame).Heading) > 180)
                    {
                        a -= 360;
                    }
                }
                else if (a < 90)
                {
                    if (Math.Abs(a - starship.Flight(starship.SurfaceReferenceFrame).Heading) > 180)
                    {
                        a += 360;
                    }
                }

                pidController.Update(starship.connection.SpaceCenter().UT, (PitchTarget - starship.Flight(starship.SurfaceReferenceFrame).Pitch) * ((1 / starship.Flight().StaticPressure) * 100000));
                PidYaw.Update(starship.connection.SpaceCenter().UT, a - starship.Flight(starship.SurfaceReferenceFrame).Heading);
                //PidRoll.Setpoint = PidYaw.Output;
                PidRoll.Update(starship.connection.SpaceCenter().UT, 0 - starship.Flight(starship.SurfaceReferenceFrame).Roll);

                if (starship.Thrust > 10000)
                {
                    Wings.WingUpR[0].Orientation(Calculs.Clamp<float>(10, 0, 80)); //90-Le reste
                    Wings.WingUpL[0].Orientation(Calculs.Clamp<float>(10, 0, 80)); //90-Le reste
                }
                else
                {
                    /*if (starship.AutoPilot.RollError < 5 && starship.AutoPilot.RollError > -5)
                    {
                        Wings.WingUpR[0].Orientation(Calculs.Clamp<float>((float)pidController.Output + ((float)PidYaw.Output / 1), 0, 80)); //90-Le reste
                        Wings.WingUpL[0].Orientation(Calculs.Clamp<float>((float)pidController.Output - ((float)PidYaw.Output / 1f), 0, 80)); //90-Le reste
                    }
                    else
                    {
                        Wings.WingUpR[0].Orientation(Calculs.Clamp<float>( 45 - ((float)PidRoll.Output / 1f), 0, 80)); //90-Le reste
                        Wings.WingUpL[0].Orientation(Calculs.Clamp<float>( 45 + ((float)PidRoll.Output / 1f), 0, 80)); //90-Le reste
                    }*/

                    Wings.WingUpR[0].Orientation(Calculs.Clamp<float>(35 + ((float)pidController.Output * 1) - ((float)PidRoll.Output / 1f) + ((float)PidYaw.Output / 1), 0, 70)); //90-Le reste
                    Wings.WingUpL[0].Orientation(Calculs.Clamp<float>(35 + ((float)pidController.Output * 1) + ((float)PidRoll.Output / 1f) - ((float)PidYaw.Output / 1f), 0, 70)); //90-Le reste
                }
                Wings.WingDownR[0].Orientation(Calculs.Clamp<float>(70 - (float)pidController.Output - ((float)PidRoll.Output / 1f) - ((float)PidYaw.Output / 1), 0, 70)); //Rien de spécial
                Wings.WingDownL[0].Orientation(Calculs.Clamp<float>(70 - (float)pidController.Output + ((float)PidRoll.Output / 1f) + ((float)PidYaw.Output / 1), 0, 70)); //Rien de spécial

                /*if (starship.AutoPilot.RollError < 5 && starship.AutoPilot.RollError > -5)
                {
                    Wings.WingDownR[0].Orientation(Calculs.Clamp<float>(90 - (float)pidController.Output - ((float)PidYaw.Output / 1), 0, 80)); //Rien de spécial
                    Wings.WingDownL[0].Orientation(Calculs.Clamp<float>(90 - (float)pidController.Output + ((float)PidYaw.Output / 1), 0, 80)); //Rien de spécial
                }
                else
                {
                    Console.WriteLine("Roll Correction -----------------------");
                    Wings.WingDownR[0].Orientation(Calculs.Clamp<float>(45 - ((float)PidRoll.Output / 1f), 0, 80)); //Rien de spécial
                    Wings.WingDownL[0].Orientation(Calculs.Clamp<float>(45 + ((float)PidRoll.Output / 1f), 0, 80)); //Rien de spécial
                }*/

                //Initial Guidage for wings DONT DELETE
                /*Wings.WingUpR[0].Orientation(Calculs.Clamp<float>((float)pidController.Output - ((float)PidRoll.Output / 1f) + ((float)PidYaw.Output / 1), 0, 80)); //90-Le reste
                Wings.WingUpL[0].Orientation(Calculs.Clamp<float>((float)pidController.Output + ((float)PidRoll.Output / 1f) - ((float)PidYaw.Output / 1f), 0, 80)); //90-Le reste
                Wings.WingDownR[0].Orientation(Calculs.Clamp<float>(90 -(float)pidController.Output - ((float)PidRoll.Output / 1f) - ((float)PidYaw.Output / 1), 0, 80)); //Rien de spécial
                Wings.WingDownL[0].Orientation(Calculs.Clamp<float>(90 -(float)pidController.Output + ((float)PidRoll.Output / 1f) + ((float)PidYaw.Output / 1), 0, 80)); //Rien de spécial*/
                DateTime date2 = DateTime.Now;

                deltaTime = date2 - date1;
            }

            starship.Control.Pitch = 0;

            Thread.Sleep(100);

            Console.WriteLine("Go To Vertical");

            Wings.WingUpR[0].Orientation(0); //90
            Wings.WingUpL[0].Orientation(0); //90
            Wings.WingDownR[0].Orientation(80); //0
            Wings.WingDownL[0].Orientation(80); //0

            starship.AutoPilot.Disengage();
            /*starship.AutoPilot.Engage();
            starship.AutoPilot.ReferenceFrame = starship.SurfaceVelocityReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(0, -3.14f, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);
            starship.AutoPilot.AutoTune = true;

            starship.Control.Pitch = 1;
            while (starship.AutoPilot.PitchError > 45 || starship.AutoPilot.HeadingError > 90) { starship.Control.Pitch = 1; }*/
            /*while (starship.Flight(starship.SurfaceReferenceFrame).Pitch < 45) { starship.Control.Pitch = 1; }
            starship.Control.Pitch = 0;*/

            //starship.AutoPilot.Disengage();
            //starship.AutoPilot.Engage();
            /*starship.AutoPilot.ReferenceFrame = starship.SurfaceReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(0, -3.14f, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);*/

            float EngineToscRoll = 5;
            EngineRoll = new PIDLoop(1, 0, 0, 1, -1);
            EngineRoll.Ki = 0.1 * EngineRoll.Kp / EngineToscRoll;
            EngineRoll.Kd = 0.1 * EngineToscRoll;
            float EngineToscYaw = 8;
            EngineYaw = new PIDLoop(1, 0, 0, 1, -1);
            EngineYaw.Ki = 0.1 * EngineYaw.Kp / EngineToscYaw;
            EngineYaw.Kd = 0.1 * EngineToscYaw;

            starship.Control.Pitch = 1;
            Thread.Sleep(1000);
            double initHead = starship.Flight(starship.SurfaceReferenceFrame).Heading;

            if (HeadingTarget < 0)
            {
                HeadingTarget += 360;
            }
            else if (HeadingTarget >= 360)
            {
                HeadingTarget -= 360;
            }
            while (/*initHead - starship.Flight(starship.SurfaceReferenceFrame).Heading > 90 || initHead - starship.Flight(starship.SurfaceReferenceFrame).Heading < -90*/ starship.Flight(starship.SurfaceReferenceFrame).Pitch < 50)
            {
                Console.WriteLine("Pitch = " + starship.Control.Pitch);
                starship.Control.Pitch = 2 - (starship.Flight(starship.SurfaceReferenceFrame).Pitch / 90) / 2;
                EngineRoll.Update(starship.connection.SpaceCenter().UT, 0 - starship.Flight(starship.SurfaceReferenceFrame).Roll);
                starship.Control.Roll = -(float)EngineRoll.Output;
                EngineYaw.Update(starship.connection.SpaceCenter().UT, HeadingTarget - starship.Flight(starship.SurfaceReferenceFrame).Heading);
                starship.Control.Yaw = -(float)EngineYaw.Output;
            }
            starship.Control.Pitch = -1;
            Console.WriteLine("Decel Flip");
            double prevRotation = starship.Rotation(starship.SurfaceVelocityReferenceFrame).Item2;
            while (prevRotation > 0)
            {
                Console.WriteLine("Rotation : " + prevRotation);
                prevRotation = starship.Rotation(starship.SurfaceVelocityReferenceFrame).Item2;
                starship.Control.Pitch = -1;
                EngineRoll.Update(starship.connection.SpaceCenter().UT, 180 - starship.Flight(starship.SurfaceReferenceFrame).Roll);
                starship.Control.Roll = 0; //-(float)EngineRoll.Output;
                EngineYaw.Update(starship.connection.SpaceCenter().UT, (HeadingTarget + 180) - starship.Flight(starship.SurfaceReferenceFrame).Heading);
                starship.Control.Yaw = (float)EngineYaw.Output;
            }
            //Thread.Sleep(1000);
            starship.Control.Pitch = 0;
            starship.Control.Yaw = 0;
            starship.Control.Roll = 0;
            //Thread.Sleep(1000);

            starship.AutoPilot.Engage();
            starship.AutoPilot.TargetPitchAndHeading(90, (float)HeadingTarget);
            //while (starship.AutoPilot.Error > 45) { }

            starship.AutoPilot.Engage();
            //starship.AutoPilot.AutoTune = true;

            /*starship.AutoPilot.TargetPitchAndHeading(85, (float)Landing.ZoneHeading(starship));
            starship.AutoPilot.TargetRoll = 90;*/

            /*while (starship.AutoPilot.PitchError > 2)
            {
                starship.AutoPilot.TargetPitchAndHeading(88, (float)Landing.ZoneHeading(starship));
            }*/

            Thread guidance = new Thread(Tamere);
            guidance.Start();

            while (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed > 25) { }

            Console.WriteLine("Wings full closed");
            Wings.WingUpR[0].Orientation(70); //90
            Wings.WingUpL[0].Orientation(70); //90

            //starship.AutoPilot.Disengage();

            while (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed > 15) //5
            {
                //starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(0, -3.14f, 0);
                //Console.WriteLine(starship.AutoPilot.TargetDirection);
            }

            Console.WriteLine("Landing legs deployed");
            /*foreach (Part legs in Legs.landingLegs)
            {
                Module legModule = legs.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                legModule.SetFieldFloat("Target Angle", 90);
            }*/
            starship.Control.Gear = true;

            /*starship.AutoPilot.ReferenceFrame = starship.SurfaceReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(3.14f, 0, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);*/

            while (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed > 0.1) { }

            Thread.Sleep(10000);

            Console.WriteLine("Wings full opened");
            Wings.WingDownR[0].Orientation(10); //90
            Wings.WingDownL[0].Orientation(10); //90
        }

        public static void WingControl()
        {
            //Part Hinge1 = Wings.WingUpR[0];
            /*Part Hinge2 = Wings.WingUpL[0];
            Part Hinge3 = Wings.WingDownR[0];
            Part Hinge4 = Wings.WingDownL[0];*/

            //Module HingeUp1 = Hinge1.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            /*Module HingeUp2 = Hinge2.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeDown1 = Hinge3.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
            Module HingeDown2 = Hinge4.Modules.First(m => m.Name == "ModuleRoboticServoHinge");*/

            CenterMass = starship.Flight(starship.SurfaceReferenceFrame).CenterOfMass;

            double PrevDistance = 0;

            while (starship.Thrust < 1000000)
            {
                Console.WriteLine(PrevDistance);

                CenterLift = starship.Flight(starship.SurfaceReferenceFrame).LiftCoefficient;

                float CenterDiff = (float)(CenterLift - CenterMass.Item2);
                Console.WriteLine(CenterDiff);

                //float pitch = starship.Flight(starship.SurfaceReferenceFrame).Pitch;

                ReferenceFrame body_ref_frame = starship.Orbit.Body.NonRotatingReferenceFrame;
                Tuple<double, double, double> angvel = starship.AngularVelocity(body_ref_frame);
                Tuple<double, double, double> angvel_corrected = starship.connection.SpaceCenter().TransformDirection(angvel, body_ref_frame, starship.ReferenceFrame);

                float pitch = ((CenterDiff + 1) * 90) / 2;

                float pitchUp = -pitch + 90 - 20;
                if (starship.Flight(starship.SurfaceReferenceFrame).Pitch < -25)
                {
                    pitchUp = pitchUp + 45;
                }
                else if(starship.Flight(starship.SurfaceReferenceFrame).Pitch > 25)
                {
                    pitchUp = pitchUp - 10;
                }
                else if (Distance(InitPos.Item1, ImpactPoint().Item1, InitPos.Item2, ImpactPoint().Item2) < Distance(starship.Flight(starship.SurfaceReferenceFrame).Latitude, InitPos.Item1, starship.Flight(starship.SurfaceReferenceFrame).Longitude, InitPos.Item2) || Distance(InitPos.Item1, ImpactPoint().Item1, InitPos.Item2, ImpactPoint().Item2) >= PrevDistance)
                {
                    pitchUp = (float)(15);
                    Console.WriteLine("11111111111111111111111111111111111111111111111111111111111");
                }
                else
                {
                    pitchUp = (float)(52 + ((double)(angvel_corrected.Item1) * 100));
                }

                float pitchDown = pitch + 40;
                if (starship.Flight(starship.SurfaceReferenceFrame).Pitch < -25)
                {
                    pitchDown = pitchDown - 65;
                }
                else if (starship.Flight(starship.SurfaceReferenceFrame).Pitch > 25)
                {
                    pitchDown = pitchDown + 52;
                }
                else if (Distance(InitPos.Item1, ImpactPoint().Item1, InitPos.Item2, ImpactPoint().Item2) < Distance(starship.Flight(starship.SurfaceReferenceFrame).Latitude, InitPos.Item1, starship.Flight(starship.SurfaceReferenceFrame).Longitude, InitPos.Item2) || Distance(InitPos.Item1, ImpactPoint().Item1, InitPos.Item2, ImpactPoint().Item2) >= PrevDistance)
                {
                    pitchDown = (float)(65);
                }
                else
                {
                    pitchDown = (float)(35 - ((double)(angvel_corrected.Item1) * 100));
                }

                PrevDistance = Distance(InitPos.Item1, ImpactPoint().Item1, InitPos.Item2, ImpactPoint().Item2);


                Console.WriteLine("AngularVelocity : " + angvel_corrected);

                Console.WriteLine(pitchUp);
                //HingeUp1.SetFieldFloat("Target Angle", pitchUp);
                /*HingeUp2.SetFieldFloat("Target Angle", pitchUp);
                HingeDown1.SetFieldFloat("Target Angle", pitchDown);
                HingeDown2.SetFieldFloat("Target Angle", pitchDown);*/
            }

            Console.WriteLine("Go To Vertical");

            //HingeUp1.SetFieldFloat("Target Angle", 90);
            /*HingeUp2.SetFieldFloat("Target Angle", 90);
            HingeDown1.SetFieldFloat("Target Angle", 0);
            HingeDown2.SetFieldFloat("Target Angle", 0);*/

            starship.AutoPilot.Engage();
            starship.AutoPilot.ReferenceFrame = starship.SurfaceVelocityReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(0, -3.14f, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);
            starship.AutoPilot.AutoTune = true;

            starship.Control.Pitch = 1;
            Thread.Sleep(1000);
            double prevPitch = starship.Control.Pitch;
            while (prevPitch < starship.Control.Pitch) { starship.Control.Pitch = 1/*(starship.Flight(starship.SurfaceReferenceFrame).Pitch * 2 - 90)*/; }
            starship.Control.Pitch = -1;
            Console.WriteLine("Decel Flip");
            double prevRotation = starship.Rotation(starship.SurfaceVelocityReferenceFrame).Item2;
            while (prevRotation > starship.Rotation(starship.SurfaceReferenceFrame).Item2) { starship.Control.Pitch = -1; }

            Thread guidance = new Thread(Tamere);
            guidance.Start();

            while (starship.Flight(starship.SurfaceReferenceFrame).TrueAirSpeed > 20) { }

            Console.WriteLine("Landing legs deployed");
            foreach (Part legs in Legs.landingLegs)
            {
                Module legModule = legs.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                legModule.SetFieldFloat("Target Angle", 90);
            }

            starship.AutoPilot.ReferenceFrame = starship.SurfaceReferenceFrame;
            starship.AutoPilot.TargetDirection = new Tuple<double, double, double>(3.14f, 0, 0);
            Console.WriteLine(starship.AutoPilot.TargetDirection);

            Console.WriteLine("Wings full opened");
            /*HingeDown1.SetFieldFloat("Target Angle", 90);
            HingeDown2.SetFieldFloat("Target Angle", 90);*/
        }

        public static void Tamere()
        {
            //Landing.EngineGuidanceKOS(starship, InitPos);
        }

        public static bool DesentControl()
        {
            TakeCoords();
            Console.WriteLine("Coords taked !");

            Console.WriteLine("Wings control started");
            /*Thread LandingControl = new Thread(LandingGuidance);
            LandingControl.Start();*/

            Thread WingsControl = new Thread(PIDcontrolNewStarship);
            WingsControl.Start();

            while (starship.Flight(starship.Orbit.Body.ReferenceFrame).VerticalSpeed > -10) { Thread.Sleep(1000); }

            

            Console.WriteLine("Landing control started");
            Landing.LandingBurn(starship);

            return true;
        }

        public static bool LandingBurnControl()
        {
            Console.WriteLine("Landing control started");
            //Landing.LandingBurn(starship);

            return true;
        }

        public static bool TakeAllWingsModules()
        {
            /*foreach (var Modules in Wings.WingUpR[0].Modules)
            {
                Console.WriteLine(Modules.Name);
            }*/
            /*foreach (var Modules in Wings.WingUpL[0].Modules)
            {
                Console.WriteLine(Modules.Name);
            }
            foreach (var Modules in Wings.WingDownR[0].Modules)
            {
                Console.WriteLine(Modules.Name);
            }
            foreach (var Modules in Wings.WingDownL[0].Modules)
            {
                Console.WriteLine(Modules.Name);
            }*/

            return true;
        }

        public static bool TakeFieldOfModule()
        {
            Module module = null;
            /*foreach (var modules in Wings.WingUpR[0].Modules)
            {
                if (modules.Name.Equals("ModuleControlSurface"))
                {
                    module = modules;
                    Console.WriteLine("Change module valor");
                }
            }*/

            foreach (var Field in module.Fields)
            {
                Console.WriteLine(Field);
            }

            return true;
        }

        public static bool HopAtmospheric()
        {
            TakeCoords();
            Console.WriteLine("Coords taked !");

            Hop.Atmospheric();

            return true;
        }

        public static bool SmallHop()
        {
            TakeCoords();
            Console.WriteLine("Coords taked !");

            Hop.Small();

            return true;
        }
    }
}
