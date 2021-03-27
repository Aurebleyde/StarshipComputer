using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Atmospheric
    {
        public static void GreenLine()
        {
            //CreateTargetedTrajectory(Starship.starship, Starship.InitPos);
            Vector2 Ligne = new Vector2(1.0f, 0.0f);

            /*Console.WriteLine("Angle 1 : " + Calculs.VectorAngleWithNegative(new Vector2(1.0f, 0.0f)));
            Console.WriteLine("Angle 2 : " + Calculs.VectorAngleWithNegative(new Vector2(1.0f, 0.0f)));*/

            Vector2 DirGreenVector = Calculs.Rotate(Ligne, (float)(Math.PI / 2f));

            Console.WriteLine("Dir Green Vector : " + DirGreenVector.ToString());
        }

        public static Vector2 targetedTrajectory;
        private static bool useLatitude;
        private static bool superior;
        public static void CreateTargetedTrajectory(Vessel vessel, Tuple<double, double> LZ)
        {
            targetedTrajectory = new Vector2((float)(vessel.Flight(vessel.SurfaceReferenceFrame).Latitude - LZ.Item1),
                (float)(vessel.Flight(vessel.SurfaceReferenceFrame).Longitude - LZ.Item2));
            Console.WriteLine("Targeted Trajectory Vector Initialized : " + targetedTrajectory.ToString());

            if (Starship.Distance(Starship.InitPos.Item1, Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Latitude, 0, 0) < Starship.Distance(Starship.InitPos.Item2, Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Longitude, 0, 0))
            {
                useLatitude = false;
                if (Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Longitude > Starship.InitPos.Item2)
                {
                    BaseHeading = 270;
                    superior = true;
                }
                else
                {
                    BaseHeading = 90;
                    superior = false;
                }
            }
            else
            {
                useLatitude = true;

                if (Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Latitude > Starship.InitPos.Item1)
                {
                    BaseHeading = 180;
                    superior = true;
                }
                else
                {
                    BaseHeading = 0;
                    superior = false;
                }
            }
        }

        public static Vector2 intersection;
        public static float teta;
        public static void FoundIntersection(Vessel vessel, Tuple<double, double> LZ)
        {
            Vector2 ls = new Vector2((float)(vessel.Flight(vessel.SurfaceReferenceFrame).Latitude - LZ.Item1),
                (float)(vessel.Flight(vessel.SurfaceReferenceFrame).Longitude - LZ.Item2));

            double lp = Calculs.VectorScal(ls, targetedTrajectory) / targetedTrajectory.Length();

            float alpha = (float)Calculs.VectorAngleWithNegative(targetedTrajectory);

            Vector2 LP = new Vector2((float)(lp * Math.Cos(alpha)), (float)(lp * Math.Sin(alpha)));

            intersection = new Vector2((float)LZ.Item1, (float)LZ.Item2) + LP;



            float beta = (float)Calculs.VectorAngleWithNegative(ls);

            teta = (float)((beta - alpha) % Math.PI);
        }

        public static double BaseHeading;
        public static void GuidanceByLatitude()
        {
            if (useLatitude == true)
            {
                Console.WriteLine("Oui 1");
                if (Starship.ImpactPoint().Item2 > Starship.InitPos.Item1 && superior == true)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item2)
                    {
                        Console.WriteLine("Non 1");
                        teta = -2;
                    }
                    else
                    {
                        Console.WriteLine("Non 2");
                        teta = 2;
                    }
                }
                else if (Starship.ImpactPoint().Item2 < Starship.InitPos.Item1 && superior == true)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item2)
                    {
                        Console.WriteLine("Non 3");
                        teta = -1;
                    }
                    else
                    {
                        Console.WriteLine("Non 4");
                        teta = 1;
                    }
                }
                else if (Starship.ImpactPoint().Item2 > Starship.InitPos.Item1 && superior == false)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item2)
                    {
                        Console.WriteLine("Non 5");
                        teta = 1;
                    }
                    else
                    {
                        Console.WriteLine("Non 6");
                        teta = 2;
                    }
                }
                else if (Starship.ImpactPoint().Item2 < Starship.InitPos.Item1 && superior == false)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item2)
                    {
                        Console.WriteLine("Non 7");
                        teta = 2;
                    }
                    else
                    {
                        Console.WriteLine("Non 8");
                        teta = -2;
                    }
                }
            }
            else
            {
                Console.WriteLine("Oui 2");
                if (Starship.ImpactPoint().Item2 > Starship.InitPos.Item2 && superior == true)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item1)
                    {
                        Console.WriteLine("Non 1");
                        teta = -2;
                    }
                    else
                    {
                        Console.WriteLine("Non 2");
                        teta = 2;
                    }
                }
                else if (Starship.ImpactPoint().Item2 < Starship.InitPos.Item2 && superior == true)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item1)
                    {
                        Console.WriteLine("Non 3");
                        teta = -1;
                    }
                    else
                    {
                        Console.WriteLine("Non 4");
                        teta = 1;
                    }
                }
                else if (Starship.ImpactPoint().Item2 > Starship.InitPos.Item2 && superior == false)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item1)
                    {
                        Console.WriteLine("Non 5");
                        teta = -2;
                    }
                    else
                    {
                        Console.WriteLine("Non 6");
                        teta = 2;
                    }
                }
                else if (Starship.ImpactPoint().Item2 < Starship.InitPos.Item2 && superior == false)
                {
                    if (Starship.ImpactPoint().Item1 > Starship.InitPos.Item1)
                    {
                        Console.WriteLine("Non 7");
                        teta = 1;
                    }
                    else
                    {
                        Console.WriteLine("Non 8");
                        teta = -1;
                    }
                }
            }
        }

        public static bool bidule()
        {
            GreenLine();
            return true;
        }
    }
}
