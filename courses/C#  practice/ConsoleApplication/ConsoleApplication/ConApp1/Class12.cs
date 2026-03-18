using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //while loop
    internal class Class12
    {
        static void Main(string[] args)
        {
            Console.Write("Enter any number :");
            int n=int.Parse(Console.ReadLine());

            int i = 1;  //loop variable or counter variable
            int sum = 0;

            while(i<=n)
            {
                Console.Write(i + "\t");
                sum = sum + i;

                i++;
            }
            Console.WriteLine("\nSum of {0} numbers is {1}", n, sum);
        }
    }
    //WAP to print even number from 1 to n
    //WAP to print odd numbers from 1 to n
    //WAP to calculate sum of even numbers and sum of odd number within n natural numbers
    //WAP to calculate factorial of a given number
    //WAP to print factors of a given number
    //WAP to calculate sum of digits of a given number
    //WAP to calculate reverse of a given number
    //WAP to check the given number of palindrome or not a palindrome
    //WAP to check the given number is armstrong number or not a armstrong number
    //WAP to check the given number of strong number or not a strong number
    //WAP to check the given number of prime number or not a prime number
    //WAP to check the given number is perfect number or not a perfect number
    //WAP to find number of digits in a given number
    //WAP to find biggest and smallest digits in a given number
}
