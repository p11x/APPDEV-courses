/*
 * TOPIC: Extension Methods
 * SUBTOPIC: Extension Method Basics
 * FILE: ExtensionMethodBasics.cs
 * PURPOSE: Demonstrates the fundamentals of extension methods including static class requirements,
 *          the 'this' keyword, and basic declaration patterns.
 */

using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._05_ExtensionMethods
{
    // Extension methods must be declared in a static class
    public static class StringExtensions
    {
        // This is an extension method - notice the 'this' keyword before the first parameter
        // The 'this' parameter specifies the type being extended (string in this case)
        // This allows us to call: "hello".ToTitleCase()
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Convert to title case: first letter uppercase, rest lowercase
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        // Extension method with additional parameters
        // The 'this' parameter is always the first one
        public static string Truncate(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Return truncated string with ellipsis if longer than maxLength
            return input.Length <= maxLength ? input : input.Substring(0, maxLength) + "...";
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public static class PersonExtensions
    {
        // Extending a custom class - Person
        public static string GetFullName(this Person person)
        {
            return $"{person.FirstName} {person.LastName}";
        }

        // Extension method with return type and parameters
        public static int GetFullNameLength(this Person person)
        {
            return person.GetFullName().Length;
        }
    }

    class ExtensionMethodBasics
    {
        static void Main(string[] args)
        {
            // Using extension method on string
            string name = "john doe";
            string titleCased = name.ToTitleCase();
            Console.WriteLine($"Original: {name}");
            Console.WriteLine($"Title Case: {titleCased}");
            // Output: Original: john doe
            // Output: Title Case: John Doe

            // Using extension method with parameter
            string longText = "This is a very long text that needs to be truncated";
            string truncated = longText.Truncate(20);
            Console.WriteLine($"Truncated: {truncated}");
            // Output: Truncated: This is a very long...

            // Using extension method on custom class
            Person person = new Person("Jane", "Smith");
            string fullName = person.GetFullName();
            Console.WriteLine($"Full Name: {fullName}");
            // Output: Full Name: Jane Smith

            // Using extension method that calls another extension method
            int nameLength = person.GetFullNameLength();
            Console.WriteLine($"Name Length: {nameLength}");
            // Output: Name Length: 10
        }
    }
}
