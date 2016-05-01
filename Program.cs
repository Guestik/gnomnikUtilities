//whoami command - Lukáš Anděl
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whoami
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.UserName); //Vypíše jméno uživatele
        }
    }
}
