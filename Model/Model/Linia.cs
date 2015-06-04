using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Linia
    {
        private Model Model;
        private List<ElementModelu> Line = new List<ElementModelu>();
        private Random Random;

        public Linia(Model m, Random r)
        {
            this.Model = m;
            this.Random = r;
            GenerujNowaLinie();
        }

        public double FunkcjaKosztu()
        {
            return 0;
        }

        private void GenerujNowaLinie()
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
                while (Line.Contains(wylosowanaStacja))
                {
                    wylosowanaStacja = sasiednieElementy[Random.Next(sasiednieElementy.Count)];
                }
                Krawedz wylosowanaKrawedz = wylosowanaStacja.GetIncydentneKrawedzie()
                    .Where(x => x.Stacja1.Id == Line[i].Id || x.Stacja2.Id == Line[i].Id).First();
                Line.Add(wylosowanaKrawedz);
                Line.Add(wylosowanaStacja);
                i = i + 2;
            }
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
