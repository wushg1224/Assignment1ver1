using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  namespace Assignment1.Core.Models
    {
        public class Admin : Person
        {
            public Admin(int id, string name, string password, string email, string phone, string address) : base(id, name, password, email, phone, address) { }
        }
    }

