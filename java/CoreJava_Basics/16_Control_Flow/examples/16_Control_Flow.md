# Java Control Flow Statements

## Table of Contents
1. [Introduction](#introduction)
2. [Decision Making Statements](#decision-making-statements)
3. [Loops](#loops)
4. [Jump Statements](#jump-statements)
5. [Switch Expressions (Java 14+)](#switch-expressions-java-14)
6. [Code Examples](#code-examples)
7. [Exercises](#exercises)
8. [Solutions](#solutions)

---

## 1. Introduction

### What are Control Flow Statements?

Control flow statements control the order in which statements are executed in a program. They allow you to make decisions, repeat actions, and transfer control.

```
┌─────────────────────────────────────────────────────────────┐
│              CONTROL FLOW STATEMENTS                          │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ┌─────────────────────────────────────────────────┐        │
│   │              DECISION MAKING                     │        │
│   │                                                 │        │
│   │   ┌─────────┐      ┌─────────┐                 │        │
│   │   │   if    │      │ switch  │                 │        │
│   │   └─────────┘      └─────────┘                 │        │
│   └─────────────────────────────────────────────────┘        │
│                                                              │
│   ┌─────────────────────────────────────────────────┐        │
│   │                 LOOPS                           │        │
│   │                                                 │        │
│   │   ┌────────┐  ┌─────────┐  ┌─────────┐         │        │
│   │   │  for   │  │  while  │  │ do-while│         │        │
│   │   └────────┘  └─────────┘  └─────────┘         │        │
│   │                                                 │        │
│   │   ┌─────────────────────────────────────┐    │        │
│   │   │         for-each (enhanced for)      │    │        │
│   │   └─────────────────────────────────────┘    │        │
│   └─────────────────────────────────────────────────┘        │
│                                                              │
│   ┌─────────────────────────────────────────────────┐        │
│   │              JUMP STATEMENTS                       │        │
│   │   break   │   continue   │   return               │        │
│   └─────────────────────────────────────────────────┘        │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Decision Making Statements

### 2.1 if-else Statement

```java
// Simple if
if (condition) {
    // executes if condition is true
}

// if-else
if (condition) {
    // executes if true
} else {
    // executes if false
}

// else-if ladder
if (condition1) {
    // block 1
} else if (condition2) {
    // block 2
} else if (condition3) {
    // block 3
 // default block
}
```

###} else {
    2.2 switch Statement

```java
// Traditional switch
switch (variable) {
    case value1:
        // code for value1
        break;
    case value2:
        // code for value2
        break;
    default:
        // default code
}
```

---

## 3. Loops

### 3.1 for Loop

```java
// Traditional for loop
for (int i = 0; i < 10; i++) {
    System.out.println(i);
}

// For loop with multiple variables
for (int i = 0, j = 10; i < j; i++, j--) {
    System.out.println(i + " " + j);
}
```

### 3.2 Enhanced for-each Loop

```java
// For-each loop (used with arrays and collections)
int[] numbers = {1, 2, 3, 4, 5};

for (int num : numbers) {
    System.out.println(num);
}
```

### 3.3 while Loop

```java
// While loop - checks condition first
while (condition) {
    // code to repeat
}
```

### 3.4 do-while Loop

```java
// do-while - executes at least once
do {
    // code to repeat
} while (condition);
```

---

## 4. Jump Statements

### 4.1 break Statement

- Used to exit from a loop or switch statement
- Terminates the nearest enclosing loop or switch

### 4.2 continue Statement

- Skips the current iteration of a loop
- Continues with the next iteration

### 4.3 return Statement

- Exits from a method
- Can return a value

---

## 5. Switch Expressions (Java 14+)

### Traditional vs Modern Switch

```java
// Traditional switch (old way)
int day = 3;
String dayName;

switch (day) {
    case 1:
        dayName = "Monday";
        break;
    case 2:
        dayName = "Tuesday";
        break;
    default:
        dayName = "Unknown";
}

// Modern switch expression (Java 14+)
String dayName = switch (day) {
    case 1 -> "Monday";
    case 2 -> "Tuesday";
    case 3 -> "Wednesday";
    default -> "Unknown";
};

// With multiple values
String weekend = switch (day) {
    case 1, 2, 3, 4, 5 -> "Weekday";
    case 6, 7 -> "Weekend";
    default -> "Invalid";
};
```

---

## 6. Code Examples

### Example 1: Grade Calculator with if-else

```java
import java.util.Scanner;

/**
 * GradeCalculator - Demonstrates if-else and switch
 * Shows how to handle different grade scenarios
 */
public class GradeCalculator {
    
    /**
     * Calculate grade using if-else
     */
    public static String getGradeIfElse(int score) {
        if (score >= 90 && score <= 100) {
            return "A - Excellent";
        } else if (score >= 80 && score < 90) {
            return "B - Good";
        } else if (score >= 70 && score < 80) {
            return "C - Average";
        } else if (score >= 60 && score < 70) {
            return "D - Below Average";
        } else if (score >= 0 && score < 60) {
            return "F - Fail";
        } else {
            return "Invalid score";
        }
    }
    
    /**
     * Calculate grade using switch expression (Java 14+)
     */
    public static String getGradeSwitch(int score) {
        // Convert score to grade range
        int gradeLevel = score / 10;
        
        return switch (gradeLevel) {
            case 10, 9 -> "A - Excellent";
            case 8 -> "B - Good";
            case 7 -> "C - Average";
            case 6 -> "D - Below Average";
            case 5, 4, 3, 2, 1, 0 -> "F - Fail";
            default -> "Invalid score";
        };
    }
    
    public static void main(String[] args) {
        System.out.println("=== GRADE CALCULATOR ===\n");
        
        // Test with different scores
        int[] testScores = {95, 87, 78, 65, 55, 105, -5};
        
        for (int score : testScores) {
            System.out.println("Score: " + score + " -> " + getGradeIfElse(score));
        }
        
        System.out.println("\n--- Using Switch Expression ---");
        for (int score : testScores) {
            System.out.println("Score: " + score + " -> " + getGradeSwitch(score));
        }
    }
}
```

---

### Example 2: Number Patterns with Loops

```java
/**
 * PatternPrinter - Demonstrates various loop patterns
 * Useful for understanding loop control
 */
public class PatternPrinter {
    
    /**
     * Print a simple triangle pattern
     * *
     * **
     * ***
     * ****
     * *****
     */
    public static void printTriangle(int rows) {
        System.out.println("=== Triangle Pattern ===");
        for (int i = 1; i <= rows; i++) {
            for (int j = 1; j <= i; j++) {
                System.out.print("* ");
            }
            System.out.println();
        }
    }
    
    /**
     * Print inverted triangle
     * *****
     * ****
     * ***
     * **
     * *
     */
    public static void printInvertedTriangle(int rows) {
        System.out.println("\n=== Inverted Triangle ===");
        for (int i = rows; i >= 1; i--) {
            for (int j = 1; j <= i; j++) {
                System.out.print("* ");
            }
            System.out.println();
        }
    }
    
    /**
     * Print number pyramid
     *     1
     *    2 2
     *   3 3 3
     *  4 4 4 4
     * 5 5 5 5 5
     */
    public static void printNumberPyramid(int rows) {
        System.out.println("\n=== Number Pyramid ===");
        for (int i = 1; i <= rows; i++) {
            // Print spaces
            for (int s = 1; s <= rows - i; s++) {
                System.out.print("  ");
            }
            // Print numbers
            for (int j = 1; j <= i; j++) {
                System.out.print(i + " ");
            }
            System.out.println();
        }
    }
    
    /**
     * Calculate factorial using while loop
     */
    public static long factorial(int number) {
        if (number < 0) {
            throw new IllegalArgumentException("Number must be non-negative");
        }
        
        long result = 1;
        int counter = number;
        
        while (counter > 0) {
            result *= counter;
            counter--;
        }
        
        return result;
    }
    
    /**
     * Calculate factorial using for loop
     */
    public static long factorialFor(int number) {
        if (number < 0) {
            throw new IllegalArgumentException("Number must be non-negative");
        }
        
        long result = 1;
        for (int i = number; i > 0; i--) {
            result *= i;
        }
        
        return result;
    }
    
    /**
     * Print Fibonacci series
     */
    public static void printFibonacci(int count) {
        System.out.println("\n=== Fibonacci Series (" + count + " terms) ===");
        int first = 0, second = 1;
        
        System.out.print(first + " " + second);
        
        for (int i = 2; i < count; i++) {
            int next = first + second;
            System.out.print(" " + next);
            first = second;
            second = next;
        }
        System.out.println();
    }
    
    public static void main(String[] args) {
        // Print patterns
        printTriangle(5);
        printInvertedTriangle(5);
        printNumberPyramid(5);
        
        // Calculate factorials
        System.out.println("\n=== Factorial Calculations ===");
        for (int i = 0; i <= 10; i++) {
            System.out.println(i + "! = " + factorial(i));
        }
        
        // Print Fibonacci
        printFibonacci(10);
    }
}
```

---

### Example 3: Menu-Driven Calculator with Switch

```java
import java.util.Scanner;

/**
 * Calculator - Menu-driven calculator using switch
 * Demonstrates switch statement and loop control
 */
public class Calculator {
    
    /**
     * Add two numbers
     */
    public static double add(double a, double b) {
        return a + b;
    }
    
    /**
     * Subtract two numbers
     */
    public static double subtract(double a, double b) {
        return a - b;
    }
    
    /**
     * Multiply two numbers
     */
    public static double multiply(double a, double b) {
        return a * b;
    }
    
    /**
     * Divide two numbers
     */
    public static double divide(double a, double b) {
        if (b == 0) {
            System.out.println("Error: Division by zero!");
            return Double.NaN;
        }
        return a / b;
    }
    
    /**
     * Calculate power
     */
    public static double power(double base, double exponent) {
        return Math.pow(base, exponent);
    }
    
    /**
     * Calculate square root
     */
    public static double squareRoot(double number) {
        if (number < 0) {
            System.out.println("Error: Cannot calculate square root of negative!");
            return Double.NaN;
        }
        return Math.sqrt(number);
    }
    
    /**
     * Perform calculation based on operator
     */
    public static double calculate(double num1, double num2, char operator) {
        return switch (operator) {
            case '+' -> add(num1, num2);
            case '-' -> subtract(num1, num2);
            case '*' -> multiply(num1, num2);
            case '/' -> divide(num1, num2);
            case '^' -> power(num1, num2);
            default -> {
                System.out.println("Invalid operator!");
                yield Double.NaN;
            }
        };
    }
    
    /**
     * Display menu
     */
    public static void displayMenu() {
        System.out.println("\n╔═══════════════════════╗");
        System.out.println("║   CALCULATOR MENU    ║");
        System.out.println("╠═══════════════════════╣");
        System.out.println("║ 1. Addition (+)      ║");
        System.out.println("║ 2. Subtraction (-)   ║");
        System.out.println("║ 3. Multiplication (*)║");
        System.out.println("║ 4. Division (/)      ║");
        System.out.println("║ 5. Power (^)        ║");
        System.out.println("║ 6. Square Root      ║");
        System.out.println("║ 7. Exit             ║");
        System.out.println("╚═══════════════════════╝");
    }
    
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int choice;
        
        System.out.println("=== MENU-DRIVEN CALCULATOR ===");
        
        do {
            displayMenu();
            System.out.print("Enter your choice (1-7): ");
            choice = scanner.nextInt();
            
            switch (choice) {
                case 1 -> {
                    System.out.print("Enter first number: ");
                    double a = scanner.nextDouble();
                    System.out.print("Enter second number: ");
                    double b = scanner.nextDouble();
                    System.out.println("Result: " + add(a, b));
                }
                case 2 -> {
                    System.out.print("Enter first number: ");
                    double a = scanner.nextDouble();
                    System.out.print("Enter second number: ");
                    double b = scanner.nextDouble();
                    System.out.println("Result: " + subtract(a, b));
                }
                case 3 -> {
                    System.out.print("Enter first number: ");
                    double a = scanner.nextDouble();
                    System.out.print("Enter second number: ");
                    double b = scanner.nextDouble();
                    System.out.println("Result: " + multiply(a, b));
                }
                case 4 -> {
                    System.out.print("Enter first number: ");
                    double a = scanner.nextDouble();
                    System.out.print("Enter second number: ");
                    double b = scanner.nextDouble();
                    System.out.println("Result: " + divide(a, b));
                }
                case 5 -> {
                    System.out.print("Enter base: ");
                    double base = scanner.nextDouble();
                    System.out.print("Enter exponent: ");
                    double exp = scanner.nextDouble();
                    System.out.println("Result: " + power(base, exp));
                }
                case 6 -> {
                    System.out.print("Enter number: ");
                    double num = scanner.nextDouble();
                    System.out.println("Result: " + squareRoot(num));
                }
                case 7 -> System.out.println("Exiting calculator. Goodbye!");
                default -> System.out.println("Invalid choice! Please try again.");
            }
        } while (choice != 7);
        
        scanner.close();
    }
}
```

---

### Example 4: Find Prime Numbers with break and continue

```java
/**
 * PrimeNumberDemo - Demonstrates break and continue
 * Shows efficient prime number checking
 */
public class PrimeNumberDemo {
    
    /**
     * Check if a number is prime using break
     */
    public static boolean isPrime(int number) {
        if (number <= 1) {
            return false;
        }
        
        for (int i = 2; i <= Math.sqrt(number); i++) {
            if (number % i == 0) {
                return false;  // Found divisor, not prime
            }
        }
        
        return true;
    }
    
    /**
     * Print all prime numbers up to n using continue
     */
    public static void printPrimesUpTo(int n) {
        System.out.println("Prime numbers up to " + n + ":");
        
        for (int i = 2; i <= n; i++) {
            if (!isPrime(i)) {
                continue;  // Skip non-prime numbers
            }
            System.out.print(i + " ");
        }
        System.out.println();
    }
    
    /**
     * Find first n prime numbers
     */
    public static void findFirstNPrimes(int count) {
        System.out.println("\nFirst " + count + " prime numbers:");
        int found = 0;
        int number = 2;
        
        while (found < count) {
            if (isPrime(number)) {
                System.out.print(number + " ");
                found++;
            }
            number++;
        }
        System.out.println();
    }
    
    /**
     * Count primes in a range
     */
    public static int countPrimesInRange(int start, int end) {
        int count = 0;
        
        for (int i = start; i <= end; i++) {
            if (isPrime(i)) {
                count++;
            }
        }
        
        return count;
    }
    
    /**
     * Print prime factors of a number
     */
    public static void printPrimeFactors(int number) {
        if (number <= 1) {
            System.out.println("No prime factors");
            return;
        }
        
        System.out.print("Prime factors of " + number + ": ");
        
        // Handle factor of 2
        while (number % 2 == 0) {
            System.out.print(2 + " ");
            number /= 2;
        }
        
        // Handle odd factors
        for (int i = 3; i <= Math.sqrt(number); i += 2) {
            while (number % i == 0) {
                System.out.print(i + " ");
                number /= i;
            }
        }
        
        // If remaining number is greater than 1
        if (number > 1) {
            System.out.print(number);
        }
        System.out.println();
    }
    
    public static void main(String[] args) {
        // Test isPrime
        System.out.println("=== Prime Number Tests ===");
        int[] testNumbers = {1, 2, 3, 4, 5, 17, 18, 19, 100};
        
        for (int num : testNumbers) {
            System.out.println(num + " is " + (isPrime(num) ? "prime" : "not prime"));
        }
        
        // Print primes
        printPrimesUpTo(50);
        
        // Find first primes
        findFirstNPrimes(15);
        
        // Count primes
        System.out.println("\nPrimes between 1 and 100: " + 
                          countPrimesInRange(1, 100));
        
        // Prime factors
        printPrimeFactors(100);
        printPrimeFactors(17);
    }
}
```

---

## 7. Exercises

### Exercise 1: Number Classification

**Requirements:**
1. Create a program that classifies a number as:
   - Positive, Negative, or Zero
   - Even or Odd
   - Prime or Composite
2. Use if-else statements
3. Test with multiple numbers

### Exercise 2: Multiplication Table

**Requirements:**
1. Print multiplication table (1-10) using nested loops
2. Format as proper table
3. Use for loops

### Exercise 3: ATM Simulator

**Requirements:**
1. Create menu with options: Check Balance, Deposit, Withdraw, Exit
2. Use switch statement for menu
3. Implement validation for withdrawal (cannot exceed balance)
4. Use do-while for menu loop

---

## 8. Solutions

### Solution 1: Number Classification

```java
public class NumberClassifier {
    
    public static void classify(int number) {
        System.out.println("\n=== Classifying " + number + " ===");
        
        // Positive/Negative/Zero
        if (number > 0) {
            System.out.println("Type: Positive");
        } else if (number < 0) {
            System.out.println("Type: Negative");
        } else {
            System.out.println("Type: Zero");
        }
        
        // Even/Odd (skip for zero)
        if (number != 0) {
            if (number % 2 == 0) {
                System.out.println("Parity: Even");
            } else {
                System.out.println("Parity: Odd");
            }
        }
        
        // Prime/Composite (only for positive)
        if (number > 1) {
            if (isPrime(number)) {
                System.out.println("Classification: Prime");
            } else {
                System.out.println("Classification: Composite");
            }
        } else if (number <= 1) {
            System.out.println("Classification: Not prime");
        }
    }
    
    public static boolean isPrime(int n) {
        if (n <= 1) return false;
        for (int i = 2; i <= Math.sqrt(n); i++) {
            if (n % i == 0) return false;
        }
        return true;
    }
    
    public static void main(String[] args) {
        int[] numbers = {0, 1, 2, 17, 18, 19, 100, -5};
        
        for (int num : numbers) {
            classify(num);
        }
    }
}
```

---

### Solution 2: Multiplication Table

```java
public class MultiplicationTable {
    
    public static void main(String[] args) {
        System.out.println("=== MULTIPLICATION TABLE (1-10) ===\n");
        
        // Print header
        System.out.print("     ");
        for (int i = 1; i <= 10; i++) {
            System.out.printf("%4d", i);
        }
        System.out.println();
        System.out.println("     " + "----".repeat(10));
        
        // Print table
        for (int i = 1; i <= 10; i++) {
            System.out.printf("%4d|", i);
            for (int j = 1; j <= 10; j++) {
                System.out.printf("%4d", i * j);
            }
            System.out.println();
        }
    }
}
```

---

### Solution 3: ATM Simulator

```java
import java.util.Scanner;

public class ATMSimulator {
    private double balance = 1000.0;
    
    public void checkBalance() {
        System.out.println("Your current balance: $" + balance);
    }
    
    public void deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            System.out.println("Deposited: $" + amount);
            System.out.println("New balance: $" + balance);
        } else {
            System.out.println("Invalid deposit amount!");
        }
    }
    
    public void withdraw(double amount) {
        if (amount > 0 && amount <= balance) {
            balance -= amount;
            System.out.println("Withdrawn: $" + amount);
            System.out.println("Remaining balance: $" + balance);
        } else if (amount > balance) {
            System.out.println("Insufficient funds!");
        } else {
            System.out.println("Invalid withdrawal amount!");
        }
    }
    
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        ATMSimulator atm = new ATMSimulator();
        int choice;
        
        do {
            System.out.println("\n╔════════════════╗");
            System.out.println("║   ATM MENU    ║");
            System.out.println("╠════════════════╣");
            System.out.println("║ 1. Balance    ║");
            System.out.println("║ 2. Deposit   ║");
            System.out.println("║ 3. Withdraw  ║");
            System.out.println("║ 4. Exit       ║");
            System.out.println("╚════════════════╝");
            
            System.out.print("Choice: ");
            choice = scanner.nextInt();
            
            switch (choice) {
                case 1 -> atm.checkBalance();
                case 2 -> {
                    System.out.print("Enter amount: ");
                    double dep = scanner.nextDouble();
                    atm.deposit(dep);
                }
                case 3 -> {
                    System.out.print("Enter amount: ");
                    double wd = scanner.nextDouble();
                    atm.withdraw(wd);
                }
                case 4 -> System.out.println("Thank you!");
                default -> System.out.println("Invalid choice!");
            }
        } while (choice != 4);
        
        scanner.close();
    }
}
```

---

## Summary

### Key Takeaways

1. **if-else** - Basic conditional execution
2. **switch** - Multi-way branching (use modern switch expressions!)
3. **for loop** - Known iteration count
4. **while loop** - Condition-based iteration
5. **do-while** - At least one execution
6. **for-each** - Iterate collections/arrays
7. **break** - Exit loops/switch
8. **continue** - Skip iteration
9. **return** - Exit methods

### Next Topics

- Strings and String Handling
- Arrays
- Exception Handling

---

*Happy Coding! 🚀*
