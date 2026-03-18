using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Generic List
    internal class Class30
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            list.Add(4);
            list.Add(5);
            list.Add(1);
            list.Add(2);
            list.Add(3);

            list.Insert(0, 40);

            list.Sort();

            list.Remove(40);

            Console.WriteLine("The List Elements are.....");
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}
            list.ForEach(x => Console.WriteLine(x));


            Console.WriteLine("Sum of All Items is :" + list.Sum());
            Console.WriteLine("Maximum Value is :" + list.Max());
            Console.WriteLine("Minimum Value is :" + list.Min());
            Console.WriteLine("Average Value is :" + list.Average());


            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee() { Id = 1, Name = "Sathesh", Salary = 25000 });

            Employee emp = new Employee();
            emp.Id = 2;
            emp.Name = "Shiva";
            emp.Salary = 45000;
            employees.Add(emp);

            emp = new Employee()
            {
                Id = 3,
                Name = "Krishna",
                Salary = 50000
            };
            employees.Add(emp);

            Console.WriteLine("The Employees List is......");
            foreach (var item in employees)
            {
                Console.WriteLine(item.Id + "\t" + item.Name + "\t" + item.Salary);
            }
        }
    }
    class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }
}
