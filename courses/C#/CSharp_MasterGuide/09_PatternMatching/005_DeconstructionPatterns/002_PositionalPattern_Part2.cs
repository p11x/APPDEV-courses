/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Deconstruction Patterns (Continued)
 * FILE      : 02_PositionalPattern_Part2.cs
 * PURPOSE   : Continues deconstruction patterns with more complex scenarios
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._05_DeconstructionPatterns
{
    /// <summary>
    /// Continues deconstruction pattern demonstrations
    /// </summary>
    public class PositionalPattern_Part2
    {
        /// <summary>
        /// Entry point for more deconstruction patterns
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Positional Pattern Part 2 ===
            Console.WriteLine("=== Positional Pattern Part 2 ===\n");

            // ── CONCEPT: Complex Deconstruction ───────────────────────────────
            // Deconstruct with multiple elements

            // Example 1: Complex deconstruction
            // Output: 1. Complex Deconstruction:
            Console.WriteLine("1. Complex Deconstruction:");
            
            // Employee with multiple properties
            var emp1 = new Employee("Alice", "Engineering", 75000);
            var emp2 = new Employee("Bob", "Sales", 50000);
            var emp3 = new Employee("Charlie", "HR", 60000);
            
            // GetEmployeeLevel returns level
            Console.WriteLine($"   {emp1.Name}: {GetEmployeeLevel(emp1)}");
            Console.WriteLine($"   {emp2.Name}: {GetEmployeeLevel(emp2)}");
            Console.WriteLine($"   {emp3.Name}: {GetEmployeeLevel(emp3)}");

            // ── CONCEPT: Nested Deconstruction ───────────────────────────────
            // Objects containing objects with Deconstruct

            // Example 2: Nested deconstruction
            // Output: 2. Nested Deconstruction:
            Console.WriteLine("\n2. Nested Deconstruction:");
            
            // Order with nested Customer
            var order1 = new Order2(
                new Customer2("Alice", "VIP"),
                150.00m,
                "Pending"
            );
            var order2 = new Order2(
                new Customer2("Bob", "Regular"),
                50.00m,
                "Completed"
            );
            
            // GetOrderSummary returns summary
            Console.WriteLine($"   Order1: {GetOrderSummary(order1)}");
            Console.WriteLine($"   Order2: {GetOrderSummary(order2)}");

            // ── CONCEPT: List Pattern with Deconstruction ────────────────────
            // Combine list patterns with positional

            // Example 3: List pattern with deconstruction
            // Output: 3. List Pattern with Deconstruction:
            Console.WriteLine("\n3. List Pattern with Deconstruction:");
            
            // Process array of points
            var points = new Point2[]
            {
                new Point2(0, 0),
                new Point2(10, 10),
                new Point2(-5, 5)
            };
            
            // AnalyzePoints returns analysis
            Console.WriteLine($"   Points: {AnalyzePoints(points)}");

            // ── REAL-WORLD EXAMPLE: Result Processing ───────────────────────
            // Output: --- Real-World: Result Processing ---
            Console.WriteLine("\n--- Real-World: Result Processing ---");
            
            // Process operation results
            var result1 = new OperationResult(true, "Success", null);
            var result2 = new OperationResult(false, "Failed", "Timeout");
            var result3 = new OperationResult(false, "Error", null);
            
            // GetResultStatus returns status
            Console.WriteLine($"   Success: {GetResultStatus(result1)}");
            Console.WriteLine($"   Failed with error: {GetResultStatus(result2)}");
            Console.WriteLine($"   Error: {GetResultStatus(result3)}");

            Console.WriteLine("\n=== Positional Pattern Part 2 Complete ===");
        }

        /// <summary>
        /// Gets employee level using complex deconstruction
        /// </summary>
        public static string GetEmployeeLevel(Employee emp)
        {
            // Deconstruct into (Name, Department, Salary)
            return emp switch
            {
                // High salary in Engineering = senior
                (var name, "Engineering", > 70000) => $"Senior Engineer: {name}",
                
                // Any in Sales = sales
                (_, "Sales", _) => "Sales Team",
                
                // Medium salary = mid-level
                (var name, _, var salary) when salary >= 50000 => $"Mid-level: {name}",
                
                // Default = junior
                (var name, var dept, _) => $"Junior in {dept}: {name}"
            };
        }

        /// <summary>
        /// Gets order summary using nested deconstruction
        /// </summary>
        public static string GetOrderSummary(Order2 order)
        {
            // Nested deconstruction: Order -> (Customer, Amount, Status)
            // Customer -> (Name, Tier)
            return order switch
            {
                // VIP customer with pending status
                (Customer2(var name, "VIP"), _, "Pending") => $"VIP Order pending: {name}",
                
                // Completed order for regular customer
                (Customer2(var name, "Regular"), var amount, "Completed") => 
                    $"Regular completed: {name} - ${amount}",
                
                // Any pending order
                (Customer2(var name, _), _, "Pending") => $"Pending: {name}",
                
                // Default
                (Customer2(var name, var tier), var amount, var status) => 
                    $"{tier} order: {name} - ${amount} ({status})"
            };
        }

        /// <summary>
        /// Analyzes points using list pattern with deconstruction
        /// </summary>
        public static string AnalyzePoints(Point2[] points)
        {
            // List pattern combined with positional
            return points switch
            {
                // Empty array
                [] => "No points",
                
                // Single point at origin
                [Point2(0, 0)] => "Single point at origin",
                
                // Single point anywhere else
                [var p] => $"Single point: ({p.X}, {p.Y})",
                
                // Two points
                [var p1, var p2] => $"Two points: ({p1.X},{p1.Y}) and ({p2.X},{p2.Y})",
                
                // Multiple points (3+)
                [var first, .., var last] => 
                    $"Multiple points: first ({first.X},{first.Y}), last ({last.X},{last.Y})"
            };
        }

        /// <summary>
        /// Real-world: Gets result status from operation result
        /// </summary>
        public static string GetResultStatus(OperationResult result)
        {
            // Deconstruct result tuple
            return result switch
            {
                // Success = green
                (true, "Success", null) => "SUCCESS",
                
                // Failed with error message = red
                (false, _, var error) when error != null => $"FAILED: {error}",
                
                // Failed without message = yellow
                (false, var message, null) => $"FAILED: {message}",
                
                // Default = unknown
                _ => "UNKNOWN"
            };
        }
    }

    // ── EXAMPLE CLASSES ───────────────────────────────────────────────────
    /// <summary>
    /// Employee with 3-property deconstruction
    /// </summary>
    public class Employee
    {
        public string Name { get; }
        public string Department { get; }
        public decimal Salary { get; }

        public Employee(string name, string dept, decimal salary)
        {
            Name = name;
            Department = dept;
            Salary = salary;
        }

        public void Deconstruct(out string name, out string department, out decimal salary)
        {
            name = Name;
            department = Department;
            salary = Salary;
        }
    }

    /// <summary>
    /// Customer with 2-property deconstruction
    /// </summary>
    public class Customer2
    {
        public string Name { get; }
        public string Tier { get; }

        public Customer2(string name, string tier)
        {
            Name = name;
            Tier = tier;
        }

        public void Deconstruct(out string name, out string tier)
        {
            name = Name;
            tier = Tier;
        }
    }

    /// <summary>
    /// Order with nested customer deconstruction
    /// </summary>
    public class Order2
    {
        public Customer2 Customer { get; }
        public decimal Amount { get; }
        public string Status { get; }

        public Order2(Customer2 customer, decimal amount, string status)
        {
            Customer = customer;
            Amount = amount;
            Status = status;
        }

        public void Deconstruct(out Customer2 customer, out decimal amount, out string status)
        {
            customer = Customer;
            amount = Amount;
            status = Status;
        }
    }

    /// <summary>
    /// Point2 for list pattern demonstration
    /// </summary>
    public class Point2
    {
        public int X { get; }
        public int Y { get; }

        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }

    /// <summary>
    /// Operation result for real-world example
    /// </summary>
    public class OperationResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public string? Error { get; }

        public OperationResult(bool success, string message, string? error)
        {
            IsSuccess = success;
            Message = message;
            Error = error;
        }

        public void Deconstruct(out bool isSuccess, out string message, out string? error)
        {
            isSuccess = IsSuccess;
            message = Message;
            error = Error;
        }
    }
}
