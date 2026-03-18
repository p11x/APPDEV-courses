using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Heirarchichal Inheritance
    class Company
    {
        int Id;
        string CompanyName;
        public Company()
        {
            Id = 101;
            CompanyName = "BAJAJ";
        }
        public void ShowCompanyDetails()
        {
            Console.WriteLine("Company Id is   :" + Id);
            Console.WriteLine("Company Name is :" + CompanyName);
        }
    }
    class Bike1 : Company
    {
        int cc;
        string BName;
        public Bike1()  
        {
            cc = 125;
            BName = "Discover";
        }
        public void ShowBike1()
        {
            ShowCompanyDetails();
            Console.WriteLine("Bike1 CC is     :" + cc);
            Console.WriteLine("Bike1 Name      :" + BName);
        }
    }
    class Bike2 : Company
    {
        int cc;
        string BName;
        public Bike2()
        {
            cc = 150;
            BName = "Pulsar";
        }
        public void ShowBike2()
        {
            ShowCompanyDetails();
            Console.WriteLine("Bike2 CC is     :" + cc);
            Console.WriteLine("Bike2 Name      :" + BName);
        }
    }
    internal class Class13
    {
        static void Main(string[] args)
        {
            Bike1 bike1 = new Bike1();
            Bike2 bike2 = new Bike2();

            bike1.ShowBike1();
            Console.WriteLine();
            bike2.ShowBike2();
        }
    }
}
