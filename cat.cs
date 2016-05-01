//cat utility - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cat
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) //Pokud nejsou žádné argumenty
            {
                Console.SetIn(new StreamReader(Console.OpenStandardInput())); //Přečte přesměrovaný input
                while (Console.In.Peek() != -1)
                {
                    string input = Console.In.ReadLine();
                    Console.WriteLine(input);
                }
                string s;
                while ((s = Console.ReadLine()) != null) //Input od uživatele (live)
                    Console.WriteLine(s);
            }
            else //Pokud je program spuštěn s argumenty
            {
                for (int i = 0; i < args.Length; i++) //Cyklus proběhne tolikrát, kolik je argumentů
                {
                    if (File.Exists(args[0]) == false) //Pokud soubor neexistuje
                        Console.WriteLine("cp: {0}: No such file", args[i]); //Chybová hláška
                    else //Pokud soubor existuje
                        Console.Write(File.ReadAllText(args[0])); //Soubor se celý vypíše
                }
            }
        }
    }
}
