using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //if...else
    internal class Class8
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a value :");
            int a=int.Parse(Console.ReadLine());
            Console.Write("Enter b value :");
            int b=int.Parse(Console.ReadLine());

            if(a>b)
            {
                Console.WriteLine("a is Big Number");
            }
            else
            {
                Console.WriteLine("b is Big Number");
            }
        }
    }
}
