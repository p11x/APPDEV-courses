/*
================================================================================
TOPIC 06: CONTROL STATEMENTS
================================================================================

Control statements allow you to make decisions in your code based on
conditions. This is fundamental to programming.

TABLE OF CONTENTS:
1. Introduction to Control Flow
2. if Statements
3. if-else Statements
4. else-if Chains
5. Nested if Statements
6. switch Statements
7. switch Expressions (Modern C#)
8. Pattern Matching with switch
================================================================================
*/

// ================================================================================
// SECTION 1: INTRODUCTION TO CONTROL FLOW
// ================================================================================

/*
WHAT IS CONTROL FLOW?
---------------------
Control flow determines the order in which your code executes.
By default, code runs top to bottom, line by line.
Control statements let you change that order based on conditions.

TYPES OF CONTROL FLOW:
----------------------
1. CONDITIONAL (Decision Making):
   - if-else
   - switch

2. ITERATIVE (Looping):
   - for, while, do-while (Topic 07)

3. JUMP (Branching):
   - break, continue, return (later topics)

REAL-WORLD ANALOGY:
-------------------
Think of driving:
- Default: Go straight
- Red light: Stop (if statement)
- Turn left or right: Decision based on destination
- Choose route: Multiple options (switch)
*/


// ================================================================================
// SECTION 2: IF STATEMENTS
// ================================================================================

namespace IfStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // SIMPLE IF STATEMENT
            // ====================================================================
            
            // Syntax:
            // if (condition)
            // {
            //     // code to run if true
            // }
            
            int temperature = 75;
            
            // Only executes if condition is true
            if (temperature > 70)
            {
                Console.WriteLine("It's a warm day!");
            }
            
            // Multiple statements in block
            if (temperature > 80)
            {
                Console.WriteLine("It's hot!");
                Console.WriteLine("Turn on the AC.");
            }
            
            // Without braces (only one statement)
            if (temperature < 50)
                Console.WriteLine("It's cold!");
            
            // ====================================================================
            // BOOLEAN CONDITIONS
            // ====================================================================
            
            bool isRaining = false;
            
            if (isRaining)
            {
                Console.WriteLine("\nTake an umbrella!");
            }
            
            // Common mistake: Don't compare to true/false explicitly
            // WRONG:   if (isRaining == true)
            // RIGHT:   if (isRaining)
            
            // ====================================================================
            // COMPARISON IN CONDITIONS
            // ====================================================================
            
            int age = 18;
            
            if (age >= 18)
            {
                Console.WriteLine("\nYou are an adult.");
            }
            
            // Using operators in conditions
            int score = 85;
            
            if (score >= 60 && score <= 100)
            {
                Console.WriteLine("You passed!");
            }
        }
    }
}

/*
KEY POINTS:
-----------
- Condition must evaluate to true/false
- Use comparison operators (==, >, <, >=, <=, !=)
- Code in braces {} executes only if condition is true
- Without braces, only the next line is conditional
- Always use braces for readability!
*/


// ================================================================================
// SECTION 3: IF-ELSE STATEMENTS
// ================================================================================

namespace IfElseStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // IF-ELSE STATEMENT
            // ====================================================================
            
            // Syntax:
            // if (condition)
            // {
            //     // true block
            // }
            // else
            // {
            //     // false block
            // }
            
            int number = 10;
            
            if (number % 2 == 0)
            {
                Console.WriteLine($"{number} is even");
            }
            else
            {
                Console.WriteLine($"{number} is odd");
            }
            
            // ====================================================================
            // IF-ELSE WITH MULTIPLE CONDITIONS
            // ====================================================================
            
            int age = 25;
            
            if (age >= 65)
            {
                Console.WriteLine("\nSenior discount applies!");
            }
            else
            {
                Console.WriteLine("\nRegular price.");
            }
            
            // ====================================================================
            // ELSE-IF CHAIN
            // ====================================================================
            
            int score = 85;
            char grade;
            
            if (score >= 90)
            {
                grade = 'A';
            }
            else if (score >= 80)
            {
                grade = 'B';
            }
            else if (score >= 70)
            {
                grade = 'C';
            }
            else if (score >= 60)
            {
                grade = 'D';
            }
            else
            {
                grade = 'F';
            }
            
            Console.WriteLine($"\nScore: {score}, Grade: {grade}");
            
            // ====================================================================
            // PRACTICAL EXAMPLE: VOTING
            // ====================================================================
            
            Console.WriteLine("\n=== Voting Eligibility ===");
            
            int userAge = 17;
            bool isCitizen = true;
            
            if (userAge >= 18 && isCitizen)
            {
                Console.WriteLine("You can vote!");
            }
            else
            {
                Console.WriteLine("You cannot vote.");
                if (userAge < 18)
                {
                    Console.WriteLine("You are too young.");
                }
                if (!isCitizen)
                {
                    Console.WriteLine("You are not a citizen.");
                }
            }
        }
    }
}

/*
ELSE-IF CHAIN STRUCTURE:
-----------------------
if (condition1)
    // A
else if (condition2)
    // B
else if (condition3)
    // C
else
    // D (fallback)

Only ONE block executes!
Conditions are checked in order from top to bottom.
*/


// ================================================================================
// SECTION 4: NESTED IF STATEMENTS
// ================================================================================

namespace NestedIf
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // NESTED IF STATEMENTS
            // ====================================================================
            
            // You can nest if statements inside other if statements
            
            int age = 25;
            bool hasLicense = true;
            
            Console.WriteLine("=== Car Rental Check ===");
            
            if (age >= 21)
            {
                Console.WriteLine("Age requirement met.");
                
                if (hasLicense)
                {
                    Console.WriteLine("License verified.");
                    Console.WriteLine("You can rent a car!");
                }
                else
                {
                    Console.WriteLine("You need a license to rent.");
                }
            }
            else
            {
                Console.WriteLine("You must be 21 or older.");
            }
            
            // ====================================================================
            // SIMPLIFYING WITH LOGICAL OPERATORS
            // ====================================================================
            
            // The above can be simplified to:
            
            if (age >= 21 && hasLicense)
            {
                Console.WriteLine("\nYou can rent a car! (Simplified)");
            }
            else
            {
                Console.WriteLine("\nCannot rent a car.");
            }
            
            // ====================================================================
            // NESTED WITH ELSE
            // ====================================================================
            
            bool isMember = true;
            double purchaseAmount = 150;
            
            Console.WriteLine("\n=== Discount Calculator ===");
            
            if (purchaseAmount > 100)
            {
                if (isMember)
                {
                    Console.WriteLine("20% discount (gold member + $100+)");
                }
                else
                {
                    Console.WriteLine("10% discount (non-member + $100+)");
                }
            }
            else
            {
                if (isMember)
                {
                    Console.WriteLine("5% discount (gold member + under $100)");
                }
                else
                {
                    Console.WriteLine("No discount");
                }
            }
        }
    }
}

/*
NESTING GUIDELINES:
-------------------
- Keep nesting to 3 levels or less when possible
- Use logical operators (&&, ||) to reduce nesting
- Consider refactoring into separate methods
- Use switch for multiple discrete values
*/


// ================================================================================
// SECTION 5: SWITCH STATEMENTS
// ================================================================================

namespace SwitchStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // SWITCH STATEMENT
            // ====================================================================
            
            // Syntax:
            // switch (variable)
            // {
            //     case value1:
            //         // code
            //         break;
            //     case value2:
            //         // code
            //         break;
            //     default:
            //         // code
            //         break;
            // }
            
            int dayNumber = 3;
            string dayName;
            
            switch (dayNumber)
            {
                case 1:
                    dayName = "Monday";
                    break;
                case 2:
                    dayName = "Tuesday";
                    break;
                case 3:
                    dayName = "Wednesday";
                    break;
                case 4:
                    dayName = "Thursday";
                    break;
                case 5:
                    dayName = "Friday";
                    break;
                case 6:
                    dayName = "Saturday";
                    break;
                case 7:
                    dayName = "Sunday";
                    break;
                default:
                    dayName = "Invalid day";
                    break;
            }
            
            Console.WriteLine($"Day {dayNumber} is {dayName}");
            
            // ====================================================================
            // MULTIPLE CASES
            // ====================================================================
            
            char grade = 'B';
            
            switch (grade)
            {
                case 'A':
                case 'B':
                case 'C':
                    Console.WriteLine("\nYou passed!");
                    break;
                case 'D':
                case 'F':
                    Console.WriteLine("\nYou failed.");
                    break;
                default:
                    Console.WriteLine("\nInvalid grade.");
                    break;
            }
            
            // ====================================================================
            // STRING SWITCH
            // ====================================================================
            
            string fruit = "apple";
            
            switch (fruit.ToLower())  // ToLower makes it case-insensitive
            {
                case "apple":
                    Console.WriteLine("\nApples are $0.50 each.");
                    break;
                case "banana":
                    Console.WriteLine("\nBananas are $0.30 each.");
                    break;
                case "orange":
                    Console.WriteLine("\nOranges are $0.75 each.");
                    break;
                default:
                    Console.WriteLine("\nUnknown fruit.");
                    break;
            }
        }
    }
}

/*
SWITCH RULES:
-------------
- Each case needs a break (or return, throw)
- default is optional (runs if no match)
- Cases must be constant values (not variables)
- Very efficient for multiple discrete values
*/


// ================================================================================
// SECTION 6: SWITCH EXPRESSIONS (MODERN C#)
// ================================================================================

namespace SwitchExpression
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // SWITCH EXPRESSION (C# 8+) - More concise!
            // ====================================================================
            
            // Traditional switch (verbose)
            int num = 2;
            string result;
            
            switch (num)
            {
                case 1:
                    result = "One";
                    break;
                case 2:
                    result = "Two";
                    break;
                case 3:
                    result = "Three";
                    break;
                default:
                    result = "Other";
                    break;
            }
            Console.WriteLine($"Traditional: {result}");
            
            // Switch expression (concise!)
            result = num switch
            {
                1 => "One",
                2 => "Two", 
                3 => "Three",
                _ => "Other"   // underscore is like default
            };
            Console.WriteLine($"Expression: {result}");
            
            // ====================================================================
            // MULTIPLE VALUES IN SWITCH EXPRESSION
            // ====================================================================
            
            char grade = 'B';
            
            string message = grade switch
            {
                'A' => "Excellent!",
                'B' => "Good job!",
                'C' => "Average.",
                'D' => "Needs improvement.",
                'F' => "Failed.",
                _ => "Invalid grade."
            };
            
            Console.WriteLine($"\nGrade: {grade} - {message}");
            
            // ====================================================================
            // WITH CONDITIONS (WHEN CLAUSE)
            // ====================================================================
            
            int score = 85;
            
            string passStatus = score switch
            {
                >= 90 => "A - Excellent",
                >= 80 => "B - Good",
                >= 70 => "C - Satisfactory",
                >= 60 => "D - Passing",
                >= 0  => "F - Failing",
                _     => "Invalid score"
            };
            
            Console.WriteLine($"\nScore {score}: {passStatus}");
            
            // ====================================================================
            // TUPLE PATTERN
            // ====================================================================
            
            int age = 25;
            bool isStudent = true;
            
            string ticketType = (age, isStudent) switch
            {
                (>= 65, _) => "Senior Ticket",
                (_, true) => "Student Ticket",
                _ => "Regular Ticket"
            };
            
            Console.WriteLine($"\nAge: {age}, Student: {isStudent}");
            Console.WriteLine($"Ticket: {ticketType}");
        }
    }
}

/*
SWITCH EXPRESSION BENEFITS:
--------------------------
- More concise and readable
- Expression returns a value
- Uses => (arrow) instead of case:
- Uses _ (discard) for default
- Can use patterns and conditions

This is the modern way - use it in new code!
*/


// ================================================================================
// SECTION 7: PATTERN MATCHING WITH SWITCH
// ================================================================================

namespace PatternMatching
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // TYPE PATTERN MATCHING
            // ====================================================================
            
            object[] values = { 42, "hello", 3.14, true, null };
            
            foreach (var value in values)
            {
                string description = value switch
                {
                    int i => $"Integer: {i}",
                    string s => $"String: {s} (length {s.Length})",
                    double d => $"Double: {d}",
                    bool b => $"Boolean: {b}",
                    null => "Null value",
                    _ => $"Unknown type: {value?.GetType().Name}"
                };
                
                Console.WriteLine(description);
            }
            
            // ====================================================================
            // RELATIONAL PATTERNS
            // ====================================================================
            
            Console.WriteLine("\n=== Temperature Advisory ===");
            
            int temp = 72;
            
            string advisory = temp switch
            {
                < 32 => "Freezing! Stay warm.",
                < 50 => "Cold weather.",
                < 70 => "Cool and pleasant.",
                < 85 => "Warm and nice.",
                < 100 => "Hot! Stay hydrated.",
                _ => "Dangerous heat!"
            };
            
            Console.WriteLine($"Temperature: {temp}°F");
            Console.WriteLine($"Advisory: {advisory}");
        }
    }
}

/*
PATTERN MATCHING POWER:
-----------------------
- Match by type
- Match by value
- Match with conditions (when)
- Match relational patterns (<, >, <=, >=)
- Much more expressive than traditional switch
*/


// ================================================================================
// SECTION 8: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Using = instead of ==
----------------------------------
if (x = 5)    // ERROR! Assignment, not comparison!


MISTAKE 2: Forgetting break in switch
---------------------------------------
case 1:
    Console.WriteLine("One");
    // No break! Falls through to next case!


MISTAKE 3: Forgetting braces
-----------------------------
if (isValid)
    Console.WriteLine("Valid");
    Console.WriteLine("This always runs!");  // Outside if!


MISTAKE 4: Wrong operator
-------------------------
if (x =! 5)   // What?! This is assignment + negation


MISTAKE 5: Comparing floats with ==
-----------------------------------
double x = 0.1 + 0.2;
if (x == 0.3)  // May be false due to precision!


MISTAKE 6: Using string in switch before C# 7
----------------------------------------------
Older C# only supported numbers and enums in switch.
Now strings are supported!
*/


// ================================================================================
// SECTION 9: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Calculator
-----------------------
Ask user for two numbers and an operator (+, -, *, /)
Use if-else or switch to perform calculation.
Handle division by zero!

EXERCISE 2: Grade System
-------------------------
Input: Score (0-100)
Output: Letter grade (A, B, C, D, F)
Use else-if chain.

EXERCISE 3: Day Type
---------------------
Input: Day number (1-7)
Output: "Weekday" or "Weekend"

EXERCISE 4: BMI Calculator
--------------------------
Calculate BMI: weight(kg) / height(m)^2
Print category: Underweight, Normal, Overweight, Obese

EXERCISE 5: Traffic Light
--------------------------
Input: Color (red, yellow, green)
Output: Action (stop, slow down, go)
*/


// ================================================================================
// SECTION 10: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between if-else and switch?
A: if-else evaluates boolean conditions, can handle complex expressions.
   switch matches against discrete values, more readable for many cases.

Q2: What is the purpose of the default case in switch?
A: default runs when no other case matches. It's like the else in if-else.

Q3: What is pattern matching in C#?
A: A feature that lets you check if a value has a certain "shape" -
   can be type, value, property, or relational pattern.

Q4: What is the difference between switch statement and switch expression?
A: Statement performs actions, expression returns a value. Expression
   is more concise and returns a value directly.

Q5: What happens if you forget break in a switch case?
A: Code "falls through" to the next case (unless using goto or throw).
   This is usually a bug - always include break!
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You now understand:
- if statements for conditional execution
- if-else for binary choices
- else-if chains for multiple conditions
- switch statements for discrete values
- Switch expressions (modern C#)
- Pattern matching

WHAT'S NEXT:
In Topic 07, we'll learn about Loops - how to repeat code multiple times.
This is essential for processing collections and automating tasks.
*/
