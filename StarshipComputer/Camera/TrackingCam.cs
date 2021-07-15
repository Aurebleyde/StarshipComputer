using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class TrackingCam
    {
        private Vessel Cam;
        private Vessel Target;

        public TrackingCam(Vessel vessel)
        {
            Cam = vessel;
            Target = vessel.connection.SpaceCenter().TargetVessel;
        }

        public void StartTracking()
        {
            Track = true;
            Thread tra = new Thread(Tracking);
            tra.Start();
        }
        public void StopTracking()
        {
            Track = false;
        }

        private static bool Track = false;
        private void Tracking()
        {
            if (Cam != null)
            {
                double Head = 0;
                double Pitch = 0;

                double Altitude = 0;
                Tuple<double, double> Position = new Tuple<double, double>(0, 0);

                double TargetAltitude = 0;
                Tuple<double, double> TargetPosition = new Tuple<double, double>(0, 0);

                Part PitchHinge = Cam.Parts.WithTag("PitchServo")[0];
                /*foreach (var item in PitchHinge.Modules)
                {
                    Console.WriteLine(item.Name);
                }*/

                Module PitchServo = PitchHinge.Modules.First(m => m.Name == "ModuleRoboticServoHinge");
                PitchServo.SetFieldFloat("Target Angle", (float)-Pitch);
                Module HeadServo = Cam.Parts.WithTag("HeadServo")[0].Modules.First(m => m.Name == "ModuleRoboticRotationServo");
                HeadServo.SetFieldFloat("Target Angle", (float)-Head);

                Console.WriteLine("Tracking Started");
                while (Track == true)
                {
                    Head = Cam.Flight(Cam.SurfaceReferenceFrame).Heading;
                    Pitch = Cam.Flight(Cam.SurfaceReferenceFrame).Pitch;
                    Head = Calculs.Mod((Head + 180), 360) - 180;

                    Altitude = Cam.Flight(Cam.SurfaceReferenceFrame).MeanAltitude;
                    Position = new Tuple<double, double>(Cam.Flight(Target.SurfaceReferenceFrame).Latitude, Cam.Flight(Target.SurfaceReferenceFrame).Longitude);

                    TargetAltitude = Target.Flight(Target.SurfaceReferenceFrame).MeanAltitude;
                    TargetPosition = new Tuple<double, double>(Target.Flight(Target.SurfaceReferenceFrame).Latitude, Target.Flight(Target.SurfaceReferenceFrame).Longitude);

                    double dAlt = (TargetAltitude - 30) - Altitude;
                    double Distance = Starship.Distance(Position.Item1, TargetPosition.Item1, Position.Item2, TargetPosition.Item2);
                    double PitchAngle = Starship.ToDegree(Math.Atan(dAlt / Distance)) - Pitch;

                    double HeadAngle = Starship.ToDegree(Calculs.VectorAngleWithNegative(new System.Numerics.Vector2((float)(TargetPosition.Item1 - Position.Item1), (float)(TargetPosition.Item2 - Position.Item2)))) - Head;
                    HeadAngle = Calculs.Mod((HeadAngle + 180), 360) - 180;

                    /*Console.WriteLine("Pitch Servo" + PitchAngle);
                    Console.WriteLine("Head Servo" + HeadAngle);*/

                    PitchServo.SetFieldFloat("Target Angle", (float)PitchAngle);
                    HeadServo.SetFieldFloat("Target Angle", (float)HeadAngle);
                }
            }
        }
    }
}
