//grep utility - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace grep
{
    class Program
    {
        static void Main(string[] args)
        {
            //Zobrazí všechny řádky obsahující zadaný vzor (v případě parametru "v" neobsahující)
            bool n = false; //Lines - čísluje řádky
            bool v = false; //Vypíše řádky které zadaný vzor neobsahují
            bool c = false; //Místo normálního výstupu napíše počet řádků obsahující zadaný vzor
            string pattern = null;
            Random nahoda = new Random();
            List<string> patch = new List<string>(); //List cest (možných souborů) se kterými se bude pracovat
            for (int i = 0; i < args.Length; i++)
            {

                if (args[i].Contains('-')) //Tento řetězec obsahuje parametry
                {
                    foreach (char z in args[i])
                    {
                        if (z == 'n') //Lines - čísluje řádky
                            n = true;
                        else if (z == 'c') //Místo normálního výstupu napíše počet řádků obsahující zadaný vzor
                            c = true;
                        else if (z == 'v') //Vypíše řádky které zadaný vzor neobsahují
                            v = true;
                    }
                }
                else //Tento řetězec obsahuje cestu k souboru
                {
                    if (patch.ToArray().Length == 0 && String.IsNullOrEmpty(pattern)) //První je vzor, další jsou soubory
                        pattern = args[i];
                    else if (File.Exists(args[i])) //Např: grep pattern C:/.../a.txt
                        patch.Add(args[i]);
                    else if (File.Exists(Directory.GetCurrentDirectory() + "\\" + args[i])) //Relativní cesta k souboru
                        patch.Add(Directory.GetCurrentDirectory() + "\\" + args[i]);
                    else //Chybová hláška, ukončení utility - Řetězec neobsahuje parametry ani cestu, je to tedy neplatný argument
                    {
                        Console.WriteLine("grep: {0}: No such file", args[i]);
                        System.Environment.Exit(0);
                    }
                }
            }
            if (String.IsNullOrEmpty(pattern))
            {
                Console.WriteLine("grep: Pattern is empty"); //Chybová hláška, ukončení utility - Žádný vzorový řetězec
                System.Environment.Exit(0);
            }
            if (patch.ToArray().Length > 0) //Pokud byla mezi argumenty i cesta k souboru
            {
                //ČTE ZE SOUBORU
                for (int i = 0; i < patch.ToArray().Length; i++)
                {
                    string[] lines = System.IO.File.ReadAllLines(@patch[i]); //Přečte všechny řádky ze souboru
                    int counter = 0;
                    for (int x = 0; x < lines.Length; x++)
                    {
                        if (!lines[x].Contains(pattern) && v == true) //Při parametru "v" se vypisují řádky, které zadaný vzor neobsahují
                        {
                            if (n == true && c == false)
                                Console.Write("{0}: ", x + 1); //Při parametru "n" se vypíše číslo řádku
                            if (c == false)
                                Console.WriteLine(patch[i] + ": " + lines[x]);
                            counter++;
                        }
                        else if (lines[x].Contains(pattern) && v == false)
                        {
                            if (n == true && c == false)
                                Console.Write("{0}: ", x + 1); //Při parametru "n" se vypíše číslo řádku
                            if (c == false)
                                Console.WriteLine(patch[i] + ": " + lines[x]);
                            counter++;
                        }
                    }
                    if (c == true) //Při parametru "c" se vypíše pouze počet odpovídajících řádků
                        Console.WriteLine(patch[i] + ": " + counter);
                }
            }
            else //Pokud nebyla mezi argumenty cesta, pokusí se vzít input ze stdin
            {
                //ČTE ZE STDIN
                Console.SetIn(new StreamReader(Console.OpenStandardInput())); //Přečte přesměrovaný input
                int lineCounter = 0;
                int counter = 0;
                while (Console.In.Peek() != -1)
                {
                    string input = Console.In.ReadLine();
                    lineCounter++;
                    if (!String.IsNullOrEmpty(input)) //Pokud není řádek prázdný
                    {
                        if (!input.Contains(pattern) && v == true) //Při parametru "v" se vypisují řádky, které zadaný vzor neobsahují
                        {
                            if (n == true && c == false)
                                Console.Write("{0}: ", lineCounter); //Při parametru "n" se vypíše číslo řádku
                            if (c == false)
                                Console.WriteLine(input);
                            counter++;
                        }
                        else  if (input.Contains(pattern) && v == false)
                        {
                            if (n==true && c == false)
                                Console.Write("{0}: ", lineCounter); //Při parametru "n" se vypíše číslo řádku
                            if (c == false)
                                Console.WriteLine(input);
                            counter++;
                        }
                    }
                }
                if (c == true) //Při parametru "c" se vypíše pouze číslovka, kolik je nalezených správných výsledků
                    Console.WriteLine("{0}", counter);
            }
        }
    }
}
