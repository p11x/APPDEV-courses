/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : CQRS Pattern
 * FILE      : 02_CQRSDemo.cs
 * PURPOSE   : Demonstrates CQRS architecture in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._20_Architecture._02_CQRS
{
    public class CQRSDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== CQRS Demo ===\n");
            Console.WriteLine("1. Command Query Responsibility Segregation:");
            var handler = new OrderHandler();
            handler.HandleCommand(new CreateOrderCommand("Product", 2));
            var query = handler.HandleQuery(new GetOrderQuery(1));
            Console.WriteLine($"   Query result: {query}");
            Console.WriteLine("\n=== CQRS Complete ===");
        }
    }

    public class CreateOrderCommand { public string Product; public int Quantity; }
    public class GetOrderQuery { public int Id; public GetOrderQuery(int id) => Id = id; }
    public class OrderHandler
    {
        public void HandleCommand(CreateOrderCommand cmd) => Console.WriteLine($"   Command: Order for {cmd.Quantity} {cmd.Product}");
        public string HandleQuery(GetOrderQuery query) => "Order #1";
    }
}