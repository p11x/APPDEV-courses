using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    internal class Class2
    {
        public void Show()              //Instance Method
        {
            Console.WriteLine("This is Show Method");
        }
        public static void Display()    //Static Method
        {
            Console.WriteLine("This is Display Method");
        }
        static void Main(string[] args)
        {
            Display();

            Class2 class2 = new Class2();
            class2.Show();

        }
    }
}
