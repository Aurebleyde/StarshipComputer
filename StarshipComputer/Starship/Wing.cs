using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Wings
    {
        public static List<StarshipWing> WingUpR = new List<StarshipWing>();
        public static List<StarshipWing> WingUpL = new List<StarshipWing>();
        public static List<StarshipWing> WingDownR = new List<StarshipWing>();
        public static List<StarshipWing> WingDownL = new List<StarshipWing>();

        public Wings(Vessel starship)
        {
            int WingNumber = 0;
            foreach (Part wing in starship.Parts.All)
            {
                if (wing.Tag == "WingUpR")
                {
                    WingNumber += 1;
                    WingUpR.Add(new StarshipWing(wing));
                    Console.WriteLine("Add " + wing + " to Wing Up List.");
                }
                if (wing.Tag == "WingUpL")
                {
                    WingNumber += 1;
                    WingUpL.Add(new StarshipWing(wing));
                    Console.WriteLine("Add " + wing + " to Wing Up List.");
                }
            }
            Console.WriteLine(WingUpR + "set with " + WingUpR.Count + " Wing Up");

            WingNumber = 0;
            foreach (Part wing in starship.Parts.All)
            {
                if (wing.Tag == "WingDownR")
                {
                    WingNumber += 1;
                    WingDownR.Add(new StarshipWing(wing));
                    Console.WriteLine("Add " + wing + " to Wing Down List.");
                }
                if (wing.Tag == "WingDownL")
                {
                    WingNumber += 1;
                    WingDownL.Add(new StarshipWing(wing));
                    Console.WriteLine("Add " + wing + " to Wing Down List.");
                }
            }
            Console.WriteLine(WingDownR + "set with " + WingDownR.Count + " Wing Down");
        }
    }
}
