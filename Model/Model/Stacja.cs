using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Stacja : ElementModelu
    {
        private const int MAX_LICZ_KRAW = 4;

        public int Id { get; private set; }
        private int InterwalyT;
        private List<Krawedz> IncydentneKrawedzie = new List<Krawedz>();
        private Dictionary<Stacja, MacierzRuchu> Ruch;
        private Random Random;
        private List<int> prawdLiczKraw = new List<int>();
        public bool IsPetla { get; set; }

        public string StringFormatDlaLinii
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        ///     Konstruktor stacji.
        /// </summary>
        /// <param name="id"> Identyfikator stacji. </param>
        public Stacja(int id)
        {
            this.Id = id;
            Ruch = new Dictionary<Stacja, MacierzRuchu>();
        }

        /// <summary>
        ///     Metoda generująca losowo krawędzie oraz ruch na stacjach.
        /// </summary>
        /// <param name="t"> Liczba interwałów czasowych. </param>
        /// <param name="st"> Stacja aktualnie uzupelaniana danymi.</param>
        /// <param name="stacje"> List stacji w modelu. </param>
        /// <param name="r"> Random r. </param>
        /// <param name="krawedzie"> Lista krawédzi w modelu.</param>
        public void Generuj(int t, Stacja st, List<Stacja> stacje, List<Krawedz> krawedzie, Random r)
        {
            this.InterwalyT = t;
            this.Random = r;      
            int randomLiczbaKrawedzi = 0;

            if (IncydentneKrawedzie.Count < MAX_LICZ_KRAW)
            {
                randomLiczbaKrawedzi = LosujLiczbeKraw(IncydentneKrawedzie.Count);
            }

            for (int i = 0; i < randomLiczbaKrawedzi; i++)
            {
                int ranNum = r.Next(stacje.Count);
                while (IncydentneKrawedzie.Any(x => x.Stacja1.Id == ranNum || x.Stacja2.Id == ranNum) || st.Id == ranNum)
                {
                    ranNum = r.Next(stacje.Count);
                }
                
                // Tymczasowa generacja Id - zmienić później. Podobnie z losowaniem kosztu.
                Stacja s = stacje.First(x => x.Id == ranNum);
                if (s.GetIncydentneKrawedzie().Count() < MAX_LICZ_KRAW)
                {
                    krawedzie.Add(new Krawedz(i + 10 * ranNum, st, s, r.Next(5)+2));
                }
            }

            foreach (Stacja s in stacje)
            {
                if (s.Id != this.Id)
                {
                    if (!Ruch.ContainsKey(s))
                    {
                        Ruch.Add(s, new MacierzRuchu(r, t));
                    }
                }
            }

            //foreach (Krawedz k in IncydentneKrawedzie)
            //{
            //    Stacja s = k.Stacja1.Id != Id ? k.Stacja1 : k.Stacja2;
            //    if (!Ruch.ContainsKey(s))
            //    {
            //        Ruch.Add(s, new MacierzRuchu(r, t));
            //    }
            //    if (!s.Ruch.ContainsKey(this))
            //    {
            //        s.Ruch.Add(this, new MacierzRuchu(r, t));
            //    }
            //}
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

        public List<ElementModelu> GetIncydentneElementy()
        {
            List<ElementModelu> sasiedzi = new List<ElementModelu>();
            foreach (Krawedz k in IncydentneKrawedzie)
            {
                if (k.Stacja1.Id != Id)
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

        public Dictionary<Stacja, MacierzRuchu> GetRuch()
        {
            return Ruch;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        private int LosujLiczbeKraw(int liczbKrawIstniejacych)
        {
            for (int i = MAX_LICZ_KRAW - liczbKrawIstniejacych; i > 0; i--)
            {
                if (NextWithProbability(1000 / (i + liczbKrawIstniejacych)))
                {
                    return i;
                }
            }

            return 0;
        }

        private bool NextWithProbability(int prawd)
        {
            int x = Random.Next(1000);
            return (x <= prawd);
        }

        // Inicjalizacja modelu z pliku.
        public void InitRuch(List<Stacja> stacje, int[] ruch)
        {
            for(int i = 0; i < ruch.Length ; i ++)
            {
                if (this.Id != stacje[i].Id)
                {
                    Ruch.Add(stacje[i], new MacierzRuchu(ruch[i]));
                }
            }
        }
    }

    public class MacierzRuchu
    {
        List<int> Ruch;

        /// <summary>
        ///     Konstruktor macierzy ruchu.
        /// </summary>
        /// <param name="t"> Liczba interwałów czasowych. </param>
        public MacierzRuchu(Random r, int t = 1)
        {
            Ruch = new List<int>();
            for (int i = 0; i < t; i++)
            {
                Ruch.Add(r.Next(13));
            }
        }

        public int GetSumaRuchu()
        {
            int suma = 0;
            foreach (int r in Ruch)
            {
                suma = suma + r;
            }

            return suma;
        }

        //Inicjalizacaja na podstawie pliku.
        //Tylko dla jednego interwału czasowego.
        public MacierzRuchu(int ruch)
        {
            Ruch = new List<int>();
            Ruch.Add(ruch);
        }

        //Implementacja dla t = 1;
        public int GetLiczbaPasazerow()
        {
            return Ruch[0];
        }

        public bool CheckIfNull()
        {
            return Ruch.Where(x => x == 0).Count() == Ruch.Count();
        }

        public override string ToString()
        {
            string buf = "";

            for(int i = 0; i < Ruch.Count(); i++)
            {
                buf = i > 0 ? buf + "," : buf + "[";
                buf = buf + Ruch[i];
            }

            return buf + "]";
        }
    }
}
