using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Services
{
    public static class AuthService
    {
        // Login Method
        public static (string role, int userId)? Login()
        {
            Console.Clear();
            // ===== header（Login）=====
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│    DOTNET Hospital Management System       │");
            Console.WriteLine("│--------------------------------------------│");
            Console.WriteLine("│                Login                       │");
            Console.WriteLine("└────────────────────────────────────────────┘");
            Console.WriteLine(); // 空行
            Console.Write("ID: ");
            var idText = Console.ReadLine();
            Console.Write("Password: ");
            var pwd = ReadPassword();

            //validate id
            if (!int.TryParse(idText, out var userId))
            {
                Console.WriteLine("\nInvalid ID. Press any key to try again");
                Console.ReadKey();
                return null;
            }

            //validate username and password
            var creds = FileManager.ReadAllLinesSafe("credentials.txt")
                .Select(line => line.Split('|'))
                .Where(p => p.Length >= 3)
                .Select(p => (id: int.Parse(p[0]), pass: p[1], role: p[2].Trim()))
                .ToList();

            var match = creds.FirstOrDefault(c => c.id == userId && c.pass == pwd);
            if (match.id == 0)
            {
                Console.WriteLine("\nInvalid credentials. Press any key try again");
                Console.ReadKey();
                return null;
            }
            return (match.role, match.id);
        }

        // use *** to mask
        public static string ReadPassword()
        {
            var pwd = new Stack<char>();
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Count > 0) { Console.Write("\b \b"); pwd.Pop(); }
                    continue;
                }
                if (!char.IsControl(key.KeyChar))
                {
                    pwd.Push(key.KeyChar);
                    Console.Write("*");
                }
            }
            return new string(pwd.Reverse().ToArray());
        }
    }
}