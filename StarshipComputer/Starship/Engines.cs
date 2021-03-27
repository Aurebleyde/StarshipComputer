using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Engines
    {
        public static List<Raptor> RaptorSL = new List<Raptor>();
        public static List<Engine> RaptorVac = new List<Engine>();
        public static List<Engine> RaptorSH = new List<Engine>();
        public static List<Engine> RCS = new List<Engine>();

        public Engines(Vessel starship)
        {
            int EngineNumber = 0;

            try
            {
                foreach (Engine engine in starship.Parts.Engines)
                {
                    if (engine.Part.Tag.Contains("RaptorSL"))
                    {
                        EngineNumber += 1;
                        
                        string EngineNum = Regex.Match(engine.Part.Tag, @"\d+").Value;
                        int Num = Int32.Parse(EngineNum) - 1;
                        RaptorSL.Insert(Num, new Raptor(engine));
                        //RaptorSL.Add(engine);
                        Console.WriteLine("Add " + engine + " to Engine Sea Level List.");
                    }
                }
                Console.WriteLine(RaptorSL + "set with " + RaptorSL.Count + " Raptor Sea Level");

                EngineNumber = 0;
                foreach (Engine engine in starship.Parts.Engines)
                {
                    if (engine.Part.Tag == "RaptorVac")
                    {
                        EngineNumber += 1;
                        RaptorVac.Add(engine);
                        Console.WriteLine("Add " + engine + " to Engine Vacuum List.");
                    }
                }
                Console.WriteLine(RaptorVac + "set with " + RaptorVac.Count + " Raptor Vacuum");

                EngineNumber = 0;
                foreach (Engine engine in starship.Parts.Engines)
                {
                    if (engine.Part.Tag == "RaptorSH")
                    {
                        EngineNumber += 1;
                        RaptorSH.Add(engine);
                        Console.WriteLine("Add " + engine + " to Super Heavy Raptor List.");
                    }
                }
                Console.WriteLine(RaptorSH + "set with " + RaptorSH.Count + " Super Heavy Raptor");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Raptor initialisation finished");
            }

            EngineNumber = 0;
            foreach (Engine engine in starship.Parts.Engines)
            {
                if (engine.Part.Tag == "RCS")
                {
                    EngineNumber += 1;
                    RCS.Add(engine);
                    Console.WriteLine("Add " + engine + " to RCS List.");
                }
            }
            Console.WriteLine(RCS + "set with " + RCS.Count + " RCS");
        }

        /*public static int ActiveEngines()
        {
            int Active = 0;
            foreach (Engine Engine in Engines.RaptorSL)
            {
                if (Engine.Active == true)
                {
                    Active += 1;
                }
            }

            return Active;
        }

        public static void ActivateEngineSL(int Number)
        {
            Number -= 1;

            RaptorSL[Number].GimbalLocked = false;

            RaptorSL[Number].Active = true;
            RaptorSL[Number].GimbalLimit = 1;
        }

        public static void CutoffEngineSL(int Number)
        {
            Number -= 1;

            RaptorSL[Number].Active = false;
            //RaptorSL[Number].GimbalLimit = 0;
        }

        public static void MoveEngineToOff(int Number)
        {
            Number -= 1;
            Starship.starship.AutoPilot.Disengage();
            switch (Number)
            {
                case 0: //Engine 1
                    while (Starship.starship.Control.Pitch < 0.99) { Starship.starship.Control.Pitch = 1; }
                    break;

                case 1: //Engine 2
                    while (Starship.starship.Control.Pitch > -0.99 && Starship.starship.Control.Yaw > -0.99)
                    {
                        Starship.starship.Control.Pitch = -1;
                        Starship.starship.Control.Yaw = -1;
                    }
                    break;
                case 2: //Engine 3
                    while (Starship.starship.Control.Pitch > -0.99 && Starship.starship.Control.Yaw < 0.99)
                    {
                        Starship.starship.Control.Pitch = -1;
                        Starship.starship.Control.Yaw = 1;
                    }
                    break;
            }

            Thread.Sleep(500);

            RaptorSL[Number].GimbalLocked = true;

            Thread.Sleep(500);

            Starship.starship.Control.Pitch = 0;
            Starship.starship.Control.Yaw = 0;
            Console.WriteLine("Raptor engine " + Number + 1 + " locked");

            Starship.starship.AutoPilot.Engage();
        }*/
    }
}
