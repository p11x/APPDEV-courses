using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //HashSet
    internal class Class33
    {
        static void Main(string[] args)
        {
            HashSet<int> hashSet = new HashSet<int>();
            hashSet.Add(1);
            hashSet.Add(2);
            hashSet.Add(3);
            hashSet.Add(4);
            hashSet.Add(5);
            hashSet.Add(1);
            hashSet.Add(2);
            hashSet.Add(3);
            hashSet.Add(4);
            Console.WriteLine("HashSet Elements are.....");
            foreach (var item in hashSet)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("After Deleting Duplicates...");

            int[] a = { 1, 2, 3, 4, 1, 2, 4, 5, 6, 7 };
            HashSet<int> b = new HashSet<int>(a.ToList());
            b.ToList().ForEach(x => Console.WriteLine(x));

            int[] array = { 1, 2, 3, 4, 5};
            //(1,2), (1,3), (1,4), (1,5), (2,3), (2,4), (2,5), (3,4), (3,5), (4,5)

            HashSet<KeyValuePair<int, int>> keyValuePairs = new HashSet<KeyValuePair<int, int>>();
            keyValuePairs.Add(new KeyValuePair<int, int>(1, 1));
            keyValuePairs.Add(new KeyValuePair<int, int>(1, 2));
            keyValuePairs.Add(new KeyValuePair<int, int>(1, 3));
            keyValuePairs.Add(new KeyValuePair<int, int>(1, 1));
            keyValuePairs.Add(new KeyValuePair<int, int>(1, 3));
            keyValuePairs.Add(new KeyValuePair<int, int>(1, 4));

            Console.WriteLine("Unique KeyValue Pairs are.....");
            foreach (var item in keyValuePairs)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }
        }
    }
}
