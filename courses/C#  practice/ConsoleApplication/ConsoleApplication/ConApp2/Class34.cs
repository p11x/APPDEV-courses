using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Userdefined generic class

    class GenericClass<T>
    {
        T Value;
        public void SetData(T value)
        {
            this.Value= value;
        }
        public T GetValue()
        {
            return Value;
        }
    }
    internal class Class34
    {
        static void Main(string[] args)
        {
            GenericClass<int> genericClass = new GenericClass<int>();
            GenericClass<string> genericClass1 = new GenericClass<string>();

            genericClass.SetData(10);
            genericClass1.SetData("Sathesh");

            Console.WriteLine(genericClass.GetValue());
            Console.WriteLine(genericClass1.GetValue());
        }
    }
}
