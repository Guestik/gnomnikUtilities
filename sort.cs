//sort utility - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sort
{
    class Program
    {
        static void Main(string[] args)
        {
            bool R = false; //Random
            bool r = false; //Reverse
            bool v = false; //Verbose - Řádky budou očíslovány
            Random nahoda = new Random();
            List<string> patch = new List<string>(); //List cest pro seřazení
            List<string> list = new List<string>(); //List řádků k seřazení
            foreach (string s in args)
            {
                if (s.Contains('-')) //Tento řetězec obsahuje parametry
                {
                    foreach (char z in s)
                    {
                        if (z == 'R') //Náhodné seřazení
                            R = true;
                        else if (z == 'r') //Seřazení podle abecedy obráceně
                            r = true;
                        else if (z == 'c') //Řádky budou číslovány
                            v = true;
                    }
                }
                else //Tento řetězec obsahuje cestu k souboru
                {
                    if (File.Exists(s)) //Např: sort C:/.../a.txt
                        patch.Add(s);
                    else if (File.Exists(Directory.GetCurrentDirectory() + "\\" + s)) //Relativní cesta k souboru
                        patch.Add(Directory.GetCurrentDirectory() + "\\" + s);
                    else //Chybová hláška, ukončení utility - Řetězec neobsahuje parametry ani cestu, je to tedy neplatný argument
                    {
                        Console.WriteLine("sort: {0}: No such file", s);
                        System.Environment.Exit(0);
                    }
                }
            }
            if (patch.ToArray().Length > 0) //Pokud byla mezi argumenty i cesta
            {
                //ČTE ZE SOUBORU
                for (int i = 0; i < patch.ToArray().Length; i++)
                {
                    string[] lines = System.IO.File.ReadAllLines(@patch[i]); //Přečte všechny řádky ze souboru
                    foreach (string line in lines)
                    {
                            list.Add(line);
                    }
                }
            }
            else //Pokud nebyla mezi argumenty cesta, pokusí se vzít input ze stdin
            {
                //ČTE ZE STDIN
                Console.SetIn(new StreamReader(Console.OpenStandardInput())); //Přečte přesměrovaný input
                while (Console.In.Peek() != -1)
                {
                    string input = Console.In.ReadLine();
                    if (!String.IsNullOrEmpty(input)) //Pokud není řádek prázdný
                        list.Add(input); //Přidá se do listu
                }
            }
            string[] pole = list.ToArray();
            Array.Sort(pole);
            if (r == true) //Seřazení podle abecedy opačně
                Array.Reverse(pole);
            if (R == true) //Náhodné řazení
            {
                List<int> vylosovane = new List<int>();
                List<string> docasny = new List<string>();
                int a = 0;
                for (int i = 0; i < pole.Length; i++)
                {
                    a = nahoda.Next(0, pole.Length);
                    if (vylosovane.Contains(a))
                        i--;
                    else
                    {
                        vylosovane.Add(a);
                        docasny.Add(pole[a]);
                    }
                }
                pole = docasny.ToArray();
            }
            for (int i = 0; i < pole.Length; i++) //Vypsání seřazených řádků
            {
                if (v == true)
                    Console.Write("{0} ",i+1);
                Console.WriteLine(pole[i]);
            }
        }
    }
}
