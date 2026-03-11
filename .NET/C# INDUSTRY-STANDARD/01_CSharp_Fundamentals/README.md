# 📘 C# Fundamentals - Complete Guide

Welcome to the C# Fundamentals module! This is where your journey to becoming a .NET developer begins.

## 🎯 Learning Objectives

By the end of this module, you will:

- ✅ Understand C# language syntax and structure
- ✅ Work with variables, data types, and operators
- ✅ Use control flow statements (if-else, switch, loops)
- ✅ Create and use methods
- ✅ Understand the difference between structs and classes
- ✅ Handle enums and nullable types

---

## 📋 Module Contents

### Topic Files

1. **[Topic_01_Language_Evolution](./Topic_01_Language_Evolution.md)** - C# evolution and .NET 8/9 features
2. **[Topic_02_Program_Structure](./Topic_02_Program_Structure.md)** - Entry point and namespace
3. **[Topic_03_Variables_DataTypes](./Topic_03_Variables_DataTypes.md)** - Types and type conversion
4. **[Topic_04_Operators](./Topic_04_Operators.md)** - All operator types
5. **[Topic_05_Control_Flow](./Topic_05_Control_Flow.md)** - Decision making and loops
6. **[Topic_06_Methods](./Topic_06_Methods.md)** - Methods and parameters
7. **[Topic_07_Arrays](./Topic_07_Arrays.md)** - Single and multidimensional arrays
8. **[Topic_08_Strings](./Topic_08_Strings.md)** - String manipulation
9. **[Topic_09_Structs_Classes](./Topic_09_Structs_Classes.md)** - Value vs reference types
10. **[Topic_10_Enums_Nullables](./Topic_10_Enums_Nullables.md)** - Enumerations and nullable types

### Code Examples

- [HelloWorld.cs](./Code/01_HelloWorld.cs)
- [VariablesDemo.cs](./Code/02_VariablesDemo.cs)
- [OperatorsDemo.cs](./Code/03_OperatorsDemo.cs)
- [ControlFlowDemo.cs](./Code/04_ControlFlowDemo.cs)
- [MethodsDemo.cs](./Code/05_MethodsDemo.cs)
- [ArraysDemo.cs](./Code/06_ArraysDemo.cs)
- [StringsDemo.cs](./Code/07_StringsDemo.cs)
- [StructsDemo.cs](./Code/08_StructsDemo.cs)
- [EnumsDemo.cs](./Code/09_EnumsDemo.cs)

### Exercises

- [Exercise_01_Basic_Programs](./Exercises/Exercise_01_Basic_Programs.md)
- [Exercise_02_Calculator](./Exercises/Exercise_02_Calculator.md)
- [Exercise_03_Number_Guessing](./Exercises/Exercise_03_Number_Guessing.md)

---

## ⏱️ Estimated Time

- **Reading & Theory**: 8 hours
- **Practice & Exercises**: 7 hours
- **Total**: 15 hours

---

## 🔑 Key Concepts Overview

### What is C#?

C# is a modern, general-purpose, object-oriented programming language developed by Microsoft. It was designed to be simple, powerful, and type-safe.

```
C# Language Evolution:
─────────────────────────────────────────────────────────
C# 1.0 (2002)  → Basic OOP features
C# 2.0 (2005)  → Generics, anonymous methods
C# 3.0 (2007)  → LINQ, lambda expressions
C# 4.0 (2010)  → Dynamic binding
C# 5.0 (2012)  → Async/await
C# 6.0 (2015)  → Expression-bodied members
C# 7.0 (2017)  → Tuples, pattern matching
C# 8.0 (2019)  → Nullable reference types
C# 9.0 (2020)  → Records, init only
C# 10.0 (2021) → Record structs, global usings
C# 11.0 (2022) → Raw string literals, pattern matching
C# 12.0 (2023) → Primary constructors, collection expressions
C# 13.0 (2024) → Extension methods, params collections
```

### The .NET Ecosystem

```
┌─────────────────────────────────────────────────────────┐
│                   .NET ECOSYSTEM                         │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   ┌─────────────┐    ┌─────────────┐    ┌───────────┐  │
│   │   C#        │    │   F#        │    │   VB.NET  │  │
│   │   Language  │    │   Language  │    │   Language│  │
│   └──────┬──────┘    └──────┬──────┘    └─────┬─────┘  │
│          │                 │                  │        │
│          └────────────────┴──────────────────┘        │
│                           │                            │
│                    ┌──────▼──────┐                     │
│                    │    .NET     │                     │
│                    │   Runtime   │                     │
│                    │    (CLR)    │                     │
│                    └──────┬──────┘                     │
│                           │                            │
│                    ┌──────▼──────┐                     │
│                    │     BCL     │                     │
│                    │ (Base Class │                     │
│                    │  Library)   │                     │
│                    └─────────────┘                     │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## 💻 Your First C# Program

Create a new console application:

```bash
dotnet new console -n MyFirstApp
cd MyFirstApp
```

Edit `Program.cs`:

```csharp
using System;

namespace MyFirstApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print hello world
            Console.WriteLine("Hello, World!");
            
            // Get user input
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            
            // Display greeting
            Console.WriteLine($"Welcome to C#, {name}!");
        }
    }
}
```

Run the program:

```bash
dotnet run
```

---

## 📊 Quick Reference

### Data Types

| Category | Types | Size |
|----------|-------|------|
| Integer | int, long, short, byte | 4, 8, 2, 1 bytes |
| Floating | float, double, decimal | 4, 8, 16 bytes |
| Character | char | 2 bytes |
| Boolean | bool | 1 byte |
| String | string | Unicode |

### Control Flow

- **Decision**: if-else, switch, ternary (?:)
- **Loops**: for, foreach, while, do-while
- **Jump**: break, continue, return, goto

---

## ✅ Next Steps

Ready to dive in? Start with the first topic:

**[Topic_01_Language_Evolution](./Topic_01_Language_Evolution.md)**

> 📝 Each topic includes:
> - Concept explanation
> - Historical context
> - Real-world analogy
> - Syntax deep dive
> - Beginner & advanced examples
> - Best practices
> - Common pitfalls
> - Mini exercises
> - Interview questions
