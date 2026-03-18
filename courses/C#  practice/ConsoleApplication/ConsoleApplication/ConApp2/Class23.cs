using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArithmeticLibrary;

namespace ConApp2
{
    //Consuming the Class Library
    internal class Class23
    {
        static void Main(string[] args)
        {
            ArithmeticOperations operations = new ArithmeticOperations();
            Console.WriteLine("Addition of two numbers is :" + operations.Addition(100, 200));
            Console.WriteLine("Substraction of Two numbers is :" + operations.Substraction(200, 100));
            Console.WriteLine("Multiplication of Two Numbers is :" + operations.Multiplication(10, 20));
            Console.WriteLine("Division of Two Numbers is :" + operations.Division(30, 10));
        }
    }
}
