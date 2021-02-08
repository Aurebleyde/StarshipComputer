using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public interface IEngine
    {
        void Activate();
        void Shutdown();
        void Move(int x, int y);
        void GimbalLimit(float value);
        void GimbalLocked(bool value);
        float Thrust();
        float MaxThrust();
        float ISP();
    }
}
