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
        public static List<Part> WingUp = new List<Part>();
        public static List<Part> WingDown = new List<Part>();

        public Wings(Vessel starship)
        {
            int WingNumber = 0;
            foreach (Part wing in starship.Parts.All)
            {
                if (wing.Tag == "WingUp")
                {
                    WingNumber += 1;
                    WingUp.Add(wing);
                    Console.WriteLine("Add " + wing + " to Wing Up List.");
                }
            }
            Console.WriteLine(WingUp + "set with " + WingUp.Count + " Wing Up");

            WingNumber = 0;
            foreach (Part wing in starship.Parts.All)
            {
                if (wing.Tag == "WingDown")
                {
                    WingNumber += 1;
                    WingDown.Add(wing);
                    Console.WriteLine("Add " + wing + " to Wing Down List.");
                }
            }
            Console.WriteLine(WingDown + "set with " + WingDown.Count + " Wing Down");
        }
    }
}
