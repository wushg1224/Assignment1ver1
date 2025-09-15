using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Core.Models
{
    public class Patient : Person
    {
        public int? DoctorId { get; set; }
        public Patient(int id, string name, string password, string email, string phone, string address, int? doctorId = null)
            : base(id, name, password, email, phone, address)
        {
            DoctorId = doctorId;
        }

        public override string ToString()
        {
            string doctorInfo = DoctorId.HasValue ?
                $" | Doctor ID: {DoctorId.Value}" :
                " | Not assigned to a doctor";

            return base.ToString() + doctorInfo;
        }
    }
}

