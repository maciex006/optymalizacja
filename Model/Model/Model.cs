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
        /// <param name="p"> Liczba petli. </param>
        public void Generuj(int n, int t, int p)
        {
            for (int i = 0; i < n; i++)
            {
                Stacje.Add(new Stacja(i));
            }


            for (int i = 0; i < p; i++)
            {
                int ranNum = Random.Next(Stacje.Count);
                while (Stacje.First(x => x.Id == ranNum).IsPetla)
                {
                    ranNum = Random.Next(Stacje.Count);
                }

                Stacje[ranNum].IsPetla = true;
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
        ///     Metoda zwracajaca liste stacji sasiednich dla stacji o podanym id.
        /// </summary>
        /// <param name="id"> Identyfikator stacji w modelu. </param>
        /// <returns> Lista stacji. </returns>
        public List<Stacja> GetSasiednieStacje(int id)
        {
           return Stacje[id].GetSasiednieStacje();
        }

        /// <summary>
        ///     Metoda zwracajaca liste stacji w modelu.
        /// </summary>
        /// <returns> Lista stacji. </returns>
        public List<Stacja> GetStacje()
        {
            return Stacje;
        }

        /// <summary>
        ///     Metoda zwracajaca liste stacji w modelu.
        /// </summary>
        /// <returns> Lista stacji. </returns>
        public List<Stacja> GetPetle()
        {
            return Stacje.Where(x => x.IsPetla).ToList();
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

    public interface ElementModelu
    {
        int Id { get; }
        List<ElementModelu> GetIncydentneElementy();
    }
}
