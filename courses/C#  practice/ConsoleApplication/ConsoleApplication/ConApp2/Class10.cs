using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * Constructors :-
     * It is a special member method whose task is to initialise the object of a class when ever the object of the 
     * corresponding class is creating. 
     * Class Name and constructor name both be same.
     * Constructor won't have any return data type.
     * Types of Constructors :-
     * 1. Default Constructor
     * 2. Paramerter Constructor
     * 3. Overloaded Constructor
     * 4. Copy Constructor
     */

    class MyClass
    {
        int a, b;
        public MyClass()
        {
            a = 10;
            b = 20;
            Console.WriteLine("Default Constructor Called");
        }
        public MyClass(int a, int y)
        {
            this.a = a;
            b = y;
            Console.WriteLine("Parameter Constructor Called");
        }
        public MyClass(MyClass x)
        {
            a = x.a;
            b = x.b;
            Console.WriteLine("Copy Constructor Called");
        }
        public void Show()
        {
            Console.WriteLine("a value is :" + a);
            Console.WriteLine("b value is :" + b);
        }
    }
    internal class Class10
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            obj.Show();

            MyClass obj1 = new MyClass(30, 40);
            obj1.Show();

            MyClass obj2 = new MyClass(obj);
            obj2.Show();
        }
    }
}
