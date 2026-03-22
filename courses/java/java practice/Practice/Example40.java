/*
 * SUB TOPIC: Stream API in Java
 * 
 * DEFINITION:
 * The Stream API in Java is used to process collections of objects. A stream represents a sequence 
 * of elements and supports various operations like filter, map, reduce, collect, and iterate. Streams 
 * are lazy - intermediate operations are not executed until a terminal operation is called.
 * 
 * FUNCTIONALITIES:
 * 1. filter() - Filter elements based on condition
 * 2. map() - Transform elements
 * 3. sorted() - Sort elements
 * 4. distinct() - Remove duplicates
 * 5. limit() - Limit number of elements
 * 6. collect() - Collect results to collection
 * 7. forEach() - Iterate over elements
 * 8. Terminal operations (count, sum, min, max)
 */

import java.util.*;
import java.util.stream.*;

public class Example40 {
    public static void main(String[] args) {
        
        // Topic Explanation: Creating Streams
        
        // Create stream from collection
        System.out.println("=== Creating Streams ===");
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        
        // Filter - keep only even numbers
        System.out.println("\n=== filter() - Even Numbers ===");
        List<Integer> evens = numbers.stream()
            .filter(n -> n % 2 == 0)
            .collect(Collectors.toList());
        System.out.println("Even numbers: " + evens);
        
        // Map - transform elements (square each number)
        System.out.println("\n=== map() - Square Numbers ===");
        List<Integer> squares = numbers.stream()
            .map(n -> n * n)
            .collect(Collectors.toList());
        System.out.println("Squares: " + squares);
        
        // Filter + Map combined
        System.out.println("\n=== filter() + map() ===");
        List<Integer> evenSquares = numbers.stream()
            .filter(n -> n % 2 == 0)
            .map(n -> n * n)
            .collect(Collectors.toList());
        System.out.println("Even number squares: " + evenSquares);
        
        // Sorted - sort elements
        System.out.println("\n=== sorted() ===");
        List<String> fruits = Arrays.asList("banana", "apple", "cherry", "date");
        List<String> sortedFruits = fruits.stream()
            .sorted()
            .collect(Collectors.toList());
        System.out.println("Sorted: " + sortedFruits);
        
        // Sorted with custom comparator
        List<String> sortedByLength = fruits.stream()
            .sorted((a, b) -> a.length() - b.length())
            .collect(Collectors.toList());
        System.out.println("Sorted by length: " + sortedByLength);
        
        // Distinct - remove duplicates
        System.out.println("\n=== distinct() ===");
        List<Integer> withDuplicates = Arrays.asList(1, 2, 2, 3, 3, 3, 4, 5, 5);
        List<Integer> distinct = withDuplicates.stream()
            .distinct()
            .collect(Collectors.toList());
        System.out.println("Original: " + withDuplicates);
        System.out.println("Distinct: " + distinct);
        
        // Limit - take first N elements
        System.out.println("\n=== limit() ===");
        List<Integer> firstFive = numbers.stream()
            .limit(5)
            .collect(Collectors.toList());
        System.out.println("First 5: " + firstFive);
        
        // Skip - skip first N elements
        System.out.println("\n=== skip() ===");
        List<Integer> skipFirstFive = numbers.stream()
            .skip(5)
            .collect(Collectors.toList());
        System.out.println("After skipping 5: " + skipFirstFive);
        
        // Terminal Operations
        System.out.println("\n=== Terminal Operations ===");
        
        // count()
        long count = numbers.stream().count();
        System.out.println("Count: " + count);
        
        // sum()
        int sum = numbers.stream().mapToInt(Integer::intValue).sum();
        System.out.println("Sum: " + sum);
        
        // min() and max()
        Optional<Integer> min = numbers.stream().min(Integer::compare);
        Optional<Integer> max = numbers.stream().max(Integer::compare);
        System.out.println("Min: " + min.orElse(0));
        System.out.println("Max: " + max.orElse(0));
        
        // findFirst() and findAny()
        Optional<Integer> first = numbers.stream().findFirst();
        System.out.println("First: " + first.orElse(0));
        
        // anyMatch(), allMatch(), noneMatch()
        System.out.println("\n=== Match Operations ===");
        boolean anyEven = numbers.stream().anyMatch(n -> n % 2 == 0);
        boolean allPositive = numbers.stream().allMatch(n -> n > 0);
        boolean noneNegative = numbers.stream().noneMatch(n -> n < 0);
        
        System.out.println("Any even: " + anyEven);
        System.out.println("All positive: " + allPositive);
        System.out.println("None negative: " + noneNegative);
        
        // Real-time Example 1: Filter products by price
        System.out.println("\n=== Example 1: Filter Products ===");
        
        class Product {
            String name;
            double price;
            
            Product(String name, double price) {
                this.name = name;
                this.price = price;
            }
        }
        
        List<Product> products = Arrays.asList(
            new Product("Laptop", 999.99),
            new Product("Mouse", 29.99),
            new Product("Keyboard", 79.99),
            new Product("Monitor", 299.99),
            new Product("Headphones", 149.99)
        );
        
        // Filter products under $100
        List<Product> affordable = products.stream()
            .filter(p -> p.price < 100)
            .collect(Collectors.toList());
        
        System.out.println("Products under $100:");
        affordable.forEach(p -> System.out.println("  " + p.name + ": $" + p.price));
        
        // Real-time Example 2: Transform data - Apply discount
        System.out.println("\n=== Example 2: Apply Discount ===");
        
        List<Double> discounted = products.stream()
            .map(p -> p.price * 0.9) // 10% discount
            .collect(Collectors.toList());
        
        System.out.println("Prices after 10% discount:");
        discounted.forEach(p -> System.out.println("  $" + String.format("%.2f", p)));
        
        // Real-time Example 3: Group by category
        System.out.println("\n=== Example 3: Product Names ===");
        
        List<String> productNames = products.stream()
            .map(p -> p.name)
            .sorted()
            .collect(Collectors.toList());
        
        System.out.println("Product names: " + productNames);
        
        // Real-time Example 4: Calculate average
        System.out.println("\n=== Example 4: Average Price ===");
        
        double average = products.stream()
            .mapToDouble(p -> p.price)
            .average()
            .orElse(0.0);
        
        System.out.println("Average price: $" + String.format("%.2f", average));
        
        // Real-time Example 5: Find most expensive product
        System.out.println("\n=== Example 5: Most Expensive ===");
        
        Product mostExpensive = products.stream()
            .max(Comparator.comparingDouble(p -> p.price))
            .orElse(null);
        
        if (mostExpensive != null) {
            System.out.println("Most expensive: " + mostExpensive.name + " - $" + mostExpensive.price);
        }
        
        // Real-time Example 6: Chain multiple operations
        System.out.println("\n=== Example 6: Complex Pipeline ===");
        
        List<Integer> result = numbers.stream()
            .filter(n -> n > 3)           // Keep > 3
            .map(n -> n * 2)              // Double
            .distinct()                   // Remove duplicates
            .sorted(Comparator.reverseOrder())  // Sort reverse
            .limit(3)                    // Take first 3
            .collect(Collectors.toList());
        
        System.out.println("Result: " + result);
        
        // forEach - iterate without collecting
        System.out.println("\n=== forEach ===");
        System.out.print("Print each number: ");
        numbers.stream().forEach(n -> System.out.print(n + " "));
        System.out.println();
        
        // Real-time Example 7: Statistics
        System.out.println("\n=== Example 7: Statistics ===");
        
        IntSummaryStatistics stats = numbers.stream()
            .mapToInt(Integer::intValue)
            .summaryStatistics();
        
        System.out.println("Count: " + stats.getCount());
        System.out.println("Sum: " + stats.getSum());
        System.out.println("Min: " + stats.getMin());
        System.out.println("Max: " + stats.getMax());
        System.out.println("Average: " + stats.getAverage());
    }
}
