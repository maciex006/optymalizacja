using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Siec
    {
        public int Id;
        private List<Linia> Linie = new List<Linia>();
        private Model Model;
        private Random Random;
        private int LiczbaLinii;
        private Dictionary<Krawedz, List<int>> WykorzystaneKrawedzie = new Dictionary<Krawedz, List<int>>();
        private List<Polaczenie> WyznaczonePolaczenia = new List<Polaczenie>();
        public double Koszt { get; set; }
        //Tymczasowe do debugu
        public int WykorzystanePrzystanki { get; set; }

        public Siec(int Id, Model m, Random r, int liczbaLinii, bool generuj = true)
        {
            this.Id = Id;
            WykorzystanePrzystanki = 1;
            this.Model = m;
            this.Random = r;
            this.LiczbaLinii = liczbaLinii;
            if (generuj)
            {
                GenerujSiec();
            }
        }

        public int LosujPrzeciecie()
        {
            return Random.Next(LiczbaLinii - 2) + 1;
        }

        public void Mutuj()
        {
            int rand = Random.Next(LiczbaLinii);
            Linie.Remove(Linie[rand]);
            Linie.Add(new Linia(rand, Model, Random));
        }

        public void PrzeliczKoszt()
        {
            WyznaczWykorzsytaneKrawedzie();
            WyznaczPolaczenia();
            WyznaczFunkcjeKosztu();
        }

        // Operator krzyżowania
        public static Siec operator +(Siec s1, Siec s2)
        {
            if (s1.LiczbaLinii == s2.LiczbaLinii)
            {
                int przeciecie = s1.LosujPrzeciecie();
                Siec s = new Siec(0, s1.Model, s1.Random, s1.LiczbaLinii, generuj: false);

                s.Linie = new List<Linia>();
                s.Linie.AddRange(s1.Linie.OrderByDescending(x => x.Koszt).Take(przeciecie));
                s.Linie.AddRange(s2.Linie.OrderByDescending(x => x.Koszt).Skip(przeciecie).Take(s2.Linie.Count() - przeciecie));
                s.WyznaczWykorzsytaneKrawedzie();
                s.WyznaczPolaczenia();
                s.WyznaczFunkcjeKosztu();
                return s;
            }
            else
            {
                return null;
            }           
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
            WyznaczFunkcjeKosztu();
        }

        public string PrintKoszt()
        {
            return "{" + string.Join("\n", Linie.Select(x => "Koszt linii " + x.Id + " = " + x.Koszt)) + "}"
                + "\n Koszt całkowity sieci = " + Koszt;
        }

        public void WyznaczFunkcjeKosztu()
        {
            if(WyznaczonePolaczenia.Count() == 0)
            {
                WyznaczWykorzsytaneKrawedzie();
                WyznaczPolaczenia();
            }

            List<Stacja> stacje = Model.GetStacje();

            foreach (Linia l in Linie)
            {
                l.Koszt = 0;
            }

            // Tymczasowe
            bool temp = true;
            // Tymczasowe

            foreach (Stacja s in stacje)
            {
                //.Where(x => !x.Value.CheckIfNull()).ToDictionary(x => x.Key, x => x.Value)
                Dictionary<Stacja, MacierzRuchu> ruch = s.GetRuch();
                foreach (Stacja cel in stacje)
                {
                    if (s.Id != cel.Id)
                    {
                        MacierzRuchu r = ruch[cel];

                        if (!(r.CheckIfNull()))
                        {
                            int[] key = (s.Id < cel.Id ? new int[2] { s.Id, cel.Id } : new int[2] { cel.Id, s.Id });
                            if (WyznaczonePolaczenia.Any(x => x.Id[0] == key[0] && x.Id[1] == key[1]))
                            {

                                // Tymczasowe
                                if(temp) WykorzystanePrzystanki++;
                                // Tymczasowe

                                Polaczenie p = WyznaczonePolaczenia.First(x => x.Id[0] == key[0] && x.Id[1] == key[1]);
                                //Implementacja dla t = 1!
                                double liczbaPasazerow = r.GetLiczbaPasazerow();
                                foreach (KrawedzSieci k in p.Krawedzie)
                                {
                                    liczbaPasazerow = liczbaPasazerow / k.IdLinii.Count();
                                    double x = (liczbaPasazerow / k.Krawedz.GetKoszt() - 1);

                                    foreach (int i in k.IdLinii)
                                    {
                                        Linie[i].Koszt = Linie[i].Koszt + x;
                                    }
                                }
                            }         
                        }
                    }
                }

                // Tymczasowe
                temp = false;
                // Tymczasowe
            }

            Koszt = Linie.Sum(x => x.Koszt);
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
