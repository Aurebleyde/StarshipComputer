using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Raptor : IEngine
    {
        public Engine raptor { get; set; }
        public Raptor(Engine engine)
        {
            Console.WriteLine("Raptor crée");
            raptor = engine;
        }
        public void Activate()
        {
            Console.WriteLine("Activation Raptor " + raptor.ToString());
            raptor.Active = true;
        }
        public void Shutdown()
        {
            Console.WriteLine("Shutdown Raptor " + raptor.ToString());
            raptor.Active = false;
        }

        public void GimbalLimit(float value)
        {
            raptor.GimbalLimit = Calculs.Clamp(value, 0, 1);
            Console.WriteLine("Set Raptor gimbal limit to " + raptor.GimbalLimit);
        }

        public void GimbalLocked(bool value)
        {
            raptor.GimbalLocked = value;
            Console.WriteLine("Set Raptor gimbal locked to " + raptor.GimbalLocked);
        }

        public void Move(int x, int y)
        {
            /*raptor.GimbalLocked = value;
            Console.WriteLine("Set Raptor gimbal locked to " + raptor.GimbalLocked);*/
        }

        public float Thrust()
        {
            return raptor.Thrust;
        }

        public float MaxThrust()
        {
            return raptor.MaxThrust;
        }

        public float ISP()
        {
            return raptor.SpecificImpulse;
        }
    }
}
