using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Write a program to print first 20 terms of the following fibonacci series
    //0 1 1 2 3 5 8 13........
    internal class Class14
    {
        static void Main(string[] args)
        {
            int a = 0, b = 1, c = a + b;
            Console.Write(a + "\t" + b + "\t" + c + "\t");
            for(int i=1;i<=17;i++)
            {
                a = b;
                b = c;
                c = a + b;
                Console.Write(c+"\t");
            }
        }
    }
}
