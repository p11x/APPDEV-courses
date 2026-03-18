using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * Jogged Array :-
     * It is an array of arrays in which each array index can differ.
     * Ex :-
     *      1  2  3
     *      1  2
     *      1  2  3  4
     */
    internal class Class7
    {
        static void Main(string[] args)
        {
            int[][] a = new int[3][];
            a[0] = new int[3];
            a[1] = new int[2];
            a[2] = new int[4];

            Console.WriteLine("Enter Jogged Array Elements....");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {
                    Console.Write($"a[{i}][{j}]=");
                    a[i][j]=int.Parse(Console.ReadLine());
                }
                Console.WriteLine();
            }

            Console.Clear();

            Console.WriteLine("The Jogged Array Elements....");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {
                    Console.Write(a[i][j]+"\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
