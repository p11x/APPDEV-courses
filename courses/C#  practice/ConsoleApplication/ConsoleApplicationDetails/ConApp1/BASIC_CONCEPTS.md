# ConApp1 - Basic Concepts

This document covers the fundamental programming concepts taught in ConApp1 classes.

## Table of Contents
1. [Variables and Data Types](#variables-and-data-types)
2. [Console Input/Output](#console-inputoutput)
3. [Methods](#methods)
4. [Variable Scope](#variable-scope)

---

## Variables and Data Types

### Primitive Data Types

C# supports several primitive data types:

| Data Type | Description | Example | Size |
|-----------|-------------|---------|------|
| `int` | 32-bit signed integer | `int a = 100;` | 4 bytes |
| `float` | Single-precision floating point | `float b = 12.23F;` | 4 bytes |
| `double` | Double-precision floating point | `double c = 23.45;` | 8 bytes |
| `decimal` | High-precision decimal | `decimal d = 26.78M;` | 16 bytes |
| `char` | Single character | `char e = 'S';` | 2 bytes |
| `bool` | Boolean (true/false) | `bool f = true;` | 1 byte |
| `string` | Text (immutable) | `string g = "Sathesh";` | Variable |

### Source Reference
See [Class3.cs](../../ConsoleApplication/ConApp1/Class3.cs) for practical examples.

---

## Console Input/Output

### Output Methods

```csharp
// Simple output
Console.WriteLine("Hello World");

// String concatenation
string name = "Sathesh";
Console.WriteLine("My name is " + name);

// Placeholder format
Console.WriteLine("Name: {0}, Age: {1}", name, 25);

// String interpolation (recommended)
Console.WriteLine($"Name: {name}, Age: {25}");
```

### Input Methods

```csharp
// Read as string
string input = Console.ReadLine();

// Parse to integer
int number = int.Parse(Console.ReadLine());

// Alternative: Convert
int number = Convert.ToInt32(Console.ReadLine());

// Read decimal
decimal salary = decimal.Parse(Console.ReadLine());
```

### Source Reference
See [Class4.cs](../../ConsoleApplication/ConApp1/Class4.cs) for practical examples.

---

## Methods

### Instance vs Static Methods

```csharp
public class MyClass
{
    // Instance method - requires object
    public void Show()
    {
        Console.WriteLine("Instance method called");
    }
    
    // Static method - can be called directly
    public static void Display()
    {
        Console.WriteLine("Static method called");
    }
}

// Usage
MyClass obj = new MyClass();
obj.Show();           // Instance method
MyClass.Display();    // Static method
```

### Source Reference
See [Class2.cs](../../ConsoleApplication/ConApp1/Class2.cs) and [Class6.cs](../../ConsoleApplication/ConApp1/Class6.cs) for more details.

---

## Variable Scope

### Local Variables
- Declared inside methods
- Only accessible within that method
- Must be initialized before use

```csharp
void MyMethod()
{
    int localVar = 10;  // Local variable
    Console.WriteLine(localVar);
}
// localVar is not accessible here
```

### Instance Variables
- Declared at class level (outside methods)
- Each instance gets its own copy
- Default values are assigned automatically

```csharp
class MyClass
{
    int instanceVar = 100;  // Instance variable
}
```

### Static Variables
- Shared across all instances
- Declared with `static` keyword
- Accessible without creating an instance

```csharp
class MyClass
{
    static int staticVar = 200;  // Static variable
}
```

### Source Reference
See [Class5.cs](../../ConsoleApplication/ConApp1/Class5.cs) and [Class6.cs](../../ConsoleApplication/ConApp1/Class6.cs) for practical examples.

---

## Key Takeaways

1. **Choose the right data type** - Use `int` for whole numbers, `double` for decimals, `decimal` for financial calculations
2. **Use string interpolation** - It's cleaner than concatenation
3. **Validate input** - Always check user input before parsing
4. **Understand scope** - Local variables are temporary, instance variables persist with the object

---

## Related Classes

| Class | Topic |
|-------|-------|
| Class3 | Variables and Data Types |
| Class4 | Console Input |
| Class5 | Local Variables |
| Class6 | Instance vs Static Variables |
| Class1 | Introduction |

---

*Last Updated: 2026-03-11*