/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Joining Operations
 * FILE: LINQ_Joining.cs
 * PURPOSE: Join and GroupJoin operations
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Joining
    {
        public static void Main()
        {
            // Join - inner join between two collections
            Console.WriteLine("=== Join - Inner Join ===");
            
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John" },
                new Customer { Id = 2, Name = "Jane" },
                new Customer { Id = 3, Name = "Bob" }
            };
            
            var orders = new List<Order>
            {
                new Order { Id = 101, CustomerId = 1, Total = 150.00m },
                new Order { Id = 102, CustomerId = 2, Total = 200.00m },
                new Order { Id = 103, CustomerId = 1, Total = 75.00m },
                new Order { Id = 104, CustomerId = 4, Total = 300.00m } // Customer 4 doesn't exist
            };
            
            // Inner join - only matching records
            var joined = customers.Join(
                orders,
                c => c.Id,
                o => o.CustomerId,
                (c, o) => new { Customer = c.Name, OrderId = o.Id, Total = o.Total }
            );
            
            Console.WriteLine("Customer Orders (Inner Join):");
            foreach (var item in joined)
            {
                Console.WriteLine($"  {item.Customer}: Order #{item.OrderId} - ${item.Total}");
                // Output: John: Order #101 - $150, Jane: Order #102 - $200, John: Order #103 - $75
            }

            // Join with multiple keys
            Console.WriteLine("\n=== Join with Multiple Keys ===");
            
            var employees = new List<Employee>
            {
                new Employee { Id = 1, DeptCode = "IT", Name = "Alice" },
                new Employee { Id = 2, DeptCode = "HR", Name = "Bob" },
                new Employee { Id = 3, DeptCode = "IT", Name = "Charlie" }
            };
            
            var departments = new List<Department>
            {
                new Department { Code = "IT", Name = "Information Technology" },
                new Department { Code = "HR", Name = "Human Resources" },
                new Department { Code = "Finance", Name = "Finance" }
            };
            
            var empDepts = employees.Join(
                departments,
                e => e.DeptCode,
                d => d.Code,
                (e, d) => new { Employee = e.Name, Department = d.Name }
            );
            
            foreach (var item in empDepts)
            {
                Console.WriteLine($"  {item.Employee}: {item.Department}");
                // Output: Alice: Information Technology, Bob: Human Resources, Charlie: Information Technology
            }

            // GroupJoin - LEFT OUTER JOIN equivalent
            Console.WriteLine("\n=== GroupJoin - Left Outer Join ===");
            
            // GroupJoin returns all customers, even those without orders
            var groupJoined = customers.GroupJoin(
                orders,
                c => c.Id,
                o => o.CustomerId,
                (c, customerOrders) => new
                {
                    Customer = c.Name,
                    OrderCount = customerOrders.Count(),
                    TotalSpent = customerOrders.Sum(o => o.Total)
                }
            );
            
            Console.WriteLine("Customer Order Summary (GroupJoin):");
            foreach (var item in groupJoined)
            {
                Console.WriteLine($"  {item.Customer}: {item.OrderCount} orders, ${item.TotalSpent}");
                // Output: John: 2 orders, $225, Jane: 1 order, $200, Bob: 0 orders, $0
            }

            // Join with query syntax
            Console.WriteLine("\n=== Query Syntax Join ===");
            
            var queryJoin = from c in customers
                            join o in orders on c.Id equals o.CustomerId
                            where o.Total > 100
                            orderby o.Total descending
                            select new { c.Name, o.Total };
            
            Console.WriteLine("Orders over $100:");
            foreach (var item in queryJoin)
            {
                Console.WriteLine($"  {item.Name}: ${item.Total}");
                // Output: Jane: $200, John: $150
            }

            // REAL WORLD EXAMPLE: Product with category info
            Console.WriteLine("\n=== Real World: Product Categories ===");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", CategoryId = 1 },
                new Product { Id = 2, Name = "Mouse", CategoryId = 1 },
                new Product { Id = 3, Name = "Chair", CategoryId = 2 }
            };
            
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Furniture" }
            };
            
            var productCategories = products.Join(
                categories,
                p => p.CategoryId,
                c => c.Id,
                (p, c) => new { Product = p.Name, Category = c.Name }
            );
            
            foreach (var item in productCategories)
            {
                Console.WriteLine($"  {item.Product} -> {item.Category}");
                // Output: Laptop -> Electronics, Mouse -> Electronics, Chair -> Furniture
            }

            // REAL WORLD EXAMPLE: Student with enrolled courses
            Console.WriteLine("\n=== Real World: Student Courses ===");
            
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Alice" },
                new Student { Id = 2, Name = "Bob" },
                new Student { Id = 3, Name = "Charlie" }
            };
            
            var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentId = 1, Course = "Math" },
                new Enrollment { StudentId = 1, Course = "Science" },
                new Enrollment { StudentId = 2, Course = "History" }
            };
            
            var studentCourses = students.GroupJoin(
                enrollments,
                s => s.Id,
                e => e.StudentId,
                (s, courses) => new
                {
                    Student = s.Name,
                    Courses = courses.Select(e => e.Course).ToList()
                }
            );
            
            foreach (var item in studentCourses)
            {
                Console.WriteLine($"  {item.Student}: {string.Join(", ", item.Courses)}");
                // Output: Alice: Math, Science; Bob: History; Charlie:
            }
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string DeptCode { get; set; }
        public string Name { get; set; }
    }

    public class Department
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Enrollment
    {
        public int StudentId { get; set; }
        public string Course { get; set; }
    }
}