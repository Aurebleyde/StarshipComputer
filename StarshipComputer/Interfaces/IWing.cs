using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public interface IWing
    {
        void Orientation(float angle);

        void Deployed();

        void Retract();
    }
}
