# 📖 Topic 03: Variables, Constants, and Data Types

## 1. Concept Explanation

### What are Variables?

A **variable** is a named storage location in memory that holds a value which can change during program execution. Each variable has:

- **Name (Identifier)**: How we reference it
- **Type**: What kind of data it stores
- **Value**: The actual data stored
- **Memory Address**: Where it's stored in RAM

### Data Types in C#

C# is a **statically-typed** language, meaning variable types are known at compile time.

#### Value Types (stored on stack)
```
┌────────────────────────────────────────────────────────────┐
│                    VALUE TYPES                             │
├────────────────────────────────────────────────────────────┤
│                                                             │
│  Numeric Integral                                          │
│  ├── sbyte   : -128 to 127 (8-bit)                        │
│  ├── byte    : 0 to 255 (8-bit)                           │
│  ├── short   : -32,768 to 32,767 (16-bit)                  │
│  ├── ushort  : 0 to 65,535 (16-bit)                       │
│  ├── int     : -2.1B to 2.1B (32-bit)                      │
│  ├── uint    : 0 to 4.2B (32-bit)                          │
│  ├── long    : ±9.2 quintillion (64-bit)                  │
│  └── ulong   : 0 to 18.4 quintillion (64-bit)             │
│                                                             │
│  Numeric Floating                                          │
│  ├── float   : ±3.4 × 10^38 (32-bit)                      │
│  ├── double  : ±1.7 × 10^308 (64-bit)                      │
│  └── decimal : ±7.9 × 10^28 (128-bit) - financial calcs   │
│                                                             │
│  Other                                                      │
│  ├── char    : Single Unicode character (16-bit)          │
│  ├── bool    : true or false (8-bit)                      │
│  └── (custom structs, enums)                              │
│                                                             │
└────────────────────────────────────────────────────────────┘
```

#### Reference Types (stored on heap)
```
┌────────────────────────────────────────────────────────────┐
│                   REFERENCE TYPES                          │
├────────────────────────────────────────────────────────────┤
│                                                             │
│  Built-in                                                  │
│  ├── string  : Unicode text                                │
│  └── object  : Base type for all types                     │
│                                                             │
│  Custom                                                    │
│  ├── class   : Reference type with methods                 │
│  ├── interface: Contract for methods                       │
│  ├── array   : Collection of elements                     │
│  ├── delegate: Type-safe function pointer                 │
│  └── record  : Immutable reference type (C# 9+)          │
│                                                             │
└────────────────────────────────────────────────────────────┘
```

---

## 2. Historical Context

### Type System Evolution

- **C# 1.0**: Basic value/reference types
- **C# 2.0**: Nullable value types (`int?`)
- **C# 4.0**: `dynamic` type for late binding
- **C# 7.0**: Tuples for multiple values
- **C# 8.0**: Nullable reference types
- **C# 9.0**: Records (reference types with value equality)
- **C# 10**: Record structs

---

## 3. Real-World Analogy

### Variable as a Box

```
┌─────────────────────────────────────────────────────────────────┐
│              VARIABLE = BOX IN A WAREHOUSE                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Variable Declaration                                           │
│  ┌──────────────┐                                               │
│  │    BOX       │  ← This is the variable                      │
│  │  ┌────────┐  │                                               │
│  │  │  42    │  │  ← This is the value                         │
│  │  └────────┘  │                                               │
│  │              │                                               │
│  │ Label: "Age" │  ← This is the variable name                 │
│  │ Type: Number │  ← This is the data type                     │
│  └──────────────┘                                               │
│                                                                  │
│  Value Types:  Copy the actual value                            │
│  ┌─────┐       ┌─────┐                                          │
│  │  5  │  ──▶  │  5  │  (Two separate copies)                   │
│  └─────┘       └─────┘                                          │
│                                                                  │
│  Reference Types: Copy the address                               │
│  ┌─────────┐     ┌─────────┐                                    │
│  │ 0x001   │ ──▶ │ [data]  │  (Both point to same data)         │
│  └─────────┘     └─────────┘                                    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 4. Syntax Deep Dive

### Variable Declaration

```csharp
// Basic declaration
int age;              // Declaration only
age = 25;             // Assignment

// Declaration with initialization
int age = 25;         // Combined

// Type inference with var (C# 3.0+)
var name = "John";    // Compiler infers string
var count = 10;       // Compiler infers int

// Multiple declarations
int x = 1, y = 2, z = 3;

// Constants (immutable)
const double PI = 3.14159;
const string APP_NAME = "MyApp";

// Readonly (set at runtime, then immutable)
readonly DateTime CreatedDate = DateTime.Now;
```

### Data Type Ranges

```csharp
// Integer types
sbyte minSbyte = -128;
sbyte maxSbyte = 127;
byte maxByte = 255;

int maxInt = 2_147_483_647;     // Underscores for readability
long maxLong = 9_223_372_036_854_775_807;

// Floating point
float piFloat = 3.14f;          // Must use f or F suffix
double piDouble = 3.14;         // Default for decimals
decimal price = 19.99m;         // Must use m or M suffix for decimal

// Special values
float positiveInfinity = float.PositiveInfinity;
double negativeInfinity = double.NegativeInfinity;
float notANumber = float.NaN;
```

---

## 5. Beginner Examples

### Basic Variable Usage

```csharp
using System;

namespace VariablesDemo
{
    class Program
    {
        static void Main()
        {
            // Integer variables
            int age = 25;
            int year = 2026;
            
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Year: {year}");
            
            // Floating point
            double temperature = 72.5;
            Console.WriteLine($"Temperature: {temperature}");
            
            // Boolean
            bool isStudent = true;
            Console.WriteLine($"Is Student: {isStudent}");
            
            // Character
            char grade = 'A';
            Console.WriteLine($"Grade: {grade}");
            
            // String (reference type)
            string name = "John Doe";
            Console.WriteLine($"Name: {name}");
        }
    }
}
```

### Constants and Readonly

```csharp
using System;

namespace ConstantsDemo
{
    class Program
    {
        // Compile-time constant
        const double PI = 3.14159;
        const int DaysInWeek = 7;
        
        // Runtime constant (readonly)
        readonly DateTime StartTime = DateTime.Now;
        
        static void Main()
        {
            Console.WriteLine($"PI: {PI}");
            Console.WriteLine($"Days: {DaysInWeek}");
            Console.WriteLine($"Started: {StartTime}");
        }
    }
}
```

---

## 6. Advanced Examples

### Nullable Types (C# 8+)

```csharp
using System;

namespace NullableDemo
{
    class Program
    {
        // Enable nullable reference types
        #nullable enable
        
        // Nullable value type
        int? nullableInt = null;
        
        // Nullable reference type (C# 8+)
        string? nullableString = null;
        
        // Null coalescing operator
        static void Main()
        {
            // ?? operator - provides default if null
            int value = nullableInt ?? 0;
            
            // ?. operator - safe navigation
            int? length = nullableString?.Length;
            
            // ??= null-coalescing assignment (C# 8+)
            nullableInt ??= 10;
            
            Console.WriteLine($"Value: {value}");
            Console.WriteLine($"Length: {length}");
            Console.WriteLine($"After ??=: {nullableInt}");
        }
        
        #nullable restore
    }
}
```

### Pattern Matching with Types (C# 7+)

```csharp
using System;

namespace PatternMatchingDemo
{
    class Program
    {
        static void Main()
        {
            object[] items = { 42, "hello", 3.14, true, DateTime.Now };
            
            foreach (var item in items)
            {
                string description = item switch
                {
                    int i when i > 0 => $"Positive integer: {i}",
                    int i => $"Integer: {i}",
                    string s => $"String of length {s.Length}",
                    double d => $"Double: {d:F2}",
                    bool b => $"Boolean: {b}",
                    _ => $"Unknown type: {item.GetType()}"
                };
                
                Console.WriteLine(description);
            }
        }
    }
}
```

---

## 7. Best Practices

### ✅ Do's

1. **Use `var`** when type is obvious from right side
2. **Use meaningful variable names**
3. **Use `const`** for values that never change
4. **Use `readonly`** for values set once in constructor
5. **Enable nullable reference types** (`#nullable enable`)
6. **Use appropriate numeric suffixes** (f, m, L)

### ❌ Don'ts

1. **Don't use reserved words** as variable names
2. **Don't use magic numbers** - use constants
3. **Don't ignore nullable warnings** - handle null properly
4. **Don't use `var`** when type isn't obvious
5. **Don't use floating-point for financial calculations** - use `decimal`

---

## 8. Common Pitfalls

### ❌ Pitfall 1: Floating Point Accuracy

```csharp
// WRONG - Floating point for money
double price = 0.1 + 0.2;
Console.WriteLine(price == 0.3);  // False! prints 0.30000000000000004

// CORRECT - Use decimal for money
decimal price2 = 0.1m + 0.2m;
Console.WriteLine(price2 == 0.3m);  // True
```

### ❌ Pitfall 2: Nullable Confusion

```csharp
// WRONG - Nullable reference without checking
string? name = null;
Console.WriteLine(name.Length);  // Runtime exception!

// CORRECT - Null check or safe navigation
string? name = null;
Console.WriteLine(name?.Length);  // Prints nothing (null)
Console.WriteLine(name?.Length ?? 0);  // Prints 0
```

### ❌ Pitfall 3: Integer Overflow

```csharp
// WRONG - Overflow without warning
int max = int.MaxValue;
Console.WriteLine(max + 1);  // -2147483648 (overflows!)

// CORRECT - Use checked for overflow detection
int max = int.MaxValue;
checked
{
    Console.WriteLine(max + 1);  // Throws OverflowException
}
```

### ❌ Pitfall 4: Using var Incorrectly

```csharp
// WRONG - var when type isn't obvious
var result = GetData();  // What type is this?

// CORRECT - Use explicit type when unclear
MyCustomType result = GetData();
```

---

## 9. Mini Exercises

### Exercise 1: Variable Declaration
- [ ] Declare variables of each numeric type
- [ ] Print their values and sizes

### Exercise 2: Type Conversion
- [ ] Convert int to double
- [ ] Convert string to int using Parse and TryParse

### Exercise 3: Nullable Practice
- [ ] Create nullable int and string
- [ ] Use null-coalescing operators

---

## 10. Interview Questions

### Q1: What is the difference between value types and reference types?

**Answer:**
- **Value Types**: Stored on stack, contain actual data, copying creates a new copy
- **Reference Types**: Stored on heap, contain reference/pointer to data, copying copies the reference

### Q2: What is the difference between const and readonly?

**Answer:**
- `const`: Compile-time constant, must be initialized with literal values, shared across all instances
- `readonly`: Runtime constant, can be initialized in constructor or declaration, per-instance values possible

### Q3: What is the default value for uninitialized variables?

**Answer:**
- **Value types**: Default value (0 for numeric, false for bool, '\0' for char)
- **Reference types**: null

### Q4: What are nullable value types?

**Answer:**
Nullable value types (C# 2.0+) allow value types to have null values:
```csharp
int? nullableInt = null;  // int? is Nullable<int>
```

### Q5: What is the difference between string and String?

**Answer:**
No difference - `string` is the C# keyword alias for `System.String`. Use `string` in C# code.

### Q6: What is the purpose of the var keyword?

**Answer:**
`var` enables implicit type declaration - the compiler infers the type from the right-hand side expression. Useful for long type names and anonymous types.

### Q7: What is the stack vs heap?

**Answer:**
- **Stack**: Fast, small memory, stores value types and references
- **Heap**: Larger memory, stores reference types, managed by Garbage Collector

---

## 📚 Additional Resources

- [Value Types](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/variables#94-value-types)
- [Reference Types](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/variables#95-reference-types)

---

## ✅ Next Topic

Continue to: [Topic 04 - Operators](./Topic_04_Operators.md)
