using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.MVVM.Core
{
    public partial class RelayCommand : ICommand
    {
        //2 private Felder mit dem Bezeichner execute und canExecute, die Delegaten für die auszuführende Aktion und die Bedingung zum Ausführen der Aktion halten!
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged //Hier wird das CanExecuteChanged-Event implementiert, um Änderungen am Ausführungszustand des Befehls zu signalisieren. Es wird verwendet, um zu benachrichtigen, wenn sich die Bedingungen geändert haben, unter denen der Befehl ausgeführt werden kann!
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) //Hier im Konstruktor werden 2 Parameter akzeptiert: execute, eine Aktion, die ausgeführt werden soll, wenn der Befehl ausgeführt wird, und optional canExecute, eine Funktion, die überprüft, ob der Befehl ausgeführt werden kann. Die canExecute-Funktion kann null sein, was bedeutet, dass der Befehl immer ausgeführt werden kann!
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) //Diese Methode überprüft ganz einfach, ob der Befehl ausgeführt werden kann, indem sie die canExecute-Funktion aufruft, wenn sie vorhanden ist, oder true zurückgibt, wenn keine Funktion angegeben ist!
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter) //Diese Methode führt die Aktion des Befehls aus, indem sie die execute-Aktion aufruft, wenn der Befehl ausgeführt wird!
        {
            this.execute(parameter);
        }
    }
}
