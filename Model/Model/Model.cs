using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Model
    {
        List<Stacja> Stacje = new List<Stacja>();
        List<Krawedz> Krawedzie = new List<Krawedz>();
        Random Random;

        public Model(Random r)
        {
            this.Random = r;
        }

        /// <summary>
        ///     Generuje losowy model.
        /// </summary>
        /// <param name="n"> Liczba stacji. </param>
        /// <param name="t"> Liczba interwałów czasowych. </param>
        public void Generuj(int n, int t)
        {
            for (int i = 0; i < n; i++)
            {
                Stacje.Add(new Stacja(i));
            }

            foreach (Stacja st in Stacje)
            {
                st.Generuj(t, st, Stacje, Krawedzie, Random);
            }
        }

        public List<Krawedz> GetIncydentneKrawedzie(int id)
        {
            return Stacje[id].GetIncydentneKrawedzie();
        }

        public List<Stacja> GetSasiednieStacje(int id)
        {
           return Stacje[id].GetSasiednieStacje();
        }
    }
}
