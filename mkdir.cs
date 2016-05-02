//mkdir command - Lukáš Anděl
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

namespace mkdir
{
    class Program
    {
        static void Main(string[] args)
        {
            string patchWithGap = unGap(args.Length, args); //Bližší popis metody v jejím kódu

            if (System.IO.Directory.Exists(Directory.GetCurrentDirectory() + "/" + patchWithGap)) //Relativní cesta
                Console.WriteLine("mkdir: cannot create directory '" + Directory.GetCurrentDirectory() + "/" + patchWithGap + "': Directory exist");
            else
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/" + patchWithGap); //Vytvoří adresář se zadanou relativní cestou
                }
                catch //Pokud se jedná o absolutní cestu, program spadne do catch
                {
                    try
                    {
                        if (System.IO.Directory.Exists(patchWithGap)) //Pokud takový adresář již existuje, nelze tudíž vytvořit. Namísto toho se zobrazí text oznamující tuto skutečnost
                            Console.WriteLine("mkdir: cannot create directory '" + patchWithGap + "': Directory already exists");
                        else
                            System.IO.Directory.CreateDirectory(patchWithGap); //Vytvoří adresář se zadanou absolutní cestou
                    }
                    catch
                    {
                        Console.WriteLine("mkdir: cannot create directory");
                    }
                }
            }
        }

        public static string unGap(int countOfParts, string[] commandParts)
        {
            //Pokud obsahuje zadaná cesta k adresáři mezeru nebo zpětné lomítko, musí se odstranit tak, aby dávali (pokud možno) platý adresář
            //Například pole obsahující prvky "[0]cd [1]C:/Program\ [2]Files" převede na výstupní řetězec tvaru "C:/Program Files"
            string patchWithGap = ""; //Cesta s mezerou
            for (int i = 0; i < countOfParts; i++) //Do řetězce přijdou všechny prvky z pole kromě prvního, kterým je příkaz "cd"
            {
                patchWithGap += commandParts[i] + " ";
            }
            string[] splitGap = patchWithGap.Trim().Split(new Char[] { '\\' }); //Rozdělení podle zpětných lomítek
            patchWithGap = "";
            foreach (string x in splitGap) //Foreach podle zpětných lomítek
            {
                patchWithGap += x;
            }
            return patchWithGap;
        }
    }
}
