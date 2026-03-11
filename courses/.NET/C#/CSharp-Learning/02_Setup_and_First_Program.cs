/*
================================================================================
TOPIC 02: SETUP AND FIRST PROGRAM
================================================================================

This file will guide you through setting up your development environment
and writing your first C# program.

TABLE OF CONTENTS:
1. Installing Visual Studio
2. Creating Your First Project
3. Understanding the Project Structure
4. Writing Your First Code
5. Running Your Program
6. Common Setup Issues
================================================================================
*/

// ================================================================================
// SECTION 1: INSTALLING VISUAL STUDIO
// ================================================================================

/*
STEP 1: Download Visual Studio
-------------------------------
1. Go to https://visualstudio.microsoft.com/downloads/
2. Click "Free Download" under Visual Studio 2022 Community
3. Run the downloaded installer

STEP 2: Install with .NET Desktop Development
---------------------------------------------
When the installer launches:
1. Check the box for ".NET desktop development"
   (This includes the C# compiler and all needed tools)
2. Click "Install" button
3. Wait for installation to complete (may take 15-30 minutes)
4. Launch Visual Studio when installed

STEP 3: Sign In (Optional)
---------------------------
- You can create a free Microsoft account
- Or click "Not now, maybe later" to skip

REAL-WORLD ANALOGY:
-------------------
Installing Visual Studio is like setting up a workshop:
- The installer brings all your tools (hammer, saw, drill)
- .NET desktop development workload is like getting specialized tools
  for woodworking
- Now you're ready to build things!
*/


// ================================================================================
// SECTION 2: CREATING YOUR FIRST PROJECT
// ================================================================================

/*
Let's create your first C# project step by step:

STEP 1: Open Visual Studio
---------------------------
- Launch Visual Studio 2022
- You'll see the "Start Window"

STEP 2: Create New Project
---------------------------
- Click "Create a new project" (first option)
- OR use keyboard shortcut: Ctrl + Shift + N

STEP 3: Select Template
------------------------
- In the search box, type "Console App"
- Select "Console App (.NET)" - NOT the .NET Framework version
- Click "Next"

STEP 4: Configure Project
--------------------------
- Project name: FirstProgram (use PascalCase - no spaces)
- Location: Choose where to save (e.g., Documents\CSharpProjects)
- Check "Place solution and project in the same directory"
- Click "Create"

CONGRATULATIONS! You've created your first C# project!

WHAT JUST HAPPENED:
-------------------
Visual Studio created several files for you:
- FirstProgram.sln (solution file - container for your project)
- FirstProgram/FirstProgram.csproj (project file - tells Visual Studio 
  how to build your project)
- FirstProgram/Program.cs (your actual code file)
- FirstProgram/AssemblyInfo.cs (metadata about your program)
*/


// ================================================================================
// SECTION 3: UNDERSTANDING THE PROJECT STRUCTURE
// ================================================================================

/*
Let's explore what Visual Studio created:

FILE STRUCTURE:
---------------
Solution 'FirstProgram'              (The container)
    └── Project 'FirstProgram'       (Your actual project)
        ├── Dependencies            (External libraries)
        ├── Program.cs              (Your source code)
        ├── FirstProgram.csproj     (Project settings)
        └── AssemblyInfo.cs        (Program metadata)

THE .csproj FILE:
-----------------
This is the project file. It looks something like:

<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>      <!-- Console application -->
    <TargetFramework>net8.0</TargetFramework>  <!-- .NET version -->
  </PropertyGroup>
</Project>

KEY CONCEPTS:
- OutputType: Exe means it creates an executable
- TargetFramework: The .NET version (we're using .NET 8.0)
- SDK: Microsoft.NET.Sdk provides all the built-in features
*/


// ================================================================================
// SECTION 4: WRITING YOUR FIRST CODE
// ================================================================================

// Here's the default code that Visual Studio creates:

using System;  // Import the System namespace

namespace FirstProgram  // Your namespace
{
    class Program  // Your class
    {
        // This is the Main method - entry point of your program
        static void Main(string[] args)
        {
            // Print a welcome message
            Console.WriteLine("Hello, World!");
        }
    }
}

/*
CODE BREAKDOWN:
---------------
1. using System;
   - This imports the System namespace
   - Gives us access to Console, Math, etc.
   - Think of it as importing a library

2. namespace FirstProgram
   - Organizes your code
   - Like a folder for related files
   - Namespaces prevent name conflicts

3. class Program
   - A class is a blueprint for creating objects
   - Every C# program needs at least one class
   - We'll learn more about classes later

4. static void Main(string[] args)
   - Main is the starting point of your program
   - When you run the program, execution starts here
   - "static" means you don't need to create an object to call it
   - "void" means it doesn't return any value
   - "string[] args" holds command-line arguments

5. Console.WriteLine("Hello, World!");
   - Console is a class for console input/output
   - WriteLine prints text and adds a new line
   - This is the famous "Hello World" program!
*/


// ================================================================================
// SECTION 5: RUNNING YOUR PROGRAM
// ================================================================================

/*
Now let's run your program:

METHOD 1: Using the Green Play Button
--------------------------------------
1. Press the green "Start" button (or F5)
2. A console window will appear with "Hello, World!"
3. Press any key to close the window

METHOD 2: Using Debug Menu
---------------------------
1. Go to Debug > Start Debugging (or press F5)

METHOD 3: Using Terminal
-------------------------
1. Open terminal/command prompt
2. Navigate to your project folder
3. Run: dotnet run

WHAT HAPPENS WHEN YOU RUN:
--------------------------
1. C# compiler (csc.exe) compiles your code
2. Creates an executable file (.exe)
3. .NET runtime loads your program
4. Main method is called
5. Console.WriteLine displays text
6. Program ends

ERROR: Program crashes?
- Check the Error List (View > Error List)
- Common issues: syntax errors, missing files
*/


// ================================================================================
// SECTION 6: PRACTICAL EXAMPLES
// ================================================================================

// Let's modify our first program with more examples:

namespace FirstProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example 1: Simple output
            Console.WriteLine("Hello, World!");
            
            // Example 2: Multiple outputs
            Console.WriteLine("Line 1");
            Console.WriteLine("Line 2");
            Console.WriteLine("Line 3");
            
            // Example 3: Empty line
            Console.WriteLine();
            
            // Example 4: Numbers (no quotes!)
            Console.WriteLine(12345);
            
            // Example 5: Math calculation
            Console.WriteLine(10 + 5);
            
            // Example 6: Using Console.Write (without line break)
            Console.Write("This is ");
            Console.Write("on the ");
            Console.WriteLine("same line!");
            
            // Example 7: Reading input
            Console.WriteLine("\n--- User Input Example ---");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();  // Wait for user input
            Console.WriteLine("Hello, " + name + "!");
            
            // Example 8: Multiple inputs
            Console.Write("Enter your age: ");
            string ageInput = Console.ReadLine();
            Console.WriteLine("You are " + ageInput + " years old.");
        }
    }
}

/*
NOTE ON READLINE:
-----------------
Console.ReadLine() pauses the program and waits for the user to type
something and press Enter. The typed text is returned as a string.
We'll cover this more in the Variables topic.

IMPORTANT: When you use Console.ReadLine() at the end of your program,
the console window will stay open so you can see the output!
*/


// ================================================================================
// SECTION 7: COMMON SETUP ISSUES
// ================================================================================

/*
PROBLEM 1: "dotnet is not recognized"
-------------------------------------
SOLUTION: 
- Install .NET SDK from https://dotnet.microsoft.com/download
- Restart your computer after installation
- Verify by running: dotnet --version

PROBLEM 2: "Could not find SDK"
--------------------------------
SOLUTION:
- Open Visual Studio Installer
- Click "Modify" on your installation
- Make sure ".NET desktop development" is checked
- Click "Modify" to apply changes

PROBLEM 3: Console window closes immediately
---------------------------------------------
SOLUTION:
- Add Console.ReadLine() at the end of Main
- OR run with Ctrl + F5 (Start Without Debugging)
- OR add Console.ReadKey() before the end

PROBLEM 4: "The build failed"
-----------------------------
SOLUTION:
- Check Error List for specific errors
- Common cause: missing using statements
- Common cause: typos or syntax errors

PROBLEM 5: Wrong .NET version
------------------------------
SOLUTION:
- Right-click project in Solution Explorer
- Select Properties
- Change Target Framework to .NET 8.0 (or latest installed)
*/


// ================================================================================
// SECTION 8: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Personal Greeting
------------------------------
Create a program that:
1. Asks for the user's name
2. Asks for their favorite color
3. Prints: "Hello [name]! Your favorite color is [color]!"

EXERCISE 2: Simple Calculator
-----------------------------
Create a program that:
1. Prints "5 + 3 = " followed by the answer
2. Prints "10 - 7 = " followed by the answer
3. Prints "4 * 6 = " followed by the answer
4. Prints "20 / 4 = " followed by the answer

EXERCISE 3: Mini Bio
--------------------
Create a program that prints a mini biography:
"Your Name"
"Your Age"
"Your City"
Something you like
Something you dislike

Example:
My name is John
I am 25 years old
I live in New York
I love coding
I hate bugs
*/


// ================================================================================
// SECTION 9: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the entry point of a C# program?
A: The Main method is the entry point. It's called when the program starts.
   It must be static and can accept command-line arguments as string[] args.

Q2: What is the difference between Console.WriteLine and Console.Write?
A: Console.WriteLine prints text and adds a new line at the end.
   Console.Write prints text but stays on the same line.

Q3: What is a namespace in C#?
A: A namespace is a way to organize code into logical groups. It helps
   prevent naming conflicts and makes code easier to navigate.

Q4: What is the purpose of "using System;"?
A: It imports the System namespace, which contains fundamental classes
   like Console, Math, and basic data types. This allows us to use
   these classes without fully qualifying them.

Q5: What is the difference between .NET Framework and .NET Core/.NET?
A: .NET Framework is Windows-only and older. .NET Core/.NET (now just ".NET")
   is cross-platform, open-source, and faster. For new projects, use .NET.
*/


// ================================================================================
// NEXT STEPS
// ================================================================================

/*
EXCELLENT! You now know how to:
- Install Visual Studio
- Create a C# project
- Write and run basic code

WHAT'S NEXT:
In Topic 03, we'll dive into Variables and Data Types - the foundation
of storing and working with data in C#. You'll learn about different
types of data and how to use them effectively.

Keep practicing creating and running projects. The more you do it,
the more comfortable you'll become!
*/
