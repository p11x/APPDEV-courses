# ArithmeticLibrary Documentation

## Table of Contents
1. [Project Overview](#project-overview)
2. [Directory Structure](#directory-structure)
3. [Source Files](#source-files)
4. [Usage Examples](#usage-examples)
5. [Integration with ConApp2](#integration-with-conapp2)

---

## Project Overview

**ArithmeticLibrary** is a reusable C# class library that provides basic arithmetic operations. It serves as an example of how to create and package a class library in .NET 8.0. This library demonstrates fundamental principles of code reusability and project references within a solution.

**Project Type:** Class Library  
**Target Framework:** .NET 8.0  
**Language:** C#

---

## Directory Structure

```
ArithmeticLibrary/
├── ArithmeticLibrary.csproj           # Project file
├── ArithmeticOperations.cs            # Main source code
├── bin/
│   └── Debug/
│       └── net8.0/
│           ├── ArithmeticLibrary.dll  # Compiled library
│           ├── ArithmeticLibrary.deps.json
│           └── ArithmeticLibrary.pdb  # Debug symbols
└── obj/
    ├── project.assets.json
    ├── ArithmeticLibrary.csproj.nuget.dgspec.json
    ├── ArithmeticLibrary.csproj.nuget.g.props
    └── Debug/
        └── net8.0/
            ├── ArithmeticLibrary.AssemblyInfo.cs
            └── ...
```

---

## Source Files

### ArithmeticOperations.cs

The main source file containing the [`ArithmeticOperations`](../../ConsoleApplication/ArithmeticLibrary/ArithmeticOperations.cs:3) class with four static methods:

```csharp
namespace ArithmeticLibrary
{
    public class ArithmeticOperations
    {
        public int Addition(int x, int y)
        {
            return x + y;
        }
        public int Substraction(int x, int y)
        {
            return x - y;
        }
        public int Multiplication(int x, int y)
        {
            return x * y;
        }
        public int Division(int x, int y)
        {
            return x / y;
        }
    }
}
```

---

## Methods Overview

| Method | Parameters | Return Type | Description | Example |
|--------|------------|-------------|-------------|---------|
| Addition | (int x, int y) | int | Returns sum of two integers | `Addition(5, 3)` returns `8` |
| Substraction | (int x, int y) | int | Returns difference of two integers | `Substraction(10, 4)` returns `6` |
| Multiplication | (int x, int y) | int | Returns product of two integers | `Multiplication(4, 5)` returns `20` |
| Division | (int x, int y) | int | Returns quotient of two integers | `Division(20, 4)` returns `5` |

---

## Usage Examples

### Using the Library in Your Project

1. **Add a Project Reference:**
   ```xml
   <ItemGroup>
     <ProjectReference Include="..\ArithmeticLibrary\ArithmeticLibrary.csproj" />
   </ItemGroup>
   ```

2. **Import and Use:**
   ```csharp
   using ArithmeticLibrary;

   class Program
   {
       static void Main()
       {
           ArithmeticOperations ops = new ArithmeticOperations();
           
           Console.WriteLine(ops.Addition(100, 200));        // Output: 300
           Console.WriteLine(ops.Substraction(200, 100));  // Output: 100
           Console.WriteLine(ops.Multiplication(10, 20)); // Output: 200
           Console.WriteLine(ops.Division(30, 10));       // Output: 3
       }
   }
   ```

---

## Integration with ConApp2

The ArithmeticLibrary is referenced by ConApp2, demonstrating project-to-project references. Here's how it's used:

**File:** [Class23.cs](../../ConsoleApplication/ConApp2/Class23.cs:12)

```csharp
using ArithmeticLibrary;

namespace ConApp2
{
    //Consuming the Class Library
    internal class Class23
    {
        static void Main(string[] args)
        {
            ArithmeticOperations operations = new ArithmeticOperations();
            Console.WriteLine("Addition of two numbers is :" + operations.Addition(100, 200));
            Console.WriteLine("Substraction of Two numbers is :" + operations.Substraction(200, 100));
            Console.WriteLine("Multiplication of Two Numbers is :" + operations.Multiplication(10, 20));
            Console.WriteLine("Division of Two Numbers is :" + operations.Division(30, 10));
        }
    }
}
```

---

## Related Documentation

- [Configuration Details](./CONFIG.md) - Build configuration and project settings
- [Source Code Analysis](./SOURCE_CODE.md) - Detailed code explanation
- [ConApp2 OOP Concepts](../ConApp2/OOP_CONCEPTS.md) - Shows library usage in context

---

## Next Steps

To learn more about how this library is used in practice, explore the ConApp2 project, specifically:
- Class23 - Demonstrates consuming the library
- Class24 - Shows various parameter types including value parameters

---

*Last Updated: 2026-03-11*