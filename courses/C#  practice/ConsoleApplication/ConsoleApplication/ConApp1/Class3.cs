using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Variable Declaration and Initialisation
    internal class Class3
    {
        static void Main(string[] args)
        {
            int a = 100;
            float b = 12.23F;
            double c = 23.45;
            decimal d = 26.78M;
            char e = 'S';
            bool f = true;
            string g = "Sathesh";

            Console.WriteLine("a value is :" + a);
            Console.WriteLine("b value is :" + b);
            Console.WriteLine("c value is :" + c);
            Console.WriteLine("d value is :" + d);
            Console.WriteLine("e value is :" + e);
            Console.WriteLine("f value is :" + f);
            Console.WriteLine("g value is :" + g);

            Console.WriteLine("\n");

            Console.WriteLine("a value is :{0}\nb value is :{1}\nc value is :{2}", a, b, c);

            Console.WriteLine("\n");

            Console.WriteLine($"a value is :{a}\nb value is :{b}\nc value is :{c}");

        }
    }
}
