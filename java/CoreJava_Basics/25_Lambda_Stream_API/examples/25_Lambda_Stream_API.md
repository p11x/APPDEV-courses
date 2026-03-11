# Java Lambda Expressions and Stream API

## Table of Contents
1. [Lambda Expressions](#lambda-expressions)
2. [Functional Interfaces](#functional-interfaces)
3. [Method References](#method-references)
4. [Stream API](#stream-api)
5. [Intermediate Operations](#intermediate-operations)
6. [Terminal Operations](#terminal-operations)
7. [Code Examples](#code-examples)
8. [Exercises](#exercises)
9. [Solutions](#solutions)

---

## 1. Lambda Expressions

### What are Lambda Expressions?

A **lambda expression** is a concise way to represent an anonymous function (a function without a name).

```
┌─────────────────────────────────────────────────────────────┐
│                    LAMBDA EXPRESSIONS                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Traditional:                 Lambda:                       │
│                                                              │
│   Runnable r = new Runnable() {                             │
│       public void run() {    ──►   () -> System.out.println │
│           System.out.println("Run");                         │
│       }                                                      │
│   };                                                         │
│                                                              │
│   Syntax:                                                    │
│   (parameters) -> expression                                 │
│   (parameters) -> { statements; }                            │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Lambda Syntax

```java
// No parameters
() -> System.out.println("Hello")

// One parameter
x -> x * 2

// Multiple parameters
(int a, int b) -> a + b

// With body
(x, y) -> {
    int sum = x + y;
    return sum;
}
```

---

## 2. Functional Interfaces

### What is a Functional Interface?

A functional interface has exactly one abstract method.

```
┌─────────────────────────────────────────────────────────────┐
│                  COMMON FUNCTIONAL INTERFACES                 │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Interface    │  Abstract Method  │  Description          │
│   ─────────────┼───────────────────┼────────────────────  │
│   Runnable     │  void run()      │  Execute without     │
│                │                   │  return value         │
│   Supplier<T>  │  T get()         │  Provide a value      │
│   Consumer<T>  │  void accept(T)  │  Consume a value      │
│   Function<T,R>│  R apply(T)      │  Transform input      │
│   Predicate<T> │  boolean test(T)│  Test a condition     │
│   BiFunction   │  R apply(T,U)   │  Two inputs, one out │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Examples

```java
// Supplier - provides values
Supplier<String> supplier = () -> "Hello";

// Consumer - consumes values
Consumer<String> consumer = s -> System.out.println(s);

// Function - transforms values
Function<String, Integer> length = s -> s.length();

// Predicate - tests conditions
Predicate<Integer> isEven = n -> n % 2 == 0;
```

---

## 3. Method References

### What are Method References?

Method references are a shorthand notation for lambda expressions.

```java
// Method reference types:

// Static method: ClassName::staticMethod
Function<String, Integer> parser = Integer::parseInt;

// Instance method of object: object::instanceMethod
String str = "Hello";
Supplier<Integer> len = str::length;

// Instance method of type: ClassName::instanceMethod
Function<String, String> upper = String::toUpperCase;

// Constructor: ClassName::new
Supplier<ArrayList<String>> listSupplier = ArrayList::new;
```

---

## 4. Stream API

### What is Stream API?

The Stream API provides a functional approach to processing collections.

```
┌─────────────────────────────────────────────────────────────┐
│                       STREAM PIPELINE                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Source ──► Intermediate Ops ──► Terminal Op              │
│   (Collection)  (filter, map,       (collect,               │
│                 sorted, distinct)   forEach, reduce)         │
│                                                              │
│   Example:                                                   │
│   list.stream()                                             │
│       .filter(x -> x > 5)     // Intermediate               │
│       .map(x -> x * 2)        // Intermediate               │
│       .collect(Collectors.toList())  // Terminal             │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 5. Intermediate Operations

### Common Intermediate Operations

| Operation | Description | Example |
|-----------|-------------|---------|
| `filter(Predicate)` | Filter elements | `.filter(x -> x > 0)` |
| `map(Function)` | Transform elements | `.map(x -> x * 2)` |
| `flatMap(Function)` | Flatten streams | `.flatMap(list::stream)` |
| `distinct()` | Remove duplicates | `.distinct()` |
| `sorted()` | Sort elements | `.sorted()` |
| `limit(n)` | Limit elements | `.limit(10)` |
| `skip(n)` | Skip elements | `.skip(5)` |

---

## 6. Terminal Operations

### Common Terminal Operations

| Operation | Description | Returns |
|-----------|-------------|---------|
| `forEach(Consumer)` | Apply action | void |
| `collect(Collector)` | Gather results | Collection |
| `reduce(BinaryOperator)` | Combine elements | Optional |
| `count()` | Count elements | long |
| `anyMatch(Predicate)` | Any match? | boolean |
| `allMatch(Predicate)` | All match? | boolean |
| `noneMatch(Predicate)` | None match? | boolean |
| `findFirst()` | First element | Optional |
| `min(Comparator)` | Minimum | Optional |
| `max(Comparator)` | Maximum | Optional |

---

## 7. Code Examples

### Example 1: Lambda Basics

```java
import java.util.*;
import java.util.function.*;

/**
 * LambdaBasicsDemo - Lambda expression fundamentals
 */
public class LambdaBasicsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== LAMBDA BASICS DEMO ===\n");
        
        // Runnable
        Runnable r = () -> System.out.println("Hello from lambda!");
        r.run();
        
        // Consumer
        Consumer<String> printer = s -> System.out.println("Printing: " + s);
        printer.accept("Hello");
        
        // Supplier
        Supplier<Double> random = () -> Math.random();
        System.out.println("Random: " + random.get());
        
        // Function
        Function<String, Integer> stringLength = s -> s.length();
        System.out.println("Length of 'Hello': " + stringLength.apply("Hello"));
        
        // Predicate
        Predicate<Integer> isPositive = n -> n > 0;
        System.out.println("Is 5 positive? " + isPositive.test(5));
        System.out.println("Is -3 positive? " + isPositive.test(-3));
        
        // BinaryOperator
        BinaryOperator<Integer> add = (a, b) -> a + b;
        System.out.println("5 + 3 = " + add.apply(5, 3));
    }
}
```

---

### Example 2: Stream Operations

```java
import java.util.*;
import java.util.stream.*;

/**
 * StreamOperationsDemo - Comprehensive stream operations
 */
public class StreamOperationsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STREAM OPERATIONS DEMO ===\n");
        
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        
        // Filter - keep even numbers
        List<Integer> evens = numbers.stream()
            .filter(n -> n % 2 == 0)
            .collect(Collectors.toList());
        System.out.println("Even numbers: " + evens);
        
        // Map - double each number
        List<Integer> doubled = numbers.stream()
            .map(n -> n * 2)
            .collect(Collectors.toList());
        System.out.println("Doubled: " + doubled);
        
        // Filter + Map - square of odd numbers
        List<Integer> oddSquares = numbers.stream()
            .filter(n -> n % 2 != 0)
            .map(n -> n * n)
            .collect(Collectors.toList());
        System.out.println("Odd squares: " + oddSquares);
        
        // Distinct
        List<Integer> withDuplicates = Arrays.asList(1, 2, 2, 3, 3, 3, 4);
        List<Integer> distinct = withDuplicates.stream()
            .distinct()
            .collect(Collectors.toList());
        System.out.println("Distinct: " + distinct);
        
        // Sorted
        List<Integer> unsorted = Arrays.asList(5, 2, 8, 1, 9, 3);
        List<Integer> sorted = unsorted.stream()
            .sorted()
            .collect(Collectors.toList());
        System.out.println("Sorted: " + sorted);
        
        // Limit and Skip
        System.out.println("First 3: " + numbers.stream().limit(3).collect(Collectors.toList()));
        System.out.println("Skip first 5: " + numbers.stream().skip(5).collect(Collectors.toList()));
        
        // Terminal operations
        long count = numbers.stream().count();
        System.out.println("\nCount: " + count);
        
        int sum = numbers.stream().mapToInt(Integer::intValue).sum();
        System.out.println("Sum: " + sum);
        
        OptionalDouble avg = numbers.stream().mapToInt(Integer::intValue).average();
        System.out.println("Average: " + avg.orElse(0));
        
        Optional<Integer> min = numbers.stream().min(Integer::compareTo);
        System.out.println("Min: " + min.orElse(0));
        
        Optional<Integer> max = numbers.stream().max(Integer::compareTo);
        System.out.println("Max: " + max.orElse(0));
        
        // Find operations
        System.out.println("\nFirst even: " + numbers.stream().filter(n -> n % 2 == 0).findFirst().orElse(0));
        System.out.println("Any > 5: " + numbers.stream().anyMatch(n -> n > 5));
        System.out.println("All > 0: " + numbers.stream().allMatch(n -> n > 0));
        System.out.println("None > 100: " + numbers.stream().noneMatch(n -> n > 100));
    }
}
```

---

### Example 3: Collectors

```java
import java.util.*;
import java.util.stream.*;

/**
 * CollectorsDemo - Various collector usages
 */
public class CollectorsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== COLLECTORS DEMO ===\n");
        
        List<String> names = Arrays.asList("Alice", "Bob", "Charlie", "David", "Eve");
        
        // toList
        List<String> list = names.stream().collect(Collectors.toList());
        System.out.println("To List: " + list);
        
        // toSet
        Set<String> set = names.stream().collect(Collectors.toSet());
        System.out.println("To Set: " + set);
        
        // toMap
        Map<String, Integer> nameLengths = names.stream()
            .collect(Collectors.toMap(name -> name, name -> name.length()));
        System.out.println("To Map: " + nameLengths);
        
        // joining
        String joined = names.stream().collect(Collectors.joining(", "));
        System.out.println("Joined: " + joined);
        
        // counting
        Long count = names.stream().collect(Collectors.counting());
        System.out.println("Count: " + count);
        
        // summing
        List<Integer> nums = Arrays.asList(1, 2, 3, 4, 5);
        Integer sum = nums.stream().collect(Collectors.summingInt(n -> n));
        System.out.println("Sum: " + sum);
        
        // averaging
        Double avg = nums.stream().collect(Collectors.averagingDouble(n -> n));
        System.out.println("Average: " + avg);
        
        // groupingBy
        Map<String, List<String>> byFirstLetter = names.stream()
            .collect(Collectors.groupingBy(name -> name.substring(0, 1)));
        System.out.println("Grouped: " + byFirstLetter);
        
        // partitioningBy
        Map<Boolean, List<Integer>> partitioned = nums.stream()
            .collect(Collectors.partitioningBy(n -> n % 2 == 0));
        System.out.println("Partitioned: " + partitioned);
    }
}
```

---

### Example 4: Method References

```java
import java.util.*;

/**
 * MethodReferenceDemo - Method reference examples
 */
public class MethodReferenceDemo {
    
    public static void main(String[] args) {
        System.out.println("=== METHOD REFERENCE DEMO ===\n");
        
        List<String> names = Arrays.asList("alice", "bob", "charlie");
        
        // Static method reference
        System.out.println("Static method:");
        names.stream()
            .map(String::toUpperCase)
            .forEach(System.out::println);
        
        // Instance method of object
        System.out.println("\nInstance method of object:");
        String prefix = "Mr. ";
        names.stream()
            .map(prefix::concat)
            .forEach(System.out::println);
        
        // Instance method of type
        System.out.println("\nInstance method of type:");
        names.stream()
            .map(String::toUpperCase)
            .map(String::toLowerCase)
            .forEach(System.out::println);
        
        // Constructor reference
        System.out.println("\nConstructor reference:");
        List<String> strings = Arrays.asList("1", "2", "3");
        List<Integer> integers = strings.stream()
            .map(Integer::new)
            .collect(java.util.stream.Collectors.toList());
        System.out.println("Converted: " + integers);
    }
}
```

---

### Example 5: Real-World Example

```java
import java.util.*;
import java.util.stream.*;

/**
 * Employee class for real-world example
 */
class Employee {
    private String name;
    private String department;
    private double salary;
    
    public Employee(String name, String department, double salary) {
        this.name = name;
        this.department = department;
        this.salary = salary;
    }
    
    public String getName() { return name; }
    public String getDepartment() { return department; }
    public double getSalary() { return salary; }
    
    @Override
    public String toString() {
        return name + " (" + department + ", $" + salary + ")";
    }
}

/**
 * RealWorldStreamDemo - Practical stream usage
 */
public class RealWorldStreamDemo {
    
    public static void main(String[] args) {
        List<Employee> employees = Arrays.asList(
            new Employee("Alice", "IT", 75000),
            new Employee("Bob", "HR", 55000),
            new Employee("Charlie", "IT", 80000),
            new Employee("Diana", "Marketing", 60000),
            new Employee("Eve", "IT", 70000),
            new Employee("Frank", "HR", 58000)
        );
        
        System.out.println("=== REAL-WORLD STREAM DEMO ===\n");
        
        // Get all IT employees
        System.out.println("IT Department:");
        employees.stream()
            .filter(e -> e.getDepartment().equals("IT"))
            .forEach(System.out::println);
        
        // Average salary in IT
        double itAvg = employees.stream()
            .filter(e -> e.getDepartment().equals("IT"))
            .mapToDouble(Employee::getSalary)
            .average()
            .orElse(0);
        System.out.println("\nIT Average Salary: $" + itAvg);
        
        // Sum of all salaries
        double totalSalary = employees.stream()
            .mapToDouble(Employee::getSalary)
            .sum();
        System.out.println("Total Salary: $" + totalSalary);
        
        // Highest paid employee
        Employee highestPaid = employees.stream()
            .max(Comparator.comparingDouble(Employee::getSalary))
            .orElse(null);
        System.out.println("Highest Paid: " + highestPaid);
        
        // Group by department
        System.out.println("\nEmployees by Department:");
        employees.stream()
            .collect(Collectors.groupingBy(Employee::getDepartment))
            .forEach((dept, emps) -> {
                System.out.println(dept + ": " + emps);
            });
        
        // Top 2 highest paid
        System.out.println("\nTop 2 Highest Paid:");
        employees.stream()
            .sorted(Comparator.comparingDouble(Employee::getSalary).reversed())
            .limit(2)
            .forEach(System.out::println);
    }
}
```

---

## 8. Exercises

### Exercise 1: Filter and Transform

**Requirements:**
1. Filter a list to keep only positive numbers
2. Square each number
3. Sum the results

---

### Exercise 2: Grouping

**Requirements:**
1. Group words by first letter
2. Count words in each group

---

### Exercise 3: Find and Match

**Requirements:**
1. Find first word starting with 'a'
2. Check if any word has length > 5

---

## 9. Solutions

### Solution 1

```java
List<Integer> numbers = Arrays.asList(-3, -2, -1, 0, 1, 2, 3, 4, 5);

int result = numbers.stream()
    .filter(n -> n > 0)
    .map(n -> n * n)
    .mapToInt(Integer::intValue)
    .sum();

System.out.println("Sum of squares: " + result);
```

---

### Solution 2

```java
List<String> words = Arrays.asList("apple", "banana", "apricot", "blueberry", "cherry");

Map<Character, Long> grouped = words.stream()
    .collect(Collectors.groupingBy(
        w -> w.charAt(0),
        Collectors.counting()
    ));

System.out.println(grouped);
```

---

### Solution 3

```java
List<String> words = Arrays.asList("hello", "world", "apple", "beautiful");

String firstA = words.stream()
    .filter(w -> w.startsWith("a"))
    .findFirst()
    .orElse("Not found");

boolean hasLong = words.stream()
    .anyMatch(w -> w.length() > 5);

System.out.println("First 'a' word: " + firstA);
System.out.println("Has length > 5: " + hasLong);
```

---

## Summary

### Key Takeaways

1. **Lambda expressions** - Concise anonymous functions
2. **Functional interfaces** - Single abstract method interfaces
3. **Method references** - Shorthand for lambdas
4. **Stream API** - Functional collection processing
5. **Intermediate operations** - Transform streams (filter, map, sorted)
6. **Terminal operations** - Produce results (collect, forEach, reduce)

---

*Happy Coding! 🚀*
