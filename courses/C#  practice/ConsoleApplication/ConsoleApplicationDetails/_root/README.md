# ConsoleApplication Project Documentation

Welcome to the comprehensive documentation for the ConsoleApplication C# learning project.

## Project Overview

This is a multi-project .NET 8.0 solution designed to teach C# programming from beginner to advanced levels.

| Project | Type | Difficulty | Classes |
|---------|------|------------|---------|
| ArithmeticLibrary | Class Library | Reference | 1 |
| ConApp1 | Console App | Beginner | 24 |
| ConApp2 | Console App | Advanced | 56 |

## Documentation Structure

### 📁 Root Documentation
- [PROJECT_SUMMARY.md](../PROJECT_SUMMARY.md) - Complete consolidated reference

---

### 📁 ArithmeticLibrary
- [README.md](./ArithmeticLibrary/README.md) - Library overview and API reference

---

### 📁 ConApp1 - Beginner Console Application

**Getting Started:**
- [README.md](./ConApp1/README.md) - Project overview and setup

**Topic Guides:**
- [BASIC_CONCEPTS.md](./ConApp1/BASIC_CONCEPTS.md) - Variables, data types, operators
- [CONTROL_FLOW.md](./ConApp1/CONTROL_FLOW.md) - If-else, switch, loops
- [PATTERNS.md](./ConApp1/PATTERNS.md) - Number and star patterns

---

### 📁 ConApp2 - Advanced Console Application

**Getting Started:**
- [README.md](./ConApp2/README.md) - Project overview and setup

**Topic Guides:**
- [OOP_CONCEPTS.md](./ConApp2/OOP_CONCEPTS.md) - Inheritance, polymorphism, abstraction
- [ARRAYS_COLLECTIONS.md](./ConApp2/ARRAYS_COLLECTIONS.md) - Lists, dictionaries, stacks, queues
- [FILE_IO_JSON.md](./ConApp2/FILE_IO_JSON.md) - File operations and JSON handling
- [HTTP_NETWORKING.md](./ConApp2/HTTP_NETWORKING.md) - REST API consumption

---

## Quick Navigation

| Topic | ConApp1 | ConApp2 |
|-------|---------|---------|
| Variables & Types | ✅ | ✅ |
| Operators | ✅ | ✅ |
| Control Flow | ✅ | ✅ |
| Loops & Patterns | ✅ | - |
| Methods | ✅ | ✅ |
| Arrays | - | ✅ |
| Collections | - | ✅ |
| OOP | - | ✅ |
| File I/O | - | ✅ |
| JSON | - | ✅ |
| HTTP | - | ✅ |

---

## Building the Solution

```bash
# Navigate to solution
cd ConsoleApplication

# Build all projects
dotnet build

# Run specific project
cd ConApp1
dotnet run

cd ConApp2
dotnet run

# Run tests (if any)
dotnet test
```

---

## Dependencies

| Package | Version | Used By |
|---------|---------|---------|
| .NET | 8.0 | All projects |
| Newtonsoft.Json | 13.0.3 | ConApp2 |

---

## Learning Roadmap

### Phase 1: Beginner (ConApp1)
1. Variables and data types
2. Operators and expressions
3. Decision making (if-else, switch)
4. Loops (for, while, do-while)
5. Pattern printing
6. Methods and parameters

### Phase 2: Advanced (ConApp2)
1. Arrays and collections
2. Exception handling
3. Object-oriented programming
4. Generics
5. File I/O
6. JSON serialization
7. HTTP client

---

## Related Documentation

- [PROJECT_SUMMARY.md](../PROJECT_SUMMARY.md) - Full consolidated documentation
- [ArithmeticLibrary](./ArithmeticLibrary/README.md)
- [ConApp1](./ConApp1/README.md)
- [ConApp2](./ConApp2/README.md)

---

*Last Updated: 2026-03-11*