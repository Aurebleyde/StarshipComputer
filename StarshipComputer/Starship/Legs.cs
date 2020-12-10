using System;
using System.Collections.Generic;
using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Legs
    {
        public static List<Part> landingLegs = new List<Part>();

        public Legs(Vessel starship)
        {
            int LegsNumber = 0;
            foreach (Part hinge in starship.Parts.All)
            {
                if (hinge.Tag == "Legs")
                {
                    LegsNumber += 1;
                    landingLegs.Add(hinge);
                    Console.WriteLine("Add " + hinge + " to Wing Up List.");
                }
            }
            Console.WriteLine(landingLegs + "set with " + landingLegs.Count + " Legs");
        }
    }
}
