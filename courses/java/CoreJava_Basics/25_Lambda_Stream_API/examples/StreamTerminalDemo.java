// StreamTerminalDemo - Demonstrates Stream API Terminal Operations
// Essential for processing collections and producing results

import java.util.*;
import java.util.stream.*;

public class StreamTerminalDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STREAM TERMINAL OPERATIONS DEMO ===\n");
        
        List<String> names = Arrays.asList("Alice", "Bob", "Charlie", "David", "Eve");
        
        // forEach
        System.out.print("forEach: ");
        names.stream().forEach(name -> System.out.print(name + " "));
        System.out.println();
        
        // collect
        List<String> filtered = names.stream()
            .filter(name -> name.length() > 3)
            .collect(Collectors.toList());
        System.out.println("Names > 3 chars: " + filtered);
        
        // count
        long count = names.stream()
            .filter(name -> name.startsWith("A"))
            .count();
        System.out.println("Names starting with A: " + count);
        
        // reduce - combine elements
        List<Integer> nums = Arrays.asList(1, 2, 3, 4, 5);
        int sum = nums.stream().reduce(0, (a, b) -> a + b);
        System.out.println("Sum: " + sum);
        
        // anyMatch, allMatch, noneMatch
        System.out.println("Any name starts with 'C': " + 
            names.stream().anyMatch(n -> n.startsWith("C")));
        System.out.println("All names > 2 chars: " + 
            names.stream().allMatch(n -> n.length() > 2));
        
        // findFirst, findAny
        Optional<String> first = names.stream().findFirst();
        System.out.println("First: " + first.orElse("None"));
        
        System.out.println("\n=== KEY TERMINAL OPERATIONS ===");
        System.out.println("forEach() - Execute action for each element");
        System.out.println("collect() - Gather elements into collection");
        System.out.println("reduce() - Combine elements into single value");
        System.out.println("anyMatch/allMatch/noneMatch - Boolean checks");
    }
}
