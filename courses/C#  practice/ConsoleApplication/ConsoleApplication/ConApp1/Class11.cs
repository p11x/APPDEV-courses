using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Ternary Operator -- expr1 ? expr2 : expr3
    internal class Class11
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a value :");
            int a = int.Parse(Console.ReadLine());
            Console.Write("Enter b value :");
            int b = int.Parse(Console.ReadLine());

            int big=(a>b)?a:b;

            Console.WriteLine("Biggest of a and b is :" + big);
        }
    }
}
