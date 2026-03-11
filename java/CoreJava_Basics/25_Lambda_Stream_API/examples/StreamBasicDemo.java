// StreamBasicDemo - Demonstrates Stream API Basic Operations
// Essential for processing collections in modern Java

import java.util.*;
import java.util.stream.*;

public class StreamBasicDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STREAM BASIC OPERATIONS DEMO ===\n");
        
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        
        // Filter - keep only even numbers
        List<Integer> evens = numbers.stream()
            .filter(n -> n % 2 == 0)
            .collect(Collectors.toList());
        System.out.println("Even numbers: " + evens);
        
        // Map - transform each element
        List<Integer> squares = numbers.stream()
            .map(n -> n * n)
            .collect(Collectors.toList());
        System.out.println("Squares: " + squares);
        
        // Filter + Map combined
        List<Integer> doubleOfEvens = numbers.stream()
            .filter(n -> n % 2 == 0)
            .map(n -> n * 2)
            .collect(Collectors.toList());
        System.out.println("Double of evens: " + doubleOfEvens);
        
        System.out.println("\n=== KEY CONCEPTS ===");
        System.out.println("filter() - Select elements based on condition");
        System.out.println("map() - Transform each element");
        System.out.println("collect() - Convert stream back to collection");
    }
}
