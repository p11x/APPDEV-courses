/*
 * TOPIC: ValueTuples
 * SUBTOPIC: ValueTuple (C# 7+) with named elements
 * FILE: ValueTuples.cs
 * PURPOSE: Demonstrate modern ValueTuple syntax introduced in C# 7+ with named elements
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._06_AnonymousTypes_Tuples
{
    public class ValueTuples
    {
        public static void Main()
        {
            // ValueTuple syntax introduced in C# 7
            // More lightweight than Tuple class (value type vs reference type)
            
            // Basic ValueTuple with parentheses
            (int, int) coordinates = (10, 20);
            Console.WriteLine($"X: {coordinates.Item1}, Y: {coordinates.Item2}");    // Output: X: 10, Y: 20

            // Named elements - more readable code
            (string Name, int Age) person = ("Alice", 30);
            Console.WriteLine($"{person.Name} is {person.Age} years old");    // Output: Alice is 30 years old

            // Type inference with 'var' and named elements
            var employee = (Name: "Bob", Department: "Engineering", Salary: 75000);
            Console.WriteLine($"{employee.Name} works in {employee.Department}");    // Output: Bob works in Engineering

            // Multiple elements with mixed types
            var product = (Id: 1, Name: "Laptop", Price: 999.99m, InStock: true);
            Console.WriteLine($"{product.Name}: ${product.Price}");    // Output: Laptop: $999.99
            Console.WriteLine($"In Stock: {product.InStock}");    // Output: In Stock: True

            // Omitting type names - compiler infers types
            var point = (X: 100, Y: 200);
            Console.WriteLine($"Point: ({point.X}, {point.Y})");    // Output: Point: (100, 200)

            // Methods returning ValueTuple
            var (quotient, remainder) = Divide(10, 3);
            Console.WriteLine($"{quotient} remainder {remainder}");    // Output: 3 remainder 1

            // Swap values using ValueTuple
            int a = 5, b = 10;
            (a, b) = (b, a);  // Swap in one line!
            Console.WriteLine($"After swap: a={a}, b={b}");    // Output: After swap: a=10, b=5

            // ValueTuple as dictionary key
            var scoreLookup = new Dictionary<(string Name, string Subject), int>
            {
                { ("Alice", "Math"), 95 },
                { ("Alice", "Science"), 88 },
                { ("Bob", "Math"), 92 }
            };

            Console.WriteLine($"Alice Math: {scoreLookup[("Alice", "Math")]}");    // Output: Alice Math: 95

            // ValueTuple in collections
            var inventory = new List<(string Item, int Quantity, decimal Price)>
            {
                ("Laptop", 10, 999.99m),
                ("Mouse", 50, 29.99m),
                ("Keyboard", 30, 79.99m)
            };

            decimal totalValue = 0;
            foreach (var item in inventory)
            {
                var subtotal = item.Quantity * item.Price;
                totalValue += subtotal;
                Console.WriteLine($"{item.Item}: {item.Quantity} x ${item.Price} = ${subtotal}");
                // Output:
                // Laptop: 10 x $999.99 = $9999.90
                // Mouse: 50 x $1499.50 = $1499.50
                // Keyboard: 30 x $2399.70 = $2399.70
            }
            Console.WriteLine($"Total Inventory Value: ${totalValue:N2}");    // Output: Total Inventory Value: $13,899.10

            // ValueTuple vs Tuple - structural differences
            Console.WriteLine();
            Console.WriteLine("=== ValueTuple vs Tuple ===");
            // Tuple is reference type (class) - stored on heap
            var oldTuple = Tuple.Create(1, 2);
            // ValueTuple is value type (struct) - stored on stack
            var newTuple = (1, 2);
            
            Console.WriteLine($"Tuple type: {oldTuple.GetType().Name} (Reference Type)");    // Output: Tuple type: Tuple<Int32, Int32> (Reference Type)
            Console.WriteLine($"ValueTuple type: {newTuple.GetType().Name} (Value Type)");    // Output: ValueTuple type: ValueTuple<Int32, Int32> (Value Type)

            // Comparison: ValueTuple has better performance for small data
            // because it's allocated on the stack (no heap allocation)

            // Naming elements at declaration level
            (int x, int y) position = (5, 10);
            Console.WriteLine($"Position: x={position.x}, y={position.y}");    // Output: Position: x=5, y=10

            // Using underscore to ignore elements
            var (ignored, _, keep) = ("ignore", "ignore2", "keep");
            Console.WriteLine($"Kept: {keep}");    // Output: Kept: keep

            Console.WriteLine();
            Console.WriteLine("=== Real-World Examples ===");
            Console.WriteLine();

            // Real-world Example 1: Processing order
            var order = ProcessOrder("Laptop", 2, 999.99m);
            Console.WriteLine($"Order {order.OrderId}: {order.ItemCount} x {order.ItemName}");
            Console.WriteLine($"  Total: ${order.Total:C}, Tax: ${order.Tax:C}");    // Output:   Total: $1,999.98, Tax: $179.9982

            // Real-world Example 2: API Response parsing
            var apiResponse = (StatusCode: 200, Message: "Success", Data: new { Id = 1, Name = "Test" });
            Console.WriteLine($"API: [{apiResponse.StatusCode}] {apiResponse.Message}");    // Output: API: [200] Success
            Console.WriteLine($"  Data ID: {apiResponse.Data.Id}");    // Output:   Data ID: 1

            // Real-world Example 3: User authentication result
            var auth = AuthenticateUser("admin", "password123");
            Console.WriteLine($"Authenticated: {auth.IsAuthenticated}");
            Console.WriteLine($"  User: {auth.Username}, Token: {auth.Token.Substring(0, 20)}...");    // Output:   User: admin, Token: eyJhbGciOiJIUzI1NiI...
        }

        // Method returning ValueTuple with named elements
        static (int Quotient, int Remainder) Divide(int dividend, int divisor)
        {
            return (dividend / divisor, dividend % divisor);
        }

        // Real-world method returning multiple named values
        static (int OrderId, string ItemName, int ItemCount, decimal Total, decimal Tax) ProcessOrder(string item, int quantity, decimal unitPrice)
        {
            var orderId = 1001;
            var subtotal = quantity * unitPrice;
            var tax = subtotal * 0.09m;  // 9% tax
            return (orderId, item, quantity, subtotal, tax);
        }

        // Real-world: User authentication
        static (bool IsAuthenticated, string Username, string Token, string[] Roles) AuthenticateUser(string username, string password)
        {
            // Simulated authentication
            if (username == "admin" && password == "password123")
            {
                var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIn0.signature";
                return (true, username, token, new[] { "Admin", "User" });
            }
            return (false, "", "", Array.Empty<string>());
        }
    }
}