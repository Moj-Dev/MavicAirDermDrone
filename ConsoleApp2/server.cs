using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp2
{
    class server
    {
        static void Main(string[] args)
        {


            while (true)
            {
                int recv;
                byte[] data = new byte[1024];
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any,
                                       9050);

                Socket newsock = new
                    Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.Tcp);

                newsock.Bind(ipep);
                newsock.Listen(10);
                Console.WriteLine("Waiting for a client...");
                Socket client = newsock.Accept();
                IPEndPoint clientep =
                             (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("Connected with {0} at port {1}",
                                clientep.Address, clientep.Port);


                string welcome = "Welcome to my test server";
                data = Encoding.ASCII.GetBytes(welcome);
                client.Send(data, data.Length,
                                  SocketFlags.None);
                while (true)
                {
                    client.Send(data, data.Length,
                      SocketFlags.None);
                    data = new byte[1024];
                    recv = client.Receive(data);
                    if (recv == 0)
                        break;

                    Console.WriteLine(
                             Encoding.ASCII.GetString(data, 0, recv));
                    client.Send(data, recv, SocketFlags.None);
                }
                Console.WriteLine("Disconnected from {0}",
                                  clientep.Address);
                // client.Close();
                // newsock.Close();
            }
        
    }
    }
}
