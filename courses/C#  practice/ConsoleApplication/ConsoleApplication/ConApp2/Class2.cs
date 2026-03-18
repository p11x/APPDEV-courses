using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Array Elements Reading, Sorting, Searching
    internal class Class2
    {
        static void Main(string[] args)
        {
            int[] a = new int[5];

            Console.WriteLine("Enter 5 Array Elements......");
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write($"a[{i}]=");
                a[i]=int.Parse(Console.ReadLine());
            }

            Console.Clear();

            Console.WriteLine("Before Sorting the Array Elements are....");
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i] + "\t");
            }

            Array.Sort(a);
            Console.WriteLine("\nAfter Sorting the Array Elements are....");
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i] + "\t");
            }

            Array.Reverse(a);
            Console.WriteLine("\nAfter Reversing the Array Elements are....");
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i] + "\t");
            }

            Console.Write("\nEnter Element to be search :");
            int x=int.Parse(Console.ReadLine());

            if (Array.IndexOf(a, x) == -1)
                Console.WriteLine("\nNo such element");
            else
                Console.WriteLine("\nElement was found");
        }
    }
}
