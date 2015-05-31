using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 10; // liczba przystanków
            int t = 1; // liczba interwalów czasowych.
            Model m = new Model(new Random());
            m.Generuj(n, t);


            List<Krawedz> kr = m.GetIncydentneKrawedzie(2);
            List<Stacja> st = m.GetSasiednieStacje(2);
            foreach (Krawedz k in kr)
            {
                Console.WriteLine(k);
            }

            foreach (Stacja s in st)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }
    }
}
