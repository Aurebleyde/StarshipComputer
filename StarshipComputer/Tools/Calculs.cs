using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Calculs
    {
        public static double Mod(double a, double n)
        {
            double mod = a - Math.Floor(a / n) * n;
            return mod;
        }
    }
}
