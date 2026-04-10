/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - Switch Statement
 * FILE      : SwitchStatement.cs
 * PURPOSE   : This file covers traditional switch statements in C# with case guards,
 *             multiple values, and fall-through behavior.
 * ============================================================
 */

// --- SECTION: Switch Statements ---
// Switch statements provide multi-way branching based on a value
// They are cleaner than multiple if-else when comparing against many values

using System;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    class SwitchStatement
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Switch Statement
            // ═══════════════════════════════════════════════════════════════
            
            // Simple switch on integer
            int month = 3;
            string monthName;
            
            switch (month)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
                default:
                    monthName = "Invalid month";
                    break;
            }
            
            Console.WriteLine($"Month {month} is {monthName}"); // Output: Month 3 is March

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch with String
            // ═══════════════════════════════════════════════════════════════
            
            string command = "start";
            
            switch (command.ToLower())
            {
                case "start":
                    Console.WriteLine("Starting service...");
                    break;
                case "stop":
                    Console.WriteLine("Stopping service...");
                    break;
                case "restart":
                    Console.WriteLine("Restarting service...");
                    break;
                case "status":
                    Console.WriteLine("Checking status...");
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Multiple Values in Case
            // ═══════════════════════════════════════════════════════════════
            
            // Single case can match multiple values
            char grade = 'B';
            
            switch (grade)
            {
                case 'A':
                case 'a':
                    Console.WriteLine("Excellent - 90-100%");
                    break;
                case 'B':
                case 'b':
                    Console.WriteLine("Good - 80-89%");
                    break;
                case 'C':
                case 'c':
                    Console.WriteLine("Average - 70-79%");
                    break;
                case 'D':
                case 'd':
                    Console.WriteLine("Below average - 60-69%");
                    break;
                case 'F':
                case 'f':
                    Console.WriteLine("Failed - below 60%");
                    break;
                default:
                    Console.WriteLine("Invalid grade");
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Case Guards
            // ═══════════════════════════════════════════════════════════════
            
            // Case guards allow conditions in switch cases
            int score = 85;
            
            switch (score)
            {
                case int n when n >= 90:
                    Console.WriteLine("Grade: A");
                    break;
                case int n when n >= 80:
                    Console.WriteLine("Grade: B");
                    break;
                case int n when n >= 70:
                    Console.WriteLine("Grade: C");
                    break;
                case int n when n >= 60:
                    Console.WriteLine("Grade: D");
                    break;
                default:
                    Console.WriteLine("Grade: F");
                    break;
            }
            
            // Multiple conditions with when
            object obj = "hello";
            
            switch (obj)
            {
                case string s when s.Length > 5:
                    Console.WriteLine("Long string");
                    break;
                case string s when s.Length <= 5:
                    Console.WriteLine("Short string");
                    break;
                case int i when i > 0:
                    Console.WriteLine("Positive integer");
                    break;
                default:
                    Console.WriteLine("Unknown type or value");
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch with Enum
            // ═══════════════════════════════════════════════════════════════
            
            // Switch works well with enums
            DayOfWeek today = DayOfWeek.Wednesday;
            
            switch (today)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    Console.WriteLine("Weekday - work day");
                    break;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    Console.WriteLine("Weekend - rest day");
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch with Goto
            // ═══════════════════════════════════════════════════════════════
            
            // goto case - jump to another case
            int option = 2;
            
            switch (option)
            {
                case 1:
                    Console.WriteLine("Option 1");
                    goto case 2; // Falls through to case 2
                case 2:
                    Console.WriteLine("Option 2 (or from 1)");
                    break;
                case 3:
                    goto default;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            
            // goto label - jump to specific label
            int num = 5;
            
            switch (num)
            {
                case 1:
                    Console.WriteLine("One");
                    break;
                case 2:
                    Console.WriteLine("Two");
                    break;
                default:
                    Console.WriteLine("Other");
                    goto afterSwitch;
            }
            
            afterSwitch:
            Console.WriteLine("After switch");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch Scope
            // ═══════════════════════════════════════════════════════════════
            
            // Variables declared in case have local scope
            int value = 10;
            
            switch (value)
            {
                case 10:
                    string message = "Ten"; // Only in this case block
                    Console.WriteLine(message);
                    break;
                case 20:
                    // string message; // ERROR: message not in scope
                    Console.WriteLine("Twenty");
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── HTTP status code handling ───────────────────────────────────
            int statusCode = 404;
            
            switch (statusCode)
            {
                case 200:
                    Console.WriteLine("OK - Success");
                    break;
                case 201:
                    Console.WriteLine("Created - Resource created");
                    break;
                case 204:
                    Console.WriteLine("No Content - Success, no body");
                    break;
                case 400:
                    Console.WriteLine("Bad Request - Invalid input");
                    break;
                case 401:
                    Console.WriteLine("Unauthorized - Please login");
                    break;
                case 403:
                    Console.WriteLine("Forbidden - No permission");
                    break;
                case 404:
                    Console.WriteLine("Not Found - Resource missing");
                    break;
                case 500:
                    Console.WriteLine("Internal Server Error");
                    break;
                default:
                    Console.WriteLine($"Unknown status: {statusCode}");
                    break;
            }
            
            // ── Menu system ───────────────────────────────────────────────
            string menuOption = "3";
            
            switch (menuOption)
            {
                case "1":
                    Console.WriteLine("1. New Game");
                    break;
                case "2":
                    Console.WriteLine("2. Load Game");
                    break;
                case "3":
                    Console.WriteLine("3. Options");
                    break;
                case "4":
                    Console.WriteLine("4. Exit");
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }
}
