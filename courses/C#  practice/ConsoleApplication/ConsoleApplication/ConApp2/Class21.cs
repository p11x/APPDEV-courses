using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public float StudentAverage { get; set; }
    }
    internal class Class21
    {
        static void Main(string[] args)
        {
            //Student student = new Student();
            //student.StudentId = 1;
            //student.StudentName = "Rajesh";
            //student.StudentAverage = 50;

            Student student = new Student()
            {
                StudentId = 1,
                StudentName = "Kiran",
                StudentAverage = 80
            };

            Console.WriteLine("Student Id is       :"+student.StudentId);
            Console.WriteLine("Student Name is     :"+student.StudentName);
            Console.WriteLine("Student Average is  :"+student.StudentAverage);
        }
    }
}
