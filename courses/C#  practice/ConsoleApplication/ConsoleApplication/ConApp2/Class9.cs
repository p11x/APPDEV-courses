using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * Polymorphism :-
     * Same thing will exhibit different behaviours in different instances.
     * 1. Compile Time Polymorphism / Early Binding / Static Binding
     * Ex :- Method Overloading, Operaror Overloading
     * 
     * 2. Runtime Polymorphism / Late Binding / Dynamic Binding
     * Ex :- Method Overriding
     * 
     * Method Overloading :-
     * Here method name will be same but either data type or number of arguments are different. Depending upon data type and
     * number of arguments the corresponding mthod will be executed.
     * Ex :-
     *      int Sum(int,int);
     *      float Sum(float,float);
     *      int Sum(int,int,int);
     */
    internal class Class9
    {
        public int Sum(int x, int y)
        {
            return x + y;
        }
        public int Sum(int x, int y, int z)
        {
            return (x + y + z);
        }
        public float Sum(float x, float y)
        { 
            return x + y; 
        }
        static void Main(string[] args)
        {
            Class9 class9 = new Class9();
            Console.WriteLine("Sum of Two Integers is :" + class9.Sum(10, 20));
            Console.WriteLine("Sum of Two Floats is   :" + class9.Sum(1.2f, 2.3f));
            Console.WriteLine("Sum of Three Integers is :" + class9.Sum(10, 20, 30));
        }
    }
}
