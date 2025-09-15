using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assignment1.Services
{
    public static class FileManager
    {
        public static string DataPath(string filename)
            => Path.Combine(AppContext.BaseDirectory, "Data", filename);


        public static IEnumerable<string> ReadAllLinesSafe(string filename)
        {
            try
            {
                var path = DataPath(filename);
                if (!File.Exists(path)) return Enumerable.Empty<string>();
                return File.ReadAllLines(path, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IO ERROR] {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }



        public static void AppendLine(string filename, string line)
        {
            try
            {
                var path = DataPath(filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.AppendAllText(path, line + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IO ERROR] {ex.Message}");
            }
        }

        public static void OverwriteAllLines(string filename, IEnumerable<string> lines)
        {
            try
            {
                var path = DataPath(filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllLines(path, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IO ERROR] {ex.Message}");
            }
        }

        //check if file exsit
        public static bool FileExists(string filename)
        {
            return File.Exists(DataPath(filename));
        }


    }
}
