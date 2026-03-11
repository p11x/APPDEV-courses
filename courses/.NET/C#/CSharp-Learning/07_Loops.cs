/*
================================================================================
TOPIC 07: LOOPS
================================================================================

Loops allow you to execute a block of code multiple times. This is essential
for processing data, automation, and many programming tasks.

TABLE OF CONTENTS:
1. Introduction to Loops
2. for Loop
3. foreach Loop
4. while Loop
5. do-while Loop
6. Nested Loops
7. Loop Control (break, continue)
8. Infinite Loops
9. Performance Considerations
================================================================================
*/

// ================================================================================
// SECTION 1: INTRODUCTION TO LOOPS
// ================================================================================

/*
WHAT ARE LOOPS?
---------------
Loops repeat code execution based on conditions. Instead of writing the same
code multiple times, you use a loop.

REAL-WORLD ANALOGY:
-------------------
Imagine you have to send emails to 100 people:
- Without loop: Write same email code 100 times!
- With loop: Write once, repeat 100 times

TYPES OF LOOPS IN C#:
---------------------
1. for loop - Known number of iterations
2. foreach loop - Iterate over collections
3. while loop - Unknown iterations, check first
4. do-while loop - At least once, check after
*/


// ================================================================================
// SECTION 2: FOR LOOP
// ================================================================================

namespace ForLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // FOR LOOP STRUCTURE
            // ====================================================================
            
            // Syntax:
            // for (initialization; condition; increment/decrement)
            // {
            //     // code to repeat
            // }
            
            // Example 1: Print numbers 1 to 5
            Console.WriteLine("=== Count 1 to 5 ===");
            
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine(i);
            }
            
            // Example 2: Countdown
            Console.WriteLine("\n=== Countdown ===");
            
            for (int i = 5; i > 0; i--)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("Blast off!");
            
            // Example 3: Even numbers
            Console.WriteLine("\n=== Even Numbers 2-10 ===");
            
            for (int i = 2; i <= 10; i += 2)
            {
                Console.WriteLine(i);
            }
            
            // Example 4: Sum of numbers
            Console.WriteLine("\n=== Sum 1 to 10 ===");
            
            int sum = 0;
            for (int i = 1; i <= 10; i++)
            {
                sum += i;
            }
            Console.WriteLine($"Sum: {sum}");
            
            // Example 5: Multiplication table
            Console.WriteLine("\n=== Multiplication Table for 5 ===");
            
            int number = 5;
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{number} x {i} = {number * i}");
            }
        }
    }
}

/*
FOR LOOP ANATOMY:
-----------------
for (① initialization; ② condition; ③ increment)
{
    ④ // body
}

① Initialize counter: int i = 1
② Check condition: i <= 5 (true → run body)
③ Increment: i++ (i becomes 2)
② Check condition: i <= 5 (true → run body)
... continues until condition is false
*/


// ================================================================================
// SECTION 3: FOREACH LOOP
// ================================================================================

namespace ForeachLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // FOREACH LOOP - For collections and arrays
            // ====================================================================
            
            // Syntax:
            // foreach (type variable in collection)
            // {
            //     // use variable
            // }
            
            // Example 1: Iterate over array
            string[] fruits = { "Apple", "Banana", "Cherry", "Date" };
            
            Console.WriteLine("=== Fruits ===");
            foreach (string fruit in fruits)
            {
                Console.WriteLine(fruit);
            }
            
            // Example 2: Sum of array
            int[] numbers = { 10, 20, 30, 40, 50 };
            int total = 0;
            
            foreach (int num in numbers)
            {
                total += num;
            }
            Console.WriteLine($"\nTotal: {total}");
            
            // Example 3: Find largest
            int[] scores = { 85, 92, 78, 95, 88 };
            int max = scores[0];  // Assume first is largest
            
            foreach (int score in scores)
            {
                if (score > max)
                {
                    max = score;
                }
            }
            Console.WriteLine($"Highest score: {max}");
            
            // Example 4: Iterate over string
            Console.WriteLine("\n=== Letters in 'HELLO' ===");
            string word = "HELLO";
            foreach (char letter in word)
            {
                Console.WriteLine(letter);
            }
        }
    }
}

/*
FOREACH ADVANTAGES:
-------------------
- Cleaner and simpler than for loop for collections
- Automatically handles array bounds
- Read-only access (can't modify the collection directly)
- Works with any IEnumerable (arrays, lists, etc.)

WHEN TO USE FOR:
----------------
- When you need the index
- When you need to skip elements
- When you need to modify the collection
*/


// ================================================================================
// SECTION 4: WHILE LOOP
// ================================================================================

namespace WhileLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // WHILE LOOP - Check first, then execute
            // ====================================================================
            
            // Syntax:
            // while (condition)
            // {
            //     // code
            // }
            
            // Example 1: Simple counter
            Console.WriteLine("=== While Counter ===");
            
            int count = 1;
            while (count <= 5)
            {
                Console.WriteLine(count);
                count++;  // Important! Don't forget!
            }
            
            // Example 2: User input validation
            Console.WriteLine("\n=== Input Validation ===");
            
            string input = "";
            while (input != "quit")
            {
                Console.Write("Enter 'quit' to exit: ");
                input = Console.ReadLine();
                Console.WriteLine($"You entered: {input}");
            }
            
            // Example 3: Find first number divisible by 7
            Console.WriteLine("\n=== Find Divisible ===");
            
            int num = 1;
            while (num % 7 != 0 || num == 0)
            {
                num++;
            }
            Console.WriteLine($"First number >100 divisible by 7: {num}");
            
            // Example 4: Calculate factorial
            Console.WriteLine("\n=== Factorial of 5 ===");
            
            int factorial = 1;
            int n = 5;
            while (n > 0)
            {
                factorial *= n;
                n--;
            }
            Console.WriteLine($"5! = {factorial}");
        }
    }
}

/*
WHILE LOOP RULES:
-----------------
- Condition checked BEFORE each iteration
- Could execute 0 times if condition is false initially
- Must have code to eventually make condition false
- Don't forget to update counter/variable!

COMMON BUG: Infinite loop if you forget to update the condition!
*/


// ================================================================================
// SECTION 5: DO-WHILE LOOP
// ================================================================================

namespace DoWhileLoop
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // DO-WHILE LOOP - Execute first, check after
            // ====================================================================
            
            // Syntax:
            // do
            // {
            //     // code
            // } while (condition);
            
            // Example 1: At least once execution
            Console.WriteLine("=== Do-While Example ===");
            
            int number = 10;
            do
            {
                Console.WriteLine(number);
                number--;
            } while (number > 5);
            
            // Example 2: Menu (always shows at least once)
            Console.WriteLine("\n=== Menu System ===");
            
            int choice;
            do
            {
                Console.WriteLine("\n1. Play Game");
                Console.WriteLine("2. View Score");
                Console.WriteLine("3. Quit");
                Console.Write("Enter choice: ");
                
                // Parse and handle
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Starting game...");
                            break;
                        case 2:
                            Console.WriteLine("Your score: 100");
                            break;
                        case 3:
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                else
                {
                    choice = 0;  // Invalid input
                }
            } while (choice != 3);
            
            // Example 3: Guessing game
            Console.WriteLine("\n=== Guessing Game ===");
            
            int secretNumber = 42;
            int guess;
            
            do
            {
                Console.Write("Guess the number (1-100): ");
                string guessStr = Console.ReadLine();
                int.TryParse(guessStr, out guess);
                
                if (guess < secretNumber)
                    Console.WriteLine("Too low!");
                else if (guess > secretNumber)
                    Console.WriteLine("Too high!");
            } while (guess != secretNumber);
            
            Console.WriteLine("You got it!");
        }
    }
}

/*
DO-WHILE vs WHILE:
------------------
- while: May execute 0 times (checks first)
- do-while: Always executes at least once (checks after)

Use do-while when:
- You need the code to run at least once
- Menu systems
- Input validation
- Games where user must act first
*/


// ================================================================================
// SECTION 6: NESTED LOOPS
// ================================================================================

namespace NestedLoops
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // NESTED LOOPS - Loop inside a loop
            // ====================================================================
            
            // Example 1: Multiplication table grid
            Console.WriteLine("=== Multiplication Grid ===");
            
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    Console.Write($"{i * j}\t");
                }
                Console.WriteLine();  // New line after each row
            }
            
            // Example 2: Rectangle of stars
            Console.WriteLine("\n=== Rectangle of Stars ===");
            
            int rows = 3;
            int cols = 5;
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            
            // Example 3: Triangle pattern
            Console.WriteLine("\n=== Triangle Pattern ===");
            
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            
            // Example 4: Sum of all elements in 2D array
            Console.WriteLine("\n=== 2D Array Sum ===");
            
            int[,] matrix = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            int sum = 0;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    sum += matrix[r, c];
                }
            }
            Console.WriteLine($"Sum: {sum}");
        }
    }
}

/*
NESTED LOOP COMPLEXITY:
-----------------------
- Each iteration of outer loop triggers complete inner loop
- Total iterations = outer count × inner count
- Be careful - nested loops can be slow!
- 3×3 = 9 iterations
- 100×100 = 10,000 iterations
*/


// ================================================================================
// SECTION 7: LOOP CONTROL - BREAK AND CONTINUE
// ================================================================================

namespace LoopControl
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // BREAK - Exit loop immediately
            // ====================================================================
            
            Console.WriteLine("=== Break Example ===");
            
            for (int i = 1; i <= 10; i++)
            {
                if (i == 5)
                {
                    break;  // Exit loop when i is 5
                }
                Console.WriteLine(i);
            }
            Console.WriteLine("Loop ended!");
            
            // Example: Find first even number
            Console.WriteLine("\n=== Find First Even ===");
            
            int[] nums = { 1, 3, 5, 8, 9, 11 };
            
            foreach (int num in nums)
            {
                if (num % 2 == 0)
                {
                    Console.WriteLine($"First even: {num}");
                    break;
                }
            }
            
            // ====================================================================
            // CONTINUE - Skip to next iteration
            // ====================================================================
            
            Console.WriteLine("\n=== Continue Example ===");
            
            for (int i = 1; i <= 5; i++)
            {
                if (i == 3)
                {
                    continue;  // Skip when i is 3
                }
                Console.WriteLine(i);
            }
            Console.WriteLine("Loop ended!");
            
            // Example: Print only odd numbers
            Console.WriteLine("\n=== Odd Numbers ===");
            
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                {
                    continue;  // Skip even numbers
                }
                Console.WriteLine(i);
            }
            
            // ====================================================================
            // BREAK IN WHILE TRUE
            // ====================================================================
            
            Console.WriteLine("\n=== While True with Break ===");
            
            int counter = 0;
            while (true)
            {
                counter++;
                Console.WriteLine(counter);
                
                if (counter >= 5)
                {
                    break;  // Exit infinite loop
                }
            }
        }
    }
}

/*
BREAK vs CONTINUE:
-----------------
BREAK: Exits the loop entirely
CONTINUE: Skips current iteration, goes to next

Use break when:
- Found what you're looking for
- Want to exit early based on condition

Use continue when:
- Want to skip specific cases
- Filter out unwanted values
*/


// ================================================================================
// SECTION 8: INFINITE LOOPS
// ================================================================================

namespace InfiniteLoops
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // INFINITE LOOP - Runs forever (use with caution!)
            // ====================================================================
            
            // Syntax: for (;;) or while (true)
            
            // Example: Use break to exit
            Console.WriteLine("=== Infinite Loop (press q to quit) ===");
            
            // Uncomment to try:
            /*
            while (true)
            {
                Console.Write("Enter input (q to quit): ");
                string input = Console.ReadLine();
                
                if (input == "q" || input == "Q")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
                
                Console.WriteLine($"You entered: {input}");
            }
            */
            
            // Example: For loop infinite
            // for (;;)
            // {
            //     Console.WriteLine("This runs forever!");
            // }
            
            Console.WriteLine("Infinite loop example skipped (commented out).");
            Console.WriteLine("Uncomment in code to try!");
        }
    }
}

/*
INFINITE LOOPS:
---------------
- for (;;) - No initialization, condition, or increment
- while (true) - Always true condition

DANGERS:
- Can crash your program!
- Must have break statement inside
- Use Ctrl+C to kill in console

LEGITIMATE USES:
- Game loops
- Server applications
- Event-driven programs
- When you need to run forever until external event
*/


// ================================================================================
// SECTION 9: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Off-by-one errors
-----------------------------
for (int i = 1; i <= n; i++)    // Runs n times (1 to n)
for (int i = 0; i < n; i++)     // Runs n times (0 to n-1)

MISTAKE 2: Forgetting to update counter
--------------------------------------
int i = 1;
while (i <= 5)
{
    Console.WriteLine(i);
    // Forgot i++ - infinite loop!

MISTAKE 3: Infinite loop
------------------------
while (true)
{
    // No break statement!

MISTAKE 4: Using wrong loop type
--------------------------------
Use for when you know iterations.
Use while when you don't know.
Use foreach for collections.

MISTAKE 5: Modifying collection in foreach
-------------------------------------------
// ERROR!
foreach (var item in collection)
{
    collection.Remove(item);  // Exception!

MISTAKE 6: Continue in wrong place
----------------------------------
for (int i = 0; i < 10; i++)
{
    continue;  // Skips i++, causes infinite loop!
}
*/


// ================================================================================
// SECTION 10: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Sum of Digits
-------------------------
Input: 123
Output: 6 (1+2+3)
Use while loop.

EXERCISE 2: Reverse Number
--------------------------
Input: 12345
Output: 54321

EXERCISE 3: FizzBuzz
--------------------
Print 1-20:
- "Fizz" if divisible by 3
- "Buzz" if divisible by 5
- "FizzBuzz" if divisible by both
- Otherwise print the number

EXERCISE 4: Prime Checker
------------------------
Input: 17
Output: "Prime" or "Not Prime"

EXERCISE 5: Pyramid
-------------------
Input: 5
Output:
*
**
***
****
*****
*/


// ================================================================================
// SECTION 11: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between for and foreach loops?
A: for gives you control over index and iteration. foreach is simpler for
   collections but doesn't give index access. foreach is often preferred
   for readability.

Q2: What is the difference between while and do-while?
A: while checks condition first, may execute 0 times. do-while checks
   after, always executes at least once.

Q3: What does break do in a loop?
A: break immediately exits the loop entirely, continuing with code after
   the loop.

Q4: What does continue do in a loop?
A: continue skips the rest of current iteration and moves to the next one.

Q5: How do you create an infinite loop in C#?
A: Either "for (;;)" or "while (true)". Must have break inside to exit.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You now understand:
- for loops for known iterations
- foreach for collections
- while loops for unknown iterations
- do-while for at-least-once execution
- Nested loops for complex patterns
- break and continue for control

WHAT'S NEXT:
In Topic 08, we'll learn about Methods - reusable blocks of code that
perform specific tasks. This is crucial for writing organized, maintainable code.
*/
