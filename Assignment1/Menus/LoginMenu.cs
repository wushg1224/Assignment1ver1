using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assignment1.Services;

namespace Assignment1.Menus
{
    public static class LoginMenu
    {
        public static void Show()
        {
            while (true)
            {
                var result = AuthService.Login();
                if (result is null) continue;

                var (role, userId) = result.Value;
                switch (role.ToLowerInvariant())
                {
                    case "patient": PatientMenu.Show(userId); break;
                    case "doctor": DoctorMenu.Show(userId); break;
                    case "admin": AdminMenu.Show(userId); break;
                    default:
                        Console.WriteLine("Unknown role. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
