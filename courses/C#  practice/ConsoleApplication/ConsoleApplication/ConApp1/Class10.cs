using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    /* Nested if */
    internal class Class10
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a value :");
            int a = int.Parse(Console.ReadLine());
            Console.Write("Enter b value :");
            int b = int.Parse(Console.ReadLine());
            Console.Write("Enter c value :");
            int c = int.Parse(Console.ReadLine());

            if(a>b)
            {
                if(a>c)
                {
                    Console.WriteLine("a is Big Number");
                }
                else
                {
                    Console.WriteLine("c is Big Number");
                }
            }
            else if(b>c)
            {
                Console.WriteLine("b is Big Number");
            }
            else
            {
                Console.WriteLine("c is Big Number");
            }
        }
    }
}
