using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public static double VectorScal(Vector2 Impact, Vector2 Zone)
        {
            Vector2 ImpactZone = Impact - Zone;
            double scal = ImpactZone.LengthSquared();
            scal = (Impact.LengthSquared() + Zone.LengthSquared() - scal) / 2;
            Console.WriteLine("Scal : " + scal);
            return scal;
        }

        public static double VectorAngle(Vector2 Impact, Vector2 Zone)
        {
            Console.WriteLine("Lenght : " + Impact.Length());
            double Cos = Math.Acos(VectorScal(Impact, Zone) / (Impact.Length() * Zone.Length()));
            Console.WriteLine("Angle : " + (Cos * 180) / Math.PI);
            return (Cos * 180) / Math.PI;
        }
    }
}
