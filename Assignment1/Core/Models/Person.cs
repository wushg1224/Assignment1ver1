using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assignment1.Core.Models
{
    //Abstract base class
    //Ensure a clear code structure: all person types share a unified interface

    public abstract class Person
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        
        //Constructor for 
        protected Person(int id, string name, string password, string email, string phone, string address)
        {
            Id = id;
            Name = name;
            Password = password;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public override string ToString() => $"{Id} | {Name} | {Email} | {Phone}";
    }
}
