using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     Abstract class 
     - It contains both abstract and concrete methods.
     - Abstrcat methods from the abstract base class must be overriden by the derived class.
     - We can not create any object of its own.
     - We can declare only a reference variable, It can be assigned the instance of derived class 
       in the same inheritance chain.
     - Common functionalities can be defined in the abstract base class so that redefining the
       same functionality in each derived class can be avoided.

     Abstract Method
     - It contains only method signature.
     - It must be overriden by derived class.
    */

    abstract class EmployeBase
    {
        public int Bonus(int basic)          //concrete method
        {
            if (basic <= 4000)
                return 400;
            else if (basic <= 10000)
                return 800;
            else
                return 1200;
        }
        public abstract int CalBonus();     //abstract method
    }
    class Designer : EmployeBase
    {
        int basic = 6500;
        public override int CalBonus()
        {
            return Bonus(basic);
        }
    }
    class Analyst : EmployeBase
    {
        int basic = 12500;
        public override int CalBonus()
        {
            return Bonus(basic);
        }
    }
    internal class Class14
    {
        static void Main(string[] args)
        {
            EmployeBase employee;

            employee = new Designer();
            Console.WriteLine("Designer Bonus is :" + employee.CalBonus());

            employee = new Analyst();
            Console.WriteLine("Analyst Bonus is :" + employee.CalBonus());
        }
    }
}
