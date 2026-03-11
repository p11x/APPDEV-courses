# Basic Java Program - Hello World

## Short Definition

A Basic Java Program is the simplest Java application that demonstrates the fundamental structure of any Java program. The traditional first program displays "Hello, World!" to the console.

---

## Key Bullet Points

- **Entry Point**: The `main()` method is the entry point where the JVM starts execution
- **Class Declaration**: Every Java program requires at least one class
- **File Naming**: The class name must match the filename (e.g., `HelloWorld.java` for class `HelloWorld`)
- **System.out.println()**: Used to print output to the console
- **Compilation**: Java source code (.java) is compiled to bytecode (.class)
- **Execution**: The JVM interprets the bytecode

---

## Basic Program Structure

```java
class ClassName {
    
    public static void main(String[] args) {
        // Your code here
        System.out.println("Hello, World!");
    }
}
```

### Components Explained

| Component | Description |
|-----------|-------------|
| `class` | Keyword to declare a class |
| `ClassName` | Must match filename (PascalCase) |
| `public` | Access modifier - visible everywhere |
| `static` | Method belongs to class, not object |
| `void` | Return type - returns nothing |
| `main()` | Entry point of the program |
| `String[] args` | Command line arguments |

---

## How to Compile and Run

```bash
# Compile
javac HelloWorld.java

# Run
java HelloWorld
```

### What Happens Behind the Scenes

```
Source Code (HelloWorld.java)
        ↓
    javac Compiler
        ↓
  Bytecode (HelloWorld.class)
        ↓
    java command
        ↓
      JVM
        ↓
   Output: Hello, World!
```

---

## Example 1: Simple Output

```java
class HelloWorld {
    public static void main(String[] args) {
        System.out.println("Hello, World!");
    }
}
```

**Output:**
```
Hello, World!
```

---

## Example 2: Multiple Print Statements

```java
class MultiplePrints {
    public static void main(String[] args) {
        System.out.println("Line 1");
        System.out.println("Line 2");
        System.out.println("Line 3");
    }
}
```

**Output:**
```
Line 1
Line 2
Line 3
```

---

## Why This Matters for Angular Developers?

When building full-stack applications:
- Java backend returns JSON responses
- Console output helps debug Java applications locally
- Understanding basic Java structure helps when reading Spring Boot code
- The `main()` method is where Spring Boot applications start

---

## Exercises

### Exercise 1: Your First Program
Create a program that prints your name.

### Exercise 2: Multiple Lines
Print three lines: your name, your city, and your favorite programming language.

### Exercise 3: ASCII Art
Print a simple ASCII art pattern using `System.out.println()`.

---

## Summary

- Every Java program needs a `main()` method as the entry point
- `System.out.println()` prints text to the console
- Java is compiled to bytecode and executed by the JVM
- This is the foundation for all Java applications including Spring Boot

---

## Next Steps

- Learn about [Variables and Data Types](11_Variables_and_DataTypes.md)
- Learn about [Control Flow Statements](16_Control_Flow_Statements.md)
- Understand [Methods and Static vs Instance](10_Static_vs_Instance_Methods.md)
