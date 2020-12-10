using KRPC.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarshipComputer
{
    public class Connections
    {
        public Connection Connection;
        public Connections(ConnectionList list, Thread thread)
        {
            try
            {
                switch (list)
                {
                    case ConnectionList.local:
                        Connection = new Connection(name: "Starship Computer : Local", address: IPAddress.Parse("127.0.0.1"), rpcPort: 50000, streamPort: 50001);
                        break;
                    case ConnectionList.ethernet:
                        Connection = new Connection(name: "Starship Computer : Ethernet", address: IPAddress.Parse("192.168.1.88"), rpcPort: 50000, streamPort: 50001);
                        break;
                    case ConnectionList.data:
                        Connection = new Connection(name: "Starship Computer : Data", address: IPAddress.Parse("26.203.104.176"), rpcPort: 50000, streamPort: 50001);
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Connection not possible...");
                Thread.Sleep(2000);
                Console.WriteLine("Restart ? y/n");
                string respond = Console.ReadLine();
                switch (respond)
                {
                    case "y":
                        Console.WriteLine("Program restarting...");
                        Thread.Sleep(1000);
                        Application.Restart();
                        thread?.Abort();
                        break;
                    default:
                        Console.WriteLine("Program will be stoped...");
                        Thread.Sleep(1000);
                        Application.Exit();
                        thread?.Abort();
                        break;
                }
            }

            Console.WriteLine($"Connection with {list} done !");
        }
    }
}
