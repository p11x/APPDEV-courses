using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Reading Line By Line from a Text file
    internal class Class53
    {
        static void Main(string[] args)
        {
            string[] Names = new string[5];
            string path = "D:\\SprintParkOnsiteBatch\\ConsoleApplication\\ConApp2\\2024\\";
            path = path + "names.txt";

            try
            {
                StreamReader sr = new StreamReader(path);

                while (sr.Peek() != -1)
                {
                    string st = sr.ReadLine();
                    Console.WriteLine(st);
                }

                sr.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
