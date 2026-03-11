# Scanner Class in Java

## Short Definition

The Scanner class is a built-in Java utility that allows reading input from various sources like the console, files, or strings. It's the most common way to get user input in Java applications.

---

## Key Bullet Points

- **Part of java.util package**: Must import to use
- **Multiple Input Sources**: Console, files, strings
- **Token-based Reading**: Reads tokens separated by whitespace
- **Various Methods**: next(), nextInt(), nextDouble(), etc.
- **Exception Handling**: Can throw InputMismatchException
- **Resource Management**: Should be closed after use

---

## Importing Scanner

```java
import java.util.Scanner;
```

---

## Creating Scanner Objects

```java
// Scanner for console input
Scanner scanner = new Scanner(System.in);

// Scanner for string input
Scanner stringScanner = new Scanner("Hello World 123");

// Scanner for file input
Scanner fileScanner = new Scanner(new File("input.txt"));
```

---

## Scanner Methods

### Reading Different Types

| Method | Returns | Description |
|--------|---------|-------------|
| `next()` | String | Reads next token |
| `nextLine()` | String | Reads entire line |
| `nextInt()` | int | Reads next integer |
| `nextDouble()` | double | Reads next double |
| `nextBoolean()` | boolean | Reads next boolean |
| `nextByte()` | byte | Reads next byte |
| `nextShort()` | short | Reads next short |
| `nextLong()` | long | Reads next long |
| `nextFloat()` | float | Reads next float |

### Check Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `hasNext()` | boolean | Checks if more tokens |
| `hasNextInt()` | boolean | Checks if next is int |
| `hasNextDouble()` | boolean | Checks if next is double |

---

## Example: Basic Input

```java
import java.util.Scanner;

class InputDemo {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.print("Enter your name: ");
        String name = scanner.nextLine();
        
        System.out.print("Enter your age: ");
        int age = scanner.nextInt();
        
        System.out.print("Enter your salary: ");
        double salary = scanner.nextDouble();
        
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Salary: $" + salary);
        
        scanner.close();
    }
}
```

---

## Example: Using hasNext() Loop

```java
import java.util.Scanner;

class TokenReader {
    public static void main(String[] args) {
        Scanner scanner = new Scanner("One Two Three Four Five");
        
        while (scanner.hasNext()) {
            System.out.println(scanner.next());
        }
        
        scanner.close();
    }
}
```

---

## Example: File Input

```java
import java.util.Scanner;
import java.io.File;
import java.io.FileNotFoundException;

class FileReader {
    public static void main(String[] args) {
        try {
            Scanner fileScanner = new Scanner(new File("data.txt"));
            
            while (fileScanner.hasNextLine()) {
                System.out.println(fileScanner.nextLine());
            }
            
            fileScanner.close();
        } catch (FileNotFoundException e) {
            System.out.println("File not found!");
        }
    }
}
```

---

## Best Practices

1. **Close Scanner**: Always close when done
2. **Use Try-With-Resources**: Auto-closes
3. **Validate Input**: Check with hasNextInt() etc.
4. **Handle Exceptions**: Catch InputMismatchException

---

## Modern Alternative: BufferedReader

For large inputs, BufferedReader is more efficient:

```java
import java.io.BufferedReader;
import java.io.InputStreamReader;

BufferedReader reader = new BufferedReader(
    new InputStreamReader(System.in)
);
String line = reader.readLine();
```

---

## Why This Matters for Angular Developers?

- In web apps, forms replace Scanner
- Angular uses ngModel for two-way binding
- REST API receives input via @RequestBody
- Understanding input helps in testing Java code

---

## Exercises

### Exercise 1: Simple Calculator
Create a calculator that reads two numbers and an operator, then performs the operation.

### Exercise 2: Student Information
Read and store student name, age, and grades using Scanner.

### Exercise 3: File Reader
Create a program that reads and displays the contents of a text file.

---

## Summary

- Scanner is used for reading user input
- Multiple methods for different data types
- Always close to prevent resource leaks
- Can read from console, files, or strings
- For web apps, forms replace console input
