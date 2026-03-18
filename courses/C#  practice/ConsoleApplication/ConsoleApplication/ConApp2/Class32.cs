using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Dictionary -- It is a generic collection
    //Hash Table
    internal class Class32
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "Sathesh Kumar");
            dic.Add(2, "Monopoly IT Solutions");
            dic.Add(3, "KPHB, Hyderabad");

            Console.WriteLine("Disctionary Values are....");
            foreach (var item in dic)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

            Hashtable ht = new Hashtable();
            ht.Add(1, "Sathesh");
            ht.Add("a", "Monopoly");
            ht.Add(2, "KPHB, Hyderabad");

            Console.WriteLine("Hash Table Elements are...");
            foreach (DictionaryEntry item in ht)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }
        }
    }
}
