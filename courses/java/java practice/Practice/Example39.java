/*
 * SUB TOPIC: Lambda Expressions in Java
 * 
 * DEFINITION:
 * Lambda expressions are anonymous functions that can be passed around as values. They provide a 
 * concise way to implement functional interfaces (interfaces with a single abstract method). 
 * Lambda expressions simplify code and enable functional programming in Java.
 * 
 * FUNCTIONALITIES:
 * 1. Syntax of lambda expressions
 * 2. Functional interfaces
 * 3. Lambda with parameters
 * 4. Lambda with body (block)
 * 5. Method references
 * 6. Built-in functional interfaces (Predicate, Function, Consumer, Supplier)
 */

import java.util.*; // Import collections
import java.util.function.*; // Import functional interfaces

public class Example39 {
    
    // Functional interface with single abstract method
    @FunctionalInterface
    interface MathOperation {
        int operation(int a, int b);
    }
    
    @FunctionalInterface
    interface StringOperation {
        String process(String str);
    }
    
    @FunctionalInterface
    interface Printer {
        void print(String message);
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Lambda Basics
        
        // Lambda without parameters
        System.out.println("=== Lambda without Parameters ===");
        Runnable runnable = () -> System.out.println("Hello from lambda!");
        runnable.run();
        
        // Lambda with parameters
        System.out.println("\n=== Lambda with Parameters ===");
        MathOperation add = (a, b) -> a + b;
        MathOperation multiply = (a, b) -> a * b;
        
        System.out.println("10 + 5 = " + add.operation(10, 5));
        System.out.println("10 * 5 = " + multiply.operation(10, 5));
        
        // Lambda with block body
        MathOperation division = (a, b) -> {
            if (b != 0) {
                return a / b;
            }
            return 0;
        };
        System.out.println("10 / 5 = " + division.operation(10, 5));
        
        // Method reference - using existing methods
        System.out.println("\n=== Method Reference ===");
        List<String> names = Arrays.asList("John", "Jane", "Mike");
        
        names.forEach(System.out::println);
        
        // Built-in Functional Interfaces
        
        // Predicate - returns boolean
        System.out.println("\n=== Predicate (filter) ===");
        Predicate<Integer> isEven = n -> n % 2 == 0;
        System.out.println("Is 10 even? " + isEven.test(10));
        System.out.println("Is 7 even? " + isEven.test(7));
        
        // Filter using predicate
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        List<Integer> evens = new ArrayList<>();
        for (Integer n : numbers) {
            if (isEven.test(n)) {
                evens.add(n);
            }
        }
        System.out.println("Even numbers: " + evens);
        
        // Function - transforms input to output
        System.out.println("\n=== Function (map) ===");
        Function<Integer, Integer> square = n -> n * n;
        System.out.println("5 squared = " + square.apply(5));
        
        // Consumer - accepts input, returns nothing
        System.out.println("\n=== Consumer ===");
        Consumer<String> printer = s -> System.out.println("Printing: " + s);
        printer.accept("Hello World");
        
        // Supplier - returns value
        System.out.println("\n=== Supplier ===");
        Supplier<Date> dateSupplier = () -> new Date();
        System.out.println("Current date: " + dateSupplier.get());
        
        // Real-time Example 1: Filter employees by salary
        System.out.println("\n=== Example 1: Employee Filter ===");
        
        class Employee {
            String name;
            double salary;
            
            Employee(String name, double salary) {
                this.name = name;
                this.salary = salary;
            }
        }
        
        List<Employee> employees = Arrays.asList(
            new Employee("John", 50000),
            new Employee("Jane", 60000),
            new Employee("Mike", 45000),
            new Employee("Sarah", 70000)
        );
        
        // Filter employees with salary > 50000
        Predicate<Employee> highSalary = e -> e.salary > 50000;
        
        System.out.println("Employees with salary > 50000:");
        for (Employee emp : employees) {
            if (highSalary.test(emp)) {
                System.out.println("  " + emp.name + ": $" + emp.salary);
            }
        }
        
        // Real-time Example 2: Transform names to uppercase
        System.out.println("\n=== Example 2: Transform Names ===");
        List<String> fruits = Arrays.asList("apple", "banana", "cherry", "date");
        
        Function<String, String> toUpperCase = s -> s.toUpperCase();
        
        System.out.print("Uppercase: ");
        for (String fruit : fruits) {
            System.out.print(toUpperCase.apply(fruit) + " ");
        }
        System.out.println();
        
        // Real-time Example 3: Process orders
        System.out.println("\n=== Example 3: Order Processing ===");
        
        class Order {
            String id;
            double amount;
            
            Order(String id, double amount) {
                this.id = id;
                this.amount = amount;
            }
        }
        
        List<Order> orders = Arrays.asList(
            new Order("ORD001", 100.0),
            new Order("ORD002", 250.0),
            new Order("ORD003", 50.0)
        );
        
        // Calculate total using consumer
        double[] total = {0};
        Consumer<Order> processOrder = order -> {
            System.out.println("Processing: " + order.id + " - $" + order.amount);
            total[0] += order.amount;
        };
        
        for (Order order : orders) {
            processOrder.accept(order);
        }
        System.out.println("Total: $" + total[0]);
        
        // Real-time Example 4: Generate random IDs
        System.out.println("\n=== Example 4: ID Generator ===");
        Supplier<String> idGenerator = () -> "ID-" + (int)(Math.random() * 10000);
        
        System.out.println("Generated IDs:");
        for (int i = 0; i < 5; i++) {
            System.out.println("  " + idGenerator.get());
        }
        
        // Real-time Example 5: Validate input
        System.out.println("\n=== Example 5: Input Validation ===");
        Predicate<String> isValidEmail = email -> email.contains("@") && email.contains(".");
        Predicate<String> isValidPhone = phone -> phone.length() == 10 && phone.matches("\\d+");
        
        String email = "user@example.com";
        String phone = "9876543210";
        
        System.out.println("Email '" + email + "' valid: " + isValidEmail.test(email));
        System.out.println("Phone '" + phone + "' valid: " + isValidPhone.test(phone));
        
        // Real-time Example 6: String operations pipeline
        System.out.println("\n=== Example 6: String Pipeline ===");
        
        // Chain functions
        Function<String, String> removeSpaces = s -> s.replaceAll(" ", "");
        Function<String, String> addPrefix = s -> "Hello " + s;
        Function<String, String> toUpper = s -> s.toUpperCase();
        
        // Compose functions
        Function<String, String> pipeline = removeSpaces.andThen(addPrefix).andThen(toUpper);
        
        String input = "World";
        System.out.println("Input: " + input);
        System.out.println("Output: " + pipeline.apply(input));
        
        // Bonus: Using lambda with Collections sort
        System.out.println("\n=== Bonus: Sorting with Lambda ===");
        List<String> cities = Arrays.asList("New York", "London", "Paris", "Tokyo");
        
        System.out.println("Original: " + cities);
        
        // Sort by length
        Collections.sort(cities, (a, b) -> a.length() - b.length());
        System.out.println("Sorted by length: " + cities);
        
        // Sort alphabetically reverse
        Collections.sort(cities, (a, b) -> b.compareTo(a));
        System.out.println("Sorted reverse: " + cities);
    }
}
