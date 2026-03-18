using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Accepting Data from Keyboard
    internal class Class4
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Employee Number :");
            //int EmpNumber = int.Parse(Console.ReadLine());         //"45"
            int EmpNumber=Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Employee Name :");
            string EmpName=Console.ReadLine();

            Console.Write("Enter Employee Salary :");
            decimal EmpSalary=decimal.Parse(Console.ReadLine());

            Console.Clear();

            Console.WriteLine("Employee Number  :" + EmpNumber);
            Console.WriteLine("Employee Name    :" + EmpName);
            Console.WriteLine("Employee Salary  :" + EmpSalary);
        }
    }
}
