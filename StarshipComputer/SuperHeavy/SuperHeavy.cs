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
    public class SuperHeavy
    {
        public static Vessel superHeavy;

        public SuperHeavy(Connection connection)
        {
            superHeavy = connection.SpaceCenter().ActiveVessel;
        }

        public void Liftoff()
        {
            superHeavy.Control.Throttle = 1;

            Console.WriteLine("SuperHeavy Ignition...");
            foreach (var engine in Engines.RaptorSH)
            {
                engine.Active = true;
            }

            float TWR = 0;

            while (TWR < 1)
            {
                TWR = Throttle.TWR(superHeavy);
            }

            Console.WriteLine("SuperHeavy Liftoff.");

            foreach (var clamp in superHeavy.Parts.LaunchClamps)
            {
                clamp.Release();
            }

            superHeavy.AutoPilot.Engage();
            superHeavy.AutoPilot.TargetPitch = 89;
            superHeavy.AutoPilot.TargetHeading = Starship.headTarget;
            superHeavy.AutoPilot.TargetRoll = 180;
        }

        public void GravityTurn()
        {
            superHeavy.AutoPilot.TargetRoll = 180;
            var Ft = superHeavy.Thrust;
            var Fw = superHeavy.Mass * superHeavy.Orbit.Body.SurfaceGravity;
            var TWR = Ft / Fw;
            var TWRstart = TWR;
            var pit = 90f;

            while (pit > 35 && superHeavy.Orbit.ApoapsisAltitude < 120000 && superHeavy.Flight(superHeavy.SurfaceReferenceFrame).TrueAirSpeed < 1500)
            {
                Ft = superHeavy.Thrust;
                Fw = superHeavy.Mass * superHeavy.Orbit.Body.SurfaceGravity;
                TWR = Ft / Fw;

                var difSup = ((90 * TWR) / TWRstart);
                double dif = (difSup - 90) / 1.8; //1.8 ASDS //2.9 RTLS
                float dif2 = Convert.ToSingle(dif);
                pit = 90 - dif2;
                superHeavy.AutoPilot.TargetPitch = pit;

                if (TWR == 0)
                {
                    superHeavy.AutoPilot.TargetPitch = 30;
                    break;
                }
            }

            superHeavy.AutoPilot.TargetPitch = 30;
        }

        public void stageSep()
        {
            var thrust = superHeavy.Thrust;

            while (true)
            {
                //thrust = firstStage.firstStage.Thrust;
                //var vesselFuel = (connection.AddStream(() => firstStage.firstStage.Resources.Amount("LiquidFuel")));
                //RTLS = 42%, ASDS = 33.5%, Exp = 1%
                //var percentage = (33.5 * (9336.4 * 3 + 4668.2)) / 100;
                superHeavy.Control.Throttle = 1;

                if (superHeavy.Flight(superHeavy.SurfaceReferenceFrame).TrueAirSpeed > 2500 /*RTLS = > 1600 | ASDS = > 2050*/ /*firstStage.firstStage.Thrust < 50*/)
                {
                    Console.WriteLine("SuperHeavy : MECO.");
                    superHeavy.Control.Throttle = 0;
                    foreach (var engine in Engines.RaptorSH)
                    {
                        engine.Active = false;
                    }

                    superHeavy.Control.RCS = true;
                    foreach (var item in Engines.RCS)
                    {
                        item.Active = true;
                    }
                    Thread.Sleep(1500);
                    superHeavy.Parts.WithTag("SepSH")[0].Decoupler.Decouple();

                    Console.WriteLine("SuperHeavy : Stage separation.");
                    break;
                }
            }
        }
    }
}
