using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates structs vs classes in C#
    /// </summary>
    
    // Struct - Value Type
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void Display()
        {
            Console.WriteLine($"({X}, {Y})");
        }
    }
    
    // Class - Reference Type
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public void Display()
        {
            Console.WriteLine($"Name: {Name}, Age: {Age}");
        }
    }
    
    public class StructsDemo
    {
        public static void Main()
        {
            // Struct (value type)
            Point p1 = new Point(10, 20);
            Point p2 = p1;  // Creates a COPY
            
            p2.X = 30;
            
            Console.WriteLine("Struct (Value Type):");
            Console.WriteLine($"p1.X = {p1.X}");  // 10 (unchanged)
            Console.WriteLine($"p2.X = {p2.X}");  // 30
            
            // Class (reference type)
            Person person1 = new Person("John", 30);
            Person person2 = person1;  // Both point to SAME object
            
            person2.Name = "Jane";
            
            Console.WriteLine("\nClass (Reference Type):");
            Console.WriteLine($"person1.Name = {person1.Name}");  // Jane
            Console.WriteLine($"person2.Name = {person2.Name}");  // Jane
        }
    }
}
