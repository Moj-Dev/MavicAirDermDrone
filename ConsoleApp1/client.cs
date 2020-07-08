using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using ProtoBuf;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ConsoleApp1
{
    class client
    {


        //CLIENT
        public static void Main()
        {

            bool flag = false;
            while (true) { 
            byte[] data = new byte[1024];
            string input, stringData;
            int COUNTER = 0;
            IPEndPoint ipep = new IPEndPoint(
                            IPAddress.Parse("127.0.0.1"), 9050);

            Socket server = new Socket(AddressFamily.InterNetwork,
                           SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);
                    flag = true;
            }
            catch (SocketException e)
            {
                    Debug.WriteLine("Unable to connect to server.");
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(e.ToString());
              // return;
            }





                while (flag)
            {
                    Debug.WriteLine("while.");


                    int recv = server.Receive(data);
                    stringData = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine(stringData);

                    // input = Console.ReadLine();
                    COUNTER++;
                input = COUNTER.ToString();
                if (input == "exit")
                    break;
                server.Send(Encoding.ASCII.GetBytes(input));
                data = new byte[1024];
                recv = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(stringData);
            }
            Console.WriteLine("Disconnecting from server...");
            // server.Shutdown(SocketShutdown.Both);
            //  server.Close();

        }
        }




    }

   

}
