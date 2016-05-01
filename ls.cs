//ls command - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Globalization;

namespace ls
{
    class Program
    {
        static void Main(string[] args)
        {
            bool a = false; //Parametr -a
            bool l = false; //Parametr -l
            bool h = false; //Parametr -h
            List<string> patch = new List<string>(); //List cest pro vypsání
            foreach(string s in args)
            {
                if (s.Contains('-')) //Tento řetězec obsahuje parametry
                {
                    foreach (char z in s)
                    {
                        if (z == 'a') //zobrazit skryté adresáře/soubory (s tečkou na začátku)
                            a = true;
                        else if (z == 'l') //Dlouhý formát vypsání: Zobrazuje typ souboru, oprávnění, počet pevných odkazů, vlastníka, skupinu, velikost datum a jméno.
                            l = true;
                        else if (z == 'h') //Velikost se vypíše ve srozumitelnějším tvaru
                            h = true;
                    }
                }
                else //Tento řetězec obsahuje cestu pro vypsání
                {
                    if (Directory.Exists(s)) //Např: ls C:/Users
                        patch.Add(s);
                    else if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + s)) //Např: ls Users (za předpokladu že se uživatel nachází v adresáři C:/)
                        patch.Add(Directory.GetCurrentDirectory() + "\\" + s);
                    else //Chybová hláška, ukončení utility - Řetězec neobsahuje parametry ani cestu, je to tedy neplatný argument
                    {
                        Console.WriteLine("ls: cannot access {0}: No such directory", s);
                        System.Environment.Exit(0);
                    }
                }
            }
            
            if (patch.ToArray().Length == 0) //Pokud pole neobsahuje žádný prvek, přidá se do něj aktuální adrersář
                patch.Add(Directory.GetCurrentDirectory());
            string[] patchArray = patch.ToArray(); //Převedení listu na pole
            for (int i = 0; i < patchArray.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(patchArray[i]);
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    if (d.Name[0] == '.' && a == false) //Tečka v prvním znaku v Linxu značí skrytý soubor/adresář
                        continue; //Přeskočit na další adresář
                    if (l == true)
                    {
                        Console.Write("d"); //adresář
                        permissions(patchArray[i]); //Vypíše práva aktuálního uživatele a vlastníka adresáře
                        DateTime creationDirDate = d.CreationTime.Date; //Datum vytvoření adresáře
                        Console.Write("  {0, 10}  {1}  {2}\n", 0 ,creationDirDate.ToString("MMM dd yyyy", CultureInfo.CreateSpecificCulture("en-US")), d.Name); //Velikost adresáře (0*), datum vytvoření adresáře
                        //* Velikost adresáře - Adresář pouze jako kontejner pro další názvy souborů/adresářů které obsahuje, zabírá na ext4 (linuxový filesystem) 256 bajtů (výchozí veliskot inodu). Terminál obvykle zobrazuje velikost bloku, což je 4096 bajtů. Vzhledem k tomu, že windows je většinou na filesystemu NTFS, vypisuji jako velikost adresáře nulu.
                        //CultureInfo je zde proto, aby se názvy měsíců vypisovali v angličtině. Pro český jazyk neexistuje formát měsíce "MMM", v kterým linuxový terminál obvykle vypisuje.
                    }
                    else //Bez podrobného výpisu
                    {
                        Console.ForegroundColor = ConsoleColor.Blue; //Změna barvy písma na modrou
                        Console.Write("{0}   ", d.Name); //Vypíše adresáře v adresáři
                        Console.ResetColor();
                    }
                }

                foreach (FileInfo f in dir.GetFiles())
                {
                    if (f.Name[0] == '.' && a == false) //Tečka v prvním znaku v Linxu značí skrytý soubor/adresář
                        continue; //Přeskočit na další soubor
                    if (l == true)
                    {
                        Console.Write("-"); //soubor
                        permissions(patchArray[i]); //Vypíše práva aktuálního uživatele a vlastníka souboru
                        DateTime creationFileDate = f.CreationTime.Date; //Datum vytvoření souboru
                        if (h == true) //Vypíše velikost souboru ve srozumitelném tvaru
                        {
                            //long sizeOfFile = f.Length;
                            humanConvert(f.Length);
                        }
                        else
                            Console.Write("  {0, 10}",f.Length); //Velikost souboru v bajtech                       
                        Console.Write("  {0}  {1}\n", creationFileDate.ToString("MMM dd yyyy", CultureInfo.CreateSpecificCulture("en-US")), f.Name);//Datum vytvoření souboru
                    
                    }
                    else //Bez podrobného výpisu
                        Console.Write("{0}   ", f.Name); //Vypíše název souboru
                }
                if (l == false) //Pokud se jednalo o jednoduchý výpis, je ještě nutné zalomit řádek
                    Console.Write("\n"); //Ukončení řádku
            }
        }
        
        public static void permissions(string patch) //Spouští metodu "hasPermission" a na základě jejího výstupu vypisuje práva
        {
            if (hasPermission(patch, FileSystemRights.ReadAndExecute | FileSystemRights.ExecuteFile) == true) //Pokud má uživatel práva pro čtení (| = Logický součet - Pokud platí alespoň jedno)
                Console.Write("r"); //Zapíše se "r"
            else //Pokud uživatel nemá právo pro čtení
                Console.Write("-"); //Zapíše se pomlčka
            if (hasPermission(patch, FileSystemRights.Write)) //Pokud má uživatel práva pro zápis
                Console.Write("w"); //Zapíše se "w"
            else //Pokud uživatel nemá právo pro zápis
                Console.Write("-"); //Zapíše se pomlčka
            if (hasPermission(patch, FileSystemRights.ReadAndExecute | FileSystemRights.ExecuteFile)) //Pokud má uživatel práva spouštět/otevírat
                Console.Write("x"); //Zapíše se "x"
            else //Pokud uživatel nemá právo pro spouštění
                Console.Write("-"); //Zapíše se pomlčka
            Console.Write("  " + System.IO.File.GetAccessControl(patch).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString()); //Vypíše vlastníka souboru/adresáře (Trochu neštastně řešeno, protože vypisuje i skupiny)
        }

        private static bool hasPermission(string FilePath, FileSystemRights r) //Zjistí práva k souboru/adresáři aktuálního uživatele k dané akci (r/w/x)
        {
            try
            {
                FileSystemSecurity security;
                if (File.Exists(FilePath)) //Pokud takový soubor existuje
                    security = File.GetAccessControl(FilePath); //Získání přístupu k souboru
                else //Je to adresář
                    security = Directory.GetAccessControl(Path.GetDirectoryName(FilePath + "/")); //Získání přístupu k adresáři
                var rules = security.GetAccessRules(true, true, typeof(NTAccount)); //Získá pravidla pro přístup, NTAccount - účet/skupiny

                var currentuser = new WindowsPrincipal(WindowsIdentity.GetCurrent()); //Zjistí členství ve skupinách (v tomto případě aktuálního uživatele)
                bool result = false;
                foreach (FileSystemAccessRule rule in rules) //Pravidla k přístupu k adresáři/souboru
                {
                    if ((rule.FileSystemRights & (r)) == 0) //Pokud se nejedná o přístupové právo jehož informaci chceme získat (např: právo číst/zapisovat/spouštět)
                        continue; //Přeskočit na další

                    if (rule.IdentityReference.Value.StartsWith("S-1-")) //Pokud takto začíná, jedná se o SID
                    {
                        var sid = new SecurityIdentifier(rule.IdentityReference.Value); //Naplní hodnotu aktuálním sid
                        if (!currentuser.IsInRole(sid)) //Pokud aktuální uživatel nemá zastoupení v této roli (sid označující)
                            continue; //Přeskočit na další
                    }
                    else
                    {
                        if (!currentuser.IsInRole(rule.IdentityReference.Value)) //Pokud aktuální uživatel nemá zastoupení v této roli
                            continue; //Přeskočit na další
                    }

                    if (rule.AccessControlType == AccessControlType.Deny) //Přístuo byl odepřen
                        return false;
                    if (rule.AccessControlType == AccessControlType.Allow) //Povolen přístup
                        result = true;
                }
                return result; //Vrátí hodnotu na jejímž základě se zapíše právo aktuálního uživatele k dané akci (r/w/x)
            }
            catch
            {
                return false; //Došlo k chybě, možná nemáme k souboru/adresáři vůbec přístup, nebo vůbec neexistuje
            }
        }

        private static void humanConvert(long a) //Zaokrouhlení velikosti souboru
        {
            if (a >= 1000)
            {
                a = a / 1000;
                if (a >= 1000)
                {
                    a = a / 1000;
                    if (a >= 1000)
                    {
                        a = a / 1000;
                        if (a >= 1000)
                        {
                            a = a / 1000;
                            if (a >= 1000)
                            {
                                a = a / 1000;
                                Console.Write("  {0, 7} PB",a);  //Petabajty
                            }
                            else
                            {
                                Console.Write("  {0, 7} TB", a); //Terabajty
                            }
                        }
                        else
                        {
                            Console.Write("  {0, 7} GB", a); //Gigabajty
                        }
                    }
                    else
                    {
                        Console.Write("  {0, 7} MB", a); //Megabajty
                    }
                }
                else
                {
                    Console.Write("  {0, 7} KB", a); //Kilobajty
                }
            }
            else
            {
                Console.Write("  {0, 4} Bytes", a); //Bajty
            }
        }
    }
}
