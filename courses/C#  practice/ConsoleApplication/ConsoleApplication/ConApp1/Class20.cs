using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    /*
     * 1 2 3 4 5
     * 1 2 3 4
     * 1 2 3
     * 1 2
     * 1
     */

    /*
     * 5 4 3 2 1
     * 5 4 3 2
     * 5 4 3
     * 5 4
     * 5
     */
    internal class Class20
    {
        static void Main(string[] args)
        {
            //for (int i = 5; i >= 1; i--)
            //{
            //    for(int j=1;j<=i;j++)
            //    {
            //        Console.Write(j + "\t");
            //    }
            //    Console.WriteLine("\n");
            //}

            for(int i=1;i<=5;i++)
            {
                for(int j=5;j>=i;j--)
                {
                    Console.Write(j + "\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
