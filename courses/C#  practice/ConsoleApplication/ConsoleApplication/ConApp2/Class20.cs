using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
        Properties :-
        Properties are members that provide a flexible mechanism to read, write, or compute the values of private fields.
        Properties are actually special methods called accessors. This enables data to be accessed easily while 
        still providing the safety and flexibility of methods.
    */

    class Car
    {
        private string _color = "Red";

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }
    }
    internal class Class20
    {
        static void Main(string[] args)
        {
            Car car = new Car();
            Console.WriteLine("Car Color is :"+car.Color);          //get
            car.Color = "White";                                    //set
            Console.WriteLine("Car Color is :" + car.Color);        //get
        }
    }
}
