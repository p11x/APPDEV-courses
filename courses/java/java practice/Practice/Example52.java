/*
 * SUB TOPIC: Advanced Stream Operations - FlatMap, Reduce, Grouping
 * 
 * DEFINITION:
 * Advanced stream operations include flatMap (flatten nested collections), reduce (combine elements),
 * and groupingBy (group elements by criteria). These enable complex data transformations.
 * 
 * FUNCTIONALITIES:
 * 1. flatMap - Flatten nested collections
 * 2. reduce - Combine elements to single value
 * 3. groupingBy - Group elements by classifier
 * 4. partitioningBy - Partition into two groups
 * 5. Collectors.joining - Join strings
 */

import java.util.*;
import java.util.stream.*;

public class Example52 {
    public static void main(String[] args) {
        
        // flatMap - Flatten nested lists
        System.out.println("=== flatMap ===");
        List<List<Integer>> nested = Arrays.asList(
            Arrays.asList(1, 2),
            Arrays.asList(3, 4),
            Arrays.asList(5, 6)
        );
        
        List<Integer> flat = nested.stream()
            .flatMap(list -> list.stream())
            .collect(Collectors.toList());
        
        System.out.println("Nested: " + nested);
        System.out.println("Flat: " + flat);
        
        // reduce - Combine elements
        System.out.println("\n=== reduce ===");
        int sum = Stream.of(1, 2, 3, 4, 5)
            .reduce(0, (a, b) -> a + b);
        System.out.println("Sum: " + sum);
        
        // groupingBy - Group by category
        System.out.println("\n=== groupingBy ===");
        
        class Product {
            String category;
            String name;
            double price;
            
            Product(String category, String name, double price) {
                this.category = category;
                this.name = name;
                this.price = price;
            }
        }
        
        List<Product> products = Arrays.asList(
            new Product("Electronics", "Laptop", 999),
            new Product("Electronics", "Phone", 699),
            new Product("Clothing", "Shirt", 29),
            new Product("Clothing", "Jeans", 59)
        );
        
        Map<String, List<Product>> byCategory = products.stream()
            .collect(Collectors.groupingBy(p -> p.category));
        
        System.out.println("By Category:");
        byCategory.forEach((cat, prods) -> 
            System.out.println(cat + ": " + prods.size() + " items"));
        
        // Real-time Example 1: Employee by department
        System.out.println("\n=== Example 1: Employees ===");
        
        class Employee {
            String name;
            String dept;
            int salary;
            
            Employee(String name, String dept, int salary) {
                this.name = name;
                this.dept = dept;
                this.salary = salary;
            }
        }
        
        List<Employee> employees = Arrays.asList(
            new Employee("John", "IT", 70000),
            new Employee("Jane", "HR", 60000),
            new Employee("Mike", "IT", 75000),
            new Employee("Sara", "HR", 65000)
        );
        
        Map<String, List<Employee>> byDept = employees.stream()
            .collect(Collectors.groupingBy(e -> e.dept));
        
        byDept.forEach((dept, emps) -> {
            System.out.println(dept + ": ");
            emps.forEach(e -> System.out.println("  - " + e.name));
        });
        
        // Real-time Example 2: Sum by category
        System.out.println("\n=== Example 2: Sales by Category ===");
        
        Map<String, Double> sales = products.stream()
            .collect(Collectors.groupingBy(
                p -> p.category,
                Collectors.summingDouble(p -> p.price)
            ));
        
        sales.forEach((cat, total) -> 
            System.out.println(cat + ": $" + total));
        
        // Real-time Example 3: Count words
        System.out.println("\n=== Example 3: Word Count ===");
        
        String text = "hello world hello java world python";
        Map<String, Long> wordCount = Arrays.stream(text.split(" "))
            .collect(Collectors.groupingBy(w -> w, Collectors.counting()));
        
        wordCount.forEach((word, count) -> 
            System.out.println(word + ": " + count));
        
        // Real-time Example 4: Top N
        System.out.println("\n=== Example 4: Top 3 Salaries ===");
        
        List<Integer> salaries = Arrays.asList(50000, 75000, 45000, 80000, 60000);
        
        List<Integer> top3 = salaries.stream()
            .sorted((a, b) -> b - a)
            .limit(3)
            .collect(Collectors.toList());
        
        System.out.println("Top 3: " + top3);
        
        // Real-time Example 5: Partition
        System.out.println("\n=== Example 5: Pass/Fail ===");
        
        List<Integer> marks = Arrays.asList(45, 78, 92, 35, 67, 88);
        
        Map<Boolean, List<Integer>> passFail = marks.stream()
            .collect(Collectors.partitioningBy(m -> m >= 50));
        
        System.out.println("Pass: " + passFail.get(true));
        System.out.println("Fail: " + passFail.get(false));
        
        // Real-time Example 6: Joining
        System.out.println("\n=== Example 6: Join Names ===");
        
        List<String> names = Arrays.asList("John", "Jane", "Mike");
        String joined = names.stream()
            .collect(Collectors.joining(", "));
        
        System.out.println("Names: " + joined);
    }
}
