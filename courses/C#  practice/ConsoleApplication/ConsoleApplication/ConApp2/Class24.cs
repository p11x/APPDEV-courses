using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /* 
        Types of Parameters :-
        1. Value Parameters
        2. Reference Parameters
        3. Default Parameters
        4. Nullable Parameters
        5. out Parameters
    */
    internal class Class24
    {
        public void Swap1(int x, int y)         //value parameters or call by value
        {
            int temp = x;
            x = y;
            y = temp;
        }
        public void Swap2(ref int x, ref int y) //Reference Parameters or call by reference
        {
            int temp = x;
            x = y;
            y = temp;
        }
        public int Sum1(int x, int y = 100)       //Default Parameters
        {
            return x + y;
        }
        public int Sum2(int x, int? y = null)         //Nullable Parameter
        {
            y ??= 1;
            return x + (int)y;
        }
        public void Sum3(int x, int y, out int z)       //out parameter
        {
            z = x + y;
        }
        static void Main(string[] args)
        {
            int a = 10, b = 20;
            Console.WriteLine("Before Swapping the values of a and b is :{0} \t {1}", a, b);

            Class24 obj=new Class24();
            obj.Swap1(a, b);
            Console.WriteLine("After  Swapping the values of a and b is :{0} \t {1}", a, b);
            obj.Swap2(ref a, ref b);
            Console.WriteLine("After  Swapping the values of a and b is :{0} \t {1}", a, b);
            Console.WriteLine();

            Console.WriteLine("Sum of Two Numbers is :" + obj.Sum1(100));
            Console.WriteLine("Sum of Two Numbers is :" + obj.Sum2(100, null));

            int c;
            obj.Sum3(a,b,out c);
            Console.WriteLine("C value is :" + c);

        }
    }
}
