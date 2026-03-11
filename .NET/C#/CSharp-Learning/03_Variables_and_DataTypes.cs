/*
================================================================================
TOPIC 03: VARIABLES AND DATA TYPES
================================================================================

Variables and data types are the foundation of programming. This topic
teaches you how to store, manipulate, and work with different kinds of data.

TABLE OF CONTENTS:
1. What are Variables?
2. Data Types Overview
3. Primitive Data Types
4. Declaring and Initializing Variables
5. Constants
6. Var Keyword
7. Nullable Types
8. Practice Examples
================================================================================
*/

// ================================================================================
// SECTION 1: WHAT ARE VARIABLES?
// ================================================================================

/*
VARIABLES DEFINED:
------------------
A variable is a named storage location in memory that holds a value.
Think of it as a labeled box where you can put things.

REAL-WORLD ANALOGY:
-------------------
Imagine you have storage boxes in your garage:
- Box labeled "Tools" - holds hammers, screwdrivers, etc.
- Box labeled "Paint" - holds different colors of paint
- Box labeled "Documents" - holds papers

In programming:
- "Tools" = variable name
- "Hammer" = the value stored
- The box itself = the memory location

VARIABLE PROPERTIES:
--------------------
1. NAME: How you identify the variable (must be unique)
2. TYPE: What kind of data it can hold
3. VALUE: The actual data stored
4. MEMORY: Where it's stored in computer RAM

WHY USE VARIABLES?
------------------
1. Store data for later use
2. Make code readable and maintainable
3. Perform calculations
4. Handle user input
5. Reuse values multiple times
*/


// ================================================================================
// SECTION 2: DATA TYPES OVERVIEW
// ================================================================================

/*
C# is a STRONGLY TYPED language - every variable has a specific type
that must be declared.

DATA TYPE CATEGORIES:
---------------------
1. VALUE TYPES (stored on the stack):
   - Integers (whole numbers)
   - Floating-point (decimals)
   - Characters
   - Booleans
   - Structures

2. REFERENCE TYPES (stored on the heap):
   - Strings
   - Arrays
   - Classes
   - Interfaces

3. CUSTOM TYPES:
   - Enumerations
   - Structures
   - Records

FOR BEGINNERS, WE'LL FOCUS ON PRIMITIVE TYPES FIRST!
*/


// ================================================================================
// SECTION 3: PRIMITIVE DATA TYPES
// ================================================================================

/*
C# PRIMITIVE TYPES CHEAT SHEET:
-------------------------------

INTEGER TYPES (Whole Numbers):
------------------------------
Type      | Size    | Range                           | Example
----------|---------|---------------------------------|------------
byte      | 8-bit   | 0 to 255                       | 42
short     | 16-bit  | -32,768 to 32,767              | 1000
int       | 32-bit  | -2.1B to 2.1B                   | 42
long      | 64-bit  | -9.2Q to 9.2Q                   | 42L

DECIMAL TYPES (Numbers with Decimals):
--------------------------------------
Type      | Size    | Precision                       | Example
----------|---------|---------------------------------|------------
float     | 32-bit  | 7 digits                        | 3.14f
double    | 64-bit  | 15 digits                       | 3.14159
decimal   | 128-bit | 28 digits (for money!)          | 3.14m

OTHER TYPES:
------------
Type      | Size    | Values                          | Example
----------|---------|---------------------------------|------------
char      | 16-bit  | Single character                | 'A'
bool      | 8-bit   | true or false                   | true
string    | varies  | Text                            | "Hello"

DECIDING WHICH TYPE TO USE:
---------------------------
- int: Most common for whole numbers
- double: Most common for decimal calculations
- decimal: Financial/monetary calculations (no rounding errors!)
- bool: True/false conditions
- string: Text
- char: Single character
*/


// ================================================================================
// SECTION 4: DECLARING AND INITIALIZING VARIABLES
// ================================================================================

namespace VariablesAndDataTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // DECLARING VARIABLES
            // ====================================================================
            
            // Syntax: dataType variableName;
            
            int age;              // Declares an integer variable named "age"
            string name;          // Declares a string variable named "name"
            double height;        // Declares a double variable named "height"
            bool isStudent;       // Declares a bool variable named "isStudent"
            char grade;           // Declares a char variable named "grade"
            
            // ====================================================================
            // INITIALIZING VARIABLES
            // ====================================================================
            
            // Assigning values using the assignment operator (=)
            
            age = 25;             // Assign 25 to age
            name = "John";        // Assign "John" to name
            height = 5.9;         // Assign 5.9 to height
            isStudent = true;     // Assign true to isStudent
            grade = 'A';          // Assign 'A' to grade
            
            // ====================================================================
            // DECLARING AND INITIALIZING IN ONE LINE
            // ====================================================================
            
            // Syntax: dataType variableName = value;
            
            int count = 10;           // Declare and initialize in one line
            string city = "New York";
            double price = 19.99;
            bool isActive = false;
            char initial = 'J';
            
            // ====================================================================
            // MULTIPLE VARIABLES IN ONE LINE
            // ====================================================================
            
            int a = 1, b = 2, c = 3;           // Same type
            string firstName = "John", lastName = "Doe";
            
            // ====================================================================
            // DISPLAYING VARIABLE VALUES
            // ====================================================================
            
            Console.WriteLine("=== Displaying Variables ===");
            Console.WriteLine("Age: " + age);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Height: " + height);
            Console.WriteLine("Is Student: " + isStudent);
            Console.WriteLine("Grade: " + grade);
            
            // String Interpolation (Modern C# way - recommended!)
            Console.WriteLine($"\n=== Using String Interpolation ===");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Height: {height}");
            Console.WriteLine($"Is Student: {isStudent}");
            
            // ====================================================================
            // CHANGING VARIABLE VALUES
            // ====================================================================
            
            Console.WriteLine("\n=== Changing Values ===");
            Console.WriteLine($"Original age: {age}");
            
            age = 30;                  // Change the value of age
            Console.WriteLine($"New age: {age}");
            
            // Variables can be used to calculate other variables
            int x = 10;
            int y = 20;
            int sum = x + y;           // sum will be 30
            Console.WriteLine($"Sum of {x} and {y} is {sum}");
        }
    }
}

/*
KEY CONCEPTS DEMONSTRATED:
--------------------------
1. Declaration: Creating a variable (int age;)
2. Initialization: Giving it a first value (age = 25;)
3. Both in one line: (int age = 25;)
4. String concatenation: "Age: " + age
5. String interpolation: $"Age: {age}"
6. Reassignment: Changing the value later
*/


// ================================================================================
// SECTION 5: CONSTANTS
// ================================================================================

namespace ConstantsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // CONSTANTS - Values that cannot change
            // ====================================================================
            
            // Use "const" keyword to declare a constant
            // Convention: Use UPPER_SNAKE_CASE for constant names
            
            const double PI = 3.14159;
            const int MAX_SCORE = 100;
            const string COMPANY_NAME = "Tech Corp";
            
            // Using constants in calculations
            double radius = 5.0;
            double area = PI * radius * radius;
            
            Console.WriteLine($"Circle with radius {radius}");
            Console.WriteLine($"Area: {area}");
            Console.WriteLine($"Max Score: {MAX_SCORE}");
            Console.WriteLine($"Company: {COMPANY_NAME}");
            
            // ERROR! You cannot change a constant:
            // PI = 3.14;  // This will cause a compile error!
            
            // Use constants when:
            // - You have a value that never changes
            // - You want to prevent accidental changes
            // - You want meaningful names for magic numbers
            // Example: const int DAYS_IN_WEEK = 7;
        }
    }
}

/*
WHY USE CONSTANTS?
------------------
1. Prevents mistakes - Can't accidentally change the value
2. Self-documenting - const int MAX_SPEED = 100 is clearer than just 100
3. Single source - Change in one place, applies everywhere
4. Performance - Compiler can optimize constant expressions
*/


// ================================================================================
// SECTION 6: VAR KEYWORD (TYPE INFERENCE)
// ================================================================================

namespace VarKeywordExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // VAR KEYWORD - Let the compiler figure out the type
            // ====================================================================
            
            // Instead of: int age = 25;
            // You can write:
            var age = 25;              // Compiler knows this is int
            var name = "John";         // Compiler knows this is string
            var price = 19.99;         // Compiler knows this is double
            var isValid = true;        // Compiler knows this is bool
            
            // You CANNOT use var without initialization!
            // var uninitialized;      // ERROR - must be initialized!
            
            // When to use var:
            // - When type is obvious from the right side
            // - When using complex generic types
            // - For LINQ queries
            
            // When NOT to use var:
            // - When type isn't clear from initialization
            // - For beginner learning (explicit types are clearer!)
            
            Console.WriteLine($"Age: {age} (type: {age.GetType().Name})");
            Console.WriteLine($"Name: {name} (type: {name.GetType().Name})");
            Console.WriteLine($"Price: {price} (type: {price.GetType().Name})");
            Console.WriteLine($"IsValid: {isValid} (type: {isValid.GetType().Name})");
        }
    }
}

/*
VAR KEYWORD NOTES:
------------------
- "var" is NOT "variant" or "dynamic"
- The type is determined at COMPILE TIME
- It's just syntactic sugar - the compiled code is identical
- Use sparingly when learning - explicit types are clearer!
*/


// ================================================================================
// SECTION 7: NULLABLE TYPES
// ================================================================================

namespace NullableTypesExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // NULLABLE TYPES - When you need to represent "no value"
            // ====================================================================
            
            // In C#, value types cannot normally be null
            // Use ? to make them nullable
            
            int? nullableInt = null;           // Can hold int OR null
            double? nullableDouble = null;    // Can hold double OR null
            bool? nullableBool = null;         // Can hold bool OR null
            
            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Nullable double: {nullableDouble}");
            Console.WriteLine($"Nullable bool: {nullableBool}");
            
            // Assigning values
            nullableInt = 42;
            nullableDouble = 3.14;
            nullableBool = true;
            
            Console.WriteLine("\nAfter assignment:");
            Console.WriteLine($"Nullable int: {nullableInt}");
            Console.WriteLine($"Nullable double: {nullableDouble}");
            Console.WriteLine($"Nullable bool: {nullableBool}");
            
            // Checking for null
            int? userInput = null;
            
            if (userInput.HasValue)
            {
                Console.WriteLine($"User entered: {userInput.Value}");
            }
            else
            {
                Console.WriteLine("No value was provided");
            }
            
            // Null coalescing operator (??)
            int result = userInput ?? 0;       // If null, use 0
            Console.WriteLine($"Result with ??: {result}");
            
            // Null conditional operator (?.)
            string text = null;
            int? length = text?.Length;        // Returns null if text is null
            Console.WriteLine($"Length (with ?.): {length}");
        }
    }
}

/*
WHY NULLABLE TYPES?
-------------------
Sometimes you need to represent "no value" or "unknown":
- User hasn't entered data yet
- Database value is missing
- Optional parameter not provided
- Operation failed

Real-world: A form field that wasn't filled in = null
*/


// ================================================================================
// SECTION 8: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Using uninitialized variables
----------------------------------------
int x;
Console.WriteLine(x);    // ERROR! x has no value yet

FIX: Always initialize before use:
int x = 0;
Console.WriteLine(x);    // OK!


MISTAKE 2: Wrong type assignment
---------------------------------
int number = "Hello";    // ERROR! Can't put string in int
string text = 42;        // ERROR! Can't put int in string

FIX: Match types or use conversion:
int number = int.Parse("42");  // Convert string to int
string text = 42.ToString();   // Convert int to string


MISTAKE 3: Using = instead of ==
--------------------------------------
if (x = 5)   // ERROR! This assigns 5 to x, doesn't compare!
if (x == 5)  // CORRECT! This compares x to 5


MISTAKE 4: Forgetting to use appropriate numeric suffix
--------------------------------------------------------
double d = 3.14;         // OK - double by default
float f = 3.14;          // ERROR - 3.14 is double, need f suffix
decimal m = 3.14;        // ERROR - need m suffix

FIX:
float f = 3.14f;
decimal m = 3.14m;


MISTAKE 5: Using double for money
----------------------------------
double price = 0.1 + 0.2;
Console.WriteLine(price);  // Prints 0.30000000000000004!

FIX: Use decimal for money:
decimal price = 0.1m + 0.2m;
Console.WriteLine(price);  // Prints 0.3
*/


// ================================================================================
// SECTION 9: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Personal Information
--------------------------------
Create variables for:
- First name (string)
- Last name (string)
- Age (int)
- Height in meters (double)
- Is employed (bool)

Print all using string interpolation.

EXERCISE 2: Rectangle Calculator
---------------------------------
Create variables for:
- Rectangle width (double)
- Rectangle height (double)

Calculate and print:
- Area = width * height
- Perimeter = 2 * (width + height)

EXERCISE 3: Temperature Converter
-----------------------------------
Create variables for:
- Temperature in Celsius (double)

Convert to Fahrenheit using: F = C * 9/5 + 32
Print both temperatures.

EXERCISE 4: Shopping Cart
--------------------------
Create variables for:
- Product name (string)
- Unit price (decimal)
- Quantity (int)

Calculate and print total cost.

EXERCISE 5: Student Profile
-----------------------------
Create a student profile with:
- Student ID (int)
- Name (string)
- GPA (double)
- Is enrolled (bool)

Print formatted profile.
*/


// ================================================================================
// SECTION 10: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between int and double in C#?
A: int is for whole numbers (integers), while double is for decimal numbers
   (floating-point). int uses 32 bits, double uses 64 bits with more precision.

Q2: What is a constant in C#? How do you declare one?
A: A constant is a value that cannot be changed after initialization. Use
   the "const" keyword: const int MaxItems = 100;

Q3: What is the var keyword in C#?
A: var enables implicit typing - the compiler infers the type from the
   right-hand side. For example: var x = 10; becomes int x = 10;

Q4: What are nullable types and when would you use them?
A: Nullable types use ? suffix (int? x) and can hold null in addition to
   values. Use when a value might be absent, like optional form fields.

Q5: Why should you use decimal instead of double for financial calculations?
A: double can have floating-point precision errors (0.1 + 0.2 = 0.30000000004).
   decimal provides exact representation for base-10 numbers, crucial for
   money where accuracy is essential.

Q6: What is the difference between value types and reference types?
A: Value types store data directly (stack), like int, double, bool.
   Reference types store a reference to data (heap), like string, arrays,
   classes. Value types can't be null (unless nullable).
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
GREAT PROGRESS! You now understand:
- What variables are
- Different data types in C#
- How to declare and use variables
- Constants and nullable types

WHAT'S NEXT:
In Topic 04, we'll learn about Type Casting - converting between different
data types. This is essential when you need to combine different types
in calculations or when user input comes as strings.
*/
