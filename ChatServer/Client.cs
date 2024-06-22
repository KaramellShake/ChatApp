using ChatServer.Net.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatServer
{
    public partial class Client
    {
        //Eigenschaften der Klasse ->
        public string UserName { get; set; } //UserName stellt den Benutzernamen des Benutzers dar!
        public Guid UID { get; set; } //Eine eindeutige ID für den Client, generiert mit Guid.NewGuid()!
        public TcpClient ClientSocket { get; set; } //Das TcpClient-Objekt, das die Verbindung des Clients repräsentiert!

        private PacketReader _packetReader; //Eine private Instanzvariable zum Lesen von Datenpaketen aus dem Netzwerkstrom des Clients!

        public Client(TcpClient client) //Der Konstruktor der Client Klasse!
        {
            ClientSocket = client; //Initialisiert die ClientSocket-Eigenschaft mit dem übergebenen TcpClient-Objekt!
            UID = Guid.NewGuid(); //Generiert eine neue eindeutige ID für den Client und weist sie der UID-Eigenschaft zu!
            _packetReader = new PacketReader(ClientSocket.GetStream()); //Erstellt ein neues PacketReader-Objekt, das den Netzwerkstrom des TcpClient verwendet!

            try
            {
                var opcode = _packetReader.ReadByte(); //Liest das erste Byte des empfangenen Pakets, das den Opcode enthält!
                UserName = _packetReader.ReadMessage(); //Liest die Nachricht aus dem Paket, die den Benutzernamen des Clients enthält!

                Console.WriteLine($"({DateTime.Now}) Client has connected with the username {UserName}"); //Gibt eine Nachricht auf der Konsole aus, dass der Client verbunden ist, zusammen mit dem aktuellen Datum und der Uhrzeit sowie dem Benutzernamen!

                Task.Run(() => Process()); //Startet eine neue asynchrone Aufgabe, die die Process-Methode ausführt!
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during client initialization: {ex.Message}"); //Fängt alle Ausnahmen ab, die während der Initialisierung auftreten, gibt eine Fehlermeldung auf der Konsole aus und schließt den ClientSocket!
                ClientSocket.Close();
            }
        }

        void Process()
        {
            while (true) //Eine Endlosschleife, die kontinuierlich Nachrichten vom Client liest und verarbeitet!
            {
                try //Ein try-catch-Block zur Fehlerbehandlung während der Nachrichtenerfassung!
                {
                    var opcode = _packetReader.ReadByte(); //Liest das nächste Byte des empfangenen Pakets, das den Opcode enthält!
                    switch (opcode) //Ein switch-Block, um basierend auf dem Opcode unterschiedliche Aktionen auszuführen!
                    {
                        case 5: //Wenn der Opcode 5 ist, wird die Nachricht als Chatnachricht behandelt!
                            var msg = _packetReader.ReadMessage(); //Liest die Nachricht aus dem Paket!
                            Console.WriteLine("( " + DateTime.Now + " )" + " Message received! " + msg); //Gibt die empfangene Nachricht auf der Konsole aus!
                            Program.BroadcastMessage("( " + DateTime.Now + " ) " + UserName + ": " + msg); //Ruft eine Methode auf, um die Nachricht an alle verbundenen Clients zu senden!
                            break;

                        default: //Standardfall für unerkannte Opcodes, bei dem nichts passiert!
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("( " + DateTime.Now + " ) " + UserName + " Disconnected!"); //Fängt alle Ausnahmen ab, die während der Nachrichtenerfassung auftreten und gibt eine Nachricht auf der Konsole aus, dass der Client getrennt wurde. Informiert anschließend andere Clients und schließt den ClientSocket!
                    Program.BroadcastDisconnect(UserName);
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}