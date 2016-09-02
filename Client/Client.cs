using System;
using System.Collections.Generic;
using ServerData;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Client
    {
        public static Socket masterSocket;
        public static string name;
        public static string ID;

        //public Dictionary<string, DateTime> messageSentTime = new Dictionary<string, date>();

        static void Main(string[] args)
        {
            Console.WriteLine("\nEnter your nickname: ");           //if no name entered, error and prompt to enter name again
            name = Console.ReadLine();
            
            Console.Clear();
            Console.WriteLine("Enter IP address:");                 //if IP address is entered through, try/catch this error
            string IP = Console.ReadLine();

            masterSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IP_end = new IPEndPoint(IPAddress.Parse(IP), 4242);
            
            try
            {
                masterSocket.Connect(IP_end);
            }
            catch
            {
                Console.WriteLine("Server connection failed. \n");
                Main(args);
            }

            Thread thread = new Thread(Data_IN);
            thread.Start();

            for (;;)
            {
                Console.Write("");
                string input = Console.ReadLine();

                Packet packet = new Packet(PacketType.chat, ID);
                packet.GeneralData.Add(name);
                packet.GeneralData.Add(input);
                masterSocket.Send(packet.ToBytes());
            }
        }

        static void Data_IN()
        {
            byte[] buffer;
            int readBytes;

            for (; ;)
            {
                try
                {
                    buffer = new byte[masterSocket.SendBufferSize];
                    readBytes = masterSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        DataManager(new Packet(buffer));
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("The server has been disconnected!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

        }
        static void DataManager(Packet packet)
        {
            switch (packet.packetType)
            {
                case PacketType.Registration:
                    Console.WriteLine("You are now connected, {0}. \nSay Hello: ", name);
                    ID = packet.GeneralData[0];
                    break;
                case PacketType.chat:
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    
                    Console.WriteLine(packet.GeneralData[0] + ": " + packet.GeneralData[1]);
                    Console.ForegroundColor = color;
                    break;
            }
        }
    }
}