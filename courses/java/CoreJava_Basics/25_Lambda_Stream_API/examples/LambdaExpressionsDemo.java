// LambdaExpressionsDemo - Demonstrates Lambda Expressions in Java
// Essential for modern Java backend development
// Maps to functional programming concepts in TypeScript

import java.util.*;

public class LambdaExpressionsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== LAMBDA EXPRESSIONS DEMO ===\n");
        
        // Traditional way
        Comparator<String> comparator1 = new Comparator<String>() {
            @Override
            public int compare(String a, String b) {
                return a.compareTo(b);
            }
        };
        
        // Lambda way - more concise
        Comparator<String> comparator2 = (a, b) -> a.compareTo(b);
        
        // More lambda examples
        Runnable runnable = () -> System.out.println("Hello from lambda!");
        runnable.run();
        
        // Function with parameters
        Calculator add = (a, b) -> a + b;
        Calculator multiply = (a, b) -> a * b;
        
        System.out.println("5 + 3 = " + add.calculate(5, 3));
        System.out.println("5 * 3 = " + multiply.calculate(5, 3));
        
        // Method references
        List<String> names = Arrays.asList("Alice", "Bob", "Charlie");
        names.forEach(System.out::println);
        
        System.out.println("\n=== KEY BENEFITS ===");
        System.out.println("1. Concise and readable code");
        System.out.println("2. Better support for functional programming");
        System.out.println("3. Maps well to JavaScript arrow functions");
    }
}

// Functional Interface
interface Calculator {
    int calculate(int a, int b);
}
