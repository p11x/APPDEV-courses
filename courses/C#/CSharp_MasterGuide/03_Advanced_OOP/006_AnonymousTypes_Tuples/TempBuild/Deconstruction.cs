/*
 * TOPIC: Deconstruction
 * SUBTOPIC: Deconstructing tuples and objects
 * FILE: Deconstruction.cs
 * PURPOSE: Demonstrate deconstruction of tuples and objects in C#
 */
using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._06_AnonymousTypes_Tuples
{
    // Class with Deconstruct method for object deconstruction
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }

        public Person(string name, int age, string city)
        {
            Name = name;
            Age = age;
            City = city;
        }

        // Deconstruct method - enables pattern: var (name, age) = person;
        public void Deconstruct(out string name, out int age, out string city)
        {
            name = Name;
            age = Age;
            city = City;
        }
    }

    // Another class for demonstration
    public class Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public void Deconstruct(out double width, out double height)
        {
            width = Width;
            height = Height;
        }

        public double Area => Width * Height;
    }

    public class Deconstruction
    {
        public static void Main()
        {
            // Basic tuple deconstruction
            var point = (X: 10, Y: 20);
            var (x, y) = point;
            Console.WriteLine($"Deconstructed: x={x}, y={y}");    // Output: Deconstructed: x=10, y=20

            // Tuple deconstruction with partial discard
            var person = (Name: "Alice", Age: 30, City: "New York");
            var (name, _, city) = person;
            Console.WriteLine($"{name} lives in {city}");    // Output: Alice lives in New York

            // Deconstructing into existing variables
            int x1, y1;
            (x1, y1) = point;
            Console.WriteLine($"Assigned: ({x1}, {y1})");    // Output: Assigned: (10, 20)

            // Swapping using deconstruction
            int a = 5, b = 10;
            (a, b) = (b, a);
            Console.WriteLine($"Swapped: a={a}, b={b}");    // Output: Swapped: a=10, b=5

            // Object deconstruction with Deconstruct method
            var personObj = new Person("Bob", 25, "Boston");
            var (personName, personAge, personCity) = personObj;
            Console.WriteLine($"{personName}, age {personAge}, in {personCity}");    // Output: Bob, age 25, in Boston

            // Nested deconstruction
            var nested = (Outer: (Inner1: 1, Inner2: 2), Value: 3);
            var (outer, value) = nested;
            var (inner1, inner2) = outer;
            Console.WriteLine($"Nested: {inner1}, {inner2}, {value}");    // Output: Nested: 1, 2, 3

            // One-liner nested deconstruction
            var ((i1, i2), v) = nested;
            Console.WriteLine($"Direct: {i1}, {i2}, {v}");    // Output: Direct: 1, 2, 3

            // Deconstruction in method parameters (C# 8+)
            PrintCoordinates((X: 100, Y: 200));

            // Deconstruction in switch patterns
            var shape = (Type: "Rectangle", Width: 10.0, Height: 5.0);
            switch (shape)
            {
                case (Type: "Rectangle", Width: var w, Height: var h):
                    Console.WriteLine($"Rectangle: {w}x{h}");    // Output: Rectangle: 10x5
                    break;
                case (Type: "Circle", Width: var r, Height: var h2):
                    Console.WriteLine($"Circle with radius {r}");
                    break;
            }

            // Deconstructing the Rectangle class
            var rect = new Rectangle(8, 5);
            var (width, height) = rect;
            Console.WriteLine($"Rectangle: {width}x{height}, Area: {rect.Area}");    // Output: Rectangle: 8x5, Area: 40

            Console.WriteLine();
            Console.WriteLine("=== Real-World Examples ===");
            Console.WriteLine();

            // Real-world Example 1: Processing HTTP response
            var response = (StatusCode: 200, Message: "OK", Data: "Some data", Headers: new string[] { "Content-Type: json" });
            var (status, message, data, _) = response;
            Console.WriteLine($"Response: [{status}] {message}");    // Output: Response: [200] OK
            Console.WriteLine($"Data: {data}");    // Output: Data: Some data

            // Real-world Example 2: Database row deconstruction
            var dbRow = (Id: 42, Name: "John Doe", Email: "john@example.com", CreatedAt: DateTime.Now);
            var (rowId, rowName, rowEmail, createdAt) = dbRow;
            Console.WriteLine($"User {rowId}: {rowName} <{rowEmail}>");    // Output: User 42: John Doe <john@example.com>
            Console.WriteLine($"  Created: {createdAt:g}");    // Output:   Created: 4/4/2026 8:29 AM

            // Real-world Example 3: Game entity position
            var entity = (Name: "Player", X: 150.5, Y: 200.75, Z: 0.0);
            var (entityName, xPos, yPos, zPos) = entity;
            Console.WriteLine($"{entityName} at ({xPos}, {yPos}, {zPos})");    // Output: Player at (150.5, 200.75, 0)

            // Real-world Example 4: Financial transaction
            var transaction = (Id: "TXN-001", Amount: 1500.00m, Currency: "USD", From: "Account A", To: "Account B");
            var (txnId, amount, currency, from, to) = transaction;
            Console.WriteLine($"Transaction {txnId}: {amount} {currency}");    // Output: Transaction TXN-001: 1500 USD
            Console.WriteLine($"  From: {from} -> To: {to}");    // Output:   From: Account A -> To: Account B

            // Real-world Example 5: Config settings
            var config = (Host: "localhost", Port: 8080, Timeout: 30, UseSSL: true);
            var (host, port, timeout, useSSL) = config;
            Console.WriteLine($"Connecting to {host}:{port}");    // Output: Connecting to localhost:8080
            Console.WriteLine($"  Timeout: {timeout}s, SSL: {useSSL}");    // Output:   Timeout: 30s, SSL: True
        }

        // Method accepting deconstructed tuple directly in parameter
        static void PrintCoordinates((int X, int Y) point)
        {
            Console.WriteLine($"Coordinates: ({point.X}, {point.Y})");    // Output: Coordinates: (100, 200)
        }
    }
}