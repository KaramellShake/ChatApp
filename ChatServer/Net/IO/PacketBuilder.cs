using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatServer.Net.IO
{
    public partial class PacketBuilder
    {

        MemoryStream _ms; //Ein Attribut der Klasse, welches einen MemoryStream repräsentiert. Ein MemoryStream ist ein Stream, der Daten im Speicher speichert!

        public PacketBuilder() //Der Konstruktor PacketBuilder() initialisiert _ms als neuen MemoryStream!
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode) //Diese Methode schreibt einen einzelnen Byte-Wert (Opcode) in den MemoryStream. Ein Opcode könnte einen bestimmten Befehl oder einen Nachrichtentyp kennzeichnen!
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg) //Diese Methode schreibt eine Zeichenkette in den MemoryStream!
        {
            var msgLength = msg.Length; //Zuerst wird die Länge der Nachricht als Ganzzahl (msgLength) berechnet!
            _ms.Write(BitConverter.GetBytes(msgLength)); //BitConverter.GetBytes(msgLength) konvertiert die Länge in ein Byte-Array!
            _ms.Write(Encoding.ASCII.GetBytes(msg)); //Encoding.ASCII.GetBytes(msg) konvertiert die Nachricht selbst in ein Byte-Array!
            // -> Beide Byte-Arrays werden nacheinander in den MemoryStream geschrieben!
        }

        public byte[] GetPacketBytes() //Diese Methode gibt den gesamten Inhalt des MemoryStream als Byte-Array zurück. Dies kann verwendet werden, um das erstellte Paket zu senden oder zu speichern!
        {
            return _ms.ToArray();
        }
    }
}

    /*
    Die PacketBuilder-Klasse ermöglicht es, Datenpakete zu erstellen, indem man Byte-Werte (OpCodes) und Zeichenketten (Nachrichten) in einen MemoryStream schreibt.
    Am Ende kann das gesamte Paket als Byte-Array abgerufen werden, was nützlich für die Netzwerkkommunikation ist. 
    Die Klasse abstrahiert die Details der Paketstruktur und bietet eine einfache Schnittstelle zum Erstellen von Paketen!
    */