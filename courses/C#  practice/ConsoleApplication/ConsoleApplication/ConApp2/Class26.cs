using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
    * Partial :- 
    * By using this we can split the definition of a class into multiple definitions
    */

    partial class Test2
    {
        public void Show()
        {
            Console.WriteLine("This is Show Method");
        }
        public partial void Display();
    }
    partial class Test2
    {
        public partial void Display()
        {
            Console.WriteLine("This is Dislay Method");
        }
    }
    internal class Class26
    {
        static void Main(string[] args)
        {
            Test2 test = new Test2();
            test.Show();
            test.Display();
        }
    }
}
