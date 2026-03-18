using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    /*
     * A
     * A B
     * A B C
     * A B C D
     * A B C D E 
     */
    internal class Class23
    {
        static void Main(string[] args)
        {
            for (int i = 1; i<=5;i++)
            {
                for(int j=1;j<=i;j++)
                {
                    Console.Write((char)(j+64) + "\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
