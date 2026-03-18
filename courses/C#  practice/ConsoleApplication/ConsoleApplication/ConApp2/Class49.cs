using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Writing Data to a text file
    internal class Class49
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("sample.txt");
            Console.Write("Enter any string :");
            string st = Console.ReadLine();
            sw.WriteLine(st);
            sw.Close();
            Console.WriteLine("File Saved");

            //Reading content from a file
            StreamReader sr = new StreamReader("sample.txt");
            if(sr==null)
            {
                Console.WriteLine("Unable to Open Source File");
                Environment.Exit(0);
            }
            else
            {
                string st1= sr.ReadToEnd();
                Console.WriteLine(st1);
            }
            sr.Close();
        }
    }
}
