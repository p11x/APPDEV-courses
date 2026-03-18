using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * 1 0 0 0 0
     * 1 1 0 0 0 
     * 1 1 1 0 0
     * 1 1 1 1 0
     * 1 1 1 1 1
     */
    internal class Class6
    {
        static void Main(string[] args)
        {
            int[,] a = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i >= j)
                        a[i, j] = 1;
                    else
                        a[i, j] = 0;

                    Console.Write(a[i, j] + "\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
