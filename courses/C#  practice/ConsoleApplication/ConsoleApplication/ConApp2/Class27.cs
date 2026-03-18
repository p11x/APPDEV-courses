using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace ConApp2
{
    //ArrayList
    internal class Class27
    {
        static void Main(string[] args)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(10);
            arrayList.Add(20);
            arrayList.Add(30);
            arrayList.Add("Sathesh");
            arrayList.Add(40);
            arrayList.Insert(2, 100);

            //arrayList.Remove(20);
            arrayList.RemoveAt(2);

            Console.WriteLine("The ArrayList Elements are.....");
            foreach (var item in arrayList)
            {
                Console.WriteLine(item);
            }


            IList collection = new ArrayList();
            collection.Add(10);
            collection.Add(20);
            collection.Add(30);

            ArrayList arrayList1 = new ArrayList();
            arrayList1.AddRange(collection);

            arrayList1.RemoveRange(0, 2);

            Console.WriteLine("The ArrayList 1 Elements are....");
            foreach (var item in arrayList1)
            {
                Console.WriteLine(item);
            }
        }
    }
}
