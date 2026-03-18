using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    internal class Class50
    {
        static void Main(string[] args)
        {
            string path = @"D:\SprintParkOnsiteBatch\ConsoleApplication\ConApp2\";
            path = path + DateTime.Now.Year.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path= path +"\\"+DateTime.Now.Month.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path=path +"\\"+DateTime.Now.Day.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            StreamWriter sw = new StreamWriter(path + "\\" + "sample.txt");
            Console.Write("Enter any string :");
            string s = Console.ReadLine();
            sw.WriteLine(s);
            sw.Close();
            Console.WriteLine("File Saved");
        }
    }
}
