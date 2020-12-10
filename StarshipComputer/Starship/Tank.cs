using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Ressources
    {
        public Resource Oxy;
        public Resource Met;

        public Ressources(Vessel starship)
        {
            foreach (Resource ressource in starship.Resources.All)
            {
                if (ressource.Name == "LqdOxygen")
                {
                    Oxy = ressource;
                    Console.WriteLine("Setup " + Oxy);
                }
                if (ressource.Name == "LqdMethane")
                {
                    Met = ressource;
                    Console.WriteLine("Setup " + Met);
                }
            }
            Console.WriteLine("Liquid Oxygen Max : " + Oxy.Max + " | Current : " + Oxy.Amount);
            Console.WriteLine("Liquid Methan Max : " + Met.Max + " | Current : " + Met.Amount);
        }
    }
}
