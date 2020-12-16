using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarshipComputer
{
    public static class StarshipCommands
    {
        [RegisterCommand("help")]
        public static bool Help(params string[] args)
        {
            if (args.Length == 0)
            {
                foreach (Command command in Command.Commands)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"{command.Name} : {command.Description}");
                    Console.ResetColor();
                }
            }
            else
            {
                if (Command.Get(args[0]) != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(Command.Get(args[0]).Help);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknown command.");
                    Console.ResetColor();
                }
            }

            return true;
        }

        [RegisterCommand("position", "Know the position of vessel for the target", "use /position")]
        public static bool Position(params string[] args)
        {
            if (Starship.KnowPosition())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [RegisterCommand("StaticFire", "Static fire test for one/all engine", "use /StaticFire [engine number (all = all engine)]")]
        public static bool StaticFire(params string[] args)
        {
            if (Starship.StaticFire(args))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [RegisterCommand("rotation", "Know the rotation of vessel for the target", "use /rotation")]
        public static bool Rotation(params string[] args)
        {
            if (Starship.KnowRotation())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [RegisterCommand("engineGimbal", "Test the gimbal of all engine one by one and all at same time", "use /engineGimbal")]
        public static bool EngineGimbal(params string[] args)
        {
            if (Starship.GimbalTest())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*[RegisterCommand("wingTest", "Test the hinge of all wings one by one and all at same time", "use /wingTest")]
        public static bool WingTest(params string[] args)
        {
            if (Starship.WingTest())
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

        [RegisterCommand("wingControlLaunch", "Start Wings Control for the Starship descent", "use /wingControlLaunch")]
        public static bool WingWontrolLaunch(params string[] args)
        {
            if (Starship.WingWontrolLaunch())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [RegisterCommand("descentControl", "Start all of Starship's control to perform a descent", "use /descentControl")]
        public static bool DescentControl(params string[] args)
        {
            if (Starship.DesentControl())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [RegisterCommand("takeCoords", "Tale Starship's coordonees", "use /takeCoords")]
        public static bool TakeCoords(params string[] args)
        {
            if (Starship.TakeCoords())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
