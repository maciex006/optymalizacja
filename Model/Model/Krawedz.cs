using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Krawedz : ElementModelu
    {
        public int Id { get; private set; }
        private int Koszt;
        public Stacja Stacja1 { get; private set; }
        public Stacja Stacja2 { get; private set; }

        public string StringFormatDlaLinii
        {
            get
            {
                return "-{" + Koszt + "}-";
            }
        }

        public string StringFormatDlaPolaczen
        {
            get
            {
                return Stacja1.Id.ToString() + Stacja2.Id.ToString();
            }
        }

        public int GetKoszt()
        {
            return Koszt;
        }

        public Krawedz(int id, Stacja st1, Stacja st2, int koszt)
        {
            st1.AddKrawedz(this);
            st2.AddKrawedz(this);
            this.Id = id;
            this.Stacja1 = st1;
            this.Stacja2 = st2;
            this.Koszt = koszt;
        }

        public List<ElementModelu> GetIncydentneElementy()
        {
            List<ElementModelu> sasiedzi = new List<ElementModelu>();
            sasiedzi.Add(Stacja1);
            sasiedzi.Add(Stacja2);
            return sasiedzi;
        }

        public override string ToString()
        {
            return Stacja1.Id + "-{" + Koszt + "}-" + Stacja2.Id;
        }
    }

    public class KrawedzSieci
    {
        public Krawedz Krawedz { get; private set; }
        public List<int> IdLinii { get; private set; }

        public KrawedzSieci(List<int> idLinii, Krawedz k)
        {
            this.IdLinii = idLinii;
            this.Krawedz = k;
        }
    }

    public class Polaczenie
    {
        public int[] Id { get; private set; }
        public List<KrawedzSieci> Krawedzie { get; private set; }


        public Polaczenie(int[] id, List<KrawedzSieci> k)
        {
            this.Id = id;
            this.Krawedzie = k;
        }
    }
}
