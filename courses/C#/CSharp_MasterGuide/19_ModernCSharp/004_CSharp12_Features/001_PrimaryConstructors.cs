/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : C# 12 Features - Primary Constructors
 * FILE      : 01_PrimaryConstructors.cs
 * PURPOSE   : Primary constructors in C# 12
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._19_ModernCSharp._04_CSharp12_Features
{
    /// <summary>
    /// C# 12 Primary Constructors
    /// </summary>
    public class PrimaryConstructorsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Primary Constructors ===\n");

            // Primary constructor in class
            var person = new Person("John", "Doe");
            Console.WriteLine($"   Name: {person.FirstName} {person.LastName}");
            
            // Primary constructor in record
            var point = new Point(10, 20);
            Console.WriteLine($"   Point: ({point.X}, {point.Y})");

            Console.WriteLine("\n=== Primary Constructors Complete ===");
        }
    }

    /// <summary>
    /// Class with primary constructor
    /// </summary>
    public class Person(string firstName, string lastName)
    {
        public string FirstName => firstName;
        public string LastName => lastName;
    }

    /// <summary>
    /// Record with primary constructor
    /// </summary>
    public record Point(int X, int Y);
}