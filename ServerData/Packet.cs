using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ServerData
{
    [Serializable]
    public class Packet
    {
        public List<string> GeneralData;
        public int packetInt;
        public bool packetBool;
        public string senderID;
        public PacketType packetType;

        public Packet(PacketType type, string senderId)
        {
            GeneralData = new List<string>();
            this.senderID = senderID;
            this.packetType = type;
        }
        public byte[] ToBytes()                                             //once intialized, will turn to array of bytes
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();                 //makes it easy to convert to and from bytes
            binaryFormatter.Serialize(memoryStream, this);                  // 'this' implies the object we're contained in
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }
        public Packet(byte[] packetBytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(packetBytes);

            Packet packet = (Packet)binaryFormatter.Deserialize(memoryStream);
            memoryStream.Close();
            GeneralData = packet.GeneralData;
            packetInt = packet.packetInt;
            packetBool = packet.packetBool;
            senderID = packet.senderID;
            packetType = packet.packetType;
        }
        public static string GetIPAddress() 
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());      //getting all IP address but we only want our IP address
            foreach (IPAddress ipAddress in ips)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }
            return "127.0.0.1";                                             //default IP address: points back to user
        }
    }

    public enum PacketType                                                  //enumerator: creating categories
    {
        Registration,
        chat
    }
}

