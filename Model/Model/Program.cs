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
            int t = 5; // liczba interwalów czasowych.
            int p = 4; // liczba petli.
            Model m = new Model(new Random());
            m.Generuj(n, t, p);

            List<Krawedz> kr = m.GetIncydentneKrawedzie(2);
            List<Stacja> st = m.GetSasiednieStacje(2);
            Dictionary<Stacja,MacierzRuchu> ruch = m.GetRuch(2);
            List<Stacja> petle = m.GetPetle();

            Console.WriteLine("\nKrawedzie incydentne dla stacji 2:");
            foreach (Krawedz k in kr)
            {
                Console.WriteLine(k);
            }

            Console.WriteLine("\nStacje sasiednie dla stacji 2:");
            foreach (Stacja s in st)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("\nRuch na stacji 2:");
            Console.WriteLine("{" + string.Join(",", ruch.Select(x => x.Key.ToString() + "=" + x.Value.ToString()).ToArray()) + "}");

            Console.WriteLine("\nPetle w modelu m:");
            foreach (Stacja pe in petle)
            {
                Console.WriteLine(pe);
            }

            Console.ReadKey();
        }
    }
}
