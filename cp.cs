//cp command - Lukáš Anděl
//This file is part of Gnomnik.

//    Gnomnik is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    Gnomnik is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cp
{
    class Program
    {
        static void Main(string[] args)
        {
            //-r zkopíruje i adresáře
            //v - píše co se aktuálně kopíruje
            bool r = false; //Parametr -r
            bool v = false; //Parametr -v
            string source = ""; //Řetězec zdroje (odkud se kopíruje)
            string target = ""; //Řetězec cíle (kam se kopíruje)
            int counter = 0;
            foreach (string s in args)
            {
                if (s.Contains('-')) //Tento řetězec obsahuje parametry
                {
                    foreach (char z in s)
                    {
                        if (z == 'r') //Kopíruje i adresáře
                            r = true;
                        else if (z == 'v') //Popisuje co se děje
                            v = true;
                    }
                }
                else if (s.Contains('\\')) //Pokud obsahuje zpětné lomítko, následuje mezera, tudíž i příští řetězec patří do toho samého source/target
                {
                    counter++;
                    if (counter == 1) //source
                        source += s.Remove(s.Length - 1) + " "; //Odstraní poslední znak ve stringu, kterým je zpětné lomítko;
                    else if (counter == 2) //target
                        target += s.Remove(s.Length - 1) + " "; //Odstraní poslední znak ve stringu, kterým je zpětné lomítko;
                    counter--;
                }
                else //Tento řetězec obsahuje source/target
                {
                    counter++;
                    if (counter == 1) //source
                        source += s;
                    else if (counter == 2) //target
                        target += s;
                    else //Řetězec obsahuje neplatné argumenty, utilita napíše chybovou hlášku a ukončí se
                    {
                        Console.WriteLine("cp: the syntax of the command is incorrect");
                        System.Environment.Exit(0);
                    }
                }
            }
            target = target.Trim(); //Odstranění mezer ma konci
            source = source.Trim(); //Odstranění mezer ma konci
            try
             {
                System.IO.File.Copy(source, target); //Kopírování souboru
                if (v == true) //Pokud uživatel chce informace o kopírování
                    Console.WriteLine("'" + source + "' -> '" + target + "'");
            }
            catch
            {
                if (r == true)
                {
                    if (System.IO.Directory.Exists(target)) //Pokud cílový adresář existuje, nelze kopírovat
                        Console.WriteLine("cp: target directory already exists");
                    else
                    {
                        try
                        {
                            DirectoryInfo dirInfoSource = new DirectoryInfo(source);
                            DirectoryInfo dirInfoTarget = new DirectoryInfo(target);
                            System.IO.Directory.CreateDirectory(target); //Vytvoří adresář do kterého bude kopírovat
                            copyDirRecursively(dirInfoSource, dirInfoTarget, v); //Zalování metody pro kopírování adresářů
                        }
                        catch
                        {
                            Console.WriteLine("cp: cannot copy"); //Chybová hláška
                        }
                    }
                }
                else
                    Console.WriteLine("cp: cannot copy"); //Chybová hláška
            }
        }

        public static void copyDirRecursively(DirectoryInfo source, DirectoryInfo target, bool v)
        {
            //Zkopíruje soubory uvnitř adresáře a pro každý svůj podadresář spustí tuto metudu znova.
            //Je zde tak vytvořena smyčka, která skončí až se zkopírují všechny soubory ze všech podadresářů
            try
            {
                foreach (DirectoryInfo dir in source.GetDirectories()) //Pro každý podadresář spuspí tuto metodu znovu
                    copyDirRecursively(dir, target.CreateSubdirectory(dir.Name), v);
                foreach (FileInfo file in source.GetFiles()) //Každý soubor v tomto adresáři se zkopíruje
                {
                    if (!Directory.Exists(target.FullName)) //Nejdříve adresář v daném místě založí, aby se měli soubory kam kopírovat
                        System.IO.Directory.CreateDirectory(target.FullName);
                    file.CopyTo(Path.Combine(target.FullName, file.Name));
                    if (v == true) //Pokud uživatel chce informace o kopírování
                        Console.WriteLine("'" + file.Name + "' -> '" + Path.Combine(target.FullName, file.Name) + "'");
                }
            }
            catch
            {
                Console.WriteLine("cp: error while copying directory"); //Například při vytažení USB nebo jakékoli jiné nehodě
            }
        }
    }
}
