using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Siec
    {
        private List<Linia> Linie = new List<Linia>();
        private Model Model;
        private Random Random;
        private int LiczbaLinii;
        private Dictionary<Krawedz, List<int>> WykorzystaneKrawedzie = new Dictionary<Krawedz, List<int>>();
        private List<Polaczenie> WyznaczonePolaczenia = new List<Polaczenie>();

        public Siec(Model m, Random r, int liczbaLinii)
        {
            this.Model = m;
            this.Random = r;
            this.LiczbaLinii = liczbaLinii;
            GenerujSiec();
        }

        private void GenerujSiec()
        {
            for (int i = 0; i < LiczbaLinii; i++)
            {
                Linie.Add(new Linia(i, Model, Random));
                // Tworzymy liste wykorzystanych krawedzi potrzebna da wyznaczania najkrotszych drog.
            }

            WyznaczWykorzsytaneKrawedzie();
            WyznaczPolaczenia();
        }

        private void WyznaczWykorzsytaneKrawedzie()
        {
            foreach (Linia l in Linie)
            {
                foreach (Krawedz k in l.GetKrawedzie())
                {
                    if (!WykorzystaneKrawedzie.ContainsKey(k))
                    {
                        List<int> linieNaKrawedzi = new List<int>();
                        linieNaKrawedzi.Add(l.Id);
                        WykorzystaneKrawedzie.Add(k, linieNaKrawedzi);
                    }
                    else
                    {
                        List<int> linieNaKrawedzi = WykorzystaneKrawedzie[k];
                        if (!linieNaKrawedzi.Contains(l.Id))
                        {
                            linieNaKrawedzi.Add(l.Id);
                        }
                    }
                }
            }  
        }

        private void WyznaczPolaczenia()
        {
            List<Stacja> stacje = Model.GetStacje();
            int j = 0;
            foreach (Stacja s in stacje)
            {  
                List<int> st = new List<int>();
                for (int i = 0; i < stacje.Count(); i++)
                {
                    if (i != s.Id)
                    {
                        st.Add(i);
                    }
                }

                List<BufforData> bufInitData = new List<BufforData>();
                BufforData b; 
                b.Sasiad = s;
                b.Krawedzie = new List<KrawedzSieci>();
                bufInitData.Add(b);

                List<BufforData> bufData = WyznaczKrawedzie(s.Id, bufInitData[0], st);

                while (bufData.Count() != 0)
                {
                    List<BufforData> newBufData = new List<BufforData>();
                    foreach (BufforData bd in bufData)
                    {
                        newBufData.AddRange(WyznaczKrawedzie(s.Id, bd, st));
                    }

                    bufData = newBufData.Select(item => item).ToList();
                }

                j++;
            }
        }

        public struct BufforData
        {
            public List<KrawedzSieci> Krawedzie;
            public Stacja Sasiad;
        }

        private List<BufforData> WyznaczKrawedzie(int idSPocz, BufforData bufInitData, List<int> st)
        {
            List<KrawedzSieci> krawedzie = bufInitData.Krawedzie;
            Stacja s = bufInitData.Sasiad;
            IEnumerable<Krawedz> inc = s.GetIncydentneKrawedzie()
                .Where(x => WykorzystaneKrawedzie.ContainsKey(x))
                .Where(x => krawedzie.Count() > 0 ? krawedzie[krawedzie.Count() - 1].Krawedz != x : true);

            List<BufforData> bufforData = new List<BufforData>();

            foreach (Krawedz k in inc)
            {
                List<KrawedzSieci> cloneKrawedzie = krawedzie.Select(item => item).ToList();
                KrawedzSieci krawedz = new KrawedzSieci(WykorzystaneKrawedzie[k], k);
                Stacja sasiad = s.Id == k.Stacja1.Id ? k.Stacja2 : k.Stacja1;
                if (st.Contains(sasiad.Id))
                {
                    int[] key = new int[2] { idSPocz, sasiad.Id };
                    cloneKrawedzie.Add(krawedz);  
                    st.Remove(sasiad.Id);
                    Polaczenie p = new Polaczenie(key, cloneKrawedzie);
                    WyznaczonePolaczenia.Add(p);

                    BufforData buf;
                    buf.Krawedzie = cloneKrawedzie;
                    buf.Sasiad = sasiad;
                    bufforData.Add(buf);
                }
            }

            return bufforData;
        }

        public override string ToString()
        {
            return string.Join("\n", Linie.Select(x => x.ToStringShortFormat()));
        }

        public Dictionary<Krawedz, List<int>> GetWykorzystaneKrawedzie()
        {
            return WykorzystaneKrawedzie;
        }

        public List<Polaczenie> GetWyznaczonePolaczenia()
        {
            return WyznaczonePolaczenia;
        }
    }
}
