using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ChatClient.Net.IO
{
    public partial class PacketBuilder
    {

        MemoryStream _ms; //Die Klasse verwendet hier eine Instanz von MemoryStream, um die Bytes des Pakets zu speichern!


        public PacketBuilder()
        {
            _ms = new MemoryStream(); //Hier im Konstruktor wird ein neuer MemoryStream erstellt, der für die Speicherung der Daten verwendet wird!
        }
        public void WriteOpCode(byte opcode) //Die WriteOpCode-Methode wird hier verwendet, um den Opcode (Operation Code) des Pakets zu schreiben. Der Opcode ist eine Art Kennzeichnung für die Art der Operation, die das Paket repräsentiert. Es wird ein einzelnes Byte geschrieben, das den Opcode darstellt!
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg) //Die WriteMessage-Methode wird hier verwendet, um eine Nachricht zum Paket hinzuzufügen. Zuerst wird die Länge der Nachricht in Bytes geschrieben, gefolgt von den eigentlichen Nachrichtenbytes, die mit ASCII-Kodierung codiert sind!
        {
            var msgLength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes() //Die GetPacketBytes-Methode gibt die Bytes des Pakets als Array zurück, das zum Senden über das Netzwerk verwendet werden kann. Sie ruft ToArray auf dem MemoryStream-Objekt auf, um die im Stream gespeicherten Bytes abzurufen!
        {
            return _ms.ToArray();
        }
    }
}
