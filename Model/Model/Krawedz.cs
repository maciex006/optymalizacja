using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Krawedz
    {
        public int Id;
        private int Koszt;
        private Stacja Stacja1;
        private Stacja Stacja2;

        public Krawedz(int id, Stacja st1, Stacja st2, int koszt)
        {
            st1.AddKrawedz(this);
            st2.AddKrawedz(this);
            this.Id = id;
            this.Stacja1 = st1;
            this.Stacja2 = st2;
            this.Koszt = koszt;
        }
    }
}
