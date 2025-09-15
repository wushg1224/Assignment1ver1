using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Core.Models
{
    public class Doctor : Person
    {
        public string Specialty { get; set; }
        public Doctor(int id, string name, string password, string email, string phone, string address, string specialty)
             : base(id, name, password, email, phone, address) 
        {
            Specialty = specialty;
        }

        public override string ToString() => $"{base.ToString()} | {Specialty}";
    }
}
