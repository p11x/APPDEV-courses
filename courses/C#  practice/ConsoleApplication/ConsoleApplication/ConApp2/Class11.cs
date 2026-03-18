using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * Inheritance :-
     * Acquiring the properties of one class into an another class is called inheritance
     * It supports the concept of reusablity.
     * 
     * Types of Inheritance :-
     * 1. Single Inheritance
     * 2. Multi Level Inheritance
     * 3. Heirarchichal Inheritance
     * 
     * base :-
     * - It is used to invoke base class version of a method from the derived class
     * - It is used to pass parameters expilicitly to the base class Constructor
     */

    //Single Inheritance

    class Sample                //Base Class / Super Class / Parent Class
    {
        protected int a, b;
        public void GetData()
        {
            Console.Write("Enter a value :");
            a=int.Parse(Console.ReadLine());
            Console.Write("Enter b value :");
            b = int.Parse(Console.ReadLine());
        }
        public void Show()
        {
            Console.WriteLine("a value is :" + a);
            Console.WriteLine("b value is :" + b);
        }
    }
    class Sample1 : Sample          //Derived Class / sub class / child class
    {
        int c;
        public void Calculation()
        {
            c = a + b;
        }
        public new void Show()
        {
            base.Show();
            Console.WriteLine("c value is :" + c);
        }
    }
    internal class Class11
    {
        static void Main(string[] args)
        {
            Sample1 sample1 = new Sample1();
            sample1.GetData();
            sample1.Calculation();
            sample1.Show();
        }
    }
}
