using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace ChatServer
{
    public partial class Program
    {
        static List<Client> _users; //Ist eine Liste von Client-Objekten, die alle aktuell verbundenen Benutzer darstellt!
        static TcpListener _listener; //Ist ein TcpListener-Objekt, das auf eingehende Verbindungen wartet und diese akzeptiert!

        static void Main(string[] args)
        {
            _users = new List<Client>(); //Initialisiert die Liste der Benutzer!
            _listener = new TcpListener(IPAddress.Parse(); //Initialisiert den TCP-Listener mit der angegebenen IP-Adresse und dem Port!
            _listener.Start(); //Startet den Listener, sodass er auf eingehende Verbindungen wartet!

            while (true) //Eine Endlosschleife, die ständig auf neue Verbindungen wartet!
            {
                var client = new Client(_listener.AcceptTcpClient()); //Akzeptiert eine eingehende Verbindung und erstellt ein neues Client-Objekt!
                _users.Add(client); //Fügt den neuen Client zur Liste der Benutzer hinzu!

                BroadcastConnection(); //Benachrichtigt alle verbundenen Benutzer über die neue Verbindung!
            }
        }

        static void BroadcastConnection()
        {
            foreach (var user in _users) //Hier wird jeder Benutzer durchlaufen!
            {
                foreach (var usr in _users) //Für jeden Benutzer, sendet eine Nachricht an alle anderen Benutzer!
                {
                    var broadcastPacket = new PacketBuilder(); //Eine Hilfsklasse zum Erstellen von Paketen!
                    broadcastPacket.WriteOpCode(1); //Setzt den Opcode auf 1, was eine Verbindungsnachricht darstellt!
                    broadcastPacket.WriteMessage(usr.UserName); //Fügt den Benutzernamen zur Nachricht hinzu!
                    broadcastPacket.WriteMessage(usr.UID.ToString()); //Fügt die eindeutige ID des Benutzers zur Nachricht hinzu!
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes()); //Sendet dann das Paket an den Benutzer!
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users) //Durchläuft jeden Benutzer!
            {
                var msgPacket = new PacketBuilder(); 
                msgPacket.WriteOpCode(5); //Setzt den Opcode auf 5, was eine normale Nachricht darstellt!
                msgPacket.WriteMessage(message); //Fügt die Nachricht hinzu!
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes()); //Sendet das Paket an den Benutzer!
            }
        }

        public static void BroadcastDisconnect(string userName)
        {
            var disconnectedUser = _users.Where(x => x.UserName == userName).FirstOrDefault(); //Sucht nach dem Benutzer mit dem angegebenen Benutzernamen!
            if (disconnectedUser != null) //Wenn der Benutzer gefunden wird, passiert dies ->
            {
                _users.Remove(disconnectedUser); //Entfernt den Benutzer aus der Liste!

                foreach (var user in _users) //Benachrichtigt alle anderen Benutzer ->
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(10); //Setzt den Opcode auf 10, was eine Trennungsnachricht darstellt!
                    broadcastPacket.WriteMessage(userName + " Disconnected!"); //Fügt die Trennungsnachricht hinzu!
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes()); //Sendet das Paket an den Benutzer!
                }

                BroadcastMessage(userName + " Disconnected!"); //Sendet eine Broadcast-Nachricht an alle Benutzer, dass der Benutzer getrennt wurde!
            }
        }
    }
}