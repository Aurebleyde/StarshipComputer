using KRPC.Client;
using StarshipComputer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarshipComputer
{
    static class Program
    {
        public static Thread MainThread = new Thread(Initialisation);

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainThread.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static int numberPrototype = 1;

        public static Starship starship;

        static void Initialisation()
        {
            Console.OpenStandardInput();
            Console.OpenStandardOutput();
            Console.WriteLine("Starship Computer in startup");

            Console.WriteLine("Connection...");
            Connections Connections = new Connections(ConnectionList.ethernet, MainThread);

            Console.WriteLine("Connection to Starship and setup of Starship...");
            starship = new Starship(Connections.Connection, numberPrototype);
            Console.WriteLine("Connection and setup done !");

            Console.WriteLine("Commands activating...");
            CommandFire.FireRegistery();
            Thread Commands = new Thread(Starship.CommandControl);
            Commands.Start();
            Console.WriteLine("Commands activated.");
        }
    }
}
