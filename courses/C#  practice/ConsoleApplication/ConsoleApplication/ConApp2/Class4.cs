using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * 1 0 0 0 0
     * 0 1 0 0 0
     * 0 0 1 0 0
     * 0 0 0 1 0
     * 0 0 0 0 1
     */
/*
 * 1 0 0 0 1
 * 0 1 0 1 0
 * 0 0 1 0 0
 * 0 1 0 1 0
 * 1 0 0 0 1
 */

/*
 * a[0,0]  a[0,1]  a[0,2]   a[0,3]  a[0,4]
 * 
 * a[1,0]  a[1,1]  a[1,2]   a[1,3]  a[1,4]
 * 
 * a[2,0]  a[2,1]  a[2,2]   a[2,3]  a[2,4]
 * 
 * a[3,0]  a[3,1]  a[3,2]   a[3,3]  a[3,4]
 * 
 * a[4,0]  a[4,1]  a[4,2]   a[4,3]  a[4,4]
 */
internal class Class4
{
    static void Main(string[] args)
    {
        int[,] a = new int[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i == j || i + j == 4)
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
