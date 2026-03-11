/*
================================================================================
TOPIC 05: OPERATORS
================================================================================

Operators are symbols that tell the compiler to perform specific operations.
C# has many types of operators for math, comparison, and logic.

TABLE OF CONTENTS:
1. Arithmetic Operators
2. Assignment Operators
3. Comparison Operators
4. Logical Operators
5. Bitwise Operators
6. Ternary Operator
7. Null-Coalescing Operators
8. Operator Precedence
================================================================================
*/

// ================================================================================
// SECTION 1: ARITHMETIC OPERATORS
// ================================================================================

namespace ArithmeticOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // BASIC ARITHMETIC OPERATORS
            // ====================================================================
            
            // +  Addition
            // -  Subtraction  
            // *  Multiplication
            // /  Division
            // %  Modulus (remainder)
            
            int a = 10;
            int b = 3;
            
            // Addition
            int sum = a + b;
            Console.WriteLine($"{a} + {b} = {sum}");
            
            // Subtraction
            int difference = a - b;
            Console.WriteLine($"{a} - {b} = {difference}");
            
            // Multiplication
            int product = a * b;
            Console.WriteLine($"{a} * {b} = {product}");
            
            // Division (note: integer division truncates)
            int quotient = a / b;    // 10 / 3 = 3 (not 3.333!)
            Console.WriteLine($"{a} / {b} = {quotient}");
            
            // Modulus (remainder)
            int remainder = a % b;   // 10 % 3 = 1
            Console.WriteLine($"{a} % {b} = {remainder}");
            
            // ====================================================================
            // DIVISION WITH DECIMALS
            // ====================================================================
            
            double x = 10.0;
            double y = 3.0;
            double decimalDiv = x / y;
            Console.WriteLine($"\n{x} / {y} = {decimalDiv}");
            
            // One must be double for decimal result
            int m = 10;
            double n = 3.0;
            double mixed = m / n;
            Console.WriteLine($"{m} / {n} = {mixed}");
            
            // ====================================================================
            // MODULUS USEFUL EXAMPLES
            // ====================================================================
            
            // Check if even or odd
            int num = 7;
            bool isEven = (num % 2 == 0);
            Console.WriteLine($"\n{num} is even: {isEven}");
            
            // Get last digit
            int number = 9876;
            int lastDigit = number % 10;
            Console.WriteLine($"Last digit of {number}: {lastDigit}");
            
            // Wrap around (like in games)
            int score = 100;
            int wrappedScore = score % 100;  // 0-99 range
            Console.WriteLine($"Wrapped score: {wrappedScore}");
        }
    }
}

/*
MODULUS (%) OPERATOR:
---------------------
Returns the remainder after division.
- 10 % 3 = 1 (10 divided by 3 = 3 remainder 1)
- 15 % 4 = 3 (15 divided by 4 = 3 remainder 3)
- 20 % 5 = 0 (evenly divisible!)

Common uses:
- Check if number is even/odd
- Get last digit
- Create cycling/wrapping behavior
- Determine time (minutes/seconds)
*/


// ================================================================================
// SECTION 2: ASSIGNMENT OPERATORS
// ================================================================================

namespace AssignmentOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // ASSIGNMENT OPERATORS
            // ====================================================================
            
            int x = 10;           // Basic assignment
            
            // +=  Add and assign
            x += 5;               // x = x + 5 = 15
            Console.WriteLine($"x += 5: {x}");
            
            // -=  Subtract and assign
            x -= 3;               // x = x - 3 = 12
            Console.WriteLine($"x -= 3: {x}");
            
            // *=  Multiply and assign
            x *= 2;               // x = x * 2 = 24
            Console.WriteLine($"x *= 2: {x}");
            
            // /=  Divide and assign
            x /= 4;               // x = x / 4 = 6
            Console.WriteLine($"x /= 4: {x}");
            
            // %=  Modulus and assign
            x = 10;
            x %= 3;               // x = x % 3 = 1
            Console.WriteLine($"x %= 3: {x}");
            
            // ====================================================================
            // INCREMENT AND DECREMENT
            // ====================================================================
            
            int count = 5;
            
            // ++count (pre-increment) - increments BEFORE using
            Console.WriteLine($"\nInitial count: {count}");
            Console.WriteLine($"++count: {++count}");   // Becomes 6, returns 6
            
            // count++ (post-increment) - uses THEN increments
            count = 5;
            Console.WriteLine($"count++: {count++}");   // Returns 5, then becomes 6
            Console.WriteLine($"After: {count}");
            
            // Pre-decrement
            count = 5;
            Console.WriteLine($"--count: {--count}");   // Becomes 4, returns 4
            
            // Post-decrement  
            count = 5;
            Console.WriteLine($"count--: {count--}");   // Returns 5, then becomes 4
            Console.WriteLine($"After: {count}");
            
            // ====================================================================
            // PRACTICAL USE
            // ====================================================================
            
            // Common in loops (covered later)
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Loop iteration: {i}");
            }
        }
    }
}

/*
WHY USE COMPOUND ASSIGNMENTS?
-----------------------------
Instead of: x = x + 5;
Write:       x += 5;

Benefits:
- Shorter and cleaner
- Less typing
- Same functionality
- Very common in C#
*/


// ================================================================================
// SECTION 3: COMPARISON OPERATORS
// ================================================================================

namespace ComparisonOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // COMPARISON (RELATIONAL) OPERATORS
            // ====================================================================
            
            // ==  Equal to
            // !=  Not equal to
            // >   Greater than
            // <   Less than
            // >=  Greater than or equal to
            // <=  Less than or equal to
            
            int a = 10;
            int b = 20;
            
            Console.WriteLine("=== Comparisons ===");
            Console.WriteLine($"a = {a}, b = {b}");
            
            // Equal to
            Console.WriteLine($"a == b: {a == b}");    // false
            
            // Not equal to
            Console.WriteLine($"a != b: {a != b}");    // true
            
            // Greater than
            Console.WriteLine($"a > b: {a > b}");      // false
            
            // Less than
            Console.WriteLine($"a < b: {a < b}");      // true
            
            // Greater or equal
            Console.WriteLine($"a >= b: {a >= b}");    // false
            
            // Less or equal
            Console.WriteLine($"a <= b: {a <= b}");    // true
            
            // ====================================================================
            // COMPARING STRINGS
            // ====================================================================
            
            string str1 = "hello";
            string str2 = "HELLO";
            string str3 = "hello";
            
            Console.WriteLine("\n=== String Comparisons ===");
            Console.WriteLine($"str1 = \"hello\", str2 = \"HELLO\"");
            
            // Case-sensitive comparison
            Console.WriteLine($"str1 == str2: {str1 == str2}");     // false
            Console.WriteLine($"str1 == str3: {str1 == str3}");     // true
            
            // Case-insensitive comparison
            bool equalIgnoreCase = str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Equals ignore case: {equalIgnoreCase}");  // true
            
            // String.Compare
            int result = string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"String.Compare: {result} (0 means equal)");
        }
    }
}

/*
COMPARISON RESULTS:
-------------------
Comparison operators return BOOLEAN values (true/false).
These are used in:
- if statements (covered in Control Statements)
- while loops
- Ternary operator
- Conditional expressions

IMPORTANT: Use == for comparison, NOT = (that's assignment!)
*/


// ================================================================================
// SECTION 4: LOGICAL OPERATORS
// ================================================================================

namespace LogicalOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // LOGICAL OPERATORS
            // ====================================================================
            
            // &&  Logical AND (both must be true)
            // ||  Logical OR (at least one must be true)
            // !   Logical NOT (negates)
            
            bool hasLicense = true;
            bool hasCar = false;
            
            // AND - both must be true
            bool canDrive = hasLicense && hasCar;
            Console.WriteLine($"Can drive (license AND car): {canDrive}");
            
            // OR - at least one must be true
            bool canBorrow = hasLicense || hasCar;
            Console.WriteLine($"Can borrow (license OR car): {canBorrow}");
            
            // NOT - negates
            bool cannotDrive = !hasCar;
            Console.WriteLine($"Cannot drive (NOT has car): {cannotDrive}");
            
            // ====================================================================
            // COMBINING CONDITIONS
            // ====================================================================
            
            int age = 25;
            bool hasIncome = true;
            bool hasDebt = false;
            
            // Complex condition
            bool canGetLoan = (age >= 18) && hasIncome && !hasDebt;
            Console.WriteLine($"\nage={age}, hasIncome={hasIncome}, hasDebt={hasDebt}");
            Console.WriteLine($"Can get loan: {canGetLoan}");
            
            // ====================================================================
            // SHORT-CIRCUIT EVALUATION
            // ====================================================================
            
            int x = 5;
            
            // With && - if first is false, second is NOT evaluated
            bool shortAnd = (x > 10) && (x++ > 0);
            Console.WriteLine($"\nx after short-circuit AND: {x}");  // Still 5!
            
            // With || - if first is true, second is NOT evaluated
            x = 5;
            bool shortOr = (x < 10) || (x++ > 0);
            Console.WriteLine($"x after short-circuit OR: {x}");   // Still 5!
            
            // ====================================================================
            // PRACTICAL EXAMPLE
            // ====================================================================
            
            Console.WriteLine("\n=== Login Check ===");
            string username = "admin";
            string password = "12345";
            bool isLocked = false;
            
            bool canLogin = (username == "admin") && 
                           (password == "12345") && 
                           !isLocked;
            Console.WriteLine($"Login successful: {canLogin}");
        }
    }
}

/*
LOGICAL OPERATOR TRUTH TABLES:
------------------------------
   A     |   B   | A && B | A || B |  !A
---------|-------|--------|--------|-------
  true   | true  |  true  |  true  | false
  true   | false | false  |  true  | false
  false  | true  | false  |  true  |  true
  false  | false | false  | false  |  true
*/


// ================================================================================
// SECTION 5: BITWISE OPERATORS (BONUS)
// ================================================================================

namespace BitwiseOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // BITWISE OPERATORS (Advanced)
            // ====================================================================
            
            // &   Bitwise AND
            // |   Bitwise OR
            // ^   Bitwise XOR (exclusive OR)
            // ~   Bitwise NOT
            // <<  Left shift
            // >>  Right shift
            
            int a = 5;    // Binary: 0101
            int b = 3;    // Binary: 0011
            
            Console.WriteLine($"a = {a} (binary: 0101)");
            Console.WriteLine($"b = {b} (binary: 0011)");
            
            // Bitwise AND
            Console.WriteLine($"a & b = {a & b}");   // 1 (0001)
            
            // Bitwise OR
            Console.WriteLine($"a | b = {a | b}");   // 7 (0111)
            
            // Bitwise XOR
            Console.WriteLine($"a ^ b = {a ^ b}");   // 6 (0110)
            
            // Left shift (multiply by 2^n)
            int shifted = a << 2;   // 5 * 4 = 20
            Console.WriteLine($"a << 2 = {shifted}");
            
            // Right shift (divide by 2^n)
            int rightShifted = a >> 1;   // 5 / 2 = 2
            Console.WriteLine($"a >> 1 = {rightShifted}");
            
            // Practical use: checking flags
            const int READ = 1;      // 0001
            const int WRITE = 2;     // 0010
            const int EXECUTE = 4;   // 0100
            
            int permissions = READ | WRITE;  // 3 = 0011
            
            bool canRead = (permissions & READ) != 0;
            bool canWrite = (permissions & WRITE) != 0;
            bool canExecute = (permissions & EXECUTE) != 0;
            
            Console.WriteLine($"\nPermissions: {permissions}");
            Console.WriteLine($"Can read: {canRead}");
            Console.WriteLine($"Can write: {canWrite}");
            Console.WriteLine($"Can execute: {canExecute}");
        }
    }
}

/*
WHEN TO USE BITWISE:
-------------------
- Flags and permissions
- Performance optimization
- Working with binary data
- Encryption/compression

For beginners: You may not need these often, but good to know they exist!
*/


// ================================================================================
// SECTION 6: TERNARY OPERATOR
// ================================================================================

namespace TernaryOperator
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // TERNARY OPERATOR (Conditional Operator)
            // ====================================================================
            
            // Syntax: condition ? valueIfTrue : valueIfFalse
            
            int age = 20;
            
            // Basic usage
            string status = (age >= 18) ? "Adult" : "Minor";
            Console.WriteLine($"Age {age}: {status}");
            
            age = 15;
            status = (age >= 18) ? "Adult" : "Minor";
            Console.WriteLine($"Age {age}: {status}");
            
            // ====================================================================
            // NESTED TERNARY (use carefully!)
            // ====================================================================
            
            int score = 85;
            string grade = score >= 90 ? "A" :
                          score >= 80 ? "B" :
                          score >= 70 ? "C" :
                          score >= 60 ? "D" : "F";
            
            Console.WriteLine($"\nScore: {score}, Grade: {grade}");
            
            // ====================================================================
            // PRACTICAL EXAMPLES
            // ====================================================================
            
            // Default value
            string input = "";
            string name = string.IsNullOrEmpty(input) ? "Guest" : input;
            Console.WriteLine($"\nInput: '{input}', Name: '{name}'");
            
            input = "John";
            name = string.IsNullOrEmpty(input) ? "Guest" : input;
            Console.WriteLine($"Input: '{input}', Name: '{name}'");
            
            // Abs() replacement
            int num = -10;
            int absValue = num < 0 ? -num : num;
            Console.WriteLine($"\nAbsolute value of {num}: {absValue}");
            
            // Even/Odd
            int number = 7;
            string evenOdd = (number % 2 == 0) ? "Even" : "Odd";
            Console.WriteLine($"{number} is: {evenOdd}");
        }
    }
}

/*
TERNARY OPERATOR:
-----------------
Shortens simple if-else into one line.
- condition ? trueValue : falseValue

Use when:
- Simple true/false choice
- Assigning one of two values

Avoid when:
- Complex conditions
- Too nested (hard to read)
*/


// ================================================================================
// SECTION 7: NULL-COALESCING OPERATORS
// ================================================================================

namespace NullCoalescing
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // NULL-COALESCING OPERATORS
            // ====================================================================
            
            // ??  Null-coalescing (if null, use alternative)
            // ??= Null-coalescing assignment (assign if null)
            
            // Basic null-coalescing
            string name = null;
            string displayName = name ?? "Guest";
            Console.WriteLine($"Name: {displayName}");  // Guest
            
            name = "Alice";
            displayName = name ?? "Guest";
            Console.WriteLine($"Name: {displayName}");  // Alice
            
            // Nullable types
            int? nullableInt = null;
            int value = nullableInt ?? 0;
            Console.WriteLine($"\nNullable int: {value}");  // 0
            
            nullableInt = 42;
            value = nullableInt ?? 0;
            Console.WriteLine($"Nullable int: {value}");   // 42
            
            // Null-coalescing assignment (C# 8+)
            string message = null;
            message ??= "Default message";
            Console.WriteLine($"\nMessage: {message}");  // Default message
            
            message ??= "Another message";  // Won't assign (not null)
            Console.WriteLine($"Message: {message}");   // Still Default message
            
            // Chain multiple
            string first = null;
            string second = null;
            string third = "Hello";
            
            string result = first ?? second ?? third ?? "Default";
            Console.WriteLine($"\nChained result: {result}");  // Hello
        }
    }
}

/*
NULL-COALESCING USE CASES:
--------------------------
- Default values for null
- Safe property access
- Providing fallbacks
- Simplifying null checks

This makes code much cleaner than nested if-null checks!
*/


// ================================================================================
// SECTION 8: OPERATOR PRECEDENCE
// ================================================================================

namespace OperatorPrecedence
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // OPERATOR PRECEDENCE (Order of Operations)
            // ====================================================================
            
            // C# follows mathematical precedence:
            // 1. Parentheses ()
            // 2. Unary (-, +, !, ~, ++, --)
            // 3. Multiplicative (*, /, %)
            // 4. Additive (+, -)
            // 5. Shift (<<, >>)
            // 6. Relational (<, >, <=, >=, is, as)
            // 7. Equality (==, !=)
            // 8. Bitwise AND (&)
            // 9. Bitwise XOR (^)
            // 10. Bitwise OR (|)
            // 11. Logical AND (&&)
            // 12. Logical OR (||)
            // 13. Ternary (?:)
            // 14. Assignment (=, +=, -=, *=, /=, %=, &=, ^=, |=, <<=, >>=)
            
            // Examples
            int result = 2 + 3 * 4;           // 14 (not 20!)
            Console.WriteLine($"2 + 3 * 4 = {result}");
            
            result = (2 + 3) * 4;             // 20
            Console.WriteLine($"(2 + 3) * 4 = {result}");
            
            // Complex
            bool b = 2 + 3 > 4 && 5 < 6;      // true && true = true
            Console.WriteLine($"2 + 3 > 4 && 5 < 6 = {b}");
            
            // Use parentheses to be clear!
            // When in doubt, use parentheses!
        }
    }
}

/*
PRECEDENCE RULE:
----------------
When unsure, use parentheses!
(2 + 3) * 4 is clearer than 2 + 3 * 4

It also prevents bugs and makes your intent clear to others.
*/


// ================================================================================
// SECTION 9: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Using = instead of ==
---------------------------------
if (x = 5)    // ERROR! Assignment, not comparison!
if (x == 5)   // CORRECT!


MISTAKE 2: Confusing & and &&
--------------------------------
&  - Bitwise AND (works on numbers)
&& - Logical AND (works on booleans)

Use && for boolean logic!


MISTAKE 3: Integer division surprise
------------------------------------
int result = 5 / 2;   // result = 2 (not 2.5!)


MISTAKE 4: Not using parentheses
---------------------------------
bool result = x + y == a + b;   // What order? Confusing!
bool result = (x + y) == (a + b);  // Clear!


MISTAKE 5: Confusing | and ||
-------------------------------
|  - Bitwise OR
|| - Logical OR (short-circuits)

Use || for boolean logic!
*/


// ================================================================================
// SECTION 10: PRACTICE EXAMPLES
// ================================================================================

/*
EXERCISE 1: Grade Calculator
---------------------------
Given a score (0-100), use ternary to print:
- "Pass" if score >= 60
- "Fail" if score < 60

EXERCISE 2: Leap Year Checker
------------------------------
Year is leap if:
- Divisible by 4 AND not by 100
- OR divisible by 400

Use operators to determine if a year is leap year.

EXERCISE 3: Login Validator
----------------------------
Check if user can login:
- Username must be "admin"
- Password must be "password123"
- Account must not be locked

Use && and || to combine conditions.

EXERCISE 4: Max of Three
------------------------
Find the largest of three numbers using only operators
(NO if statements!)

EXERCISE 5: Number Properties
-----------------------------
Given a number, determine:
- Is positive? (greater than 0)
- Is even? (modulus 2 equals 0)
- Is it a single digit? (0-9)
*/


// ================================================================================
// SECTION 11: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between == and === in C#?
A: C# only has == (no ===). The == compares values. For reference types,
   it compares references unless Equals() is overridden.

Q2: What is the difference between && and &?
A: && is logical AND with short-circuit evaluation (stops if first false).
   & is bitwise AND (evaluates both) or logical AND without short-circuit.

Q3: What does the % (modulus) operator do?
A: Returns the remainder after division. Example: 10 % 3 = 1.

Q4: What is the ternary operator?
A: Shorthand for if-else: condition ? valueIfTrue : valueIfFalse

Q5: What is operator precedence?
A: The order in which operators are evaluated. Use () to override
   default precedence when unsure.

Q6: What is short-circuit evaluation?
A: With &&, if first condition is false, second is never evaluated.
   With ||, if first condition is true, second is never evaluated.
   This improves performance and allows null checking.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
GREAT WORK! You now understand:
- All arithmetic, assignment, comparison, and logical operators
- The ternary operator for conditional expressions
- Null-coalescing operators for null handling
- Operator precedence

WHAT'S NEXT:
In Topic 06, we'll learn Control Statements - if-else, switch,
and other ways to control the flow of your program based on conditions.
*/
