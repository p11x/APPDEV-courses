using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Dynamic Types
    //Inferred Type or Implicit Type
    internal class Class35
    {
        static void Main(string[] args)
        {
            var a = 100;    //Implicit Type
            var name = "Sathesh";

            dynamic myVariable = 100;
            Console.WriteLine("Type of myVariable is {0} and value is {1}", myVariable.GetType(), myVariable);

            myVariable = 12.23;
            Console.WriteLine("Type of myVariable is {0} and value is {1}", myVariable.GetType(), myVariable);

            myVariable = "Sathesh";
            Console.WriteLine("Type of myVariable is {0} and value is {1}", myVariable.GetType(), myVariable);

            myVariable = true;
            Console.WriteLine("Type of myVariable is {0} and value is {1}", myVariable.GetType(), myVariable);

        }
    }
}
