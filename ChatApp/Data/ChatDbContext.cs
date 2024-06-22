using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChatClient.Data
{
    public partial class ChatDbContext : DbContext //Die Klasse ChatDbContext erbt von der Klasse DbContext. DbContext ist die Hauptklasse im Entity Framework Core, die den Zugriff auf die Datenbank ermöglicht!
    {
        public DbSet<User> Users { get; set; } //Die Eigenschaft Users vom Typ DbSet<User> definiert die Entität User als eine Menge von Benutzern in der Datenbank. Jedes DbSet<> repräsentiert eine Tabelle in der Datenbank!

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Die Konfigurationen für die Verbindung zur Datenbank!
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-46N32KQ\XSQL;Initial Catalog=LoginDB;Integrated Security=True; Encrypt=False;");
        }
    }
}