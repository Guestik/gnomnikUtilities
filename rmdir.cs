//rmdir command - Lukáš Anděl
﻿//This file is part of Gnomnik.

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

namespace rmdir
{
    class Program
    {
        static void Main(string[] args)
        {
            string patch = ""; //cesta k adresáři
            int counter = 0;
            bool v = false; //Parametr v
            foreach (string s in args)
            {
                if (s.Contains('-')) //Tento řetězec obsahuje parametry
                {
                    foreach (char z in s)
                    {
                        if (z == 'v') //Popisuje co se děje
                            v = true;
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
                        Console.WriteLine("rmdir: the syntax of the command is incorrect"); //Chybová hláška
                        System.Environment.Exit(0); //Ukončí aplikaci
                    }
                }
            }
            patch = patch.Trim(); //Odstranění mezer ma konci
            if (Directory.Exists(patch)) //Pokud adresář existuje
            {
                if (IsDirectoryEmpty(patch)) //Pokud je adresář prázdný
                {
                    System.IO.Directory.Delete(patch); //Smaže adresář
                    if (v == true) //Pokud chce uživatel informace
                        Console.WriteLine("'" + patch + "' deleted");
                }
                else
                    Console.WriteLine("rmdir: failed to remove '" + patch + "': Directory not empty"); //Chybová hláška
            }
            else
                Console.WriteLine("rmdir: directory does not exist"); //Chybová hláška
        }

        public static bool IsDirectoryEmpty(string path)
        {
            //Vrátí hodnotu true, pokud adresář neobsahuje žádné podadresáře/soubory
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}
