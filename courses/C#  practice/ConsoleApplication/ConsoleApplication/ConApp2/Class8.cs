using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //foreach
    internal class Class8
    {
        static void Main(string[] args)
        {
            int[] a = { 10, 20, 30, 40, 50 };

            Console.WriteLine("The Array Elements are.....");
            foreach (int ele in a)
            {
                Console.WriteLine(ele);
            }
        }
    }
}
