using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     Indexers
           Indexers permit instances of a class to be indexed in the same way as arrays.
           Indexers are similar to properties except that their accessors take parameters.

      enum
           The enum keyword is used to declare an enumeration, a distinct type consisting of a set of 
           named constants called the enumerator list.
    */

    enum Clrs
    {
        Red,
        Green,
        Blue,
        Yellow,
        Pink
    }

    class Car1
    {
        private string[] Colors = new string[3];

        public string this[int x]
        {
            get
            {
                return Colors[x];
            }
            set
            {
                Colors[x] = value;
            }
        }
    }


    internal class Class36
    {
        static void Main(string[] args)
        {
            Car1 car = new Car1();
            car[0] = Clrs.Red.ToString();
            car[1] = Clrs.Pink.ToString();
            car[2] = Clrs.Blue.ToString();

            Console.WriteLine("Available Car Colors are.....");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(car[i]);
            }
        }
    }
}
