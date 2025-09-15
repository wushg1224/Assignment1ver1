using Assignment1.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Services
{
    public static class DataRepository
    {
        // data reading method
        // list all patient 
        public static List<Patient> GetAllPatients()
        {
            var list = new List<Patient>();
            foreach (var line in FileManager.ReadAllLinesSafe("patients.txt"))
            {
                var p = ParsePatient(line);
                if (p != null) list.Add(p);
            }
            return list;
        }
        //list all doctor
        public static List<Doctor> GetAllDoctors()
        {
            var list = new List<Doctor>();
            foreach (var line in FileManager.ReadAllLinesSafe("doctors.txt"))
            {
                var d = ParseDoctor(line);
                if (d != null) list.Add(d);
            }
            return list;
        }
        //list all appointment
        public static List<Appointment> GetAllAppointments()
        {
            var list = new List<Appointment>();
            foreach (var line in FileManager.ReadAllLinesSafe("appointments.txt"))
            {
                var a = ParseAppointment(line);
                if (a != null) list.Add(a);
            }
            return list;
        }

        //search by id
        public static Patient? GetPatientById(int id) => GetAllPatients().FirstOrDefault(p => p.Id == id);
        public static Doctor? GetDoctorById(int id) => GetAllDoctors().FirstOrDefault(d => d.Id == id);

        //get appointment base on id
        public static List<Appointment> GetAppointmentsByPatient(int patientId)
            => GetAllAppointments().Where(a => a.PatientId == patientId).OrderBy(a => a.Id).ToList();

        public static List<Appointment> GetAppointmentsByDoctor(int doctorId)
            => GetAllAppointments().Where(a => a.DoctorId == doctorId).OrderBy(a => a.Id).ToList();

        //add new appointment and save to fild
        public static int AddAppointment(int doctorId, int patientId, string notes)
        {
            var used = GetUsedAppointmentIds();
            var newId = IdGenerator.NewId(used);
            var line = $"{newId}|{doctorId}|{patientId}|{notes}";
            FileManager.AppendLine("appointments.txt", line);
            return newId;
        }

        private static HashSet<int> GetUsedAppointmentIds()
            => GetAllAppointments().Select(a => a.Id).ToHashSet();

        // Parser for data structure
        // patients.txt:  id|name|password|email|phone|address|doctorId
        private static Patient? ParsePatient(string line)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line)) return null;
                var s = line.Split('|');
                if (s.Length < 7)
                {
                    Console.WriteLine($"Invalid patient line: {line}");
                    return null;
                }

                // parse logic
                int id = int.Parse(s[0]);
                string name = s[1];
                string pwd = s[2];
                string email = s[3];
                string phone = s[4];
                string address = s[5];
                int? doctorId = int.TryParse(s[6], out var did) ? did : null;
                return new Patient(id, name, pwd, email, phone, address, doctorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing patient line '{line}': {ex.Message}");
                return null;
            }
        }

        //private static Patient? ParsePatient(string line)
        //{
        //    if (string.IsNullOrWhiteSpace(line)) return null;
        //    var s = line.Split('|');
        //    if (s.Length < 7) return null;

        //    int id = int.Parse(s[0]);
        //    string name = s[1];
        //    string pwd = s[2];
        //    string email = s[3];
        //    string phone = s[4];
        //    string address = s[5];
        //    int? doctorId = int.TryParse(s[6], out var did) ? did : null;

        //    return new Patient(id, name, pwd, email, phone, address, doctorId);
        //}



        // doctors.txt:  id|name|password|email|phone|address|specialty
        private static Doctor? ParseDoctor(string line)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("[WARNING] blank line in doctor's file");
                    return null;
                }

                var s = line.Split('|');
                if (s.Length < 7)
                {
                    Console.WriteLine($"[ERROR] $\"Invalid doctor line:{s.Length}: {line}");
                    return null;
                }

                int id = int.Parse(s[0]);
                string name = s[1];
                string pwd = s[2];
                string email = s[3];
                string phone = s[4];
                string address = s[5];
                string specialty = s[6];

                return new Doctor(id, name, pwd, email, phone, address, specialty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]Error parsing doctor line '{line}': {ex.Message}");
                return null;
            }
        }

        // appointments.txt: id|doctorId|patientId|notes
        private static Appointment? ParseAppointment(string line)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("[WARNING]  blank line in appointment's file");
                    return null;
                }

                var s = line.Split('|');
                if (s.Length < 4)
                {
                    Console.WriteLine($"[ERROR] Invalid appintment line{s.Length}: {line}");
                    return null;
                }

                int id = int.Parse(s[0]);
                int doctorId = int.Parse(s[1]);
                int patientId = int.Parse(s[2]);
                string notes = s[3];

                return new Appointment(id, doctorId, patientId, notes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing appintment line '{line}': {ex.Message}");
                return null;
            }
        }
    }
}
