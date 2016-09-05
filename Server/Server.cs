using ServerData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Server                                // IObserverable to alert Server when a client has connected?
    {
        static Socket listenerSocket;
        static List<ClientData> clients;

        public static Dictionary<TcpClient, string> clientDictionary = new Dictionary<TcpClient, string>();

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
        public static void addToClientList()
        {
            TcpListener myList;
            myList = new TcpListener(IPAddress.Any, 4242);
            while (true)
            {
                TcpClient client = myList.AcceptTcpClient();
                clientDictionary.Add(client, "name");
            }
        }

        static void ListenThread()
        {
            while (true)
            {
                listenerSocket.Listen(0);
                clients.Add(new ClientData(listenerSocket.Accept()));
                Console.WriteLine("A client has joined your server!");
            }
        }

        public static void Data_IN(object client_socket)                        //client data thread - reads data from client
        {
            Socket clientSocket = (Socket)client_socket;

            byte[] buffer;
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
