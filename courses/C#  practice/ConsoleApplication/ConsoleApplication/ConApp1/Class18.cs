using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //switch....case
    internal class Class18
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Computers");
            Console.WriteLine("2. Mechanical");
            Console.WriteLine("3. Civil");
            Console.WriteLine("4. Electrical");

            Console.Write("\nEnter Your Choice :");
            int ch=int.Parse(Console.ReadLine());

            switch(ch)
            {
                case 1:
                    Console.WriteLine("You are selected Computers");
                    break;
                case 2:
                    Console.WriteLine("\nYou are selected Mechanical");
                    break;
                case 3:
                    Console.WriteLine("\nYou are selected Civil");
                    break;
                case 4:
                    Console.WriteLine("\nYou are selected Electrical");
                    break;
                default:
                    Console.WriteLine("\nInvalid Choice");
                    break;
            }
        }
    }
}
