using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Pure Virtual Method (abstract method) : It must be overriden
    //Virtual Method :- If you want you can override

    abstract class Test3
    {
        public virtual void Show()          //Virtual Method / concrete method
        {
            Console.WriteLine("This is Show from Base Class");
        }
        public abstract void Display();     //Pure virtual method / abstract method
    }
    class Test4 : Test3
    {
        public override void Display()
        {
            Console.WriteLine("This is Display from Derived Class");
        }
        public override void Show()
        {
            base.Show();
            Console.WriteLine("This is Show From Derived Class");
        }
    }
    internal class Class38
    {
        static void Main(string[] args)
        {
            Test4 test = new Test4();
            test.Show();
            test.Display();
        }
    }
}
