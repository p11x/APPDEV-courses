/*
================================================================================
TOPIC 01: INTRODUCTION TO C#
================================================================================

This file introduces you to the world of C# programming.
By the end, you'll understand what C# is, why it's popular, and its key features.

TABLE OF CONTENTS:
1. What is C#?
2. History and Evolution
3. Why Learn C#?
4. Key Features
5. C# vs Other Languages
6. Setting Expectations
================================================================================
*/

// ================================================================================
// SECTION 1: WHAT IS C#?
// ================================================================================

/*
C# (pronounced "C Sharp") is a modern, object-oriented programming language
developed by Microsoft as part of its .NET initiative.

Think of C# as a tool for giving instructions to computers. Just like how
you might give directions to a friend, C# allows you to write instructions
that the computer can understand and execute.

REAL-WORLD ANALOGY:
-------------------
If you think of a computer as a very obedient but literal worker:
- English: "Can you make me a coffee?" (too vague)
- C#: "Fill kettle with 500ml water, boil it, add 1 spoon coffee, 
       add 1 spoon sugar, stir 10 times, pour into blue mug" (precise)

C# is like creating a detailed recipe that the computer follows exactly.
*/


// ================================================================================
// SECTION 2: HISTORY AND EVOLUTION
// ================================================================================

/*
C# was created in the early 2000s by a team led by Anders Hejlsberg
(who also created Turbo Pascal and Delphi).

TIMELINE:
---------
- 2000: C# 1.0 released with .NET Framework 1.0
- 2003: C# 1.2 with .NET Framework 1.1
- 2005: C# 2.0 - Added generics, nullable types
- 2007: C# 3.0 - Added LINQ, lambda expressions
- 2010: C# 4.0 - Added dynamic typing
- 2012: C# 5.0 - Added async/await
- 2015: C# 6.0 - Many syntax improvements
- 2017: C# 7.0 - Tuples, pattern matching
- 2019: C# 8.0 - Nullable reference types
- 2020: C# 9.0 - Records, init only
- 2021: C# 10.0 - Record structs, global usings
- 2023: C# 12.0 - Primary constructors, collection expressions

CURRENT VERSION: C# 12.0 (released with .NET 8.0)

FUN FACT: The name "C#" was inspired by musical notation where "#" means
"sharp" - indicating C# is "one step higher" than C++!
*/


// ================================================================================
// SECTION 3: WHY LEARN C#?
// ================================================================================

/*
C# IS ONE OF THE MOST POPULAR PROGRAMMING LANGUAGES FOR GOOD REASONS:

1. JOB MARKET:
   - High demand in enterprise software development
   - Competitive salaries (average $90,000+ in US)
   - Thousands of job openings worldwide

2. VERSATILITY:
   - Web applications (ASP.NET Core)
   - Desktop applications (WPF, WinForms)
   - Mobile apps (Xamarin, .NET MAUI)
   - Games (Unity)
   - Cloud services (Azure)
   - AI/ML (.NET AI)

3. STRONG ECOSYSTEM:
   - Extensive libraries and frameworks
   - Excellent tooling (Visual Studio)
   - Large community support
   - Comprehensive documentation

4. MODERN LANGUAGE:
   - Regular updates with new features
   - Strong type safety
   - Memory management included
   - Cross-platform (.NET Core/5+)
*/


// ================================================================================
// SECTION 4: KEY FEATURES OF C#
// ================================================================================

/*
C# has many powerful features that make it great for development:

1. OBJECT-ORIENTED: Everything in C# revolves around objects and classes.
   You'll organize your code into logical units.

2. TYPE-SAFE: C# prevents you from making common programming mistakes
   by enforcing strict type checking at compile time.

3. MEMORY MANAGEMENT: No need to manually free memory - C# has a
   Garbage Collector that handles cleanup automatically.

4. COMPONENT-ORIENTED: C# was designed to create reusable software
   components that can be easily shared and reused.

5. MODERN SYNTAX: Clean, expressive syntax that's easy to read and write.

6. ASYNCHRONOUS SUPPORT: Built-in support for async/await makes
   writing responsive applications easy.

7. LINQ: Language Integrated Query allows you to query data in a
   SQL-like manner directly in C# code.

8. DYNAMIC FEATURES: Support for dynamic types, lambda expressions,
   and functional programming patterns.
*/


// ================================================================================
// SECTION 5: C# VS OTHER LANGUAGES
// ================================================================================

/*
Let's compare C# with other popular languages:

C# vs Java:
-----------
- Very similar syntax (both derived from C++)
- Both run on virtual machines (CLR vs JVM)
- C# has more modern features in some areas
- Java is more platform-agnostic historically

C# vs Python:
-------------
- C# is statically typed, Python is dynamically typed
- C# requires compilation, Python is interpreted
- C# is faster for compiled code
- Python has simpler syntax for beginners

C# vs C++:
----------
- C# is managed (automatic memory management)
- C++ gives more control but is more complex
- C# is safer with built-in bounds checking
- C++ is used for system-level programming

BOTTOM LINE: C# offers a great balance of power, safety, and ease of use.
It's professional-grade but still beginner-friendly.
*/


// ================================================================================
// SECTION 6: PRACTICAL EXAMPLES
// ================================================================================

// Let's look at some simple C# code to see what it looks like:

// Example 1: Simple output to console
// This is the classic "Hello World" program that every programmer writes first

using System;  // This imports the System namespace for basic functionality

namespace CSharpIntroduction
{
    class Program
    {
        static void Main(string[] args)
        {
            // This line prints text to the console
            Console.WriteLine("Hello, World!");
            
            // You can also print numbers
            Console.WriteLine(42);
            
            // And mathematical expressions
            Console.WriteLine(5 + 3);
            
            // Variables (we'll learn about these in detail later)
            string myName = "Student";
            int myAge = 25;
            
            // Printing with variables
            Console.WriteLine("My name is " + myName);
            Console.WriteLine("I am " + myAge + " years old");
            
            // Using string interpolation (a modern C# feature)
            Console.WriteLine($"Hello, my name is {myName} and I am {myAge}!");
            
            // Arrays (collections of items - we'll cover this later)
            string[] favoriteLanguages = { "C#", "Python", "JavaScript" };
            Console.WriteLine("I love " + favoriteLanguages[0] + " the most!");
        }
    }
}

/*
CODE BREAKDOWN:
---------------
1. using System; - Imports the System library which provides basic functionality
2. namespace - Organizes code into logical groups
3. class - A blueprint for creating objects (more on this later)
4. Main() - The entry point where program execution begins
5. Console.WriteLine() - Prints text to the screen and moves to new line
6. string - A text data type
7. int - A whole number data type
8. $"" - String interpolation for embedding variables in text

Try running this code to see the output!
*/


// ================================================================================
// SECTION 7: ANATOMY OF A C# PROGRAM
// ================================================================================

/*
Every C# program follows this basic structure:

1. USING STATEMENTS:
   using System;          // Import built-in functionality
   using System.Collections.Generic;  // Import collections
   
2. NAMESPACE:
   namespace MyProject    // Logical organization of code
   
3. CLASS:
   class Program          // A class contains methods and data
   
4. MAIN METHOD:
   static void Main()     // Entry point - where program starts
   
5. METHODS:
   void DoSomething()     // Actions the program can perform

Think of it like a book:
- Namespace = The book title
- Class = A chapter
- Methods = Sections within chapters
- Variables = Facts and details
*/


// ================================================================================
// SECTION 8: COMMON MISTAKES TO AVOID
// ================================================================================

/*
MISTAKE 1: Forgetting the semicolon
-----------------------------------
C# requires a semicolon at the end of each statement.
WRONG:  Console.WriteLine("Hello")
RIGHT:  Console.WriteLine("Hello");

MISTAKE 2: Case sensitivity
----------------------------
C# is case-sensitive. "Console" is different from "console".
WRONG:  console.WriteLine("Hello")
RIGHT:  Console.WriteLine("Hello");

MISTAKE 3: Not matching braces
-------------------------------
Every opening brace { must have a closing brace }.
WRONG:  if (true) {
            Console.WriteLine("Yes");
RIGHT:  if (true) {
            Console.WriteLine("Yes");
        }

MISTAKE 4: Using wrong data type
---------------------------------
You can't put text in an integer variable.
WRONG:  int number = "Hello";
RIGHT:  string text = "Hello";
*/


// ================================================================================
// SECTION 9: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Hello Program
--------------------------
Create a program that prints your name, favorite color, and favorite food.

EXERCISE 2: Simple Math
------------------------
Create a program that performs and prints:
- 15 + 27
- 100 - 43
- 8 * 7
- 144 / 12

EXERCISE 3: Personal Info
-------------------------
Create variables for:
- Your name (string)
- Your age (int)
- Your favorite number (double)
- Whether you like programming (bool)

Print all of them using string interpolation like: $"My name is {name}"
*/


// ================================================================================
// SECTION 10: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is C# and what is it used for?
A: C# is a modern, object-oriented programming language developed by Microsoft.
   It's used for building Windows applications, web services, games (Unity),
   mobile apps, and enterprise software. It's part of the .NET ecosystem.

Q2: Who created C# and when?
A: C# was created by Anders Hejlsberg and his team at Microsoft, first
   released in 2002 as part of .NET Framework 1.0.

Q3: What are the main features of C#?
A: Key features include: object-oriented programming, type safety, automatic
   memory management (garbage collection), LINQ, async/await, and strong
   tooling support through Visual Studio.

Q4: What is the difference between C# and .NET?
A: C# is the programming language, while .NET is the framework/platform that
   provides the runtime and libraries. C# code runs on the .NET runtime (CLR).

Q5: Why should I learn C#?
A: C# has strong job demand, works great for various application types,
   has excellent tooling, and provides a solid foundation for learning
   other .NET technologies like ASP.NET, Xamarin, and Unity.
*/


// ================================================================================
// NEXT STEPS
// ================================================================================

/*
GREAT JOB! You've completed the introduction to C#.

WHAT YOU LEARNED:
- What C# is and its history
- Why C# is a great language to learn
- Key features that make C# powerful
- Basic program structure
- Common mistakes to avoid

WHAT'S NEXT:
In Topic 02, we'll set up your development environment and write your
first actual C# program. You'll learn how to install Visual Studio and
run your first "Hello World" application.

Keep practicing the concepts you've learned here. See you in the next topic!
*/
