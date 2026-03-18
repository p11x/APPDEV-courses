using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
      * Interfaces :-
      * It contains only method signature and must be implemented by implemented class.
      * It contains all abstract methods only.
      * By default all interface members are public and abstract
      * We can not create any object of its own.
      * These will help us in implementing the loosely coupled architecturs
    */

    interface IInter
    {
        void GetData();
        void ShowData();
    }

    class Product : IInter
    {
        int ProductId;
        string Name;
        public void GetData()
        {
            Console.Write("Enter Product Id :");
            ProductId = int.Parse(Console.ReadLine());
            Console.Write("Enter Product Name :");
            Name = Console.ReadLine();
        }
        public void ShowData()
        {
            Console.WriteLine("Product Id is   :" + ProductId);
            Console.WriteLine("Product Name is :" + Name);
        }
        public void Show()
        {
            Console.WriteLine("This is Show Method");
        }
    }

    internal class Class15
    {
        static void Main(string[] args)
        {
            IInter inter;
            inter = new Product();
            inter.GetData();
            inter.ShowData();
          

            Product product = new Product();
            product.Show();
        }
    }
}
