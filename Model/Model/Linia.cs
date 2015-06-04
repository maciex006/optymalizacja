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

        private void GenerujNowaLinie()
        {
            List<Stacja> petle = Model.GetPetle();
            // Losowanie pętli startowej.
            Line.Add(petle[Random.Next(petle.Count)]);
            int i = 0;
            while (i == 0 || !((Stacja)Line[i]).IsPetla)
            {
                // Losowanie krawędzi.
                List<ElementModelu> sasiednieElementy = Line[i].GetIncydentneElementy();
                ElementModelu wylosowanaKrawedz = sasiednieElementy[Random.Next(sasiednieElementy.Count)];
                Line.Add(wylosowanaKrawedz);
                ElementModelu dalszaStacja = wylosowanaKrawedz.GetIncydentneElementy().First(x => x.Id != Line[i].Id);
                Line.Add(dalszaStacja);
                i = i + 2;
            }
        }

        public override string ToString()
        {
            return "{" + string.Join(",", Line) + "}";
        }
    }
}
