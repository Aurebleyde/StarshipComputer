using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Startup
    {
        public static bool Igntion(int engineNumber)
        {
            Console.WriteLine("Go for launch ?");
            string Respond = Console.ReadLine();
            while (Respond != "y" && Respond != "Y") { Respond = Console.ReadLine(); }
            Console.WriteLine("Litoff in 13s");

            Thread Recorder = new Thread(Starship.Record);
            //Recorder.Start();

            bool ignition = false;
            Timer.countDownStart = 13000;
            Thread count = new Thread(Timer.Countdown);
            count.Start();

            while (Timer.countDown == 0) { }

            try
            {
                while (Timer.countDown > 0)
                {
                    if (Timer.countDown < 6000 && ignition == false)
                    {
                        Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 0.8f, 3);

                        if (engineNumber == 3)
                        {
                            foreach (var raptor in Engines.RaptorSL)
                            {
                                raptor.Activate();
                                Console.WriteLine("Activation of " + raptor.ToString());
                            }
                        }
                        else if (engineNumber == 1)
                        {
                            Engines.RaptorSL[0].Activate();
                        }

                        ignition = true;
                    }
                }

                Starship.starship.AutoPilot.Engage();
                Starship.starship.AutoPilot.TargetPitchAndHeading(90, 59);
                //Starship.starship.AutoPilot.TargetRoll = Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Roll;
                //Starship.starship.AutoPilot.AutoTune = true;
                Console.WriteLine("Autopilot started");

                double MaxTotalThrust = 0;
                if (engineNumber == 3)
                {
                    foreach (var raptor in Engines.RaptorSL)
                    {
                        MaxTotalThrust += raptor.MaxThrust();
                    }
                }
                else if (engineNumber == 1)
                {
                    MaxTotalThrust += Engines.RaptorSL[0].Thrust();
                }

                double desiredThrust = (MaxTotalThrust * Starship.starship.Control.Throttle) / 1;

                double TotalThrust = 0;
                foreach (var raptor in Engines.RaptorSL)
                {
                    TotalThrust += raptor.Thrust();
                }

                if (TotalThrust > (desiredThrust * 95) / 100 && Throttle.MaxTWR(Starship.starship) > 1.05)
                {
                    /*foreach (var ports in Starship.starship.Parts.a)
                    {
                        ports.Decouple();
                    }*/

                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.5f, engineNumber);
                    Starship.starship.AutoPilot.TargetPitchAndHeading(90, 59);
                    //Starship.starship.AutoPilot.TargetRoll = Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).Roll;

                    Starship.starship.Control.ToggleActionGroup(0);

                    Console.WriteLine("Liftoff");
                    Starship.starship.Control.Throttle = Throttle.ThrottleToTWR(Starship.starship, 1.2f, engineNumber);
                    return true;
                }
                else
                {
                    Console.WriteLine("Engine Abort");
                    Starship.starship.Control.Throttle = 0;

                    foreach (var raptor in Engines.RaptorSL)
                    {
                        raptor.Shutdown();
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Abort : " + e);
                return false;
            }
        }
    }
}
