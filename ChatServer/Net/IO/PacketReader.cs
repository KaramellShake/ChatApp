using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    public partial class PacketReader : BinaryReader
    {


        private NetworkStream _ns; //Ein Attribut der Klasse, welches den NetworkStream repräsentiert, von dem die Daten gelesen werden!

        //Konstruktor
        public PacketReader(NetworkStream ns) : base(ns) //Der Konstruktor PacketReader(NetworkStream ns) initialisiert das _ns-Attribut und ruft den Basiskonstruktor von BinaryReader auf, wobei der NetworkStream übergeben wird. Dadurch kann der PacketReader die Methoden des BinaryReader nutzen, um Daten aus dem NetworkStream zu lesen!
        {
            _ns = ns;
        }

        public string ReadMessage() //Diese Methode liest eine Nachricht aus dem NetworkStream!
        {
            byte[] msgBuffer;
            var length = ReadInt32(); //Liest die Länge der kommenden Nachricht als 32-Bit-Ganzzahl (int). Diese Methode wird von BinaryReader geerbt!
            msgBuffer = new byte[length]; //Erstellt ein Byte-Array (msgBuffer) der gelesenen Länge!
            _ns.Read(msgBuffer, 0, length); //Liest die Nachricht aus dem NetworkStream in das msgBuffer. Dies liest length Bytes aus dem Stream und speichert sie ab dem Index 0 im msgBuffer!

            var msg = Encoding.ASCII.GetString(msgBuffer); //Konvertiert das Byte-Array (msgBuffer) in eine Zeichenkette (String) unter Verwendung der ASCII-Kodierung!

            return msg; //Gibt die gelesene Nachricht als String zurück!
        }
    }
}

    /*
    Die PacketReader-Klasse ermöglicht das Lesen von Nachrichten aus einem NetworkStream. 
    Sie erweitert BinaryReader, um von dessen Methoden zu profitieren und liest zuerst die Länge der Nachricht (als int) und anschließend die eigentliche Nachricht basierend auf der gelesenen Länge.
    Die Methode ReadMessage() kümmert sich darum, die Nachricht in einen lesbaren String umzuwandeln, nachdem sie aus dem Stream gelesen wurde.
    Dies ist besonders nützlich in Netzwerkkommunikationsszenarien, wie z.B. in einem Chat-Server, wo Nachrichten in einem bestimmten Format empfangen und verarbeitet werden müssen!
    */
