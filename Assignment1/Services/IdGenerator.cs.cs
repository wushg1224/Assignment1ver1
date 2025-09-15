using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Services
{

    //id generator
    public static class IdGenerator
    {
        private static readonly Random _rng = new();

        public static int NewId(HashSet<int>? used = null)
        {
            while (true)
            {
                int digits = _rng.Next(5, 9); // 5–8 位
                int min = (int)Math.Pow(10, digits - 1);
                int max = (int)Math.Pow(10, digits) - 1;
                int id = _rng.Next(min, max);
                if (used == null || !used.Contains(id)) return id;
            }
        }
    }
}
