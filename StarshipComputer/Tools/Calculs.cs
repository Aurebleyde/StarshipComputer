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

        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}
