using ChatClient.Data;
using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace ChatClient.MVVM.ViewModel
{
    public partial class MainViewModel
    {
        //Die Eigenschaften der Klasse!
        public ObservableCollection<UserModel> Users { get; set; } //Dies definiert eine ObservableCollection von UserModel-Objekten, die die Benutzer darstellen, die mit dem Server verbunden sind!
        public ObservableCollection<string> Messages { get; set; } //Eine ObservableCollection von Zeichenfolgen, die die empfangenen Nachrichten darstellen!
        public RelayCommand ConnectToServerCommand { get; set; } //Hier werden 2 RelayCommand-Objekte definiert, die Befehle darstellen, um sich mit dem Server zu verbinden und Nachrichten zu senden.
        public RelayCommand SendMessageCommand { get; set; }

        public string UserName { get; set; }
        public string Message { get; set; }

        private Server _server;

        public MainViewModel() //Der Konstruktor initialisiert die Eigenschaften und erstellt eine Instanz des Server-Objekts. Es abonniert auch Ereignisse des Servers für Benutzeranmeldung, Nachrichtenerhalt und Benutzerabmeldung!
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.ConnectedEvent += UserConnected;
            _server.MessageReceivedEvent += MessageReceived;
            _server.UserDisconnectEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(UserName), o => !string.IsNullOrEmpty(UserName));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        private void UserConnected() //Diese Methode wird aufgerufen, wenn ein Benutzer erfolgreich mit dem Server verbunden ist!
        {
            var user = new UserModel
            {
                UserName = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

        private void MessageReceived() //Diese Methode wird aufgerufen, wenn eine Nachricht vom Server empfangen wird!
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void RemoveUser() //Diese Methode wird aufgerufen, wenn ein Benutzer sich vom Server abmeldet. Es entfernt somit den Benutzer aus der Users-Collection!
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void ClearMessages()
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Clear());
        }
    }
}

