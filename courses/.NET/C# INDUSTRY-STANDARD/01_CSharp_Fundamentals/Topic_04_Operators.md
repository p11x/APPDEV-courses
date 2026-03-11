# 📖 Topic 04: Operators in C#

## 1. Concept Explanation

### What are Operators?

Operators are symbols that perform operations on operands (variables, values, or expressions). C# provides a rich set of operators categorized by their function.

### Types of Operators

| Category | Operators | Description |
|----------|-----------|-------------|
| Arithmetic | +, -, *, /, % | Mathematical operations |
| Relational | ==, !=, <, >, <=, >= | Compare values |
| Logical | &&, \|\|, ! | Boolean operations |
| Bitwise | &, \|, ^, ~, <<, >> | Binary operations |
| Assignment | =, +=, -=, *=, /=, %= | Assign values |
| Null Coalescing | ??, ??= | Null handling |
| Conditional | ?: | Ternary operator |
| Null Conditional | ?., ?[] | Safe navigation |

---

## 2. Syntax Deep Dive

### Arithmetic Operators

```csharp
int a = 10, b = 3;

// Addition
int sum = a + b;        // 13

// Subtraction
int diff = a - b;       // 7

// Multiplication
int product = a * b;    // 30

// Division (integer division truncates)
int quotient = a / b;   // 3

// Modulus (remainder)
int remainder = a % b;  // 1
```

### Compound Assignment

```csharp
int x = 10;
x += 5;    // x = 15
x -= 3;    // x = 12
x *= 2;    // x = 24
x /= 4;    // x = 6
x %= 5;    // x = 1
```

### Increment/Decrement

```csharp
int a = 5;
int b = a++;   // b = 5, a = 6 (post-increment)
int c = ++a;   // c = 7, a = 7 (pre-increment)

int d = 5;
int e = d--;   // e = 5, d = 4 (post-decrement)
int f = --d;   // f = 3, d = 3 (pre-decrement)
```

### Relational Operators

```csharp
int a = 10, b = 20;

bool isEqual = a == b;      // false
bool notEqual = a != b;     // true
bool greater = a > b;       // false
bool less = a < b;          // true
bool greaterOrEqual = a >= b; // false
bool lessOrEqual = a <= b;  // true
```

### Logical Operators

```csharp
bool x = true, y = false;

bool and = x && y;          // false (both must be true)
bool or = x || y;           // true (at least one is true)
bool not = !x;              // false (negation)
```

### Null Coalescing Operators

```csharp
string? name = null;
string displayName = name ?? "Unknown";     // "Unknown"
name ??= "Default";                          // "Default" (assignment)

// With nullable int
int? count = null;
int value = count ?? 0;                     // 0
```

### Ternary Operator

```csharp
int age = 20;
string status = age >= 18 ? "Adult" : "Minor";
```

---

## 3. Beginner Example

```csharp
using System;

namespace OperatorsDemo
{
    class Program
    {
        static void Main()
        {
            // Basic arithmetic
            int a = 15, b = 4;
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"a - b = {a - b}");
            Console.WriteLine($"a * b = {a * b}");
            Console.WriteLine($"a / b = {a / b}");
            Console.WriteLine($"a % b = {a % b}");
            
            // Compound assignment
            int x = 10;
            x += 5;
            Console.WriteLine($"x += 5: {x}");
            
            // Ternary operator
            int score = 75;
            string grade = score >= 60 ? "Pass" : "Fail";
            Console.WriteLine($"Grade: {grade}");
            
            // Null coalescing
            string? name = null;
            Console.WriteLine($"Name: {name ?? "Guest"}");
        }
    }
}
```

---

## 4. Advanced Examples

### Bitwise Operations

```csharp
using System;

namespace BitwiseDemo
{
    class Program
    {
        static void Main()
        {
            int a = 5;   // 0101 in binary
            int b = 3;   // 0011 in binary
            
            // AND - both bits must be 1
            Console.WriteLine(a & b);  // 1 (0001)
            
            // OR - at least one bit is 1
            Console.WriteLine(a | b);  // 7 (0111)
            
            // XOR - bits are different
            Console.WriteLine(a ^ b);  // 6 (0110)
            
            // NOT - flips bits
            Console.WriteLine(~a);     // -6
            
            // Left shift (multiply by 2)
            Console.WriteLine(a << 1); // 10
            
            // Right shift (divide by 2)
            Console.WriteLine(a >> 1); // 2
        }
    }
}
```

### Null Conditional Operators

```csharp
using System;

namespace NullConditionalDemo
{
    class Program
    {
        static void Main()
        {
            // Person with possibly null address
            Person? person = null;
            
            // Safe navigation - won't throw
            int? length = person?.Name?.Length;
            Console.WriteLine($"Length: {length}");  // null
            
            // With null coalescing
            int len = person?.Name?.Length ?? 0;
            Console.WriteLine($"Length: {len}");  // 0
            
            // Array safe access
            int[]? numbers = null;
            int first = numbers?[0] ?? -1;
            Console.WriteLine($"First: {first}");  // -1
        }
    }
    
    class Person
    {
        public string? Name { get; set; }
    }
}
```

---

## 5. Interview Questions

### Q1: What is the difference between == and Equals()?

**Answer:**
- `==` can be overloaded, behavior depends on type
- For value types: compares values
- For reference types: compares references (unless overridden)
- `Equals()` is a virtual method that can be overridden

### Q2: What is the difference between && and &?

**Answer:**
- `&&`: Short-circuit AND - stops evaluating if first condition is false
- `&`: Bitwise AND (for integers) or logical AND (always evaluates both)

### Q3: What is the null coalescing operator (??)?

**Answer:**
Returns left operand if not null, otherwise returns right operand:
```csharp
string name = null;
string result = name ?? "Default";  // "Default"
```

### Q4: What is the difference between ?? and ??=?

**Answer:**
- `??`: Returns left if not null, right if null
- `??=`: Assigns right to left if left is null (C# 8+)

### Q5: What does the ?: operator do?

**Answer:**
Ternary operator - condition ? valueIfTrue : valueIfFalse:
```csharp
string status = age >= 18 ? "Adult" : "Minor";
```

---

## 📚 Additional Resources

- [Operators in C#](https://docs.microsoft.com/dotnet/csharp/language-reference/operators/)

---

## ✅ Next Topic

Continue to: [Topic 05 - Control Flow](./Topic_05_Control_Flow.md)
