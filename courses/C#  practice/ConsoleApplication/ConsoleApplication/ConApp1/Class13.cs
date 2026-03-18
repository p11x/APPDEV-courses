using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //for loop
    internal class Class13
    {
        static void Main(string[] args)
        {
            Console.Write("Enter any number :");
            int n = int.Parse(Console.ReadLine());

            int sum = 0;

            for (int i = 1; i <= n; i++)
            {
                Console.Write(i + "\t");
                sum = sum + i;
            }

            Console.WriteLine("\nSum of {0} numbers is {1}", n, sum);
        }
    }
    //WAP to print 1 to 1000 palindrome numbers
    //WAP to print 1 to 1000 armstrong numbers  
    //WAP to print 1 to 1000 strong numbers 
    //WAP to print 1 to 1000 prime numbers  
    //WAP to print 1 to 1000 perfect numbers  

}
