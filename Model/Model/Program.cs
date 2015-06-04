﻿using System;
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
            int n = 10; // liczba przystanków
            int t = 5; // liczba interwalów czasowych.
            int p = 4; // liczba petli.
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

            Linia T1 = new Linia(m, r);
            Console.WriteLine("\nWylosowana linia:");
            Console.WriteLine(T1);
            Console.ReadKey();
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