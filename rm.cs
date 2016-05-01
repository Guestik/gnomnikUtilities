//rm command - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rm
{
    class Program
    {
        static void Main(string[] args)
        {
            string patch = ""; //cesta k adresáři
            int counter = 0;
            bool v = false; //Parametr v
            bool r = false; //Parametr r
            foreach (string s in args)
            {
                if (s.Contains('-')) //Tento řetězec obsahuje parametry
                {
                    foreach (char z in s)
                    {
                        if (z == 'v') //Popisuje co se děje
                            v = true;
                        else if (z == 'r') //Maže i adresáře
                            r = true;
                    }
                }
                else if (s.Contains('\\')) //Pokud obsahuje zpětné lomítko, následuje mezera, tudíž i příští řetězec patří do patch (cesta k adresáři)
                {
                    counter++;
                    if (counter == 1)
                        patch += s.Remove(s.Length - 1) + " "; //Odstraní poslední znak ve stringu, kterým je zpětné lomítko;
                    counter--;
                }
                else
                {
                    counter++;
                    if (counter == 1)
                        patch += s;
                    else //Řetězec obsahuje neplatné argumenty, utilita napíše chybovou hlášku a ukončí se
                    {
                        Console.WriteLine("rm: the syntax of the command is incorrect"); //Chybová hláška
                        System.Environment.Exit(0); //Ukončí aplikaci
                    }
                }
            }
            patch = patch.Trim(); //Odstranění mezer ma konci
            try
            {
                if (File.Exists(patch)) //Pokud takový soubor existuje
                {
                    System.IO.File.Delete(patch); //Smaže soubor
                    if (v == true) //Pokud uživatel chce zobrazovat informace
                        Console.WriteLine("'" + patch + "' deleted");
                }
                else
                {
                    if (r == true) //Parametr 'r' značí mazání adresářů
                    {
                        if (Directory.Exists(patch)) //Smaže adresář
                        {
                            System.IO.Directory.Delete(patch, true);
                            if (v == true) //Pokud uživatel chce zobrazovat informace
                                Console.WriteLine("'" + patch + "' deleted");
                        }
                        else
                        {
                            Console.WriteLine("rm: file does not exist"); //Chybová hláška
                        }
                    }
                    else
                        Console.WriteLine("rm: cannot remove"); //Chybová hláška
                }
            }
            catch
            {
                Console.WriteLine("rm: cannot remove"); //Chybová hláška
            }
        }
    }
}
