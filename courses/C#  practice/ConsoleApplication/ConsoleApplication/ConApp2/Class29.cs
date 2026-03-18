using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Queue -- FIFO (First In First Out)
    internal class Class29
    {
        static void Main(string[] args)
        {
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(10);
            queue.Enqueue(20);

            Console.WriteLine(queue.Dequeue()); //10
            Console.WriteLine(queue.Dequeue()); //20
        }
    }
}
