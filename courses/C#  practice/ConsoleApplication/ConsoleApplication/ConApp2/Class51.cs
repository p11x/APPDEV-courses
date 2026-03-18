using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Displyaing the File info for specified file
    internal class Class51
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter File Name :");
            string file = Console.ReadLine();

            string src = @"D:\SprintParkOnsiteBatch\ConsoleApplication\ConApp2\";
            var today = DateTime.Now;

            string path = src + today.Year + "\\" + today.Month + "\\" + today.Day + "\\" + file;

            FileInfo fileInfo = new FileInfo(path);

            Console.WriteLine("File Full Name :" + fileInfo.FullName);
            Console.WriteLine("File Created Time :" + fileInfo.CreationTime);
            Console.WriteLine("File Last Written Time :" + fileInfo.LastWriteTime);
            Console.WriteLine("File Size :" + fileInfo.Length);
            Console.WriteLine("File Last Access Time :" + fileInfo.LastAccessTimeUtc);
        }
    }
}
