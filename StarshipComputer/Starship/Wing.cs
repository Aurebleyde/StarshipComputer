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
        public static List<Part> WingUpR = new List<Part>();
        public static List<Part> WingUpL = new List<Part>();
        public static List<Part> WingDownR = new List<Part>();
        public static List<Part> WingDownL = new List<Part>();

        public Wings(Vessel starship)
        {
            int WingNumber = 0;
            foreach (Part wing in starship.Parts.All)
            {
                if (wing.Tag == "WingUpR")
                {
                    WingNumber += 1;
                    WingUpR.Add(wing);
                    Console.WriteLine("Add " + wing + " to Wing Up List.");
                }
                if (wing.Tag == "WingUpL")
                {
                    WingNumber += 1;
                    WingUpL.Add(wing);
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
                    WingDownR.Add(wing);
                    Console.WriteLine("Add " + wing + " to Wing Down List.");
                }
                if (wing.Tag == "WingDownL")
                {
                    WingNumber += 1;
                    WingDownL.Add(wing);
                    Console.WriteLine("Add " + wing + " to Wing Down List.");
                }
            }
            Console.WriteLine(WingDownR + "set with " + WingDownR.Count + " Wing Down");
        }
    }
}
