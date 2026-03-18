using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Filtering based on data type from a collection
    internal class Class43
    {
        static void Main(string[] args)
        {
            ArrayList list=new ArrayList();
            list.Add(10);
            list.Add(20);
            list.Add(30);
            list.Add("Sathesh");
            list.Add("Kumar");
            list.Add(40);
            list.Add(50);

            var list1=list.OfType<int>().ToList();
            Console.WriteLine("The List Elements are.....");
            list1.ForEach(x => Console.WriteLine(x));
        }
    }
}
