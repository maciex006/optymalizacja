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
}
