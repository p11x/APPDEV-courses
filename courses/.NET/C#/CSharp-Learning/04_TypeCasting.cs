/*
================================================================================
TOPIC 04: TYPE CASTING
================================================================================

Type casting is the process of converting one data type to another.
This is essential when working with different types together.

TABLE OF CONTENTS:
1. What is Type Casting?
2. Implicit Casting (Widening)
3. Explicit Casting (Narrowing)
4. Conversion Methods
5. Parsing Strings
6. TryParse for Safe Conversion
7. Convert Class Methods
================================================================================
*/

// ================================================================================
// SECTION 1: WHAT IS TYPE CASTING?
// ================================================================================

/*
TYPE CASTING DEFINED:
---------------------
Type casting (or type conversion) is converting a value from one data type
to another. This happens often in real programming.

REAL-WORLD ANALOGY:
-------------------
Think of currency exchange:
- You have dollars (int) and need euros (double)
- The exchange rate is the "cast" - converting the value
- $10 becomes €9.50 (approximately)

In programming:
- You have an integer: 10
- You need a double: 10.0
- This is type casting!

WHY DO WE NEED IT?
------------------
1. Combining different types in calculations
2. Converting user input (always strings) to numbers
3. Working with libraries that expect specific types
4. Performing precise mathematical operations
*/


// ================================================================================
// SECTION 2: IMPLICIT CASTING (WIDENING)
// ================================================================================

namespace ImplicitCasting
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // IMPLICIT CASTING - Automatic conversion from smaller to larger
            // ====================================================================
            
            // The C# compiler automatically converts when there's no data loss
            
            // byte -> short -> int -> long -> float -> double
            // char -> int -> double
            
            byte b = 100;          // byte (8-bit)
            short s = b;           // byte automatically converts to short
            int i = s;             // short automatically converts to int
            long l = i;            // int automatically converts to long
            float f = l;           // long automatically converts to float
            double d = f;          // float automatically converts to double
            
            Console.WriteLine($"byte: {b}");
            Console.WriteLine($"to short: {s}");
            Console.WriteLine($"to int: {i}");
            Console.WriteLine($"to long: {l}");
            Console.WriteLine($"to float: {f}");
            Console.WriteLine($"to double: {d}");
            
            // Character to number
            char c = 'A';
            int charToInt = c;    // char converts to int (ASCII value)
            Console.WriteLine($"\nChar '{c}' = {charToInt} (ASCII)");
            
            // Integer to double
            int num = 42;
            double numToDouble = num;
            Console.WriteLine($"\nint {num} = double {numToDouble}");
            
            /*
            NO DATA LOSS in implicit casting!
            It's safe because larger types can always hold smaller values.
            */
        }
    }
}

/*
IMPLICIT CASTING RULES:
-----------------------
byte  -> short -> int -> long -> float -> double
char  -> int   -> long -> float -> double

The compiler automatically does these conversions.
You don't need to do anything special!
*/


// ================================================================================
// SECTION 3: EXPLICIT CASTING (NARROWING)
// ================================================================================

namespace ExplicitCasting
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // EXPLICIT CASTING - Manual conversion from larger to smaller
            // ====================================================================
            
            // Use (type) to explicitly cast
            
            double d = 9.78;           // double (larger)
            int i = (int)d;            // Explicit cast to int (truncates!)
            
            Console.WriteLine($"double: {d}");
            Console.WriteLine($"cast to int: {i}");  // Loses decimal part!
            
            // More examples
            double pi = 3.14159;
            int piInt = (int)pi;
            Console.WriteLine($"\nPI: {pi}");
            Console.WriteLine($"PI as int: {piInt}");  // Just 3
            
            // Long to int
            long bigNumber = 1000;
            int smallNumber = (int)bigNumber;
            Console.WriteLine($"\nLong: {bigNumber}");
            Console.WriteLine($"As int: {smallNumber}");
            
            // Overflow example - careful!
            long veryBig = 3000000000;  // 3 billion
            int overflow = (int)veryBig;  // Wraps around to negative!
            Console.WriteLine($"\nVery big: {veryBig}");
            Console.WriteLine($"Overflow: {overflow}");
            
            // Double to float
            double bigD = 123.456;
            float smallF = (float)bigD;
            Console.WriteLine($"\nDouble: {bigD}");
            Console.WriteLine($"As float: {smallF}");
        }
    }
}

/*
EXPLICIT CASTING NOTES:
-----------------------
1. Use syntax: (targetType)value
2. May lose data (decimal parts truncated)
3. May cause overflow (wrong results for very large numbers)
4. Always be aware of potential data loss!

RULE OF THUMB:
- Only cast when you're sure it's safe
- Use TryParse for user input (covered later)
*/


// ================================================================================
// SECTION 4: CONVERSION METHODS
// ================================================================================

namespace ConversionMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // BUILT-IN CONVERSION METHODS
            // ====================================================================
            
            // ToString() - Convert anything to string
            int num = 42;
            string str = num.ToString();
            Console.WriteLine($"int.ToString(): {str}");
            
            double d = 3.14;
            string strD = d.ToString();
            Console.WriteLine($"double.ToString(): {strD}");
            
            bool b = true;
            string strB = b.ToString();
            Console.WriteLine($"bool.ToString(): {strB}");
            
            // Format specific
            double price = 19.99;
            string formatted = price.ToString("C");  // Currency format
            Console.WriteLine($"As currency: {formatted}");
            
            double percent = 0.75;
            string percentStr = percent.ToString("P");  // Percentage
            Console.WriteLine($"As percentage: {percentStr}");
            
            // ToString with decimal places
            double pi = 3.14159265;
            string piStr = pi.ToString("F2");  // 2 decimal places
            Console.WriteLine($"PI with 2 decimals: {piStr}");
        }
    }
}

/*
ToString() is your friend!
---------------------------
Every type in C# has a ToString() method.
Use it to convert anything to a string representation.
*/


// ================================================================================
// SECTION 5: PARSING STRINGS
// ================================================================================

namespace ParsingStrings
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // PARSE - Convert string to number
            // ====================================================================
            
            // Parse throws exception if invalid!
            
            string numStr = "42";
            int num = int.Parse(numStr);
            Console.WriteLine($"Parsed int: {num}");
            
            string doubleStr = "3.14";
            double d = double.Parse(doubleStr);
            Console.WriteLine($"Parsed double: {d}");
            
            string boolStr = "true";
            bool b = bool.Parse(boolStr);
            Console.WriteLine($"Parsed bool: {b}");
            
            // Parse with different types
            string intStr = "100";
            long l = long.Parse(intStr);
            Console.WriteLine($"Parsed long: {l}");
            
            string floatStr = "2.5";
            float f = float.Parse(floatStr);
            Console.WriteLine($"Parsed float: {f}");
            
            string decimalStr = "99.99";
            decimal m = decimal.Parse(decimalStr);
            Console.WriteLine($"Parsed decimal: {m}");
            
            // PROBLEM with Parse - throws exception on invalid input:
            // string invalid = "hello";
            // int bad = int.Parse(invalid);  // throws FormatException!
        }
    }
}

/*
PARSING NOTES:
--------------
- Parse() converts string to number
- Throws exception if string is invalid
- Use TryParse for safer conversion (next section!)
*/


// ================================================================================
// SECTION 6: TRYPARSE FOR SAFE CONVERSION
// ================================================================================

namespace TryParseExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // TRYPARSE - Safe conversion that doesn't throw exceptions
            // ====================================================================
            
            // Format: bool success = Type.TryParse(string, out variable)
            
            // Valid input
            string validNum = "42";
            if (int.TryParse(validNum, out int result))
            {
                Console.WriteLine($"Success! Parsed: {result}");
            }
            else
            {
                Console.WriteLine("Failed to parse");
            }
            
            // Invalid input - doesn't crash!
            string invalidNum = "hello";
            if (int.TryParse(invalidNum, out int badResult))
            {
                Console.WriteLine($"Parsed: {badResult}");
            }
            else
            {
                Console.WriteLine("Invalid input - TryParse returned false");
            }
            
            // Real-world example: User input
            Console.WriteLine("\n=== Calculator ===");
            Console.Write("Enter first number: ");
            string input1 = Console.ReadLine();
            
            Console.Write("Enter second number: ");
            string input2 = Console.ReadLine();
            
            if (int.TryParse(input1, out int num1) && int.TryParse(input2, out int num2))
            {
                int sum = num1 + num2;
                Console.WriteLine($"Sum: {num1} + {num2} = {sum}");
            }
            else
            {
                Console.WriteLine("Invalid numbers entered!");
            }
            
            // TryParse with double
            string priceStr = "19.99";
            if (double.TryParse(priceStr, out double price))
            {
                Console.WriteLine($"Price: ${price}");
            }
            
            // TryParse with bool
            string answerStr = "true";
            if (bool.TryParse(answerStr, out bool answer))
            {
                Console.WriteLine($"Answer: {answer}");
            }
        }
    }
}

/*
WHY USEPARSE?
-------------
1. Doesn't throw exceptions on invalid input
2. Returns true/false for success
3. Safer for user input
4. Prevents program crashes

ALWAYS use TryParse for user input!
*/


// ================================================================================
// SECTION 7: CONVERT CLASS
// ================================================================================

namespace ConvertClassExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // CONVERT CLASS - Another way to convert types
            // ====================================================================
            
            // Convert.ToXXX methods handle null and provide alternatives
            
            string strNum = "42";
            int fromString = Convert.ToInt32(strNum);
            Console.WriteLine($"Convert.ToInt32: {fromString}");
            
            // Convert from bool
            bool flag = true;
            int fromBool = Convert.ToInt32(flag);  // 1 for true, 0 for false
            Console.WriteLine($"bool to int: {fromBool}");
            
            // Convert from double to int (rounds)
            double d = 9.7;
            int fromDouble = Convert.ToInt32(d);  // Rounds to 10!
            Console.WriteLine($"double 9.7 to int: {fromDouble}");
            
            // Convert to string
            int num = 123;
            string fromInt = Convert.ToString(num);
            Console.WriteLine($"int to string: {fromInt}");
            
            // Convert to double
            string strD = "3.14";
            double fromStrD = Convert.ToDouble(strD);
            Console.WriteLine($"string to double: {fromStrD}");
            
            // Convert class handles null differently
            string nullStr = null;
            int fromNull = Convert.ToInt32(nullStr);  // Returns 0, no exception
            Console.WriteLine($"null to int: {fromNull}");
        }
    }
}

/*
CONVERT CLASS vs PARSE vs TRYPARSE:
------------------------------------
Convert.ToInt32() | Handles null -> returns 0 | Throws on invalid
int.Parse()       | Throws on null            | Throws on invalid  
int.TryParse()    | Returns false on null     | Returns false on invalid

Best choice:
- User input: int.TryParse()
- Working with nulls: Convert.ToInt32()
- Known valid strings: int.Parse()
*/


// ================================================================================
// SECTION 8: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Forgetting explicit cast
------------------------------------
double d = 3.14;
int i = d;           // ERROR! Need explicit cast

FIX: int i = (int)d;


MISTAKE 2: Not accounting for truncation
-----------------------------------------
double d = 9.99;
int i = (int)d;      // i = 9, not 10!


MISTAKE 3: Using Parse for user input
--------------------------------------
Console.Write("Enter number: ");
int num = int.Parse(Console.ReadLine());  // Crashes if invalid!

FIX: Use TryParse!


MISTAKE 4: Confusing Parse and TryParse
----------------------------------------
int.TryParse("abc", out int result);  // Returns false, result = 0


MISTAKE 5: Wrong numeric suffix
-------------------------------
decimal price = 19.99;   // ERROR - needs m suffix

FIX: decimal price = 19.99m;


MISTAKE 6: String concatenation vs addition
--------------------------------------------
string result = "5" + 3;   // "53" (string concatenation!)
int result2 = 5 + 3;       // 8 (numeric addition)

FIX: Convert first:
int result = int.Parse("5") + 3;  // 8
*/


// ================================================================================
// SECTION 9: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Temperature Converter
---------------------------------
1. Ask user for temperature in Fahrenheit
2. Convert to Celsius using: C = (F - 32) * 5/9
3. Display result (use explicit cast or conversion)

EXERCISE 2: Circle Calculator
------------------------------
1. Ask user for radius (as string)
2. Convert to double
3. Calculate area = π * r²
4. Display area (cast to int for whole number)

EXERCISE 3: Safe Calculator
---------------------------
1. Ask for two numbers
2. Use TryParse to convert safely
3. Only perform calculation if both are valid
4. Handle invalid input gracefully

EXERCISE 4: String to Bool
--------------------------
1. Ask user yes/no question
2. Convert their answer to bool using Parse or TryParse
3. Display result based on their answer

EXERCISE 5: Currency Calculator
--------------------------------
1. Ask for amount in dollars
2. Convert to cents (multiply by 100)
3. Use explicit cast to show cents as integer
*/


// ================================================================================
// SECTION 10: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between implicit and explicit casting?
A: Implicit casting (widening) automatically converts smaller types to 
   larger types (int to double) without data loss. Explicit casting 
   (narrowing) manually converts larger to smaller (double to int) and
   may lose data.

Q2: What is the difference between Parse() and TryParse()?
A: Parse() throws an exception if conversion fails. TryParse() returns
   a boolean indicating success/failure and doesn't throw exceptions.
   Always use TryParse for user input.

Q3: What happens when you cast 9.7 to int in C#?
A: The decimal part is truncated, so 9.7 becomes 9 (not rounded to 10).

Q4: What is the Convert class used for?
A: The Convert class provides methods to convert between base types,
   handling null values differently than Parse (returns 0 for null).

Q5: How do you convert an int to a string in C#?
A: Use ToString() method: int.ToString() or Convert.ToString(), or
   string interpolation: $" {number}".
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You now understand:
- Implicit and explicit type casting
- Converting between different data types
- Using Parse and TryParse
- Handling user input safely

WHAT'S NEXT:
In Topic 05, we'll explore Operators in C# - the symbols that let you
perform calculations, make comparisons, and combine conditions.
*/
