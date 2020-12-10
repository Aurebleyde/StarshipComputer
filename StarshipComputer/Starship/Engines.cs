using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Engines
    {
        public static List<Engine> RaptorSL = new List<Engine>();
        public static List<Engine> RaptorVac = new List<Engine>();
        public static List<Engine> RCS = new List<Engine>();

        public Engines(Vessel starship)
        {
            int EngineNumber = 0;

            try
            {
                foreach (Engine engine in starship.Parts.Engines)
                {
                    if (engine.Part.Tag == "RaptorSL")
                    {
                        EngineNumber += 1;
                        RaptorSL.Add(engine);
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
            }
            catch { }

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
    }
}
