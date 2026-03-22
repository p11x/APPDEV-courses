/*
 * SUB TOPIC: Java 8+ Features - Default Methods, Static Interface Methods, Optional
 * 
 * DEFINITION:
 * Java 8 introduced default methods (methods with body in interfaces), static interface methods, and 
 * Optional class for null handling. Java 9+ added private interface methods and other enhancements.
 * 
 * FUNCTIONALITIES:
 * 1. Default methods in interfaces
 * 2. Static methods in interfaces
 * 3. Optional for null safety
 * 4. Method references
 * 5. Lambda with Optional
 */

import java.util.*;

public class Example54 {
    
    // Interface with default method
    interface Drawable {
        void draw();
        
        default void display() {
            System.out.println("Displaying drawable");
        }
    }
    
    // Interface with static method
    interface MathUtils {
        static int add(int a, int b) {
            return a + b;
        }
        
        static int multiply(int a, int b) {
            return a * b;
        }
    }
    
    static class Circle implements Drawable {
        @Override
        public void draw() {
            System.out.println("Drawing Circle");
        }
    }
    
    public static void main(String[] args) {
        
        // Default methods
        System.out.println("=== Default Methods ===");
        Circle circle = new Circle();
        circle.draw();
        circle.display();
        
        // Static interface methods
        System.out.println("\n=== Static Interface Methods ===");
        System.out.println("Add: " + MathUtils.add(5, 3));
        System.out.println("Multiply: " + MathUtils.multiply(5, 3));
        
        // Optional - null handling
        System.out.println("\n=== Optional ===");
        
        String nullName = null;
        Optional<String> opt = Optional.ofNullable(nullName);
        
        System.out.println("isPresent: " + opt.isPresent());
        System.out.println("orElse: " + opt.orElse("Default"));
        
        // Optional with value
        String name = "John";
        Optional<String> optName = Optional.ofNullable(name);
        System.out.println("\nWith value:");
        System.out.println("isPresent: " + optName.isPresent());
        System.out.println("orElse: " + optName.orElse("Default"));
        
        // Real-time Example 1: Optional with database
        System.out.println("\n=== Example 1: Find User ===");
        
        class User {
            String name;
            String email;
            
            User(String name, String email) {
                this.name = name;
                this.email = email;
            }
        }
        
        Map<Integer, User> users = new HashMap<>();
        users.put(1, new User("John", "john@email.com"));
        users.put(2, new User("Jane", null));
        
        Optional<User> found = Optional.ofNullable(users.get(1));
        System.out.println("User: " + found.map(u -> u.name).orElse("Not found"));
        
        Optional<User> notFound = Optional.ofNullable(users.get(3));
        System.out.println("Missing: " + notFound.map(u -> u.name).orElse("Not found"));
        
        // Real-time Example 2: Default method for validation
        System.out.println("\n=== Example 2: Validatable ===");
        
        interface Validatable {
            boolean isValid();
            
            default String validate() {
                return isValid() ? "Valid" : "Invalid";
            }
        }
        
        class Order implements Validatable {
            double amount;
            
            Order(double amount) {
                this.amount = amount;
            }
            
            @Override
            public boolean isValid() {
                return amount > 0;
            }
        }
        
        Order order1 = new Order(100);
        Order order2 = new Order(-50);
        
        System.out.println("Order 1: " + order1.validate());
        System.out.println("Order 2: " + order2.validate());
        
        // Real-time Example 3: Optional orElseThrow
        System.out.println("\n=== Example 3: Required Value ===");
        
        String config = null;
        
        try {
            String value = Optional.ofNullable(config)
                .orElseThrow(() -> new RuntimeException("Config required"));
        } catch (RuntimeException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Real-time Example 4: Optional map
        System.out.println("\n=== Example 4: Transform ===");
        
        Integer num = 10;
        Optional<Integer> optNum = Optional.ofNullable(num);
        
        String result = optNum
            .map(n -> n * 2)
            .map(n -> "Value: " + n)
            .orElse("No value");
        
        System.out.println(result);
        
        // Real-time Example 5: Method reference
        System.out.println("\n=== Example 5: Method Reference ===");
        
        List<String> names = Arrays.asList("John", "Jane", "Mike");
        names.forEach(System.out::println);
        
        // Real-time Example 6: Static method in utility
        System.out.println("\n=== Example 6: String Utils ===");
        
        interface StringUtils {
            static boolean isEmpty(String s) {
                return s == null || s.isEmpty();
            }
            
            static String reverse(String s) {
                return new StringBuilder(s).reverse().toString();
            }
        }
        
        System.out.println("Is empty: " + StringUtils.isEmpty(""));
        System.out.println("Reverse: " + StringUtils.reverse("Hello"));
    }
}
