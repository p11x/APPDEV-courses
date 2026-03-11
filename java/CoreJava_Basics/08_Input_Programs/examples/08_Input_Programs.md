# Input in Java

## Short Definition

Input in Java allows programs to receive data from users during runtime. The most common way to read user input is through the `Scanner` class, which provides methods to read different types of data from the keyboard.

---

## Key Bullet Points

- **Scanner class**: Built-in class for reading user input
- **Import required**: Must import `java.util.Scanner`
- **Create Scanner object**: `Scanner scanner = new Scanner(System.in);`
- **Read different types**: nextInt(), nextDouble(), nextLine(), next()
- **Close Scanner**: Always close the scanner when done

---

## Scanner Class Basics

### Step 1: Import the Scanner class

```java
import java.util.Scanner;
import java.util.*;
```

### Step 2: Create Scanner object

```java
Scanner scanner = new Scanner(System.in);
```

### Step 3: Read input

```java
// Read integer
system.out.println("Enter a number: ");
int number = scanner.nextInt();

system.out.println("Enter a salary: ");
// Read decimal number
double price = scanner.nextDouble();

// Read text line
String name = scanner.nextLine();

// Read single word
String word = scanner.next();
```

---

## Simple Input Example

### Example 1: Reading a Name

```java
import java.util.Scanner;

public class InputExample {
    public static void main(String[] args) {
        // Create Scanner object
        Scanner scanner = new Scanner(System.in);
        
        // Prompt user
        System.out.print("Enter your name: ");
        
        // Read input
        String name = scanner.nextLine();
        
        // Display output
        System.out.println("Hello, " + name + "!");
        
        // Close scanner
        scanner.close();
    }
}
```

### Expected Output (Sample)
```
Enter your name: Alice
Hello, Alice!
```

---

## Example 2: Reading Numbers

```java
import java.util.Scanner;

public class NumberInputExample {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Read integer
        System.out.print("Enter your age: ");
        int age = scanner.nextInt();
        
        // Read decimal
        System.out.print("Enter your height (in meters): ");
        double height = scanner.nextDouble();
        
        // Display results
        System.out.println("\n--- User Information ---");
        System.out.println("Age: " + age);
        System.out.println("Height: " + height + " meters");
        
        scanner.close();
    }
}
```

### Expected Output (Sample)
```
Enter your age: 25
Enter your height (in meters): 1.75

--- User Information ---
Age: 25
Height: 1.75 meters
```

---

## Important Notes

1. **Import Scanner**: Always include `import java.util.Scanner;` at the top
2. **Next vs NextLine**: 
   - `next()` - reads single word (stops at space)
   - `nextLine()` - reads entire line (includes spaces)
3. **Type matching**: Use the correct method for the data type
4. **Close scanner**: Always call `scanner.close()` when done

---

## Common Scanner Methods

| Method | Description | Example |
|--------|-------------|---------|
| `nextInt()` | Reads an integer | 42 |
| `nextDouble()` | Reads a double | 3.14 |
| `nextFloat()` | Reads a float | 3.14f |
| `nextLine()` | Reads entire line | "Hello World" |
| `next()` | Reads single word | "Hello" |
| `nextBoolean()` | Reads boolean | true/false |
| `nextByte()` | Reads byte | 127 |
| `nextShort()` | Reads short | 32767 |
| `nextLong()` | Reads long | 123456789 |

---

## Tips for Beginners

1. **Import first**: Always import Scanner at the beginning
2. **Prompt user**: Always print a message asking for input
3. **Handle different types**: Use the right method for each data type
4. **Close resources**: Close Scanner to prevent memory leaks
