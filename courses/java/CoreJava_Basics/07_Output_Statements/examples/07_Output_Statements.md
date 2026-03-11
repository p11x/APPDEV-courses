# Output Statements in Java

## Short Definition

Output statements in Java display information to the user. The most common way to output data is through the `System.out` object, which provides methods for printing to the console.

---

## Key Bullet Points

- **System.out.println()**: Prints text and adds a new line after output
- **System.out.print()**: Prints text without a new line
- **System.out.printf()**: Prints formatted output (like C's printf)
- **String concatenation**: Use `+` to combine strings and variables
int nun = 10;
System.out.println("The number is: " + num);
sy

---

## System.out.println()

Prints text and moves to a new line after the output.

### Example 1: Simple Text

```java
public class PrintlnExample {
    public static void main(String[] args) {
        System.out.println("Hello");
        System.out.println("World");
    }
}
```

### Expected Output
```
Hello
World
```

---

## System.out.print()

Prints text without moving to a new line. The next output will appear on the same line.

### Example 2: Same Line Output

```java
public class PrintExample {
    public static void main(String[] args) {
        System.out.print("Hello ");
        System.out.print("World");
    }
}
```

### Expected Output
```
Hello World
```

---

## System.out.printf()

Prints formatted output using format specifiers.

### Common Format Specifiers

| Specifier | Meaning | Example |
|-----------|---------|---------|
| `%d` | Integer | 42 |
| `%f` | Float/Decimal | 3.14 |
| `%s` | String | "Hello" |
| `%c` | Character | 'A' |
| `%n` | New line | (line break) |

### Example 3: Formatted Output

```java
public class PrintfExample {
    public static void main(String[] args) {
        String name = "John";
        int age = 25;
        double height = 5.9;
        
        System.out.printf("Name: %s%n", name);
        System.out.printf("Age: %d%n", age);
        System.out.printf("Height: %.1f%n", height);
    }
}
```

### Expected Output
```
Name: John
Age: 25
Height: 5.9
```

---

## String Concatenation

You can combine strings and variables using the `+` operator.

### Example 4: Combining Strings

```java
public class ConcatenationExample {
    public static void main(String[] args) {
        String firstName = "John";
        String lastName = "Doe";
        int score = 95;
        
        System.out.println("Name: " + firstName + " " + lastName);
        System.out.println("Score: " + score);
        System.out.println("Result: " + firstName + " scored " + score + " points!");
    }
}
```

### Expected Output
```
Name: John Doe
Score: 95
Result: John scored 95 points!
```

---

## Quick Reference

| Method | Adds New Line | Use Case |
|--------|---------------|-----------|
| `println()` | Yes | General output |
| `print()` | No | Output on same line |
| `printf()` | No (use `%n`) | Formatted output |

---

## Tips

1. Use `println()` for most output needs
2. Use `printf()` when you need formatted output (decimals, alignment)
3. Use `print()` when building output gradually on one line
4. Remember to add `%n` for new lines in `printf()`
