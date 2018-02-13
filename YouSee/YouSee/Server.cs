using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace YouSee
{
    class Server
    {
            public static void StartListening()
            {


                //initilizes count of clients
                int ClientCount = 0;

                //creates a tcplistener socket for server
                //create a tclclient for client
                TcpListener ServerSocket = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), 10000);
                TcpClient ClientSocket = default(TcpClient);

                //start the server
                ServerSocket.Start();
                Console.WriteLine(">> Starting server...");

                List<string> Locations = new List<string>();

                while (true)
                {
                    ClientCount++;
                    ClientSocket = ServerSocket.AcceptTcpClient();
                    Console.WriteLine(" >> Client Number: " + ClientCount + " started >>");

                    BinaryReader reader = new BinaryReader(ClientSocket.GetStream());
                    //Console.WriteLine(reader.ReadString());
                    Locations.Add(reader.ReadString());

                    foreach (string s in Locations)
                    {
                        Console.WriteLine(s);
                    }



                    //handleClient hc = new handleClient();
                    //hc.StartClient(ClientSocket, ClientCount);
                }

            }





            //Class to handle each client request separatly
            public class handleClient
            {

                TcpClient clientSocket;
                int ClientNum;


                public void StartClient(TcpClient inClientSocket, int ClientNum)
                {
                    this.clientSocket = inClientSocket;
                    this.ClientNum = ClientNum;
                    Thread ClientThread = new Thread(SendInfo);
                    ClientThread.Start();
                }



                private void SendInfo()
                {

                    try
                    {

                        BinaryReader reader = new BinaryReader(clientSocket.GetStream());
                        Console.WriteLine(reader.ReadString());
                    }

                    catch (Exception ex) { Console.WriteLine(" >> " + ex.ToString()); }
                }
            }



            //gets the IP address of the current Client
            public static string GetLocalIPAddress()
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }




            public static void Main(String[] args)
            {
                StartListening();

            }


        }
    }

