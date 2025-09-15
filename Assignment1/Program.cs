using Assignment1.Menus;

namespace Assignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 保障运行时有 Data 目录（用于首次写入等）
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Data"));

            Console.Title = "Assignment1 – Hospital Console App";
            LoginMenu.Show();

            Console.WriteLine("\nGoodbye!");
        }
    }
}

