using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ServerData
{
    [Serializable]
    public class Packet : BinaryWriter
    {
        public List<string> GeneralData;
        //public int packetInt;
        //public bool packetBool;
        public string senderID;
        public PacketType packetType;

        public Packet(PacketType type, string senderId)
            : base()
        {
            GeneralData = new List<string>();
            this.senderID = senderID;
            this.packetType = type;
        }
        public byte[] ToBytes()                                             //once intialized, will turn to array of bytes
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();                 //makes it easy to convert to and from bytes
            binaryFormatter.Serialize(memoryStream, this);
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
            //packetInt = packet.packetInt;
            //packetBool = packet.packetBool;
            senderID = packet.senderID;
            packetType = packet.packetType;
        }
        public byte[] PacketToByteArray(object packet)
        {
            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);
                return stream.ToArray();
            }
        }
        public static string GetIPAddress() 
        {
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipAddress in ip)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }
            return GetIPAddress();
        }
    }

    public enum PacketType
    {
        Registration,
        chat,
    }
}

