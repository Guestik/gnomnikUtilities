//head utility - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace head
{
    class Program
    {
        static void Main(string[] args)
        {
            bool n = false; //Lines - Počet řádků
            bool v = false; //Verbose - zobrazuje i čísla řádků
            Random nahoda = new Random();
            int nN = 0; //Počet řádků pro vypsání
            List<string> patch = new List<string>(); //List cest (možných souborů) pro vypsání
            List<string> list = new List<string>(); //List řádků k vypsání
            bool nasledujePocetRadku = false;
            foreach (string s in args)
            {
                if (nasledujePocetRadku == true) //V případě "head -n 6"
                {
                    nasledujePocetRadku = false;
                    try
                    {
                        nN = int.Parse(s);
                    }
                    catch
                    {
                        Console.WriteLine("head: {0}: invalid number of lines", s); //Chybová hláška - špatně zadaný počet řádků k vypsání
                        System.Environment.Exit(0);
                    }
                }
                else if (s.Contains('-')) //Tento řetězec obsahuje parametry
                {
                    nasledujePocetRadku = false;
                    string pocetNaVypsani = "";
                    foreach (char z in s)
                    {
                        if (nasledujePocetRadku == true) //V případě "head -n5"
                        {
                            pocetNaVypsani += z;
                            nasledujePocetRadku = false;
                        }
                        else
                        {
                            if (z == 'n') //Lines - počet řádků
                            {
                                n = true;
                                nasledujePocetRadku = true;
                            }
                            else if (z == 'v') //Verbose - zobrazuje i čísla řádků
                                v = true;
                        }
                    }
                    if (!String.IsNullOrEmpty(pocetNaVypsani))
                    {
                        try
                        {
                            nN = int.Parse(pocetNaVypsani);
                        }
                        catch
                        {
                            Console.WriteLine("head: {0}: invalid number of lines", pocetNaVypsani); //Chybová hláška - špatně zadaný počet řádků k vypsání
                            System.Environment.Exit(0);
                        }
                    }
                }
                else //Tento řetězec obsahuje cestu k souboru
                {
                    if (File.Exists(s)) //Např: head C:/.../a.txt
                        patch.Add(s);
                    else if (File.Exists(Directory.GetCurrentDirectory() + "\\" + s)) //Relativní cesta k souboru
                        patch.Add(Directory.GetCurrentDirectory() + "\\" + s);
                    else //Chybová hláška, ukončení utility - Řetězec neobsahuje parametry ani cestu, je to tedy neplatný argument
                    {
                        Console.WriteLine("head: {0}: No such file", s);
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
            if (n != true)
                nN = pole.Length;
            for (int i = 0; i < nN; i++) //Vypsání řádků
            {
                if (v == true) //Parametr "v" zobrazí i čísla řádků
                    Console.Write("{0} ", i+1);
                Console.WriteLine(pole[i]);
            }

        }
    }
}
