using System;
using System.Text;
using System.Linq;
using Assignment1.Services;
using Assignment1.Core.Models;
using System.Text.Json;

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

                var d = DataRepository.GetDoctorById(userId);
                Console.WriteLine($"Welcome to DOTNET hospital management system Doctor {d.Name}\n");
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

            Console.WriteLine("");
            string tableTitle = "name     |email address  | phone     |address";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");
            string content = d.Name + "    |" + d.Email + "         |" + d.Phone + "        |" + d.Address;
            Console.WriteLine(content);

            //Console.WriteLine($"ID:        {d.Id}");
            //Console.WriteLine($"Name:      {d.Name}");
            //Console.WriteLine($"Address:   {d.Address}");
            //Console.WriteLine($"Email:     {d.Email}");
            //Console.WriteLine($"Phone:     {d.Phone}");
            //Console.WriteLine($"Specialty: {d.Specialty}");
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
            var d = DataRepository.GetDoctorById(userId);
            if (d == null) { Console.WriteLine("Doctor not found."); Pause(); return; }

            Console.WriteLine($"Patients assigned to {d.Name}");
            string tableTitle = "patient     |doctor  |email address    |phone";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");

            foreach (var p in all)
                Console.WriteLine($" {p.Name} | {d.Name}|{p.Email}|{p.Phone}");

            Pause();
        }

        private static void ListAppointments(int userId)
        {
            Console.Clear();
            Header("My Appointments");

            var appts = DataRepository.GetAppointmentsByDoctor(userId);
            if (appts.Count == 0) { Console.WriteLine("No appointments."); Pause(); return; }
            string tableTitle = "doctor   |patient  | Description";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");
            foreach (var a in appts)
            {
                var d = DataRepository.GetDoctorById(userId);
                var p = DataRepository.GetPatientById(a.PatientId);
                Console.WriteLine($"{d.Name} | {p.Name} | {a.Notes}");
            }

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
            Console.WriteLine($"Enter the id of the patient to check:{pid}");
            string tableTitle = "Patient  |Docotr | Email Address |phone |address";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");
            var d = DataRepository.GetDoctorById(Convert.ToInt32(p.DoctorId));
            Console.WriteLine($"{p.Name} | {d.Name} | {p.Email}|{p.Phone}|{p.Address}");
            //Console.WriteLine($"ID:       {p.Id}");
            //Console.WriteLine($"Name:     {p.Name}");
            //Console.WriteLine($"Address:  {p.Address}");
            //Console.WriteLine($"Email:    {p.Email}");
            //Console.WriteLine($"Phone:    {p.Phone}");
            //Console.WriteLine($"DoctorId: {(p.DoctorId.HasValue ? p.DoctorId.Value : -1)}");
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
