using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //continue, break;
    internal class Class17
    {
        static void Main(string[] args)
        {
            Console.Write("Enter howmany numbers to accept :");
            int n=int.Parse(Console.ReadLine());

            int sum = 0, i = 1;

            while(i<=n)
            {
                Console.Write("Enter any +ve integer :");
                int num = int.Parse(Console.ReadLine());

                if (num < 1)
                {
                    break;
                }

                sum = sum + num;
                i++;
            }
            Console.WriteLine("Sum of numbers is :" + sum);
        }
    }
}
