using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Stacja
    {
        private const int MAX_LICZ_KRAW = 4;

        private int Id;
        private int InterwalyT;
        private List<Krawedz> IncydentneKrawedzie = new List<Krawedz>();
        private Dictionary<Stacja, MacierzRuchu> Ruch;
        private Random Random;

        /// <summary>
        ///     Konstruktor stacji.
        /// </summary>
        /// <param name="id"> Identyfikator stacji. </param>
        public Stacja(int id)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Metoda generująca losowo krawędzie oraz ruch na stacjach.
        /// </summary>
        /// <param name="t"> Liczba interwałów czasowych. </param>
        /// <param name="st"> Stacja aktualnie uzupelaniana danymi.</param>
        /// <param name="stacje"> List stacji w modelu. </param>
        /// <param name="r"> Random r. </param>
        public void Generuj(int t, Stacja st, List<Stacja> stacje, List<Krawedz> krawedzie, Random r)
        {
            this.InterwalyT = t;
            this.Random = r;

            Ruch = new Dictionary<Stacja, MacierzRuchu>();

            for (int i = 0; i < MAX_LICZ_KRAW - IncydentneKrawedzie.Count; i++)
            {
                int ranNum = r.Next(stacje.Count);
                while (IncydentneKrawedzie.Any(x => x.Id == ranNum) || st.Id == ranNum)
                {
                    ranNum = r.Next(stacje.Count);
                }
                
                // Tymczasowa generacja Id - zmienić później. Podobnie z losowaniem kosztu.
                Stacja s = stacje.First(x => x.Id == ranNum);
                krawedzie.Add(new Krawedz(i + 10 * ranNum, st, s, r.Next(15)));
                Ruch.Add(s, new MacierzRuchu(r, t));
            }
        }

        /// <summary>
        ///     Dodaje krawędź do stacji.
        /// </summary>
        /// <param name="kr"> Dodawana krawędź.</param>
        public void AddKrawedz(Krawedz kr)
        {
            IncydentneKrawedzie.Add(kr);
        }
    }

    public class MacierzRuchu
    {
        int[] Ruch;

        /// <summary>
        ///     Konstruktor macierzy ruchu.
        /// </summary>
        /// <param name="t"> Liczba interwałów czasowych. </param>
        public MacierzRuchu(Random r, int t = 1)
        {
            Ruch = new int[t];
            for (int i = 0; i < t; i++)
            {
                Ruch[i] = r.Next(10);
            }
        }
    }
}
