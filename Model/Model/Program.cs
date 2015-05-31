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
        }
    }
}
