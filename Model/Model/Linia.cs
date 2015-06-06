using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Linia
    {
        public int Id;
        private Model Model;
        private List<ElementModelu> Line = new List<ElementModelu>();
        private Random Random;
        public double Koszt { get; set; }


        public int GetUniqueCode()
        {
            int code = 0;
            foreach (ElementModelu e in Line)
            {
                code += e.Id;
            }

            return code;
        }

        public Linia(int id, Model m, Random r)
        {
            this.Id = id;
            this.Model = m;
            this.Random = r;
            bool generated = false;
            while (!generated)
            {
                generated = GenerujNowaLinie();
            }
        }

        private bool GenerujNowaLinie()
        {
            List<Stacja> petle = Model.GetPetle();
            // Losowanie pętli startowej.
            Line.Add(petle[Random.Next(petle.Count)]);
            int i = 0;
            while (i == 0 || !((Stacja)Line[i]).IsPetla)
            {
                // Losowanie stacji.
                List<Stacja> sasiednieElementy = ((Stacja)Line[i]).GetSasiednieStacje();
                Stacja wylosowanaStacja = sasiednieElementy[Random.Next(sasiednieElementy.Count)];
                int loopGuard = 0;
                while (Line.Contains(wylosowanaStacja))
                {
                    loopGuard++;
                    if (loopGuard > 5)
                    {
                        Line = new List<ElementModelu>();
                        return false;
                    }

                    wylosowanaStacja = sasiednieElementy[Random.Next(sasiednieElementy.Count)];
                }

                Krawedz wylosowanaKrawedz = wylosowanaStacja.GetIncydentneKrawedzie()
                    .Where(x => x.Stacja1.Id == Line[i].Id || x.Stacja2.Id == Line[i].Id).First();
                Line.Add(wylosowanaKrawedz);
                Line.Add(wylosowanaStacja);
                i = i + 2;
            }

            return true;
        }

        public List<Krawedz> GetKrawedzie()
        {
            List<Krawedz> krawedzie = new List<Krawedz>();
            foreach(ElementModelu k in Line.Where(x => x.GetType() == typeof(Krawedz)))
            {
                krawedzie.Add((Krawedz)k);
            }

            return krawedzie;
        }

        public override string ToString()
        {
            return "{" + string.Join("-", Line.Select(x => x.StringFormatDlaLinii)) + "}";
        }

        public string ToStringShortFormat()
        {
            return "{" + string.Join(",", Line.Where(x => x.GetType() == typeof(Stacja))) + "}";
        }
    }
}
