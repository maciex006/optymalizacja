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

        /// <summary>
        ///     Metoda zwracajáca liste krawedzi incydentnych ze stacja o podanym id.
        /// </summary>
        /// <param name="id"> Identyfikator stacji w modelu. </param>
        /// <returns> Lista krawedzi. </returns>
        public List<Krawedz> GetIncydentneKrawedzie(int id)
        {
            return Stacje[id].GetIncydentneKrawedzie();
        }

        /// <summary>
        ///     Metoda zwracajáca liste stacji sasiednich dla stacji o podanym id.
        /// </summary>
        /// <param name="id"> Identyfikator stacji w modelu. </param>
        /// <returns> Lista stacji. </returns>
        public List<Stacja> GetSasiednieStacje(int id)
        {
           return Stacje[id].GetSasiednieStacje();
        }

        /// <summary>
        ///     Metoda zwracajáca slownik przechowujacy informacje o ruchu na danym przystanku.
        /// </summary>
        /// <param name="id"> Identyfikator stacji w modelu. </param>
        /// <returns> Slownik przechowujacy informacje o ruchu. </returns>
        public Dictionary<Stacja, MacierzRuchu> GetRuch(int id)
        {
            return Stacje[id].GetRuch();
        }
    }
}
