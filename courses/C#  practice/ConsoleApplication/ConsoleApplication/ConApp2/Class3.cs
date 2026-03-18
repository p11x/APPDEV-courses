using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * Double Dimension Array :-
     * Syntax :-
     *          <data_type> [,] <array_name> = new <data_type>[size,size];
     * Ex :-
     *          int [,]a=new int[3,3];
     *          a[0,0]  a[0,1]  a[0,2]
     *          a[1,0]  a[1,1]  a[1,2]
     *          a[2,0]  a[2,1]  a[2,2]
     */
    internal class Class3
    {
        static void Main(string[] args)
        {
            //int[,] a = new int[,] { { 10, 20, 30 }, { 40, 50, 60 }, { 70, 80, 90 } };
            int[,] a = new int[3, 3];

            Console.WriteLine("Enter 3x3 Matrix Elements.....");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"a[{i},{j}]=");
                    a[i, j] = int.Parse(Console.ReadLine());
                }
                Console.WriteLine();
            }

            Console.Clear();

            Console.WriteLine("The Matrix Elements are.....");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(a[i, j] + "\t");
                }
                Console.WriteLine("\n");
            }

            Console.WriteLine("\nAfter Transposing the Matrix Elements are.....");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(a[j, i] + "\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
