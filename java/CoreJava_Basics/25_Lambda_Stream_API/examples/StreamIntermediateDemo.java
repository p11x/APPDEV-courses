// StreamIntermediateDemo - Demonstrates Stream API Intermediate Operations
// Essential for transforming and filtering streams

import java.util.*;
import java.util.stream.*;

public class StreamIntermediateDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STREAM INTERMEDIATE OPERATIONS DEMO ===\n");
        
        // distinct - remove duplicates
        List<Integer> withDupes = Arrays.asList(1, 2, 2, 3, 3, 3, 4);
        List<Integer> distinct = withDupes.stream().distinct().collect(Collectors.toList());
        System.out.println("Distinct: " + distinct);
        
        // sorted
        List<String> unsorted = Arrays.asList("Banana", "Apple", "Cherry");
        List<String> sorted = unsorted.stream().sorted().collect(Collectors.toList());
        System.out.println("Sorted: " + sorted);
        
        // limit and skip
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        List<Integer> limited = numbers.stream().limit(5).collect(Collectors.toList());
        List<Integer> skipped = numbers.stream().skip(5).collect(Collectors.toList());
        System.out.println("Limit 5: " + limited);
        System.out.println("Skip 5: " + skipped);
        
        // flatMap - flatten nested collections
        List<List<Integer>> nested = Arrays.asList(
            Arrays.asList(1, 2),
            Arrays.asList(3, 4),
            Arrays.asList(5, 6)
        );
        List<Integer> flat = nested.stream()
            .flatMap(list -> list.stream())
            .collect(Collectors.toList());
        System.out.println("FlatMap: " + flat);
        
        System.out.println("\n=== KEY INTERMEDIATE OPERATIONS ===");
        System.out.println("distinct() - Remove duplicate elements");
        System.out.println("sorted() - Sort elements");
        System.out.println("limit(n) - Take first n elements");
        System.out.println("skip(n) - Skip first n elements");
        System.out.println("flatMap() - Flatten nested collections");
    }
}
