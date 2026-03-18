using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    internal class Class47
    {
        static void Main(string[] args)
        {
            string sourceDirectory = @"E:\";        //Verbatim String

            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);

            try
            {
                var files1 = Directory.GetFiles(sourceDirectory); //string array

                foreach (var file in files1)
                {
                    Console.WriteLine(file);
                }

                var files = Directory.EnumerateFiles(sourceDirectory, "*.*");   //IEnumerable string array

                foreach (string currentFile in files)
                {
                    Console.WriteLine("File Name is :" + currentFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
