using System;
using System.Text;
using System.Linq;
using Assignment1.Services;
using Assignment1.Core.Models;

namespace Assignment1.Menus
{
    public static class AdminMenu
    {
        public static void Show(int userId)
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Header("Admin Menu");

                Console.WriteLine($"Welcome Admin {userId}\n");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1) List all doctors");
                Console.WriteLine("2) Check doctor details");
                Console.WriteLine("3) List all patients");
                Console.WriteLine("4) Check patient details");
                Console.WriteLine("5) Add doctor");
                Console.WriteLine("6) Add patient");
                Console.WriteLine("L) Exit to login");
                Console.WriteLine("E) Exit system");
                Console.Write("\nSelect: ");

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        ListAllDoctors();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        CheckDoctorDetails();
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ListAllPatients();
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        CheckPatientDetails();
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        AddDoctor();
                        break;

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        AddPatient();
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

        // list all doctor
        private static void ListAllDoctors()
        {
            Console.Clear();
            Header("All Doctors");

            var docs = DataRepository.GetAllDoctors().OrderBy(d => d.Id).ToList();
            if (docs.Count == 0) { Console.WriteLine("No doctors."); Pause(); return; }

            foreach (var d in docs)
                Console.WriteLine($"{d.Id} | {d.Name} | {d.Specialty}");

            Pause();
        }

        // list doctor detail
        private static void CheckDoctorDetails()
        {
            Console.Clear();
            Header("Doctor Details");

            Console.Write("Enter doctor ID: ");
            if (!int.TryParse(Console.ReadLine(), out var did))
            {
                Console.WriteLine("Invalid ID.");
                Pause(); return;
            }

            var d = DataRepository.GetDoctorById(did);
            if (d == null) { Console.WriteLine("Doctor not found."); Pause(); return; }

            Console.WriteLine($"ID:        {d.Id}");
            Console.WriteLine($"Name:      {d.Name}");
            Console.WriteLine($"Address:   {d.Address}");
            Console.WriteLine($"Email:     {d.Email}");
            Console.WriteLine($"Phone:     {d.Phone}");
            Console.WriteLine($"Specialty: {d.Specialty}");
            Pause();
        }

        // list all patient
        private static void ListAllPatients()
        {
            Console.Clear();
            Header("All Patients");

            var pts = DataRepository.GetAllPatients().OrderBy(p => p.Id).ToList();
            if (pts.Count == 0) { Console.WriteLine("No patients."); Pause(); return; }

            foreach (var p in pts)
                Console.WriteLine($"{p.Id} | {p.Name} | DoctorId:{(p.DoctorId.HasValue ? p.DoctorId.Value : -1)}");

            Pause();
        }

        // check patient detial
        private static void CheckPatientDetails()
        {
            Console.Clear();
            Header("Patient Details");

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

        // add doctor
        private static void AddDoctor()
        {
            Console.Clear();
            Header("Add Doctor");

            var used = DataRepository.GetAllDoctors().Select(d => d.Id).ToHashSet();
            var newId = IdGenerator.NewId(used);

            Console.Write("Full name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";
            Console.Write("Phone: ");
            var phone = Console.ReadLine() ?? "";
            Console.Write("Address: ");
            var address = Console.ReadLine() ?? "";
            Console.Write("Specialty: ");
            var spec = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            var pwd = Console.ReadLine() ?? "";

            // doctors.txt: id|name|password|email|phone|address|specialty
            var line = $"{newId}|{name}|{pwd}|{email}|{phone}|{address}|{spec}";
            FileManager.AppendLine("doctors.txt", line);

            // credentials.txt: id|password|role
            FileManager.AppendLine("credentials.txt", $"{newId}|{pwd}|doctor");

            Console.WriteLine($"\nDoctor created. ID = {newId}");
            Pause();
        }

        //add patient to  patients.txt + credentials.txt
        private static void AddPatient()
        {
            Console.Clear();
            Header("Add Patient");

            var used = DataRepository.GetAllPatients().Select(p => p.Id).ToHashSet();
            var newId = IdGenerator.NewId(used);

            Console.Write("Full name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";
            Console.Write("Phone: ");
            var phone = Console.ReadLine() ?? "";
            Console.Write("Address: ");
            var address = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            var pwd = Console.ReadLine() ?? "";
            Console.Write("Doctor ID (optional, Enter to skip): ");
            var docText = Console.ReadLine();

            string doctorIdField = "";
            if (int.TryParse(docText, out var did))
                doctorIdField = did.ToString(); // 绑定医生
            // 留空则保持 ""，解析时会得到 null

            // patients.txt: id|name|password|email|phone|address|doctorId
            var line = $"{newId}|{name}|{pwd}|{email}|{phone}|{address}|{doctorIdField}";
            FileManager.AppendLine("patients.txt", line);

            // credentials.txt
            FileManager.AppendLine("credentials.txt", $"{newId}|{pwd}|patient");

            Console.WriteLine($"\nPatient created. ID = {newId}");
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
