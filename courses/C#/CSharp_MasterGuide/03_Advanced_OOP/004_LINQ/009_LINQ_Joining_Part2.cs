/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Joining Operations - Part 2
 * FILE: LINQ_Joining_Part2.cs
 * PURPOSE: Complex join patterns and multiple joins
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Joining_Part2
    {
        public static void Main()
        {
            // Multiple Joins - join more than two tables
            Console.WriteLine("=== Multiple Joins ===");
            
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "J.K. Rowling" },
                new Author { Id = 2, Name = "Stephen King" }
            };
            
            var books = new List<Book>
            {
                new Book { Id = 101, Title = "Harry Potter", AuthorId = 1 },
                new Book { Id = 102, Title = "The Shining", AuthorId = 2 }
            };
            
            var reviews = new List<Review>
            {
                new Review { BookId = 101, Rating = 5, Comment = "Amazing!" },
                new Review { BookId = 101, Rating = 4, Comment = "Great!" },
                new Review { BookId = 102, Rating = 5, Comment = "Scary!" }
            };
            
            // Join books, authors, and reviews together
            var fullData = authors
                .Join(books, a => a.Id, b => b.AuthorId, (a, b) => new { Author = a, Book = b })
                .Join(reviews, b => b.Book.Id, r => r.BookId, (b, r) => new
                {
                    Author = b.Author.Name,
                    Book = b.Book.Title,
                    Rating = r.Rating,
                    Comment = r.Comment
                });
            
            Console.WriteLine("Book Reviews with Authors:");
            foreach (var item in fullData)
            {
                Console.WriteLine($"  {item.Author} - {item.Book}: {item.Rating} stars ({item.Comment})");
                // Output: J.K. Rowling - Harry Potter: 5 stars (Amazing!)
                // Output: J.K. Rowling - Harry Potter: 4 stars (Great!)
                // Output: Stephen King - The Shining: 5 stars (Scary!)
            }

            // Join with composite key
            Console.WriteLine("\n=== Composite Key Join ===");
            
            var salesReps = new List<SalesRep>
            {
                new SalesRep { Id = 1, Region = "North", Name = "Alice" },
                new SalesRep { Id = 2, Region = "South", Name = "Bob" }
            };
            
            var sales = new List<SaleRecord>
            {
                new SaleRecord { Region = "North", Quarter = 1, Amount = 5000 },
                new SaleRecord { Region = "North", Quarter = 2, Amount = 6000 },
                new SaleRecord { Region = "South", Quarter = 1, Amount = 4000 },
                new SaleRecord { Region = "South", Quarter = 2, Amount = 4500 }
            };
            
            // Join on composite key (Region)
            var repSales = salesReps
                .Join(sales, 
                    rep => rep.Region, 
                    sale => sale.Region,
                    (rep, sale) => new 
                    { 
                        Rep = rep.Name, 
                        Region = rep.Region, 
                        Quarter = sale.Quarter,
                        Amount = sale.Amount 
                    });
            
            Console.WriteLine("Sales by Rep and Quarter:");
            foreach (var item in repSales)
            {
                Console.WriteLine($"  {item.Rep} ({item.Region}): Q{item.Quarter} = ${item.Amount}");
                // Output: Alice (North): Q1 = $5000, etc.
            }

            // Non-equi join (cross join with filter)
            Console.WriteLine("\n=== Non-Equi Join (Cross Apply) ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "Alice", Salary = 75000 },
                new Employee { Name = "Bob", Salary = 50000 },
                new Employee { Name = "Charlie", Salary = 90000 }
            };
            
            var budgets = new List<Budget>
            {
                new Budget { MinSalary = 0, MaxSalary = 60000, Level = "Junior" },
                new Budget { MinSalary = 60001, MaxSalary = 80000, Level = "Mid" },
                new Budget { MinSalary = 80001, MaxSalary = 100000, Level = "Senior" }
            };
            
            // Cross join with condition (like SQL WHERE in JOIN)
            var salaryLevels = employees
                .SelectMany(e => budgets.Where(b => e.Salary >= b.MinSalary && e.Salary <= b.MaxSalary),
                    (e, b) => new { Employee = e.Name, Salary = e.Salary, Level = b.Level });
            
            Console.WriteLine("Employee Salary Levels:");
            foreach (var item in salaryLevels)
            {
                Console.WriteLine($"  {item.Employee}: ${item.Salary} -> {item.Level}");
                // Output: Alice: $75000 -> Mid, Bob: $50000 -> Junior, Charlie: $90000 -> Senior
            }

            // LEFT OUTER JOIN using GroupJoin with DefaultIfEmpty
            Console.WriteLine("\n=== Left Outer Join ===");
            
            var departments = new List<Dept>
            {
                new Dept { Id = 1, Name = "IT" },
                new Dept { Id = 2, Name = "HR" },
                new Dept { Id = 3, Name = "Sales" }
            };
            
            var emps = new List<EmployeeRecord>
            {
                new EmployeeRecord { Id = 1, Name = "John", DeptId = 1 },
                new EmployeeRecord { Id = 2, Name = "Jane", DeptId = 1 },
                new EmployeeRecord { Id = 3, Name = "Bob", DeptId = 2 }
            };
            
            // Left outer join: all departments, employees if they exist
            var leftJoin = departments
                .GroupJoin(
                    emps,
                    d => d.Id,
                    e => e.DeptId,
                    (d, empsInDept) => new
                    {
                        Dept = d.Name,
                        Employees = empsInDept.Select(e => e.Name).DefaultIfEmpty("No employees")
                    }
                );
            
            Console.WriteLine("Departments and Employees:");
            foreach (var item in leftJoin)
            {
                Console.WriteLine($"  {item.Dept}: {string.Join(", ", item.Employees)}");
                // Output: IT: John, Jane; HR: Bob; Sales: No employees
            }

            // REAL WORLD EXAMPLE: Order processing with multiple joins
            Console.WriteLine("\n=== Real World: Order Processing ===");
            
            var customers = new List<CustomerInfo>
            {
                new CustomerInfo { Id = 1, Name = "John", City = "New York" },
                new CustomerInfo { Id = 2, Name = "Jane", City = "Los Angeles" }
            };
            
            var orders = new List<OrderInfo>
            {
                new OrderInfo { Id = 101, CustomerId = 1, ProductId = 1 },
                new OrderInfo { Id = 102, CustomerId = 1, ProductId = 2 },
                new OrderInfo { Id = 103, CustomerId = 2, ProductId = 3 }
            };
            
            var products = new List<ProductInfo>
            {
                new ProductInfo { Id = 1, Name = "Laptop", Price = 999.99m },
                new ProductInfo { Id = 2, Name = "Mouse", Price = 29.99m },
                new ProductInfo { Id = 3, Name = "Keyboard", Price = 79.99m }
            };
            
            var orderDetails = customers
                .Join(orders, c => c.Id, o => o.CustomerId, (c, o) => new { c, o })
                .Join(products, combined => combined.o.ProductId, p => p.Id, (combined, p) => new
                {
                    Customer = combined.c.Name,
                    City = combined.c.CustomerId,
                    Product = p.Name,
                    Price = p.Price
                });
            
            Console.WriteLine("Complete Order Details:");
            foreach (var item in orderDetails)
            {
                Console.WriteLine($"  {item.Customer} in City #{item.City}: {item.Product} - ${item.Price}");
                // Output: John in City #1: Laptop - $999.99, etc.
            }
        }
    }

    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }

    public class Review
    {
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class SalesRep
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }
    }

    public class SaleRecord
    {
        public string Region { get; set; }
        public int Quarter { get; set; }
        public decimal Amount { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }

    public class Budget
    {
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string Level { get; set; }
    }

    public class Dept
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EmployeeRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DeptId { get; set; }
    }

    public class CustomerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class OrderInfo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}