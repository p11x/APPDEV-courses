using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * 1 1 1 1 1
     * 1 2 2 2 1
     * 1 2 3 2 1
     * 1 2 2 2 1
     * 1 1 1 1 1
     */
    internal class Class5
    {
        static void Main(string[] args)
        {
            int[,] a = new int[5, 5];

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == 0 || i == 4 || j == 0 || j == 4)
                        a[i, j] = 1;
                    else if (i == 2 && j == 2)
                        a[i, j] = 3;
                    else
                        a[i, j] = 2;

                    Console.Write(a[i, j]+"\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
