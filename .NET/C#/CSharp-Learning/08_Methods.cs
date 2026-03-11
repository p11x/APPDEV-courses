/*
================================================================================
TOPIC 08: METHODS
================================================================================

Methods (also called functions) are reusable blocks of code that perform
specific tasks. They are fundamental to writing organized, maintainable code.

TABLE OF CONTENTS:
1. What are Methods?
2. Method Structure
3. Method Parameters
4. Return Values
5. Method Overloading
6. Recursion
7. Expression-Bodied Methods
8. Local Functions
================================================================================
*/

// ================================================================================
// SECTION 1: WHAT ARE METHODS?
// ================================================================================

/*
METHODS DEFINED:
----------------
A method is a reusable block of code that performs a specific task.
You define it once and can call it many times.

REAL-WORLD ANALOGY:
-------------------
Think of a recipe:
- Recipe = Method
- Ingredients = Parameters
- Final dish = Return value
- Following the recipe = Calling the method
- Making the same dish multiple times = Calling method multiple times

WHY USE METHODS?
----------------
1. REUSABILITY - Write once, use many times
2. ORGANIZATION - Break code into logical pieces
3. MAINTENANCE - Change in one place, affects all
4. READABILITY - Easier to understand code
5. TESTABILITY - Test each piece separately

EXAMPLE:
--------
Instead of:
Console.WriteLine("Hello");
Console.WriteLine("World");

Write:
SayHello();
SayHello();

Where SayHello() is a method that prints "Hello, World!"
*/


// ================================================================================
// SECTION 2: METHOD STRUCTURE
// ================================================================================

namespace MethodStructure
{
    class Program
    {
        // ====================================================================
        // BASIC METHOD STRUCTURE
        // ====================================================================
        
        // Syntax:
        // accessModifier returnType MethodName(parameters)
        // {
        //     // method body
        // }
        
        // Simple method with no return value (void)
        static void Greet()
        {
            Console.WriteLine("Hello, World!");
        }
        
        // Method with parameters
        static void GreetPerson(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
        
        // Method with return value
        static int GetAge()
        {
            return 25;
        }
        
        // Full example
        static void Main(string[] args)
        {
            // Call the methods
            Console.WriteLine("=== Method Calls ===");
            
            Greet();                    // Calls the greet method
            
            GreetPerson("Alice");       // Passes "Alice" as parameter
            
            int age = GetAge();         // Gets the return value
            Console.WriteLine($"Age: {age}");
            
            // Method with multiple parameters
            int sum = Add(10, 20);
            Console.WriteLine($"10 + 20 = {sum}");
        }
        
        // Additional method
        static int Add(int a, int b)
        {
            return a + b;
        }
    }
}

/*
METHOD ANATOMY:
---------------
static void GreetPerson(string name)
|       |    |          |
|       |    |          └── Parameter
|       |    └── Method name (PascalCase)
|       └── Return type (void = nothing)
└── Access modifier (static = can call without creating object)

Return Types:
- void: Returns nothing
- int, double, string, etc.: Returns that type
- bool: Returns true/false
*/


// ================================================================================
// SECTION 3: METHOD PARAMETERS
// ================================================================================

namespace MethodParameters
{
    class Program
    {
        // ====================================================================
        // DIFFERENT WAYS TO PASS PARAMETERS
        // ====================================================================
        
        // 1. Value Parameters (default)
        static void Square(int number)
        {
            number = number * number;
            Console.WriteLine($"Inside method: {number}");
        }
        
        // 2. Reference Parameters (ref keyword)
        static void SquareRef(ref int number)
        {
            number = number * number;
            Console.WriteLine($"Inside method: {number}");
        }
        
        // 3. Out Parameters (out keyword)
        static void GetMinMax(int a, int b, out int min, out int max)
        {
            min = Math.Min(a, b);
            max = Math.Max(a, b);
        }
        
        // 4. Parameters Array (params keyword)
        static int Sum(params int[] numbers)
        {
            int total = 0;
            foreach (int n in numbers)
            {
                total += n;
            }
            return total;
        }
        
        // 5. Optional Parameters
        static void Greet(string name = "Guest", string greeting = "Hello")
        {
            Console.WriteLine($"{greeting}, {name}!");
        }
        
        static void Main(string[] args)
        {
            // Value parameter example
            Console.WriteLine("=== Value Parameter ===");
            int x = 5;
            Square(x);
            Console.WriteLine($"After method: {x}");  // Still 5!
            
            // Reference parameter example
            Console.WriteLine("\n=== Ref Parameter ===");
            x = 5;
            SquareRef(ref x);
            Console.WriteLine($"After method: {x}");  // Now 25!
            
            // Out parameter example
            Console.WriteLine("\n=== Out Parameter ===");
            GetMinMax(10, 5, out int min, out int max);
            Console.WriteLine($"Min: {min}, Max: {max}");
            
            // Params array example
            Console.WriteLine("\n=== Params Array ===");
            Console.WriteLine($"Sum of 1,2,3: {Sum(1, 2, 3)}");
            Console.WriteLine($"Sum of 1-5: {Sum(1, 2, 3, 4, 5)}");
            Console.WriteLine($"Sum of nothing: {Sum()}");
            
            // Optional parameters example
            Console.WriteLine("\n=== Optional Parameters ===");
            Greet("John");                      // Uses default greeting
            Greet("John", "Welcome");          // Uses both
            Greet();                            // Uses both defaults
        }
    }
}

/*
PARAMETER TYPES SUMMARY:
-------------------------
1. VALUE (default): Copy of value passed, doesn't change original
2. REF: Reference to original, changes persist
3. OUT: Like ref but must be assigned inside method
4. PARAMS: Variable number of arguments as array
5. OPTIONAL: Has default value, can be omitted

WHEN TO USE WHAT:
----------------
- Default: Most cases
- ref: When you need to modify original
- out: When method needs to return multiple values
- params: When number of arguments varies
- optional: For convenience and overloading alternatives
*/


// ================================================================================
// SECTION 4: RETURN VALUES
// ================================================================================

namespace ReturnValues
{
    class Program
    {
        // ====================================================================
        // RETURNING VALUES
        // ====================================================================
        
        // Return an integer
        static int Add(int a, int b)
        {
            return a + b;
        }
        
        // Return a boolean
        static bool IsEven(int number)
        {
            return number % 2 == 0;
        }
        
        // Return a string
        static string GetFullName(string first, string last)
        {
            return $"{first} {last}";
        }
        
        // Return an array
        static int[] GetNumbers()
        {
            return new int[] { 1, 2, 3, 4, 5 };
        }
        
        // Multiple returns (early exit pattern)
        static string GetGradeMessage(int score)
        {
            if (score < 0)
                return "Invalid score";
            
            if (score < 60)
                return "Failed";
            
            if (score < 70)
                return "Passed";
            
            if (score < 80)
                return "Good";
            
            if (score < 90)
                return "Very Good";
            
            return "Excellent";
        }
        
        // Return multiple values using tuple
        static (int sum, int product) Calculate(int a, int b)
        {
            return (a + b, a * b);
        }
        
        static void Main(string[] args)
        {
            // Basic return
            int result = Add(5, 3);
            Console.WriteLine($"5 + 3 = {result}");
            
            // Boolean return
            Console.WriteLine($"\nIs 10 even? {IsEven(10)}");
            Console.WriteLine($"Is 7 even? {IsEven(7)}");
            
            // String return
            string fullName = GetFullName("John", "Doe");
            Console.WriteLine($"\nFull name: {fullName}");
            
            // Array return
            int[] nums = GetNumbers();
            Console.Write("\nNumbers: ");
            foreach (int n in nums)
                Console.Write(n + " ");
            
            // Multiple returns
            Console.WriteLine($"\n\nGrade: {GetGradeMessage(85)}");
            
            // Tuple return
            var (sum, product) = Calculate(4, 5);
            Console.WriteLine($"\n4 + 5 = {sum}");
            Console.WriteLine($"4 * 5 = {product}");
        }
    }
}

/*
RETURN STATEMENT RULES:
----------------------
1. Return exits the method immediately
2. Return value must match declared type
3. void methods can use "return;" without value
4. Multiple return statements are allowed
5. All paths must return a value (non-void)

TUPLES (C# 7+):
---------------
Allow returning multiple values without creating a class.
Very useful for related values like (sum, product).
*/


// ================================================================================
// SECTION 5: METHOD OVERLOADING
// ================================================================================

namespace MethodOverloading
{
    class Program
    {
        // ====================================================================
        // METHOD OVERLOADING
        // ====================================================================
        
        // Same method name, different parameters!
        
        // Overload 1: No parameters
        static void Print()
        {
            Console.WriteLine("(no parameters)");
        }
        
        // Overload 2: One integer
        static void Print(int number)
        {
            Console.WriteLine($"Integer: {number}");
        }
        
        // Overload 3: One string
        static void Print(string text)
        {
            Console.WriteLine($"String: {text}");
        }
        
        // Overload 4: Two integers
        static void Print(int a, int b)
        {
            Console.WriteLine($"Two integers: {a}, {b}");
        }
        
        // Overload 5: Different types
        static void Print(string text, int number)
        {
            Console.WriteLine($"String: {text}, Number: {number}");
        }
        
        // Overload resolution - compiler picks best match
        static int Add(int a, int b)
        {
            Console.WriteLine("Adding ints");
            return a + b;
        }
        
        static double Add(double a, double b)
        {
            Console.WriteLine("Adding doubles");
            return a + b;
        }
        
        static void Main(string[] args)
        {
            // Calls different overloads based on arguments
            Console.WriteLine("=== Method Overloading ===");
            
            Print();
            Print(42);
            Print("Hello");
            Print(1, 2);
            Print("Text", 100);
            
            // Automatic type conversion
            Console.WriteLine("\n=== Type Matching ===");
            int i = Add(5, 10);           // Calls int version
            double d = Add(2.5, 3.5);     // Calls double version
            double d2 = Add(5, 10.5);     // 5 promoted to double
            
            Console.WriteLine($"int result: {i}");
            Console.WriteLine($"double result: {d}");
            Console.WriteLine($"mixed result: {d2}");
        }
    }
}

/*
OVERLOADING RULES:
-----------------
1. Method name must be same
2. Parameters must differ (type, count, or order)
3. Return type doesn't matter for overloading
4. Compiler picks best match

WHY USE IT?
-----------
- Same operation, different inputs
- Convenient API
- Intuitive naming
- Console.WriteLine() has many overloads!
*/


// ================================================================================
// SECTION 6: RECURSION
// ================================================================================

namespace Recursion
{
    class Program
    {
        // ====================================================================
        // RECURSION - Method calling itself
        // ====================================================================
        
        // Factorial: n! = n * (n-1)!
        static int Factorial(int n)
        {
            // Base case - stop condition!
            if (n <= 1)
                return 1;
            
            // Recursive case
            return n * Factorial(n - 1);
        }
        
        // Fibonacci: F(n) = F(n-1) + F(n-2)
        static int Fibonacci(int n)
        {
            if (n <= 1)
                return n;
            
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
        
        // Sum of array using recursion
        static int SumArray(int[] arr, int index = 0)
        {
            if (index >= arr.Length)
                return 0;
            
            return arr[index] + SumArray(arr, index + 1);
        }
        
        // Count down
        static void CountDown(int n)
        {
            if (n <= 0)
            {
                Console.WriteLine("Done!");
                return;
            }
            
            Console.WriteLine(n);
            CountDown(n - 1);
        }
        
        static void Main(string[] args)
        {
            // Factorial
            Console.WriteLine("=== Factorial ===");
            Console.WriteLine($"5! = {Factorial(5)}");  // 120
            Console.WriteLine($"3! = {Factorial(3)}");  // 6
            
            // Fibonacci
            Console.WriteLine("\n=== Fibonacci ===");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{Fibonacci(i)} ");
            }
            Console.WriteLine();
            
            // Sum array
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"\nSum of array: {SumArray(numbers)}");
            
            // Countdown
            Console.WriteLine("\n=== Countdown ===");
            CountDown(5);
        }
    }
}

/*
RECURSION ESSENTIALS:
---------------------
1. Base case: When to stop (prevents infinite loop)
2. Recursive case: Call itself with changed parameters
3. Must move toward base case

RECURSION vs ITERATION:
-----------------------
- Recursion: Elegant, easier to understand for some problems
- Iteration: More efficient (no function call overhead)

Use recursion for:
- Tree/graph traversal
- Divide and conquer
- Problems with natural recursive structure
*/


// ================================================================================
// SECTION 7: EXPRESSION-BODIED METHODS (C# 6+)
// ================================================================================

namespace ExpressionBodied
{
    class Program
    {
        // ====================================================================
        // EXPRESSION-BODIED METHODS
        // ====================================================================
        
        // Traditional method
        static int SquareTraditional(int x)
        {
            return x * x;
        }
        
        // Expression-bodied method (concise!)
        static int Square(int x) => x * x;
        
        // Traditional
        static bool IsPositiveTraditional(int x)
        {
            return x > 0;
        }
        
        // Expression-bodied
        static bool IsPositive(int x) => x > 0;
        
        // Void expression-bodied
        static void PrintMessage(string msg) => Console.WriteLine($"Message: {msg}");
        
        // Multiple statements NOT allowed
        // static void Complex() => Console.WriteLine("One"); Console.WriteLine("Two");
        // Use traditional braces for multiple statements
        
        static void Main(string[] args)
        {
            Console.WriteLine($"Square of 5: {Square(5)}");
            Console.WriteLine($"Is 10 positive: {IsPositive(10)}");
            PrintMessage("Hello!");
        }
    }
}

/*
EXPRESSION-BODIED RULES:
------------------------
- Use => instead of { return }
- Only for single-expression methods
- Works for void and return types
- Makes code concise and readable
- Use for simple operations
*/


// ================================================================================
// SECTION 8: LOCAL FUNCTIONS (C# 7+)
// ================================================================================

namespace LocalFunctions
{
    class Program
    {
        // ====================================================================
        // LOCAL FUNCTIONS
        // ====================================================================
        
        static void Main(string[] args)
        {
            // Local function inside a method
            int Sum(int a, int b)
            {
                return a + b;
            }
            
            Console.WriteLine($"Sum: {Sum(5, 3)}");
            
            // Local function with closure (accesses outer variables)
            int multiplier = 10;
            
            int Multiply(int x)
            {
                return x * multiplier;
            }
            
            Console.WriteLine($"Multiply: {Multiply(5)}");
            
            // Practical example: Validate before processing
            Console.WriteLine("\n=== Validated Calculation ===");
            
            double CalculateDiscountedPrice(double price, double discountPercent)
            {
                // Local function for validation
                bool IsValidPrice(double p) => p > 0 && p < 10000;
                
                if (!IsValidPrice(price))
                {
                    Console.WriteLine("Invalid price!");
                    return 0;
                }
                
                if (discountPercent < 0 || discountPercent > 100)
                {
                    Console.WriteLine("Invalid discount!");
                    return price;
                }
                
                return price * (1 - discountPercent / 100);
            }
            
            Console.WriteLine($"Price: {CalculateDiscountedPrice(100, 10)}");  // 90
            Console.WriteLine($"Price: {CalculateDiscountedPrice(50, 20)}");  // 40
            Console.WriteLine($"Price: {CalculateDiscountedPrice(-10, 10)}"); // 0 (invalid)
        }
    }
}

/*
LOCAL FUNCTIONS:
---------------
- Defined inside another method
- Only visible to that method
- Can access local variables (closures)
- Useful for helper functions
- Better than private methods for simple tasks
*/


// ================================================================================
// SECTION 9: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Forgetting return statement
--------------------------------------
static int GetValue()
{
    // ERROR - no return!
}

FIX: return value;


MISTAKE 2: Wrong return type
----------------------------
static int GetValue()
{
    return "hello";  // ERROR - wrong type!
}


MISTAKE 3: Infinite recursion
-----------------------------
static int CountDown(int n)
{
    return CountDown(n - 1);  // No base case!
}


MISTAKE 4: Using ref when not needed
-------------------------------------
Usually you don't need ref. It's for special cases!


MISTAKE 5: Forgetting to pass arguments
---------------------------------------
Print();  // ERROR if parameters required!

MISTAKE 6: Not handling null
-----------------------------
static void PrintName(string name)
{
    Console.WriteLine(name.Length);  // Crashes if name is null!


MISTAKE 7: Modifying parameters accidentally
---------------------------------------------
Use ref only when you intentionally want to modify!
*/


// ================================================================================
// SECTION 10: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Temperature Converter
----------------------------------
Create methods:
- CelsiusToFahrenheit(celsius)
- FahrenheitToCelsius(fahrenheit)

EXERCISE 2: Palindrome Checker
------------------------------
Method: bool IsPalindrome(string text)
Returns true if palindrome (same forwards/backwards)

EXERCISE 3: Array Methods
-------------------------
- int[] ReverseArray(int[] arr)
- int FindMax(int[] arr)
- int FindMin(int[] arr)

EXERCISE 4: Calculator
---------------------
Create overloaded Add methods:
- Add(int a, int b)
- Add(double a, double b)
- Add(int a, int b, int c)

EXERCISE 5: Recursive Power
---------------------------
double Power(double base, int exponent)
Use recursion: base^exp = base * base^(exp-1)
*/


// ================================================================================
// SECTION 11: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between ref and out parameters?
A: ref requires initialization before passing, out doesn't.
   Both allow method to modify the original variable.

Q2: What is method overloading?
A: Multiple methods with same name but different parameters.
   Compiler picks the right one based on arguments.

Q3: What is recursion?
A: A method that calls itself. Must have base case to stop.

Q4: What are expression-bodied methods?
A: Concise syntax for single-expression methods using =>.

Q5: What is the params keyword?
A: Allows variable number of arguments passed as array.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You now understand:
- Method structure and declaration
- Different parameter types (ref, out, params)
- Return values and multiple returns
- Method overloading
- Recursion
- Expression-bodied methods

WHAT'S NEXT:
In Topic 09, we'll learn about Arrays - how to store and work with
multiple values of the same type efficiently.
*/
