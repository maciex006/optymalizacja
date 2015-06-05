using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class AlgorytmEwolucyjny
    {
        Model Model;
        Random Random;
        int LiczbaLinii;
        int LiczebnoscPop;
        List<Siec> Populacja = new List<Siec>();

        public AlgorytmEwolucyjny(Model m, Random r, int lLinii, int liczebnoscPopulacji)
        {
            this.Model = m;
            this.LiczbaLinii = lLinii;
            this.Random = r;
            this.LiczebnoscPop = liczebnoscPopulacji;
        }

        public void Run()
        {
            int COUNTER_INIT_VALUE = 150;
            InitPopulacja();
            Populacja = Populacja.OrderByDescending(x => x.Koszt).ToList();

            int counter = COUNTER_INIT_VALUE;
            Siec najlepsza = Populacja[0];
            double maxValue = Populacja[0].Koszt;

            while(counter > 0)
            {
                if (maxValue < Populacja[0].Koszt)
                {
                    counter = COUNTER_INIT_VALUE;
                    maxValue = Populacja[0].Koszt;
                    najlepsza = Populacja[0];
                }
                else 
                {
                    counter--;
                }

                Sequence();
                Populacja = Populacja.OrderByDescending(x => x.Koszt).ToList();
            }

            Console.WriteLine("Najlepsza siec: \n" + najlepsza + ", koszt = " + najlepsza.Koszt);
        }

        public string PrintPopulacja()
        {
            return "{" + string.Join("\n", Populacja.Select(x => "Osobnik " + x.Id + " ; Koszt = " + x.Koszt)) + "}";
        }

        private void InitPopulacja()
        {
            for (int i = 0; i < LiczebnoscPop; i++)
            {
                Populacja.Add(new Siec(i, Model, Random, LiczbaLinii));
            }
        }

        private bool Sequence()
        {
            using (StreamWriter sw = new StreamWriter("D:\\wykres.txt", true))
            {
                sw.WriteLine(Populacja[0].Koszt);
                sw.Close();
            }

            Console.WriteLine(PrintPopulacja());

            //Console.ReadKey();
            List<Siec> temp = Selekcja();
            //Console.WriteLine( "{" + string.Join("\n", temp.OrderByDescending(x => x.Koszt).Select(x => "Osobnik " + x.Id + " ; Koszt = " + x.Koszt)) + "}");
            List<Siec> nowaPopulacja = Krzyzowanie(temp);
            if (nowaPopulacja != null)
            {
                Mutacja(nowaPopulacja);
                Populacja = nowaPopulacja;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Mutacja(List<Siec> populacja)
        {
            int prawdMutacji = 25;
            int lMutacji = 0;

            foreach (Siec s in populacja)
            {
                if (NextWithProbability(prawdMutacji))
                {
                    lMutacji++;
                    s.Mutuj();
                    s.PrzeliczKoszt();
                }
            }

            Console.WriteLine("Liczba mutacji: " + lMutacji);
        }

        private List<Siec> Krzyzowanie(List<Siec> populacja)
        {
            List<Siec> nowaPopulacja = new List<Siec>();
            int i = 0;

            foreach (Siec s in populacja)
            {
                List<Siec> pula = populacja.Where(x => x.Id != s.Id).ToList();
                if (pula.Count() == 0)
                {
                    return null;
                }

                int ran = Random.Next(pula.Count() - 1);
                Siec newS = pula[ran] + s;
                newS.Id = i++;
                nowaPopulacja.Add(newS);
            }

            return nowaPopulacja;
        }

        private List<Siec> Selekcja()
        {
            //Populacja = Populacja.OrderByDescending(x => x.Koszt).ToList();
            List<double> prawd = new List<double>();
            
            double kosztNajgorszego = Populacja[Populacja.Count() - 1].Koszt;
            double sumaKosztu = 0;
            foreach(Siec s in Populacja)
            {
                double wyskalowanyKoszt = kosztNajgorszego < 0 ? s.Koszt - kosztNajgorszego : s.Koszt;
                sumaKosztu = sumaKosztu + wyskalowanyKoszt;
                prawd.Add(wyskalowanyKoszt);
            }

            for (int i = 0; i < prawd.Count(); i++)
            {
                prawd[i] = (prawd[i] / sumaKosztu) * 1000;
            }

            List<Siec> selectedPop = new List<Siec>();

            for (int k = 0; k < LiczebnoscPop; k++)
            {
                int wylosWart = Random.Next(975);
                selectedPop.Add(Populacja[LosujKopie(prawd, wylosWart)]);
            }

            return selectedPop;
        }

        private int LosujKopie(List<double> prawd, int wylosWart)
        {
            double sump = 0;
            for(int i = 0; i < prawd.Count(); i++)
            {
                if (wylosWart <= prawd[i] + sump)
                {
                    return i;
                }
                else
                {
                    sump = sump + prawd[i];
                }
            }

            return -1;
        }

        private bool NextWithProbability(int prawd)
        {
            int x = Random.Next(1000);
            return (x <= prawd);
        }
    }
}
