﻿using ServerData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Server
    {
        static Socket listenerSocket;
        static List<ClientData> clients;

        public static void Main(string[] args)
        {
            Console.Title = "*** TCP ChatRoom ***";
            Console.WriteLine("Starting Server on: " + Packet.GetIPAddress());

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<ClientData>();

            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(Packet.GetIPAddress()), 4242);
            listenerSocket.Bind(IP_End);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();

            Console.WriteLine("Success! \nListening IP: " + Packet.GetIPAddress());
        }

        static void ListenThread()
        {
            for (;;)                                                            //for loop is infinite b/c we want the listener to listen for new clients until the program closes
            {
                listenerSocket.Listen(0);                                       // 0 is backlog 
                clients.Add(new ClientData(listenerSocket.Accept()));           //adding new client data and socket is listening and will accept the new client
                Console.WriteLine("A client has joined your server!");
            }
        }

        public static void Data_IN(object client_socket)                        //client data thread - reads data from client
        {
            Socket clientSocket = (Socket)client_socket;

            byte[] buffer;                                                      //buffer array
            int readBytes;

            for (; ; )
            {
                try
                {
                    buffer = new byte[clientSocket.SendBufferSize];             //same size of buffer array size - buffer contains data read
                    readBytes = clientSocket.Receive(buffer);                   //constantly reading to see how many buffers read

                    if (readBytes > 0)                                          //reading data from each client if client sends data
                    {
                        Packet packet = new Packet(buffer);
                        DataManager(packet);
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("A client has disconnected.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }

        public static void DataManager(Packet packet)
        {
            switch (packet.packetType)
            {
                case PacketType.chat:
                    foreach (ClientData client in clients)                      //server takes the information and foward to all connected clients.
                    {
                        client.clientSocket.Send(packet.ToBytes());
                    }
                    break;
            }
        }
    }
}
