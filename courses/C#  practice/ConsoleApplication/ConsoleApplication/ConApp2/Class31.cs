using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Linked List
    internal class Class31
    {
        static void Main(string[] args)
        {
            LinkedList<int> list = new LinkedList<int>();
            list.AddFirst(10);
            list.AddLast(20);
            LinkedListNode<int> node = list.AddLast(30);
            list.AddLast(40);

            list.AddBefore(node, 25);
            list.AddAfter(node, 35);

            list.AddFirst(5);

            Console.WriteLine("The Linked List Elements are.....");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
