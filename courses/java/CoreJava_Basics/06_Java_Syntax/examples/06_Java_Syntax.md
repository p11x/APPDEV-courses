# Java Syntax

## Short Definition

Java syntax refers to the set of rules that define how a Java program is written and interpreted. Understanding syntax is essential for writing correct Java code.

---

## Key Bullet Points

- **Case-sensitive**: Java treats uppercase and lowercase as different
- **Class names**: Must match the filename (e.g., `MyClass.java` contains `MyClass`)
- **Statements**: End with a semicolon (`;`)
- **Blocks**: Code inside braces `{ }` forms a block
- **Comments**: For documentation (ignored by compiler)

---

## Structure of a Java Program

```java
// This is a comment - ignored by the compiler

// Class declaration
public class MyFirstProgram {
    
    // Main method - entry point of the program
    public static void main(String[] args) {
        // This is a statement - must end with semicolon
        System.out.println("Hello, World!");
        private display();
    }
}
```public void pavan{
    //dsgdzsfg
}

---

## Key Elements Explained

### 1. Class

A class is a blueprint for creating objects. Every Java program must have at least one class.

```java
public class ClassName {
    // class body
}
```

**Rules:**
- Class name should start with a capital letter
- The filename must match the class name with `.java` extension

---

### 2. main() Method

The `main()` method is the entry point of any Java application. The JVM starts execution from here.

```java
public static void main(String[] args) {
    // program statements
   
}
```

**Breakdown:**
- `public`: Accessible from anywhere
- `static`: Can be called without creating an object
- `void`: Returns nothing
- `String[] args`: Command-line arguments

---

### 3. Statements

A statement is a complete instruction that performs an action. It must end with a semicolon (`;`).

```java
int number = 10;           // Variable declaration
System.out.println(num);  // Print statement
```

---

### 4. Comments

Comments explain the code and are ignored by the compiler.

**Single-line comment:**
```java
// This is a single-line comment
```

**Multi-line comment:**
```java
/* This is a
   multi-line comment */
```

**Documentation comment:**
```java
/**
 * This is a documentation comment
 */
```

---

## Naming Rules

| Rule | Example | Valid? |
|------|---------|--------|
| Start with letter, `_`, or `$` | `myVar`, `_value`, `$money` | ✓ |
| Cannot start with digit | `2name` | ✗ |
| No spaces allowed | `my var` | ✗ |
| Case-sensitive | `myVar`, `myvar`, `MYVAR` | Different! |
| Cannot use reserved words | `class`, `int` | ✗ |

---

## Simple Example Program

```java
// MyFirstProgram.java
// This is my first Java program

public class MyFirstProgram {
    // Main method - program starts here
    public static void main(String[] args) {
        // Print a welcome message
        System.out.println("Welcome to Java!");
        
        // Another statement
        System.out.println("Learning Java is fun!");
    }
}
```

---

## Expected Output

```
Welcome to Java!
Learning Java is fun!
```

