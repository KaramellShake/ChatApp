using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Data
{
    public partial class User
    {
        public int UserID { get; set; } //Deklaration einer UserID, UserName und Password Eigenschaft!
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
