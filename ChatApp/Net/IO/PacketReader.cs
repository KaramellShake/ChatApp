using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    public partial class PacketReader : BinaryReader
    {


        private NetworkStream _ns; //Diese Klasse hat eine private Instanzvariable _ns, die eine Verbindung zu einem NetworkStream darstellt. Der NetworkStream wird im Konstruktor übergeben und zum Lesen von Daten aus dem Netzwerk verwendet!


        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns; //Der Konstruktor initialisiert die Basisklasse BinaryReader mit dem übergebenen NetworkStream. Dadurch kann die PacketReader-Instanz Daten aus dem Netzwerkstream lesen!
        }

        public string ReadMessage() //Die ReadMessage-Methode wird hier verwendet, um eine Nachricht aus dem Netzwerkstream zu lesen. Zuerst wird die Länge der Nachricht als Integer gelesen, da die Länge der Nachricht normalerweise vor der Nachricht gesendet wird. Anschließend wird ein Byte-Array mit der entsprechenden Länge erstellt und die Nachricht aus dem Netzwerkstream gelesen. Schließlich wird die Nachricht von Bytes in einen String mit ASCII-Kodierung konvertiert und zurückgegeben!
        {
            byte[] msgBuffer;
            var length = ReadInt32();
            msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);

            var msg = Encoding.ASCII.GetString(msgBuffer);

            return msg;
        }
    }
}
