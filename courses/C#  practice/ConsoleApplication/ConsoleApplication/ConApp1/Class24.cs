using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Tables
    internal class Class24
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 20; i++)
            {
                for (int j = 1; j <= 20; j++)
                {
                    Console.WriteLine($"{i} X {j} = {i * j}");
                }
                Console.WriteLine("\n");
                Console.ReadKey();
            }

            string FirstName = "Sathesh";
            string LastName = "Kumar";
            Console.WriteLine("My Name is " + FirstName + " " + LastName);
            Console.WriteLine("My Name is {0} {1}", FirstName, LastName);
            Console.WriteLine($"My Name is {FirstName} {LastName}");
        }
    }
}
