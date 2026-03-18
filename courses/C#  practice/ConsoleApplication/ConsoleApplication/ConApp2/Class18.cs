using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
       * Delegates :-
       * A delegate is a type that references a method. Once a delegate is assigned a method(function), 
       * it behaves exactly like that method. 
       * 
       * Delegates are similar to function pointers in C++; however, delegates are type-safe and secure.
    */

    delegate int Delegate(int x, int y);        //delegate class

    internal class Class18
    {
        public int Sum(int x,int y)
        {
            return x + y;
        }
        public int Mul(int x, int y)
        {
            return x * y;
        }
        static void Main(string[] args)
        {
            Class18 obj=new Class18 ();

            Delegate d1 = new Delegate(obj.Sum);
            Delegate d2= new Delegate(obj.Mul);

            Console.WriteLine("Sum of Two Numbers is :" + d1.Invoke(100, 200));
            Console.WriteLine("Mul of Two Numbers is :" + d2.Invoke(10, 20));
        }
    }
}
