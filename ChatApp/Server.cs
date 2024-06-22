using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatClient.Net.IO;

namespace ChatClient
{
    public partial class Server
    {
        private TcpClient _client; //Hier nutzen wir eine private Instanzvariable _client vom Typ TcpClient, die die TCP-Verbindung zum Server darstellt!

        public PacketReader PacketReader; //Die Server-Klasse enthält eine öffentliche Instanzvariable PacketReader vom Typ PacketReader. PacketReader ist eine Klasse, die zum Lesen von Datenpaketen aus dem Netzwerkstream verwendet wird!

        //Wir definieren hier 3 Events: Das ConnectedEvent, MessageReceivedEvent und UserDisconnectEvent. Diese Events werden ausgelöst, wenn der Client mit dem Server verbunden ist, eine Nachricht empfangen wird oder ein Benutzer sich vom Server abmeldet!
        public event Action ConnectedEvent;
        public event Action MessageReceivedEvent;
        public event Action UserDisconnectEvent;

        public Server()
        {
            _client = new TcpClient(); //Der Konstruktor initialisiert hier eine neue Instanz von TcpClient, die für die Kommunikation mit dem Server verwendet wird!
        }

        public void ConnectToServer(string username) //Diese Methode wird aufgerufen, um eine Verbindung zum Server herzustellen. Sie überprüft zunächst, ob der Client bereits verbunden ist. Wenn nicht, wird eine Verbindung zum Server hergestellt und ein PacketReader für den Netzwerkstream des Clients erstellt. Anschließend wird ein Verbindungspaket an den Server gesendet, das den Benutzernamen des Clients enthält!
        {
            try
            {
                if (!_client.Connected)
                {
                    _client.Connect();
                    PacketReader = new PacketReader(_client.GetStream());

                    if (!string.IsNullOrEmpty(username))
                    {
                        var connectPacket = new PacketBuilder();
                        connectPacket.WriteOpCode(0);
                        connectPacket.WriteMessage(username);
                        _client.Client.Send(connectPacket.GetPacketBytes());
                    }

                    ReadPackets();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
            }
        }

        private void ReadPackets() //Diese Methode liest kontinuierlich Datenpakete aus dem Netzwerkstream des Clients. Sie verwendet einen asynchronen Task, der in einer Endlosschleife läuft. Je nach Opcode des Pakets werden die entsprechenden Events ausgelöst, um den Status der Verbindung mit dem Server zu aktualisieren!
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        var opcode = PacketReader.ReadByte();

                        switch (opcode)
                        {
                            case 1:
                                ConnectedEvent?.Invoke();
                                break;

                            case 5:
                                MessageReceivedEvent?.Invoke();
                                break;

                            case 10:
                                UserDisconnectEvent?.Invoke();
                                break;

                            default:
                                Console.WriteLine("Unknown opcode received!");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading packets: {ex.Message}");
                }
            });
        }

        public void SendMessageToServer(string message) //Diese Methode wird verwendet, um eine Nachricht an den Server zu senden. Sie erstellt ein Paket mit dem Opcode für eine Nachricht und dem Inhalt der Nachricht, und sendet dieses Paket über den Netzwerkstream an den Server!
        {
            try
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(message);
                _client.Client.Send(messagePacket.GetPacketBytes());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to server: {ex.Message}");
            }
        }
    }
}
