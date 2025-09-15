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
                string ad = DataRepository.GetAdminsName();

                Console.WriteLine($"Welcome to DOTNET Hospital Management System {ad}\n");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1) List all doctors");
                Console.WriteLine("2) Check doctor details");
                Console.WriteLine("3) List all patients");
                Console.WriteLine("4) Check patient details");
                Console.WriteLine("5) Add doctor");
                Console.WriteLine("6) Add patient");
                Console.WriteLine("L) Logout");
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

        // list all doctor info
        private static void ListAllDoctors()
        {
            Console.Clear();
            Header("All Doctors");

            var docs = DataRepository.GetAllDoctors().OrderBy(d => d.Id).ToList();
            if (docs.Count == 0) { Console.WriteLine("No doctors."); Pause(); return; }
            string tableTitle = "Doctor        |Email Adress            | Phone       |Address";
            Console.WriteLine(tableTitle);
            Console.WriteLine("------------------------------------------------------------------");
            foreach (var d in docs)

            {
                //var d = DataRepository.GetDoctorById(userId);
                //var p = DataRepository.GetPatientById(a.PatientId);
                Console.WriteLine($"{d.Name} | {d.Email} | {d.Phone}|{d.Address}");
            }


            Pause();
        }

        // list select doctor detail
        private static void CheckDoctorDetails()
        {
            Console.Clear();
            Header("Doctor Details");

            Console.Write("Please enterr the ID of patient who's details you are checking. or press n to return to menu: ");
            var input = Console.ReadLine()?.Trim();
            if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase)) return;
            if (!int.TryParse(input, out var pid))
            {
                Console.WriteLine("Invalid ID.");
                Pause(); return;
            }

            var d = DataRepository.GetDoctorById(did);
            if (d == null) { Console.WriteLine("Doctor not found."); Pause(); return; }

            Console.WriteLine("your doctor:");
            Console.WriteLine("");
            string tableTitle = "Name        |Email Adress            | Phone       |Address";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");
            string content = d.Name + "    |" + d.Email + "         |" + d.Phone + "        |" + d.Address;
            Console.WriteLine(content);
            //Console.WriteLine($"Name:      {d.Name}");
            //Console.WriteLine($"Address:   {d.Address}");
            //Console.WriteLine($"Email:     {d.Email}");
            //Console.WriteLine($"Phone:     {d.Phone}");
            //Console.WriteLine($"Specialty: {d.Specialty}");
            Pause();
        }

        // list all patient
        private static void ListAllPatients()
        {
            Console.Clear();
            Header("All Patients");

            var pts = DataRepository.GetAllPatients().OrderBy(p => p.Id).ToList();
            if (pts.Count == 0) { Console.WriteLine("No patients."); Pause(); return; }

            //foreach (var p in pts)
            //    Console.WriteLine($"{p.Id} | {p.Name} | DoctorId:{(p.DoctorId.HasValue ? p.DoctorId.Value : -1)}");



            string tableTitle = "patient     |doctor  |email address    |phone| Address";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");

            foreach (var p in pts)
            {
                var doc = DataRepository.GetPatientById(Convert.ToInt32(p.DoctorId));
                Console.WriteLine($" {p.Name} | {doc.Name}|{p.Email}|{p.Phone}|{p.Address}");
            }
            Pause();
        }

        // check patient detial
        private static void CheckPatientDetails()
        {
            Console.Clear();
            Header("Patient Details");

            Console.Write("Please enterr the ID of patient who's details you are checking. or press n to return to menu: ");
            var input = Console.ReadLine()?.Trim();
            if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase)) return;
            if (!int.TryParse(Console.ReadLine(), out var pid))
            {
                Console.WriteLine("Invalid ID.");
                Pause(); return;
            }

            var p = DataRepository.GetPatientById(pid);
            if (p == null) { Console.WriteLine("Patient not found."); Pause(); return; }


            string tableTitle = "patient     |doctor  |email address    |phone  |adress";
            Console.WriteLine(tableTitle);
            Console.WriteLine("---------------------------------------------------");
            var doc = DataRepository.GetDoctorById(Convert.ToInt32(p.DoctorId));

            Console.WriteLine($" {p.Name} | {doc.Name}|{p.Email}|{p.Phone}|{p.Address}");
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
