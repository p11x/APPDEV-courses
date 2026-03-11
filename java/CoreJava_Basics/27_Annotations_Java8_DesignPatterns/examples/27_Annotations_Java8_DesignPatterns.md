# Java Annotations, Java 8+ Features, and Design Patterns

## Table of Contents
1. [Annotations](#annotations)
2. [Java 8+ Features](#java-8-features)
3. [Design Patterns](#design-patterns)
4. [Code Examples](#code-examples)
5. [Solutions](#solutions)

---

## 1. Annotations

### What are Annotations?

Annotations provide metadata about code and are used by the compiler and runtime.

```
┌─────────────────────────────────────────────────────────────┐
│                     ANNOTATIONS                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Built-in Annotations:                                     │
│   @Override     - Override parent method                    │
│   @Deprecated  - Mark as deprecated                        │
│   @SuppressWarnings - Suppress compiler warnings           │
│   @FunctionalInterface - Mark as functional interface     │
│                                                              │
│   Custom Annotations:                                       │
│   @interface MyAnnotation {                                │
│       String value() default "";                           │
│   }                                                         │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Built-in Annotations

```java
@Override  // Override parent method
public void display() { }

@Deprecated  // Mark as deprecated
public void oldMethod() { }

@SuppressWarnings("unchecked")  // Suppress warnings
public void legacyCode() { }

@FunctionalInterface  // Single abstract method
interface MyFunction {
    void apply();
}
```

### Custom Annotations

```java
// Define annotation
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
@interface MyAnnotation {
    String value() default "";
    int priority() default 1;
}

// Use annotation
@MyAnnotation(value = "Test", priority = 5)
public void testMethod() { }
```

---

## 2. Java 8+ Features

### New Features Overview

| Feature | Description |
|---------|-------------|
| Lambda Expressions | Anonymous functions |
| Stream API | Functional collection processing |
| Optional | Null-safe container |
| Date/Time API | Improved date handling |
| Default Methods | Interfaces with implementations |
| Static Methods in Interfaces | Utility methods in interfaces |
| Method References | Short lambda syntax |
| CompletableFuture | Async programming |

### Date/Time API

```java
import java.time.*;

// Current date/time
LocalDate today = LocalDate.now();
LocalTime now = LocalTime.now();
LocalDateTime now2 = LocalDateTime.now();

// Create specific date
LocalDate date = LocalDate.of(2024, 1, 15);

// Operations
LocalDate tomorrow = today.plusDays(1);
LocalDate lastMonth = today.minusMonths(1);

// Formatting
DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd");
String formatted = today.format(formatter);
```

### Optional

```java
import java.util.Optional;

// Create Optional
Optional<String> optional = Optional.of("Hello");
Optional<String> empty = Optional.empty();
Optional<String> nullable = Optional.ofNullable(null);

// Use Optional
String value = optional.orElse("Default");
String value2 = optional.map(String::toUpperCase).orElseGet(() -> "N/A");
```

### Default Methods

```java
interface Drawable {
    void draw();
    
    // Default method
    default void display() {
        System.out.println("Displaying...");
    }
    
    // Static method
    static void info() {
        System.out.println("Drawable interface");
    }
}
```

---

## 3. Design Patterns

### Common Design Patterns

| Pattern | Category | Description |
|---------|----------|-------------|
| Singleton | Creational | One instance only |
| Factory | Creational | Object creation via factory |
| Builder | Creational | Step-by-step object creation |
| Observer | Behavioral | Event notification |
| Repository | Architectural | Data access abstraction |
| DAO | Architectural | Data Access Object |

### Singleton Pattern

```java
class Singleton {
    private static Singleton instance;
    
    private Singleton() { }  // Private constructor
    
    public static synchronized Singleton getInstance() {
        if (instance == null) {
            instance = new Singleton();
        }
        return instance;
    }
}
```

### Factory Pattern

```java
interface Shape {
    void draw();
}

class Circle implements Shape {
    public void draw() { System.out.println("Circle"); }
}

class Rectangle implements Shape {
    public void draw() { System.out.println("Rectangle"); }
}

class ShapeFactory {
    public Shape getShape(String type) {
        if (type.equals("CIRCLE")) return new Circle();
        if (type.equals("RECTANGLE")) return new Rectangle();
        return null;
    }
}
```

### Observer Pattern

```java
interface Observer {
    void update(String message);
}

class Subscriber implements Observer {
    public void update(String message) {
        System.out.println("Received: " + message);
    }
}

class Publisher {
    private List<Observer> observers = new ArrayList<>();
    
    public void subscribe(Observer o) { observers.add(o); }
    
    public void notifyAll(String message) {
        for (Observer o : observers) {
            o.update(message);
        }
    }
}
```

---

## 4. Code Examples

### Example 1: Custom Annotation

```java
import java.lang.annotation.*;
import java.lang.reflect.*;

// Define annotation
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
@interface Test {
    String description() default "No description";
    int priority() default 1;
}

// Class using annotation
class Calculator {
    @Test(description = "Addition test", priority = 1)
    public void testAdd() {
        System.out.println("Testing addition");
    }
    
    @Test(description = "Division test", priority = 2)
    public void testDivide() {
        System.out.println("Testing division");
    }
}

// Process annotations
public class AnnotationDemo {
    public static void main(String[] args) throws Exception {
        for (Method m : Calculator.class.getDeclaredMethods()) {
            if (m.isAnnotationPresent(Test.class)) {
                Test test = m.getAnnotation(Test.class);
                System.out.println("Method: " + m.getName());
                System.out.println("Description: " + test.description());
                System.out.println("Priority: " + test.priority());
            }
        }
    }
}
```

---

### Example 2: Date/Time API

```java
import java.time.*;
import java.time.format.*;

/**
 * DateTimeAPIDemo - Modern date/time handling
 */
public class DateTimeAPIDemo {
    public static void main(String[] args) {
        System.out.println("=== DATE/TIME API DEMO ===\n");
        
        // Current date/time
        System.out.println("Current: " + LocalDate.now());
        System.out.println("Time: " + LocalTime.now());
        System.out.println("DateTime: " + LocalDateTime.now());
        
        // Specific date
        LocalDate date = LocalDate.of(2024, 1, 15);
        System.out.println("\nSpecific date: " + date);
        
        // Operations
        System.out.println("Plus 10 days: " + date.plusDays(10));
        System.out.println("Minus 1 month: " + date.minusMonths(1));
        
        // Formatting
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("MMMM dd, yyyy");
        System.out.println("Formatted: " + date.format(formatter));
        
        // Duration
        LocalTime start = LocalTime.of(9, 0);
        LocalTime end = LocalTime.of(17, 30);
        Duration duration = Duration.between(start, end);
        System.out.println("\nWork hours: " + duration.toHours() + " hours");
        
        // Period
        LocalDate birthday = LocalDate.of(2000, 5, 15);
        Period age = Period.between(birthday, LocalDate.now());
        System.out.println("Age: " + age.getYears() + " years");
    }
}
```

---

### Example 3: Design Patterns

```java
/**
 * Singleton - Single instance
 */
class DatabaseConnection {
    private static DatabaseConnection instance;
    
    private DatabaseConnection() {
        System.out.println("Database connected");
    }
    
    public static DatabaseConnection getInstance() {
        if (instance == null) {
            instance = new DatabaseConnection();
        }
        return instance;
    }
}

/**
 * Factory - Object creation
 */
interface Payment {
    void pay(double amount);
}

class CreditCardPayment implements Payment {
    public void pay(double amount) {
        System.out.println("Paid $" + amount + " via Credit Card");
    }
}

class PayPalPayment implements Payment {
    public void pay(double amount) {
        System.out.println("Paid $" + amount + " via PayPal");
    }
}

class PaymentFactory {
    public static Payment getPayment(String type) {
        if (type.equalsIgnoreCase("CREDIT")) return new CreditCardPayment();
        if (type.equalsIgnoreCase("PAYPAL")) return new PayPalPayment();
        return null;
    }
}

/**
 * Builder - Step-by-step construction
 */
class User {
    private String name;
    private String email;
    private int age;
    
    private User(Builder builder) {
        this.name = builder.name;
        this.email = builder.email;
        this.age = builder.age;
    }
    
    static class Builder {
        private String name;
        private String email;
        private int age;
        
        public Builder name(String name) { this.name = name; return this; }
        public Builder email(String email) { this.email = email; return this; }
        public Builder age(int age) { this.age = age; return this; }
        public User build() { return new User(this); }
    }
}

/**
 * DesignPatternsDemo
 */
public class DesignPatternsDemo {
    public static void main(String[] args) {
        System.out.println("=== DESIGN PATTERNS DEMO ===\n");
        
        // Singleton
        DatabaseConnection db1 = DatabaseConnection.getInstance();
        DatabaseConnection db2 = DatabaseConnection.getInstance();
        System.out.println("Same instance? " + (db1 == db2));
        
        // Factory
        Payment payment = PaymentFactory.getPayment("CREDIT");
        payment.pay(100);
        
        // Builder
        User user = new User.Builder()
            .name("John")
            .email("john@example.com")
            .age(25)
            .build();
        System.out.println("\nUser created: " + user);
    }
}
```

---

## 5. Exercises

### Exercise 1: Create Custom Annotation

**Requirements:**
1. Create @Author annotation with name and date
2. Apply to a class
3. Read annotation using reflection

---

### Exercise 2: Implement Observer Pattern

**Requirements:**
1. Create Subject class with observers
2. Notify all observers when state changes

---

## 6. Solutions

### Solution 1: Custom Annotation

```java
import java.lang.annotation.*;
import java.lang.reflect.*;

@Retention(RetentionPolicy.RUNTIME)
@interface Author {
    String name();
    String date();
}

@Author(name = "John", date = "2024-01-15")
class MyClass { }

public class AuthorDemo {
    public static void main(String[] args) {
        if (MyClass.class.isAnnotationPresent(Author.class)) {
            Author a = MyClass.class.getAnnotation(Author.class);
            System.out.println("Author: " + a.name() + ", Date: " + a.date());
        }
    }
}
```

---

### Solution 2: Observer Pattern

```java
interface Observer {
    void update(String message);
}

class Subscriber implements Observer {
    private String name;
    Subscriber(String name) { this.name = name; }
    public void update(String message) {
        System.out.println(name + " received: " + message);
    }
}

class Publisher {
    private List<Observer> list = new java.util.ArrayList<>();
    
    public void subscribe(Observer o) { list.add(o); }
    public void notifyAllObservers(String msg) {
        for (Observer o : list) o.update(msg);
    }
}

public class ObserverDemo {
    public static void main(String[] args) {
        Publisher p = new Publisher();
        p.subscribe(new Subscriber("A"));
        p.subscribe(new Subscriber("B"));
        p.notifyAllObservers("Hello!");
    }
}
```

---

## Summary

### Key Takeaways

1. **Annotations** - Metadata for code (overrides, custom annotations)
2. **Java 8+ Features** - Lambda, Streams, Optional, Date/Time API
3. **Design Patterns** - Singleton, Factory, Builder, Observer

---

*Happy Coding! 🚀*
