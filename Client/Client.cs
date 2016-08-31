﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerData;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Client
    {
        public static Socket masterSocket;
        public static string name;
        public static string ID;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter Your Name: ");
            name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("What is your IP?");
            string IP = Console.ReadLine();

            masterSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint IP_end = new IPEndPoint(IPAddress.Parse(IP), 4242);                      // I don't like 4242
            
            try
            {
                masterSocket.Connect(IP_end);
            }
            catch
            {
                Console.WriteLine("Could not connect to server");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Thread thread = new Thread(data_IN);
            thread.Start();

            for (;;)
            {
                Console.Write("::>");
                string input = Console.ReadLine();

                Packet packet = new Packet(PacketType.chat, ID);
                packet.GeneralData.Add(name);
                packet.GeneralData.Add(input);                                                  //gets input and sends to server
                masterSocket.Send(packet.ToBytes());
            }
        }

        static void data_IN()
        {
            byte[] buffer;
            int readBytes;

            for (; ; )
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
                catch (SocketException ex)
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
                    Console.WriteLine("Connected to Server!");
                    ID = packet.GeneralData[0];
                    break;
                case PacketType.chat:
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(packet.GeneralData[0] + ": " + packet.GeneralData[1]);        //0 = name, 1 = message
                    Console.ForegroundColor = color;
                    break;
            }
        }
    }
}