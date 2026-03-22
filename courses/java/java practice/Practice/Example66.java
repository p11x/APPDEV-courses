/*
 * SUB TOPIC: Java Control Flow - If-Else, Switch, Loops
 * 
 * DEFINITION:
 * Control flow statements direct the execution order of a program. They include conditional statements 
 * (if-else, switch) and loops (for, while, do-while).
 * 
 * FUNCTIONALITIES:
 * 1. if-else statement
 * 2. switch statement
 * 3. for loop
 * 4. while loop
 * 5. do-while loop
 * 6. Enhanced for loop
 */

public class Example66 {
    public static void main(String[] args) {
        
        // if-else
        System.out.println("=== If-Else ===");
        
        int age = 20;
        if (age >= 18) {
            System.out.println("Adult");
        } else {
            System.out.println("Minor");
        }
        
        // switch
        System.out.println("\n=== Switch ===");
        
        int day = 3;
        switch (day) {
            case 1: System.out.println("Monday"); break;
            case 2: System.out.println("Tuesday"); break;
            case 3: System.out.println("Wednesday"); break;
            default: System.out.println("Other");
        }
        
        // for loop
        System.out.println("\n=== For Loop ===");
        for (int i = 1; i <= 5; i++) {
            System.out.print(i + " ");
        }
        System.out.println();
        
        // while loop
        System.out.println("\n=== While Loop ===");
        int count = 1;
        while (count <= 5) {
            System.out.print(count + " ");
            count++;
        }
        System.out.println();
        
        // do-while
        System.out.println("\n=== Do-While ===");
        int n = 1;
        do {
            System.out.print(n + " ");
            n++;
        } while (n <= 5);
        System.out.println();
        
        // Real-time Example 1: Calculator
        System.out.println("\n=== Example 1: Calculator ===");
        
        int a = 10, b = 5;
        char op = '+';
        
        switch (op) {
            case '+': System.out.println(a + b); break;
            case '-': System.out.println(a - b); break;
            case '*': System.out.println(a * b); break;
            case '/': System.out.println(a / b); break;
        }
        
        // Real-time Example 2: Fibonacci
        System.out.println("\n=== Example 2: Fibonacci ===");
        
        int n1 = 0, n2 = 1, n3;
        System.out.print(n1 + " " + n2 + " ");
        for (int i = 2; i < 10; i++) {
            n3 = n1 + n2;
            System.out.print(n3 + " ");
            n1 = n2;
            n2 = n3;
        }
        System.out.println();
        
        // Real-time Example 3: Prime check
        System.out.println("\n=== Example 3: Prime ===");
        
        int primeCheck = 17;
        boolean isPrime = true;
        for (int i = 2; i <= primeCheck/2; i++) {
            if (primeCheck % i == 0) {
                isPrime = false;
                break;
            }
        }
        System.out.println(primeCheck + " is prime: " + isPrime);
        
        // Real-time Example 4: Sum
        System.out.println("\n=== Example 4: Sum ===");
        
        int sum = 0;
        for (int i = 1; i <= 100; i++) {
            sum += i;
        }
        System.out.println("Sum 1-100: " + sum);
        
        // Real-time Example 5: Number pattern
        System.out.println("\n=== Example 5: Pattern ===");
        
        for (int i = 1; i <= 5; i++) {
            for (int j = 1; j <= i; j++) {
                System.out.print(j + " ");
            }
            System.out.println();
        }
        
        // Real-time Example 6: Continue
        System.out.println("\n=== Example 6: Skip ===");
        
        for (int i = 1; i <= 10; i++) {
            if (i % 2 == 0) continue;
            System.out.print(i + " ");
        }
        System.out.println();
    }
}
