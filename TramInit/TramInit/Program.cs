using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramInit
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 10;
            Model model = new Model(n, 5);
            Console.WriteLine(model.CostMatrixToString());
            for (int i = 0; i < n; ++i)
            {
                Console.WriteLine("\nPrzystanek numer " + i);
                Console.WriteLine(model.TrafficMatrixToString(i));
            }

            Console.ReadKey();
        }
    }
}
