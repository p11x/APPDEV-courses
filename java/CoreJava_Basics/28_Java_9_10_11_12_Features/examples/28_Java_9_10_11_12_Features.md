# Java 9-17 New Features

## Table of Contents
1. [Java 9 Features](#java-9-features)
2. [Java 10 Features](#java-10-features)
3. [Java 11 Features](#java-11-features)
4. [Java 14-17 Features](#java-14-17-features)

---

## 1. Java 9 Features

### New Methods

```java
// List.of() - Immutable lists
List<String> list = List.of("a", "b", "c");

// Set.of() - Immutable sets
Set<Integer> set = Set.of(1, 2, 3);

// Map.of() - Immutable maps
Map<String, Integer> map = Map.of("a", 1, "b", 2);

// Stream takeWhile, dropWhile
stream.takeWhile(x -> x < 5)
stream.dropWhile(x -> x < 5)

// Optional orElseThrow
Optional<String> opt = Optional.empty();
String value = opt.orElseThrow();
```

### Private Methods in Interfaces

```java
interface MyInterface {
    default void method() {
        helper();
    }
    
    private void helper() {
        // Private implementation
    }
}
```

---

## 2. Java 10 Features

### Local Variable Type Inference

```java
// var keyword (Java 10+)
var message = "Hello";
var numbers = new ArrayList<Integer>();
var map = new HashMap<String, Integer>();
```

---

## 3. Java 11 Features

### String Methods

```java
// New String methods (Java 11+)
"  Hello  ".isBlank();           // true
"Hello".lines();                  // Stream of lines
"Hello".repeat(3);                // "HelloHelloHello"
"  Hello  ".strip();              // "Hello" (Unicode aware)
"  Hello  ".stripLeading();       // "Hello  "
"  Hello  ".stripTrailing();      // "  Hello"
```

### Files Methods

```java
// New Files methods
String content = Files.readString(Path.of("file.txt"));
Files.writeString(Path.of("file.txt"), "content");
boolean isBlank = Files.isSameFile(Path.of("a"), Path.of("b"));
```

### Run Source File

```java
// Java 11 can run single-file programs directly
// java MyProgram.java
```

---

## 4. Java 14-17 Features

### Switch Expressions (Java 14+)

```java
// Traditional switch
int result = switch (day) {
    case 1, 2, 3, 4, 5 -> 1;  // Weekday
    case 6, 7 -> 0;            // Weekend
    default -> -1;
};

// With blocks
int result = switch (day) {
    case 1, 2, 3, 4, 5 -> {
        yield 1;
    }
    case 6, 7 -> {
        yield 0;
    }
    default -> -1;
};
```

### Records (Java 14+)

```java
// Record - immutable data carrier
public record Person(String name, int age) {
    // Auto-generates:
    // - Private final fields
    // - Constructor
    // - equals(), hashCode(), toString()
    // - Getters: name(), age()
}

// Usage
Person p = new Person("John", 30);
System.out.println(p.name());  // John
```

### Pattern Matching (Java 16+)

```java
// Pattern matching for instanceof
if (obj instanceof String s) {
    // s is automatically typed as String here
    System.out.println(s.length());
}
```

### Text Blocks (Java 15+)

```java
// Multi-line strings
String json = """
    {
        "name": "John",
        "age": 30
    }
    """;
```

---

## Code Examples

### Java9PlusFeatures

```java
public class Java9PlusFeatures {
    public static void main(String[] args) {
        System.out.println("=== JAVA 9+ FEATURES DEMO ===\n");
        
        // Immutable collections (Java 9+)
        var immutableList = List.of(1, 2, 3);
        var immutableMap = Map.of("a", 1, "b", 2);
        System.out.println("Immutable List: " + immutableList);
        
        // var keyword (Java 10+)
        var message = "Hello, Java!";
        var number = 42;
        System.out.println("var: " + message + " " + number);
        
        // New String methods (Java 11+)
        var blank = "   ".isBlank();
        var repeated = "Hi".repeat(3);
        var lines = "Hello\nWorld".lines().toList();
        System.out.println("\nisBlank: " + blank);
        System.out.println("repeat: " + repeated);
        System.out.println("lines: " + lines);
        
        // Switch expressions (Java 14+)
        var day = 3;
        var dayType = switch (day) {
            case 1, 2, 3, 4, 5 -> "Weekday";
            case 6, 7 -> "Weekend";
            default -> "Invalid";
        };
        System.out.println("\nSwitch expression: Day " + day + " is " + dayType);
    }
}
```

---

## Summary

### Quick Reference

| Version | Key Features |
|---------|--------------|
| Java 9 | Immutable collections, Private interface methods |
| Java 10 | Local variable type inference (var) |
| Java 11 | New String methods, Run source files |
| Java 14 | Switch expressions |
| Java 15 | Text blocks |
| Java 16 | Pattern matching for instanceof |
| Java 17 | Sealed classes, Records |

---

*Java 9-17 Features Complete!*
