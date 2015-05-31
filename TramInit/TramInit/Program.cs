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
            //liczba przystanków.
            int n = 10;
            Model model = new Model(n);
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
