using Assignment1.Menus;

namespace Assignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // create directory
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Data"));

            Console.Title = "Assignment1 – Hospital Console App";

            //show medu
            LoginMenu.Show();

            Console.WriteLine("\nExit!");
        }
    }
}

