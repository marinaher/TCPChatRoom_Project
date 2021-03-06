﻿using System;
using ServerData;
using System.Net.Sockets;
using System.Threading;


namespace Server
{
    public class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;
        public string ID;

        public ClientData()
        {
            ID = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.Data_IN);
            clientThread.Start(clientSocket);
            SendRegistrationPacket();
        }

        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            ID = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.Data_IN);
            clientThread.Start(clientSocket);
            SendRegistrationPacket();
        }

        public void SendRegistrationPacket()
        {
            Packet packet = new Packet(PacketType.Registration, "server");
            packet.GeneralData.Add(ID);
            clientSocket.Send(packet.ToBytes());
        }
    }
}
