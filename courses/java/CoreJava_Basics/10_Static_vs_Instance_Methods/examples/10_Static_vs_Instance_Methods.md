# Static vs Instance Methods in Java

## Short Definition

Static methods belong to the class itself rather than to any instance (object). Instance methods require an object to be called. Understanding the difference is crucial for writing effective Java code.

---

## Key Bullet Points

- **Static Methods**: Called using `ClassName.method()`, belong to the class
- **Instance Methods**: Called using `object.method()`, belong to objects
- **Static Variables**: Shared across all instances of a class
- **Instance Variables**: Each object has its own copy
- **Static Context**: Cannot use `this` or `super` keywords
- **Memory**: Static members are loaded when class is loaded

---

## Static vs Instance Comparison

| Feature | Static | Instance |
|---------|--------|----------|
| Called by | ClassName.method() | object.method() |
| Keyword | static | (none) |
| Access to instance | Cannot access | Can access |
| Access to static | Can access | Can access |
| Memory | One copy per class | One copy per object |
| Loaded | At class loading | At object creation |

---

## Static Method Example

```java
class Calculator {
    
    // Static method - belongs to class
    public static int add(int a, int b) {
        return a + b;
    }
    
    public static void main(String[] args) {
        // No object needed to call static method
        int result = Calculator.add(5, 3);
        System.out.println("Sum: " + result);  // Output: 8
    }
}
```

---

## Instance Method Example

```java
class Person {
    private String name;
    
    // Constructor
    public Person(String name) {
        this.name = name;
    }
    
    // Instance method - requires object
    public void introduce() {
        System.out.println("Hello, I am " + name);
    }
    
    public static void main(String[] args) {
        // Need to create object to call instance method
        Person p = new Person("Alice");
        p.introduce();  // Output: Hello, I am Alice
    }
}
```

---

## Static Variable Example

```java
class Counter {
    // Static variable - shared across all objects
    private static int count = 0;
    
    public Counter() {
        count++;  // Increment for each new object
    }
    
    public static void main(String[] args) {
        new Counter();
        new Counter();
        new Counter();
        
        System.out.println("Count: " + Counter.count);  // Output: 3
    }
}
```

---

## Common Static Methods in Java

### Math Class (java.lang.Math)
```java
int max = Math.max(10, 20);      // Returns larger value
int abs = Math.abs(-25);          // Returns absolute value
double sqrt = Math.sqrt(16);     // Returns square root
int random = (int)(Math.random() * 100);  // Random number
```

### System Class
```.out.println();` is a static method
```java
System.out.println("Hello");  // static method call
System.exit(0);               // terminates JVM
```

---

## Best Practices

1. **Use static for utilities**: Helper methods that don't need object state
2. **Use static for constants**: Values that don't change
3. **Don't overuse static**: Can make code harder to test
4. **Instance methods**: When behavior depends on object state

---

## Why This Matters for Angular Developers?

- Spring Boot uses static methods in utility classes
- Static factory methods are common in design patterns
- Understanding this helps read Java backend code
- Main method is static - entry point for Spring Boot apps

---

## Exercises

### Exercise 1: Static Calculator
Create a class with static methods for subtract, multiply, and divide.

### Exercise 2: Object Counter
Create a class that counts how many objects have been created.

### Exercise 3: Hybrid Class
Create a class with both static and instance methods demonstrating when each is appropriate.

---

## Summary

- Static methods belong to the class, instance methods to objects
- Static members are shared across all instances
- Cannot access instance members from static context
- Essential for utility methods and main entry points
