using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //DateTime
    internal class Class45
    {
        static void Main(string[] args)
        {
            Console.WriteLine("UTC Date and Time  :" + DateTime.UtcNow);
            Console.WriteLine("System Date and Time :" + DateTime.Now);
            Console.WriteLine("Short Date :" + DateTime.Now.ToShortDateString());
            Console.WriteLine("Long Date :" + DateTime.Now.ToLongDateString());
            Console.WriteLine("Short Time :" + DateTime.Now.ToShortTimeString());
            Console.WriteLine("Long Time :" + DateTime.Now.ToLongTimeString());
            Console.WriteLine("Date Component is :" + DateTime.Now.Day);
            Console.WriteLine("Month Component is :" + DateTime.Now.Month);
            Console.WriteLine("Year Component is :" + DateTime.Now.Year);
            Console.WriteLine("After Adding 10 Days date is :" + DateTime.Now.AddDays(10));
        }
    }
}
