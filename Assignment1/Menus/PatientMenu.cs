using System;
using System.Text;
using Assignment1.Services;       
using Assignment1.Core.Models;   

namespace Assignment1.Menus
{
    public static class PatientMenu
    {
        public static void Show(int userId)
        {
            
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Header("Patient Menu");

                // 取当前病人对象
                var me = DataRepository.GetPatientById(userId);

                // 从 Name 里提取 “Firstname Lastname”，如果只有一个词就原样
                string DisplayFirstLast(string name)
                {
                    if (string.IsNullOrWhiteSpace(name)) return "";
                    var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    return parts.Length >= 2 ? $"{parts[0]} {parts[^1]}" : parts[0];
                }

                var who = me != null ? DisplayFirstLast(me.Name) : $"User {userId}";
                Console.WriteLine($"Welcome to DOTNET Hospital Management System {who}\n");

                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1) List patient details");
                Console.WriteLine("2) List my doctor details");
                Console.WriteLine("3) List all appointments");
                Console.WriteLine("4) Book appointment");
                Console.WriteLine("L) Exit to login");
                Console.WriteLine("E) Exit system");
                Console.Write("\nSelect: ");

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        ListPatientDetails(userId);
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ListMyDoctorDetails(userId);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ListAllAppointments(userId);
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        BookAppointment(userId);
                        break;

                    case ConsoleKey.L:
                        return; 

                    case ConsoleKey.E:
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        // list patient detail method
        private static void ListPatientDetails(int userId)
        {
            Console.Clear();
            Header("My Details");

            var p = DataRepository.GetPatientById(userId);
            if (p == null)
            {
                Console.WriteLine("Patient not found.");
                Pause(); return;
            }

            Console.WriteLine($"{p.Name}'s Details\n");
            Console.WriteLine($"Patient ID: {p.Id}");
            Console.WriteLine($"Full Name:  {p.Name}");
            Console.WriteLine($"Address:    {p.Address}");
            Console.WriteLine($"Email:      {p.Email}");
            Console.WriteLine($"Phone:      {p.Phone}");
            Console.WriteLine($"DoctorId:   {(p.DoctorId.HasValue ? p.DoctorId.Value : -1)}");

            Pause();
        }

        // list my doctor method
        private static void ListMyDoctorDetails(int userId)
        {
            Console.Clear();
            Header("My Doctor");

            var p = DataRepository.GetPatientById(userId);
            if (p == null) { Console.WriteLine("Patient not found."); Pause(); return; }

            if (!p.DoctorId.HasValue)
            {
                Console.WriteLine("No doctor is currently assigned.");
                Pause(); return;
            }

            var d = DataRepository.GetDoctorById(p.DoctorId.Value);
            if (d == null) { Console.WriteLine("Doctor not found."); Pause(); return; }

            Console.WriteLine($"ID:       {d.Id}");
            Console.WriteLine($"Name:     {d.Name}");
            Console.WriteLine($"Address:  {d.Address}");
            Console.WriteLine($"Email:    {d.Email}");
            Console.WriteLine($"Phone:    {d.Phone}");
            Console.WriteLine($"Specialty:{d.Specialty}");

            Pause();
        }

        // list all appintment
        private static void ListAllAppointments(int userId)
        {
            Console.Clear();
            Header("My Appointments");

            var appts = DataRepository.GetAppointmentsByPatient(userId);
            if (appts.Count == 0)
            {
                Console.WriteLine("No appointments yet.");
                Pause(); return;
            }

            foreach (var a in appts)
            {
                Console.WriteLine($"{a.Id} | Doctor:{a.DoctorId} | Notes: {a.Notes}");
            }
            Pause();
        }

        // add new appintment
        private static void BookAppointment(int userId)
        {
            Console.Clear();
            Header("Book Appointment");

            var p = DataRepository.GetPatientById(userId);
            if (p == null) { Console.WriteLine("Patient not found."); Pause(); return; }

            int doctorId;

            // if already has doctor, use it directly
            //otherwise select from list
            if (p.DoctorId.HasValue)
            {
                doctorId = p.DoctorId.Value;
                Console.WriteLine($"Using your assigned doctor: {doctorId}");
            }
            else
            {
                var doctors = DataRepository.GetAllDoctors();
                if (doctors.Count == 0) { Console.WriteLine("No doctors available."); Pause(); return; }

                Console.WriteLine("Select a doctor by ID:");
                foreach (var d in doctors)
                    Console.WriteLine($"{d.Id} | {d.Name} | {d.Specialty}");

                Console.Write("\nDoctor ID: ");
                if (!int.TryParse(Console.ReadLine(), out doctorId))
                {
                    Console.WriteLine("Invalid doctor ID.");
                    Pause(); return;
                }
            }

            Console.Write("Notes: ");
            var notes = Console.ReadLine() ?? "";

            var newId = DataRepository.AddAppointment(doctorId, userId, notes);
            Console.WriteLine($"\nAppointment created. ID = {newId}");

            Pause();
        }

        // ========== header ==========
        private static void Header(string subtitle)
        {
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│ DOTNET Hospital Management System          │");
            Console.WriteLine("│--------------------------------------------│");
            Console.WriteLine($"│ {subtitle.PadRight(42)} │");
            Console.WriteLine("└────────────────────────────────────────────┘");
            Console.WriteLine();
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey(true);
        }
    }
}
