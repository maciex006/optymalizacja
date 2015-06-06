using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "D:\\konfig.txt"; // ścieżka do pliku z modelem.
            int n = 50; // liczba przystanków
            int t = 1; // liczba interwalów czasowych.
            int p = 14; // liczba petli.
            int l = 2; // liczba linii.
            Random r = new Random();
            Model m = new Model(r);
            m.Generuj(n, t, p);

            string command = "";      
            while(command != "exit")
            {
                command = Console.ReadLine();
                int[] param;
                string txtCommand;

                if (GetCommand(command, out txtCommand, out param))
                {
                    switch (txtCommand)
                    {
                        case "load":
                            if (param == null || param.Count() == 0)
                            {
                                m = new Model(r);
                                m.Wczytaj(filePath);
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "inc":
                            if (param != null && param.Count() == 1)
                            {
                                WriteIncydentneKrawedzie(m, param[0]);
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "neig":
                            if (param != null && param.Count() == 1)
                            {
                                WriteSasiedzi(m, param[0]);
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "term":
                            if (param == null || param.Count() == 0)
                            {
                                WritePetle(m);
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "traf":
                            if (param != null && param.Count() == 1)
                            {
                                WriteRuch(m, param[0]);
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "newline":
                            if (param == null || param.Count() == 0)
                            {
                                Linia T = LosujLinie(m, r);
                                WriteLinia(T);
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "cntk":
                            if (param != null && param.Count() == 1)
                            {
                                Console.WriteLine("Liczba stacji o krawędziach " + param[0] + " = " + m.CountStacje(param[0]).ToString() + "\n");
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "cntp":
                            if (param != null && param.Count() == 1)
                            {
                                Console.WriteLine("Liczba pętli o krawędziach " + param[0] + " = " + m.CountPetle(param[0]).ToString() + "\n");
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "newweb":
                            if (param == null || param.Count() == 0)
                            {
                                Siec s = new Siec(1, m, r, l);
                                Console.WriteLine(s);
                                // Wyk kraw.
                                Dictionary<Krawedz, List<int>> wyk = s.GetWykorzystaneKrawedzie();
                                Console.WriteLine();
                                Console.WriteLine(string.Join("\n", wyk.Select(x => x.Key.ToString() + "=" +
                                    "{" + string.Join(",", x.Value) + "}").ToArray()));
                                List<Polaczenie> pol = s.GetWyznaczonePolaczenia();
                                Console.WriteLine();
                                Console.WriteLine(string.Join("\n", pol.Select(x => "[" + x.Id[0] + "," + x.Id[1] + "] = " +
                                    "{" + string.Join(",", x.Krawedzie.Select(y => y.Krawedz.StringFormatDlaPolaczen)) + "}").ToArray()));
                                Console.WriteLine();
                                Console.WriteLine(s.PrintKoszt());
                                Console.WriteLine();
                                Console.WriteLine("Wykorzystane przystanki: " + s.WykorzystanePrzystanki + "/" + m.GetStacje().Count() + "\n");
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "run":
                            if (param == null || param.Count() == 0)
                            {
                                AlgorytmEwolucyjny alg = new AlgorytmEwolucyjny(m, r, l, 10);
                                alg.Run();
                            }
                            else
                            {
                                Console.WriteLine("Błąd skladni");
                            }
                            break;

                        case "exit":
                            Console.WriteLine("Wychodzę");
                            break;

                        default:
                            Console.WriteLine("Nieznana komenda");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Błąd skladni");
                }   
            }  
        }

        private static Linia LosujLinie(Model m, Random r)
        {
            return new Linia(0, m, r);
        }

        private static void WriteLinia(Linia T)
        {
            Console.WriteLine("\nWylosowana linia:");
            Console.WriteLine(T.ToStringShortFormat());
            Console.WriteLine();
        }

        private static void WritePetle(Model m)
        {
            List<Stacja> petle = m.GetPetle();
            Console.WriteLine("\nPetle w modelu m:");
            foreach (Stacja pe in petle)
            {
                Console.WriteLine(pe);
            }
            Console.WriteLine();
        }

        private static void WriteRuch(Model m, int idStacji)
        {
            Dictionary<Stacja, MacierzRuchu> ruch = m.GetRuch(idStacji);
            Console.WriteLine("\nRuch na stacji " + idStacji + " :");
            Console.WriteLine("{" + string.Join(",", ruch.Select(x => x.Key.ToString() + "=" + x.Value.ToString()).ToArray()) + "}");
            Console.WriteLine();
        }

        private static void WriteSasiedzi(Model m, int idStacji)
        {
            List<Stacja> st = m.GetSasiednieStacje(idStacji);
            Console.WriteLine("\nStacje sasiednie dla stacji " + idStacji + " :");
            foreach (Stacja s in st)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        private static void WriteIncydentneKrawedzie(Model m, int idStacji)
        {
            List<Krawedz> kr = m.GetIncydentneKrawedzie(idStacji);
            Console.WriteLine("\nKrawedzie incydentne dla stacji " + idStacji + " :");
            foreach (Krawedz k in kr)
            {
                Console.WriteLine(k);
            }
            Console.WriteLine();
        }

        private static bool GetCommand(string command, out string txtCommand, out int[] param)
        {
            string[] buf = command.Split(new char[] { '(' }, 2);
            txtCommand = buf[0];

            if (buf.Count() > 1)
            {
                string[] parameters = buf[1].Split(new char[] { ',' });
                parameters[parameters.Count() - 1] = parameters[parameters.Count() - 1].Trim(new char[] { ')' });
                param = new int[parameters.Count()];
                for (int i = 0; i < parameters.Count(); i++)
                {
                    int parbuf;
                    Int32.TryParse(parameters[i], out parbuf);
                    if (parbuf != null)
                    {
                        param[i] = parbuf;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                param = null;
            }
            
            return true;
        }
    }
}
