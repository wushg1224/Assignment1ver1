using System;
using System.Text;
using System.Linq;
using Assignment1.Services;
using Assignment1.Core.Models;

namespace Assignment1.Menus
{
    public static class DoctorMenu
    {
        public static void Show(int userId)
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Header("Doctor Menu");

                Console.WriteLine($"Welcome Doctor {userId}\n");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1) List doctor details");
                Console.WriteLine("2) List patients");
                Console.WriteLine("3) List appointments");
                Console.WriteLine("4) Check particular patient");
                Console.WriteLine("5) List appointments with patient");
                Console.WriteLine("L) Exit to login");
                Console.WriteLine("E) Exit system");
                Console.Write("\nSelect: ");

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        ListDoctorDetails(userId);
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ListPatients(userId);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ListAppointments(userId);
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        CheckParticularPatient();
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        ListAppointmentsWithPatient(userId);
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

        private static void ListDoctorDetails(int userId)
        {
            Console.Clear();
            Header("My Details");

            var d = DataRepository.GetDoctorById(userId);
            if (d == null) { Console.WriteLine("Doctor not found."); Pause(); return; }

            Console.WriteLine($"ID:        {d.Id}");
            Console.WriteLine($"Name:      {d.Name}");
            Console.WriteLine($"Address:   {d.Address}");
            Console.WriteLine($"Email:     {d.Email}");
            Console.WriteLine($"Phone:     {d.Phone}");
            Console.WriteLine($"Specialty: {d.Specialty}");
            Pause();
        }

        private static void ListPatients(int userId)
        {
            Console.Clear();
            Header("My Patients");

            var all = DataRepository.GetAllPatients()
                                    .Where(p => p.DoctorId.HasValue && p.DoctorId.Value == userId)
                                    .OrderBy(p => p.Id)
                                    .ToList();
            if (all.Count == 0) { Console.WriteLine("No patients assigned."); Pause(); return; }

            foreach (var p in all)
                Console.WriteLine($"{p.Id} | {p.Name} | {p.Email}");

            Pause();
        }

        private static void ListAppointments(int userId)
        {
            Console.Clear();
            Header("My Appointments");

            var appts = DataRepository.GetAppointmentsByDoctor(userId);
            if (appts.Count == 0) { Console.WriteLine("No appointments."); Pause(); return; }

            foreach (var a in appts)
                Console.WriteLine($"{a.Id} | Patient:{a.PatientId} | Notes: {a.Notes}");

            Pause();
        }

        private static void CheckParticularPatient()
        {
            Console.Clear();
            Header("Check Patient");

            Console.Write("Enter patient ID: ");
            if (!int.TryParse(Console.ReadLine(), out var pid))
            {
                Console.WriteLine("Invalid ID.");
                Pause(); return;
            }

            var p = DataRepository.GetPatientById(pid);
            if (p == null) { Console.WriteLine("Patient not found."); Pause(); return; }

            Console.WriteLine($"ID:       {p.Id}");
            Console.WriteLine($"Name:     {p.Name}");
            Console.WriteLine($"Address:  {p.Address}");
            Console.WriteLine($"Email:    {p.Email}");
            Console.WriteLine($"Phone:    {p.Phone}");
            Console.WriteLine($"DoctorId: {(p.DoctorId.HasValue ? p.DoctorId.Value : -1)}");
            Pause();
        }

        private static void ListAppointmentsWithPatient(int userId)
        {
            Console.Clear();
            Header("Appointments With Patient");

            Console.Write("Enter patient ID: ");
            if (!int.TryParse(Console.ReadLine(), out var pid))
            {
                Console.WriteLine("Invalid ID.");
                Pause(); return;
            }

            var appts = DataRepository.GetAppointmentsByDoctor(userId)
                                      .Where(a => a.PatientId == pid)
                                      .OrderBy(a => a.Id)
                                      .ToList();

            if (appts.Count == 0) { Console.WriteLine("No appointments with this patient."); Pause(); return; }

            foreach (var a in appts)
                Console.WriteLine($"{a.Id} | Patient:{a.PatientId} | Notes: {a.Notes}");

            Pause();
        }

        private static void Header(string subtitle)
        {
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│ DOTNET Hospital Management System          │");
            Console.WriteLine("│--------------------------------------------│");
            Console.WriteLine($"│ {subtitle.PadRight(42)}│");
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
