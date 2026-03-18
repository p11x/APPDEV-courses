using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Simple if
    internal class Class7
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your age :");
            int age=int.Parse(Console.ReadLine());

            if (age >= 21) 
            {
                Console.WriteLine("You are eligibe for voting");
                Console.WriteLine("You are Major");
            }
            Console.WriteLine("\nThanking you");
        }
    }
}
