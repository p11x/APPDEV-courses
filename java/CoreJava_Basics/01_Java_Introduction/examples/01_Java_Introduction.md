# Java Introduction

## Short Definition

Java is a high-level, object-oriented programming language developed by Sun Microsystems (now Oracle). It was designed to be platform-independent, meaning Java programs can run on any device that has a Java Virtual Machine (JVM). Java is known for its simplicity, robustness, and security features.

---

## Key Bullet Points

- **Object-Oriented**: Java follows object-oriented programming (OOP) principles, organizing code into classes and objects.
- **Platform Independent**: Java achieves platform independence through the "Write Once, Run Anywhere" (WORA) principle.
- **Simple and Easy to Learn**: Java syntax is clean and easy to understand for beginners.
- **Robust**: Java has strong memory management and exception handling.
- **Secure**: Java provides security features to protect against viruses and unauthorized access.
- **Multithreaded**: Java supports multithreading, allowing concurrent execution of tasks.
- **Distributed**: Java is designed for distributed computing and network-based applications.

---

## Why Java is Platform Independent

Java achieves platform independence through the following mechanism:

1. **Source Code to Bytecode**: When you compile a Java program, the compiler translates your source code (`.java` file) into bytecode (`.class` file). Bytecode is a platform-neutral intermediate code.

2. **Java Virtual Machine (JVM)**: The JVM is a virtual machine that runs on different operating systems (Windows, macOS, Linux). It interprets the bytecode and executes it.

3. **Key Point**: The same bytecode can run on any platform that has a JVM installed. You don't need to recompile the code for different operating systems.

```
Source Code (.java)
        ↓
    Compiler
        ↓
  Bytecode (.class)
        ↓
   JVM (Windows)
        ↓
   JVM (macOS)
        ↓
   JVM (Linux)
```

---

## Simple Example Program

```java
// HelloWorld.java
public class HelloWorld {
    public static void main(String[] args) {
        System.out.println("Hello, World!");
    }
}
```

---

## Expected Output

```
Hello, World!
```
