/*
 * SUB TOPIC: Java Methods - Definition and Usage
 * 
 * DEFINITION:
 * Methods are blocks of code that perform specific tasks. They promote code reuse and organization.
 * 
 * FUNCTIONALITIES:
 * 1. Method declaration
 * 2. Parameters and return
 * 3. Method overloading
 * 4. Recursive methods
 * 5. Static methods
 * 6. Varargs in methods
 */

public class Example68 {
    
    static int add(int a, int b) {
        return a + b;
    }
    
    static int add(int a, int b, int c) {
        return a + b + c;
    }
    
    static void greet(String name) {
        System.out.println("Hello, " + name);
    }
    
    public static void main(String[] args) {
        
        System.out.println("=== Method Call ===");
        System.out.println("5 + 3 = " + add(5, 3));
        System.out.println("5 + 3 + 2 = " + add(5, 3, 2));
        
        greet("John");
        
        System.out.println("\n=== Example 1: Factorial ===");
        System.out.println("5! = " + factorial(5));
        
        System.out.println("\n=== Example 2: Palindrome ===");
        System.out.println("madam: " + isPalindrome("madam"));
    }
    
    static int factorial(int n) {
        if (n <= 1) return 1;
        return n * factorial(n - 1);
    }
    
    static boolean isPalindrome(String s) {
        String rev = new StringBuilder(s).reverse().toString();
        return s.equals(rev);
    }
}
