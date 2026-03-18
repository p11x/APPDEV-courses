using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //goto keyword
    internal class Class16
    {
        static void Main(string[] args)
        {
            Console.Write("Enter any number :");
            int n = int.Parse(Console.ReadLine());
            int i = 1;
        start:

            Console.Write(i + "\t");
            i++;

            if(i<=n)
                goto start;
        }
    }
}
