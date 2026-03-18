using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    internal class Class52
    {
        //Writing Line by line to a text file
        static void Main(string[] args)
        {
            string[] Names = new string[5];
            string path = "D:\\SprintParkOnsiteBatch\\ConsoleApplication\\ConApp2\\2024\\";
            path = path + "names.txt";

            StreamWriter sw = new StreamWriter(path, true);

            for (int i = 0; i < Names.Length; i++)
            {
                Console.Write("Names[" + i + "]=");
                Names[i] = Console.ReadLine();            //Keyboard to Program File

                sw.WriteLine(Names[i]);                 //Program file to Disk File
            }
            sw.Close();
            Console.WriteLine("File Saved");
        }
    }
}
