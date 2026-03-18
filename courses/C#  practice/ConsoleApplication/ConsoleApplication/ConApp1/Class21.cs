using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    /*
     *         1
     *       1 2
     *     1 2 3
     *   1 2 3 4
     * 1 2 3 4 5
     */
    internal class Class21
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int k = 1; k <= 5 - i; k++)
                {
                    Console.Write("\t");
                }
                for (int j = 1; j <= i; j++)
                {
                    Console.Write(j + "\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
