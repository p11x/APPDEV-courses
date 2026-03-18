using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ConApp2
{
    //Stack -- LIFO (Last In First Out)
    internal class Class28
    {
        static void Main(string[] args)
        {
            Stack stack = new Stack();
            stack.Push(10);
            stack.Push("Sathesh");

            Console.WriteLine(stack.Pop()); //Sathesh
            Console.WriteLine(stack.Pop()); //10

            Console.WriteLine();

            Stack<int> stack2 = new Stack<int>();
            stack2.Push(10);
            stack2.Push(20);

            Console.WriteLine(stack2.Pop());
            Console.WriteLine(stack2.Pop());
        }
    }
}
