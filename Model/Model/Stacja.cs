﻿using System;
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
        public bool IsPetla { get; set; }

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

            if (IncydentneKrawedzie.Count < 4)
            {
                randomLiczbaKrawedzi = r.Next(0, MAX_LICZ_KRAW - IncydentneKrawedzie.Count) + 1;
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
                if (s.GetIncydentneKrawedzie().Count() < 4)
                {
                    krawedzie.Add(new Krawedz(i + 10 * ranNum, st, s, r.Next(15)));
                }
            }

            foreach (Krawedz k in IncydentneKrawedzie)
            {
                Stacja s = k.Stacja1.Id != Id ? k.Stacja1 : k.Stacja2;
                if (!Ruch.ContainsKey(s))
                {
                    Ruch.Add(s, new MacierzRuchu(r, t));
                }
                if (!s.Ruch.ContainsKey(this))
                {
                    s.Ruch.Add(this, new MacierzRuchu(r, t));
                }
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

        public override string ToString()
        {
            string buf = "";

            for(int i = 0; i < Ruch.Length; i++)
            {
                buf = i > 0 ? buf + "," : buf + "[";
                buf = buf + Ruch[i];
            }

            return buf + "]";
        }
    }
}