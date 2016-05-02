//mv command - Lukáš Anděl
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

namespace mv
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = ""; //Řetězec zdroje (odkud se kopíruje)
            string target = ""; //Řetězec cíle (kam se kopíruje)
            int counter = 0;
            foreach (string s in args)
            {
                counter++;

                if (s.Contains('\\')) //Pokud obsahuje zpětné lomítko, následuje mezera, tudíž i příští řetězec patří do toho samého source/target
                {
                    if (counter == 1) //source
                        source += s.Remove(s.Length - 1) + " "; //Odstraní poslední znak ve stringu, kterým je zpětné lomítko;
                    else if (counter == 2) //target
                        target += s.Remove(s.Length - 1) + " "; //Odstraní poslední znak ve stringu, kterým je zpětné lomítko;
                    counter--;
                }
                else
                {
                    if (counter == 1) //source
                        source += s;
                    else if (counter == 2) //target
                        target += s;
                    else //Řetězec obsahuje neplatné argumenty, utilita napíše chybovou hlášku a ukončí se
                    {
                        Console.WriteLine("mv: the syntax of the command is incorrect");
                        System.Environment.Exit(0);
                    }
                }
            }
            target = target.Trim(); //Odstranění mezer ma konci
            source = source.Trim(); //Odstranění mezer ma konci
            try
            {
                if (File.Exists(target)) //Pokud již cíl existuje
                    Console.WriteLine("mv: target file already exists"); //Chybová hláška
                else
                    System.IO.File.Move(source, target); //Přesunutí souboru
            }
            catch
            {
                try
                {
                    if (Directory.Exists(target)) //Pokud již cíl existuje
                        Console.WriteLine("mv: target directory already exists"); //Chybová hláška
                    else
                        System.IO.Directory.Move(source, target); //Přesunutí adresáře
                }
                catch
                {
                    Console.WriteLine("mv: cannot move"); //Chybová hláška
                }
            }
        }
    }
}
