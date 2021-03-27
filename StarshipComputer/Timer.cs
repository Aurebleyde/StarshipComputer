using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public class Timer
    {
        public static double countDownStart;
        public static double countDown;
        public static void Countdown()
        {
            DateTime startTime = DateTime.Now;
            countDown = countDownStart;

            while (countDown > 0)
            {
                countDown = countDownStart - Math.Abs((startTime - DateTime.Now).TotalMilliseconds);
                Console.WriteLine(countDown);
            }

            countDown = 0;
            Console.WriteLine("Timer finished");
        }
    }
}
