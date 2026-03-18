# ConApp2 - Advanced Console Application

ConApp2 is an advanced-level C# console application that builds upon the fundamentals learned in ConApp1, diving deep into Object-Oriented Programming, collections, file I/O, and networking.

## Project Overview

| Attribute | Value |
|-----------|-------|
| **Project Type** | Console Application |
| **Framework** | .NET 8.0 |
| **Language** | C# |
| **Classes** | 56 |
| **Difficulty** | Advanced |
| **NuGet Packages** | Newtonsoft.Json 13.0.3 |

## Learning Path

This project is organized into several advanced topics:

### 1. Object-Oriented Programming
- **OOP Concepts** - Inheritance, polymorphism, abstraction, encapsulation
  - See: [OOP_CONCEPTS.md](./OOP_CONCEPTS.md)

### 2. Collections and Data Structures
- **Arrays & Collections** - Lists, dictionaries, stacks, queues, sets
  - See: [ARRAYS_COLLECTIONS.md](./ARRAYS_COLLECTIONS.md)

### 3. File Operations and JSON
- **File I/O & JSON** - StreamReader/Writer, JSON serialization
  - See: [FILE_IO_JSON.md](./FILE_IO_JSON.md)

### 4. Networking
- **HTTP & Networking** - REST API consumption
  - See: [HTTP_NETWORKING.md](./HTTP_NETWORKING.md)

## Directory Structure

```
ConApp2/
├── Class1.cs          # Arrays Basics
├── Class2.cs          # Array Operations
├── Class3.cs          # 2D Arrays
├── Class4.cs          # Array Parameters
├── Class5.cs          # Params Keyword
├── Class6.cs          # Array Methods
├── Class7.cs          # Jagged Arrays
├── Class8.cs          # ArrayList
├── Class9.cs          # Collections Intro
├── Class10.cs         # Hashtable
├── Class11.cs         # Exception Handling
├── Class12.cs         # Custom Exceptions
├── Class13.cs         # File Operations
├── Class14.cs         # Directory Operations
├── Class15.cs         # Advanced OOP
├── Class16.cs         # Inheritance
├── Class17.cs         # Polymorphism
├── Class18.cs         # Abstraction
├── Class19.cs         # Encapsulation
├── Class20.cs         # Interfaces
├── Class21.cs         # Generics
├── Class22.cs         # Generic Classes
├── ... (up to Class56)
├── data.json          # Sample data file
├── Employees.json     # Employee data
├── 2024/8/20/        # Sample folder structure
└── ConApp2.csproj
```

## Topics Covered

### Object-Oriented Programming
- Classes and objects
- Constructors (default, parameterized, copy, static)
- Inheritance (single, multilevel, hierarchical)
- Polymorphism (method overloading, overriding)
- Abstraction (abstract classes, interfaces)
- Encapsulation (access modifiers, properties)
- Composition and aggregation

### Collections
- Single and multidimensional arrays
- ArrayList (non-generic)
- List<T>, LinkedList<T>
- Dictionary<TKey, TValue>
- HashSet<T>
- Stack<T>, Queue<T>
- LINQ operations

### File I/O
- StreamReader/StreamWriter
- FileInfo, DirectoryInfo
- DriveInfo
- JSON serialization (Newtonsoft.Json, System.Text.Json)
- Working with paths

### Networking
- HttpClient
- REST API consumption
- Async/Await patterns
- JSON parsing from HTTP

## Key Classes Summary

| Class Range | Topic Area |
|-------------|------------|
| Class1-7 | Arrays |
| Class8-10 | Non-Generic Collections |
| Class11-12 | Exception Handling |
| Class13-14 | File & Directory I/O |
| Class15-26 | OOP Concepts |
| Class27-34 | Generic Collections |
| Class35-45 | Advanced OOP & Patterns |
| Class46-55 | File I/O & JSON |
| Class56 | HTTP Client |

## Building the Project

### Build
```bash
cd ConApp2
dotnet build
```

### Run
```bash
dotnet run
```

### Release Build
```bash
dotnet publish -c Release
```

## Dependencies

### NuGet Packages
- **Newtonsoft.Json** 13.0.3 - JSON serialization/deserialization

### .NET Assemblies
- System.IO - File operations
- System.Net.Http - HTTP client
- System.Collections.Generic - Generic collections
- System.Linq - LINQ operations

## Sample Data Files

The project includes sample data for learning file I/O:

| File | Purpose |
|------|---------|
| data.json | Student/course training data |
| Employees.json | Employee records |
| 2024/8/20/sample.txt | Nested directory example |

## Prerequisites

Before starting ConApp2, you should have:
- Completed ConApp1 or equivalent beginner knowledge
- Understanding of basic C# syntax
- Familiarity with loops and methods
- Basic understanding of classes and objects

## Related Documentation

- [OOP Concepts](./OOP_CONCEPTS.md)
- [Arrays & Collections](./ARRAYS_COLLECTIONS.md)
- [File I/O & JSON](./FILE_IO_JSON.md)
- [HTTP & Networking](./HTTP_NETWORKING.md)

---

*Last Updated: 2026-03-11*