using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    internal class Class15
    {
        static void Main(string[] args)
        {
            Console.Write("Enter any number :");
            int n = int.Parse(Console.ReadLine());

            int i = 5;  //loop variable or counter variable
            int sum = 0;

            do
            {
                Console.Write(i + "\t");
                sum = sum + i;

                i++;
            } while (i <= n);

            Console.WriteLine("\nSum of {0} numbers is {1}", n, sum);
        }
    }
}
