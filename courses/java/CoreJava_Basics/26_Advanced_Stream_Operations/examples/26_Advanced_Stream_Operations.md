# Java Advanced Stream Operations

## Table of Contents
1. [Parallel Streams](#parallel-streams)
2. [Stream Reduction](#stream-reduction)
3. [Collecting Results](#collecting-results)
4. [FlatMap Operations](#flatmap-operations)
5. [Stream Pipeline Internals](#stream-pipeline-internals)

---

## 1. Parallel Streams

### Using Parallel Streams

```java
// Sequential stream
list.stream()
    .map(x -> x * 2)
    .collect(Collectors.toList());

// Parallel stream - uses multiple cores
list.parallelStream()
    .map(x -> x * 2)
    .collect(Collectors.toList());
```

### When to Use Parallel Streams

- Large datasets
- Independent operations
- Non-blocking operations

---

## 2. Stream Reduction

### reduce() Method

```java
// Sum using reduce
Integer sum = numbers.stream()
    .reduce(0, (a, b) -> a + b);

// Using method reference
Integer sum2 = numbers.stream()
    .reduce(0, Integer::sum);

// Max/Min with reduce
numbers.stream()
    .reduce(Integer::max)
    .orElse(0);
```

---

## 3. Collecting Results

### Advanced Collectors

```java
// Grouping by
Map<String, List<Person>> byCity = people.stream()
    .collect(Collectors.groupingBy(Person::getCity));

// Partitioning
Map<Boolean, List<Integer>> partitioned = numbers.stream()
    .collect(Collectors.partitioningBy(n -> n % 2 == 0));

// Summarizing
IntSummaryStatistics stats = numbers.stream()
    .collect(Collectors.summarizingInt(Integer::intValue));
```

---

## 4. FlatMap Operations

### Using FlatMap

```java
// Flatten lists
List<List<Integer>> listOfLists = Arrays.asList(
    Arrays.asList(1, 2),
    Arrays.asList(3, 4)
);

List<Integer> flatList = listOfLists.stream()
    .flatMap(list -> list.stream())
    .collect(Collectors.toList());
```

---

## 5. Code Examples

### AdvancedStreamDemo

```java
import java.util.*;
import java.util.stream.*;

public class AdvancedStreamDemo {
    public static void main(String[] args) {
        System.out.println("=== ADVANCED STREAM DEMO ===\n");
        
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        
        // Reduce
        System.out.println("--- Reduce Operations ---");
        int sum = numbers.stream().reduce(0, Integer::sum);
        System.out.println("Sum: " + sum);
        
        int product = numbers.stream().reduce(1, (a, b) -> a * b);
        System.out.println("Product: " + product);
        
        // Collect - groupingBy
        System.out.println("\n--- Grouping ---");
        List<String> words = Arrays.asList("apple", "banana", "apricot", "blue", "cherry");
        Map<Character, List<String>> grouped = words.stream()
            .collect(Collectors.groupingBy(w -> w.charAt(0)));
        System.out.println("Grouped by first letter: " + grouped);
        
        // Partitioning
        System.out.println("\n--- Partitioning ---");
        Map<Boolean, List<Integer>> evenOdd = numbers.stream()
            .collect(Collectors.partitioningBy(n -> n % 2 == 0));
        System.out.println("Even: " + evenOdd.get(true));
        System.out.println("Odd: " + evenOdd.get(false));
        
        // FlatMap
        System.out.println("\n--- FlatMap ---");
        List<List<Integer>> nested = Arrays.asList(
            Arrays.asList(1, 2, 3),
            Arrays.asList(4, 5, 6),
            Arrays.asList(7, 8, 9)
        );
        
        List<Integer> flat = nested.stream()
            .flatMap(List::stream)
            .collect(Collectors.toList());
        System.out.println("Flattened: " + flat);
        
        // Statistics
        System.out.println("\n--- Statistics ---");
        IntSummaryStatistics stats = numbers.stream()
            .collect(Collectors.summarizingInt(Integer::intValue));
        System.out.println("Count: " + stats.getCount());
        System.out.println("Sum: " + stats.getSum());
        System.out.println("Min: " + stats.getMin());
        System.out.println("Max: " + stats.getMax());
        System.out.println("Average: " + stats.getAverage());
    }
}
```

---

## Summary

### Key Takeaways

1. **Parallel streams** - Multi-core processing
2. **reduce()** - Combine elements into single value
3. **Collectors** - Grouping, partitioning, statistics
4. **flatMap()** - Flatten nested structures

---

*Advanced Streams Complete!*
