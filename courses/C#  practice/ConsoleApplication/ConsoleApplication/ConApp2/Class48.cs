using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Enumerating all directories and files in specified Location
    internal class Class48
    {
        static void Main(string[] args)
        {
            string src = @"D:\FullStack\html";

            Console.WriteLine(src);

            PrintSubDirectories(src);
        }

        public static void PrintSubDirectories(string src)
        {
            string[] directories = Directory.GetDirectories(src);
            string[] files = Directory.GetFiles(src);

            foreach (var file in files)
            {
                Console.WriteLine("\t\t" + file);
            }

            foreach (var dir in directories)
            {
                Console.WriteLine(dir);
                PrintSubDirectories(dir);
            }
        }
    }
}
