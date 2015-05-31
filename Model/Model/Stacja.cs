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

        public int Id { get; private set; }
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

            int randomLiczbaKrawedzi = r.Next(0, MAX_LICZ_KRAW - IncydentneKrawedzie.Count) + 1;

            for (int i = 0; i < randomLiczbaKrawedzi; i++)
            {
                int ranNum = r.Next(stacje.Count);
                while (IncydentneKrawedzie.Any(x => x.Id == ranNum) || st.Id == ranNum)
                {
                    ranNum = r.Next(stacje.Count);
                }
                
                // Tymczasowa generacja Id - zmienić później. Podobnie z losowaniem kosztu.
                Stacja s = stacje.First(x => x.Id == ranNum);
                krawedzie.Add(new Krawedz(i + 10 * ranNum, st, s, r.Next(15)));
                //Ruch.Add(s, new MacierzRuchu(r, t));
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

        public List<Krawedz> GetIncydentneKrawedzie()
        {
            return IncydentneKrawedzie;
        }

        public List<Stacja> GetSasiednieStacje()
        {
            List<Stacja> sasiedzi = new List<Stacja>();
            foreach(Krawedz k in IncydentneKrawedzie)
            {
                if(k.Stacja1.Id != Id)
                {
                   sasiedzi.Add(k.Stacja1);
                }
                else
                {
                    sasiedzi.Add(k.Stacja2);
                }
            }

            return sasiedzi;
        }

        public override string ToString()
        {
            return Id.ToString();
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
