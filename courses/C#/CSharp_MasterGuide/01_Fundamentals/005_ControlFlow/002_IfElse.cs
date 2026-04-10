/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - If-Else Statements (Part 1)
 * FILE      : IfElse.cs
 * PURPOSE   : This file covers basic if-else control flow statements in C#.
 *             If-else is fundamental for decision making in programs.
 * ============================================================
 */

// --- SECTION: If-Else Statements ---
// If-else statements control program flow based on boolean conditions
// Essential for decision-making in every application

using System;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    class IfElse
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Simple If Statement
            // ═══════════════════════════════════════════════════════════════
            
            // Basic if - executes block only if condition is true
            int age = 25;
            
            if (age >= 18)
            {
                Console.WriteLine("You are an adult"); // This executes
            }
            
            // Condition is false - block skipped
            age = 15;
            if (age >= 18)
            {
                Console.WriteLine("You are an adult"); // This does NOT execute
            }
            
            // Single statement can omit braces (not recommended for clarity)
            int number = 10;
            if (number > 0)
                Console.WriteLine("Positive number"); // Executes
            
            if (number < 0) 
                Console.WriteLine("Negative"); // Doesn't execute

            // ═══════════════════════════════════════════════════════════════
            // SECTION: If-Else Statement
            // ═══════════════════════════════════════════════════════════════
            
            // If-else: one block always executes
            int score = 75;
            
            if (score >= 60)
            {
                Console.WriteLine("You passed!"); // Executes
            }
            else
            {
                Console.WriteLine("You failed");
            }
            
            // Using ternary-style (block without braces)
            string result = (score >= 60) ? "Pass" : "Fail";
            
            // Different approach with else
            if (score >= 90)
                Console.WriteLine("Grade: A");
            else if (score >= 80)
                Console.WriteLine("Grade: B");
            else if (score >= 70)
                Console.WriteLine("Grade: C");
            else if (score >= 60)
                Console.WriteLine("Grade: D");
            else
                Console.WriteLine("Grade: F");
            // Output: Grade: C

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nested If-Else
            // ═══════════════════════════════════════════════════════════════
            
            // Multiple conditions nested
            int hour = 14;
            
            if (hour < 12)
            {
                Console.WriteLine("Good morning");
            }
            else
            {
                if (hour < 17)
                {
                    Console.WriteLine("Good afternoon");
                }
                else
                {
                    Console.WriteLine("Good evening");
                }
            }
            // Output: Good afternoon
            
            // Cleaner: use else if
            if (hour < 12)
                Console.WriteLine("Morning");
            else if (hour < 17)
                Console.WriteLine("Afternoon");
            else if (hour < 21)
                Console.WriteLine("Evening");
            else
                Console.WriteLine("Night");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Boolean Logic in Conditions
            // ═══════════════════════════════════════════════════════════════
            
            // Combining conditions with && and ||
            bool isLoggedIn = true;
            bool isPremium = false;
            
            if (isLoggedIn && isPremium)
            {
                Console.WriteLine("Premium member area");
            }
            else if (isLoggedIn)
            {
                Console.WriteLine("Regular member area"); // This executes
            }
            else
            {
                Console.WriteLine("Please log in");
            }
            
            // Using || for either condition
            int userLevel = 1;
            bool isAdmin = false;
            
            if (userLevel >= 5 || isAdmin)
            {
                Console.WriteLine("Access granted"); // Would execute for admin
            }
            else
            {
                Console.WriteLine("Access denied");
            }
            
            // Negation with !
            bool hasPermission = false;
            
            if (!hasPermission)
            {
                Console.WriteLine("Permission denied"); // Executes
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Null Checking in Conditions
            // ═══════════════════════════════════════════════════════════════
            
            // Checking for null
            string? name = null;
            
            if (name == null)
            {
                Console.WriteLine("Name is null"); // Executes
            }
            else
            {
                Console.WriteLine($"Name: {name}");
            }
            
            // Using string.IsNullOrEmpty
            string? input = "";
            
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input is empty"); // Executes
            }
            
            // Null-conditional with null check
            int? length = name?.Length;
            
            if (length == null || length == 0)
            {
                Console.WriteLine("No name provided");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Common Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // Range checking
            int value = 75;
            
            if (value >= 0 && value <= 100)
            {
                Console.WriteLine("Value is in valid range 0-100"); // Executes
            }
            
            // Early return pattern
            bool ProcessUser(string? username)
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Invalid username");
                    return false;
                }
                
                // Continue processing...
                Console.WriteLine($"Processing: {username}");
                return true;
            }
            
            ProcessUser(null); // Output: Invalid username
            ProcessUser("John"); // Output: Processing: John
            
            // Flag-based condition
            bool errorOccurred = true;
            bool logEnabled = true;
            
            if (errorOccurred && logEnabled)
            {
                Console.WriteLine("Error logged");
            }
        }
    }
}
