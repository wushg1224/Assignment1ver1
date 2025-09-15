using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Core.Models
{
    public class Appointment
    {
        public int Id { get; init; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Notes { get; set; }

        // Appointment Constructor
        public Appointment(int id, int doctorId, int patientId, string notes)
        { Id = id; DoctorId = doctorId; PatientId = patientId; Notes = notes; }

        public Appointment(int doctorId, int patientId) : this(0, doctorId, patientId, "") { }

        public override string ToString() => $"{Id} | D:{DoctorId} | P:{PatientId} | {Notes}";
    }
}

