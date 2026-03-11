# Java Advanced Topics: Generics, Multithreading, and File I/O

## Table of Contents
1. [Generics](#generics)
2. [Multithreading](#multithreading)
3. [File I/O Operations](#file-io-operations)
4. [Code Examples](#code-examples)
5. [Exercises](#exercises)
6. [Solutions](#solutions)

---

## Part 1: Generics

### What are Generics?

Generics enable types (classes and interfaces) to be parameters when defining classes, interfaces, and methods.

```
┌─────────────────────────────────────────────────────────────┐
│                        GENERICS                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Without Generics:           With Generics:                  │
│   List list = new ArrayList(); List<String> list = new     │
│   list.add("Hello");         ArrayList<>();                 │
│   String s = (String)        list.add("Hello");             │
│     list.get(0);             String s = list.get(0);        │
│                                                              │
│   Benefits:                                                 │
│   ✓ Type safety at compile time                            │
│   ✓ No type casting needed                                 │
│   ✓ Reusable code                                          │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Generic Classes

```java
// Generic class
class Box<T> {
    private T content;
    
    public void set(T content) { this.content = content; }
    public T get() { return content; }
}

// Usage
Box<String> stringBox = new Box<>();
stringBox.set("Hello");
String s = stringBox.get();  // No cast needed

Box<Integer> intBox = new Box<>();
intBox.set(42);
int i = intBox.get();
```

### Generic Methods

```java
public static <T> void printArray(T[] array) {
    for (T element : array) {
        System.out.println(element);
    }
}

// Usage
String[] strings = {"A", "B", "C"};
Integer[] numbers = {1, 2, 3};
printArray(strings);
printArray(numbers);
```

### Bounded Type Parameters

```java
// Only accept Number or its subclasses
class Calculator<T extends Number> {
    T value;
    
    public double doubleValue() {
        return value.doubleValue();
    }
}

// Usage
Calculator<Integer> intCalc = new Calculator<>();  // OK
Calculator<String> strCalc = new Calculator<>();   // Error!
```

---

## Part 2: Multithreading

### What is Multithreading?

Multithreading allows concurrent execution of multiple parts of a program.

```
┌─────────────────────────────────────────────────────────────┐
│                    MULTITHREADING                            │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Single Thread:              Multiple Threads:              │
│                                                              │
│   ┌─────────┐                 ┌─────────┐                   │
│   │ Main    │                 │Thread 1 │                   │
│   │Thread   │                 ├─────────┤                   │
│   │         │                 │Thread 2 │                   │
│   │         │                 ├─────────┤                   │
│   │         │                 │Thread 3 │                   │
│   └─────────┘                 └─────────┘                   │
│   Time →                     Time →                        │
│                                                              │
│   Benefits:                                                 │
│   ✓ Better CPU utilization                                  │
│   ✓ Responsive applications                                 │
│   ✓ Concurrent execution                                    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Creating Threads

```java
// Method 1: Extend Thread class
class MyThread extends Thread {
    @Override
    public void run() {
        System.out.println("Thread running");
    }
}

// Method 2: Implement Runnable
class MyRunnable implements Runnable {
    @Override
    public void run() {
        System.out.println("Runnable running");
    }
}

// Usage
Thread t1 = new MyThread();
Thread t2 = new Thread(new MyRunnable());

t1.start();
t2.start();
```

### Thread Lifecycle

```
    NEW ──► RUNNABLE ──► TERMINATED
              │
              ▼
           BLOCKED/WAITING ──► RUNNABLE
```

### Thread Synchronization

```java
class Counter {
    private int count = 0;
    
    // Synchronized method
    public synchronized void increment() {
        count++;
    }
    
    public synchronized int getCount() {
        return count;
    }
}
```

### Executor Service

```java
import java.util.concurrent.*;

// Create thread pool
ExecutorService executor = Executors.newFixedThreadPool(3);

// Submit tasks
executor.submit(() -> System.out.println("Task 1"));
executor.submit(() -> System.out.println("Task 2"));

// Shutdown
executor.shutdown();
```

---

## Part 3: File I/O Operations

### File I/O Overview

```
┌─────────────────────────────────────────────────────────────┐
│                       FILE I/O                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Byte Streams:              Character Streams:             │
│   InputStream/OutputStream  Reader/Writer                  │
│   ├─ FileInputStream        ├─ FileReader                 │
│   ├─ BufferedInputStream     ├─ BufferedReader            │
│   └─ ObjectInputStream       └─ PrintWriter                │
│                                                              │
│   NIO (Java 7+):                                            │
│   Path, Files, Channels, Buffers                           │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Reading Files

```java
// Using BufferedReader
try (BufferedReader reader = new BufferedReader(new FileReader("file.txt"))) {
    String line;
    while ((line = reader.readLine()) != null) {
        System.out.println(line);
    }
}

// Using Files (Java 7+)
String content = Files.readString(Path.of("file.txt"));
List<String> lines = Files.readAllLines(Path.of("file.txt"));
```

### Writing Files

```java
// Using BufferedWriter
try (BufferedWriter writer = new BufferedWriter(new FileWriter("output.txt"))) {
    writer.write("Hello");
    writer.newLine();
    writer.write("World");
}

// Using Files (Java 7+)
Files.writeString(Path.of("output.txt"), "Hello World");
Files.write(Path.of("output.txt"), List.of("Line 1", "Line 2"));
```

### File Operations

```java
import java.nio.file.*;

// Check if file exists
boolean exists = Files.exists(Path.of("file.txt"));

// Create file
Files.createFile(Path.of("newfile.txt"));

// Delete file
Files.delete(Path.of("file.txt"));

// Copy file
Files.copy(Path.of("source.txt"), Path.of("dest.txt"));

// Get file info
System.out.println(Files.size(Path.of("file.txt")));
System.out.println(Files.getLastModifiedTime(Path.of("file.txt")));
```

---

## 4. Code Examples

### Example 1: Generic Repository

```java
/**
 * Generic Repository - Pattern for data access
 */
class Repository<T> {
    private java.util.Map<Integer, T> database = new java.util.HashMap<>();
    private int idCounter = 0;
    
    public T save(T entity) {
        int id = ++idCounter;
        database.put(id, entity);
        System.out.println("Saved with id: " + id);
        return entity;
    }
    
    public T findById(int id) {
        return database.get(id);
    }
    
    public java.util.List<T> findAll() {
        return new java.util.ArrayList<>(database.values());
    }
    
    public void delete(int id) {
        database.remove(id);
    }
}

class User {
    private String name;
    public User(String name) { this.name = name; }
    @Override public String toString() { return "User: " + name; }
}

public class GenericRepositoryDemo {
    public static void main(String[] args) {
        Repository<User> userRepo = new Repository<>();
        
        userRepo.save(new User("Alice"));
        userRepo.save(new User("Bob"));
        
        System.out.println(userRepo.findAll());
    }
}
```

---

### Example 2: Thread Synchronization

```java
/**
 * BankAccount with thread-safe operations
 */
class BankAccount {
    private double balance;
    
    public BankAccount(double initialBalance) {
        this.balance = initialBalance;
    }
    
    public synchronized void deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            System.out.println("Deposited: $" + amount + ", Balance: $" + balance);
        }
    }
    
    public synchronized void withdraw(double amount) {
        if (amount > 0 && amount <= balance) {
            balance -= amount;
            System.out.println("Withdrawn: $" + amount + ", Balance: $" + balance);
        }
    }
    
    public synchronized double getBalance() {
        return balance;
    }
}

public class ThreadSynchronizationDemo {
    public static void main(String[] args) throws InterruptedException {
        BankAccount account = new BankAccount(1000);
        
        // Create multiple threads
        Thread t1 = new Thread(() -> {
            for (int i = 0; i < 5; i++) {
                account.deposit(100);
            }
        });
        
        Thread t2 = new Thread(() -> {
            for (int i = 0; i < 5; i++) {
                account.withdraw(50);
            }
        });
        
        t1.start();
        t2.start();
        
        t1.join();
        t2.join();
        
        System.out.println("Final balance: $" + account.getBalance());
    }
}
```

---

### Example 3: File Operations

```java
import java.nio.file.*;
import java.io.*;

/**
 * FileOperationsDemo - Comprehensive file handling
 */
public class FileOperationsDemo {
    
    public static void main(String[] args) throws IOException {
        Path filePath = Paths.get("demo.txt");
        
        // Write to file
        String content = "Hello, Java File I/O!\nWelcome to the course.";
        Files.writeString(filePath, content);
        System.out.println("Written to file");
        
        // Read from file
        String readContent = Files.readString(filePath);
        System.out.println("\nFile content:\n" + readContent);
        
        // Read lines
        System.out.println("\nLine by line:");
        for (String line : Files.readAllLines(filePath)) {
            System.out.println("  " + line);
        }
        
        // Append to file
        Files.writeString(filePath, "\nAppended line", 
            StandardOpenOptionAPPEND);
        
        // File info
        System.out.println("\nFile info:");
        System.out.println("  Exists: " + Files.exists(filePath));
        System.out.println("  Size: " + Files.size(filePath) + " bytes");
        System.out.println("  ReadOnly: " + Files.isReadOnly(filePath));
        
        // Delete file
        Files.delete(filePath);
        System.out.println("\nFile deleted");
    }
}
```

---

### Example 4: Executor Service Demo

```java
import java.util.concurrent.*;

/**
 * ExecutorServiceDemo - Thread pool example
 */
public class ExecutorServiceDemo {
    
    public static void main(String[] args) {
        // Create fixed thread pool
        ExecutorService executor = Executors.newFixedThreadPool(3);
        
        // Submit tasks
        for (int i = 1; i <= 10; i++) {
            final int taskId = i;
            executor.submit(() -> {
                System.out.println("Task " + taskId + " executing in " + 
                    Thread.currentThread().getName());
                try {
                    Thread.sleep(500);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            });
        }
        
        // Shutdown
        executor.shutdown();
        try {
            if (!executor.awaitTermination(10, TimeUnit.SECONDS)) {
                executor.shutdownNow();
            }
        } catch (InterruptedException e) {
            executor.shutdownNow();
        }
        
        System.out.println("All tasks completed");
    }
}
```

---

## 5. Exercises

### Exercise 1: Generic Stack

**Requirements:**
1. Create generic Stack class with push/pop operations
2. Use bounded type to accept only Numbers

---

### Exercise 2: Producer-Consumer

**Requirements:**
1. Create producer thread that adds items
2. Create consumer thread that removes items
3. Use synchronized blocks

---

### Exercise 3: Copy File

**Requirements:**
1. Copy content from one file to another
2. Use both traditional I/O and NIO

---

## 6. Solutions

### Solution 1: Generic Stack

```java
class Stack<T extends Number> {
    private java.util.Stack<T> stack = new java.util.Stack<>();
    
    public void push(T item) { stack.push(item); }
    public T pop() { return stack.pop(); }
    public boolean isEmpty() { return stack.isEmpty(); }
}

public class StackDemo {
    public static void main(String[] args) {
        Stack<Integer> intStack = new Stack<>();
        intStack.push(10);
        intStack.push(20);
        System.out.println("Pop: " + intStack.pop());
    }
}
```

---

### Solution 2: Producer-Consumer

```java
class SharedQueue {
    private java.util.Queue<Integer> queue = new java.util.LinkedList<>();
    private int maxSize = 5;
    
    public synchronized void produce(int value) {
        while (queue.size() >= maxSize) {
            try { wait(); } catch (InterruptedException e) {}
        }
        queue.add(value);
        System.out.println("Produced: " + value);
        notify();
    }
    
    public synchronized int consume() {
        while (queue.isEmpty()) {
            try { wait(); } catch (InterruptedException e) {}
        }
        int value = queue.poll();
        System.out.println("Consumed: " + value);
        notify();
        return value;
    }
}
```

---

### Solution 3: Copy File

```java
import java.nio.file.*;

public class CopyFile {
    public static void main(String[] args) throws Exception {
        // Using NIO
        Files.copy(Paths.get("source.txt"), Paths.get("dest.txt"));
        System.out.println("File copied successfully");
    }
}
```

---

## Summary

### Key Takeaways

1. **Generics** - Type-safe reusable code, bounded types restrict type parameters
2. **Multithreading** - Thread class, Runnable interface, synchronized keyword
3. **File I/O** - Byte/Character streams, NIO (Path, Files), try-with-resources

---

*Happy Coding! 🚀*
