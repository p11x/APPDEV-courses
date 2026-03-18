using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
       * Boxing & UnBoxing 
       * Converting value type to reference type is called boxing
       * Converting reference type to value type is called unboxing
    */
    internal class Class17
    {
        static void Main(string[] args)
        {
            int a = 100;

            Console.WriteLine("a value is :" + a);

            object o = a;               //Boxing

            Console.WriteLine("Object Value is :" + o);

            int b = (int)o;             //UnBoxing

            Console.WriteLine("b value is :" + b);
        }
    }
}
