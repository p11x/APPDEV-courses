using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
     /*
           Namespace :-
           Namespaces are used as an organizational system. They provide a way of classifying and presenting 
           programming elements that are exposed to other programs and applications. Note that a namespace is 
           not a type in the sense that a class or structure is — you cannot declare a programming element to 
           have the data type of a namespace.
     */

    class Test1
    {
        public void Show()
        {
            Console.WriteLine("This is method from ConApp2 Namespace");
        }
    }
    internal class Class22
    {
        static void Main(string[] args)
        {
            Test1 test = new Test1();
            Namespace1.Test1 test1 = new Namespace1.Test1();
            Namespace1.Namespace2.Test1 test2 = new Namespace1.Namespace2.Test1();

            test.Show();
            test1.Show();
            test2.Show();
        }
    }
}
namespace Namespace1
{
    class Test1
    {
        public void Show()
        {
            Console.WriteLine("This is method from Namespace1 Namespace");
        }
    }
    namespace Namespace2
    {
        class Test1
        {
            public void Show()
            {
                Console.WriteLine("This is method from Namespace2 Namespace");
            }
        }
    }
}
