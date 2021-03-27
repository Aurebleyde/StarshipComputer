using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class StarshipWing : IWing
    {
        public Part wing { get; set; }
        public StarshipWing(Part wing)
        {
            Console.WriteLine("Aile crée");
            this.wing = wing;
        }

        public void Orientation(float angle)
        {
            Module Wing = wing.Modules.First(m => m.Name == "ModuleControlSurface");
            Wing.SetFieldFloat("Angle de déploiement", angle);
        }

        public void Deployed()
        {
            wing.ControlSurface.Deployed = true;
            Console.WriteLine(wing.ToString() + " deployed");
        }

        public void Retract()
        {
            wing.ControlSurface.Deployed = false;
            Console.WriteLine(wing.ToString() + " retracted");
        }
    }
}
