# 📖 Topic 02: C# Program Structure and Entry Point

## 1. Concept Explanation

### Understanding the Program Structure

Every C# application follows a specific structure that the compiler and runtime understand. The key components are:

1. **Using Directives** - Import namespaces for types
2. **Namespace Declaration** - Logical organization of code
3. **Class Definition** - Blueprint for objects
4. **Main Method** - Entry point of the application

### The Entry Point

In C#, the `Main` method is the entry point of your application. When you run a C# program, execution begins at the `Main` method.

The Main method can have different signatures:

```csharp
static void Main()           // No arguments
static void Main(string[] args)  // Command-line arguments
static async Task Main()     // Async entry point (C# 7.1+)
static async Task Main(string[] args)  // Async with args
```

### Namespaces

Namespaces organize your code and prevent name conflicts. They act like folders for your classes:

```csharp
namespace MyCompany.MyProject
{
    // Classes here are MyCompany.MyProject.ClassName
}
```

---

## 2. Historical Context

### Evolution of Entry Points

**C# 1.0-8.0**: Required explicit Main method
```csharp
static void Main(string[] args)
{
    Console.WriteLine("Hello");
}
```

**C# 9.0+**: Top-level statements (simplified)
```csharp
Console.WriteLine("Hello");
```

**C# 10+**: File-scoped namespaces
```csharp
namespace MyApp;  // No braces needed

class Program
{
    static void Main()
    {
        // ...
    }
}
```

---

## 3. Real-World Analogy

### The Building Analogy

Think of a C# program as a building:

```
┌─────────────────────────────────────────────────────────────┐
│                     C# PROGRAM STRUCTURE                     │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ NAMESPACE (Building)                                 │   │
│  │                                                     │   │
│  │  ┌─────────────────────────────────────────────┐   │   │
│  │  │ CLASS (Floor)                                │   │   │
│  │  │                                              │   │   │
│  │  │  ┌─────────────────────────────────────┐    │   │   │
│  │  │  │ METHOD (Room)                        │    │   │   │
│  │  │  │                                      │    │   │   │
│  │  │  │  ┌─────────────────────────────┐    │    │   │   │
│  │  │  │  │ STATEMENTS (Furniture)      │    │    │   │   │
│  │  │  │  └─────────────────────────────┘    │    │   │   │
│  │  │  └─────────────────────────────────────┘    │   │   │
│  │  └─────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                              │
│  USING DIRECTIVES = Blueprint for what's in each room       │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 4. Syntax Deep Dive

### Traditional Program Structure

```csharp
// Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Namespace
namespace MyApplication
{
    // Class
    public class Program
    {
        // Main method - Entry point
        static void Main(string[] args)
        {
            // Statements
            Console.WriteLine("Hello, World!");
        }
    }
}
```

### Top-Level Statements (C# 9+)

```csharp
// Everything becomes part of a generated Program class
Console.WriteLine("Hello, World!");
```

### File-Scoped Namespaces (C# 10+)

```csharp
// File-scoped namespace
namespace MyApplication;

// Class at file level
public class Program
{
    static void Main()
    {
        Console.WriteLine("Hello!");
    }
}
```

### Global Usings (C# 10+)

Create a file called `GlobalUsings.cs`:

```csharp
// These apply to all files in the project
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
```

Now in other files:

```csharp
// No need to add using System;
Console.WriteLine("Works!");  // Console is from System namespace
```

---

## 5. Beginner Example

### Simple Hello World

```csharp
using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print greeting
            Console.WriteLine("Hello, World!");
            
            // Print with newline
            Console.WriteLine("Welcome to C# Programming!");
            
            // Print without newline
            Console.Write("Enter your name: ");
            
            // Read user input
            string name = Console.ReadLine();
            
            // Check input is not null or empty
            if (!string.IsNullOrEmpty(name))
            {
                // String interpolation
                Console.WriteLine($"Hello, {name}!");
            }
        }
    }
}
```

### With Command-Line Arguments

```csharp
using System;

namespace ArgsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if arguments provided
            if (args.Length > 0)
            {
                Console.WriteLine("Arguments received:");
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"  [{i}]: {args[i]}");
                }
            }
            else
            {
                Console.WriteLine("No arguments provided.");
            }
        }
    }
}
```

Run with: `dotnet run -- arg1 arg2 arg3`

---

## 6. Advanced Example

### Modern Entry Point with DI

```csharp
// Program.cs - Full modern setup
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Build and run the host
var builder = Host.CreateApplicationBuilder(args);

// Configure services
builder.Services.AddLogging();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddTransient<IGreetingService, GreetingService>();

var host = builder.Build();

// Get service and run
using var scope = host.Services.CreateScope();
var greetingService = scope.ServiceProvider.GetRequiredService<IGreetingService>();
greetingService.Greet("Developer");

Console.WriteLine("Application started successfully!");
```

### Async Main Entry Point

```csharp
using System;
using System.Threading.Tasks;

namespace AsyncEntry
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting async operation...");
            
            // Async operations at entry point
            var result = await DownloadDataAsync();
            
            Console.WriteLine($"Downloaded {result.Length} bytes");
        }
        
        static async Task<byte[]> DownloadDataAsync()
        {
            await Task.Delay(1000);  // Simulate async work
            return new byte[] { 1, 2, 3, 4, 5 };
        }
    }
}
```

---

## 7. Best Practices

### ✅ Do's

1. **Use top-level statements** for simple console apps
2. **Use global usings** (C# 10+) to reduce repetition
3. **Use file-scoped namespaces** (C# 10+) for cleaner code
4. **Pass args parameter** if you need command-line args
5. **Use async Main** if you have async initialization

### ❌ Don'ts

1. **Don't put business logic** directly in Main
2. **Don't hardcode configurations** - use configuration systems
3. **Don't use WriteLine for debugging** - use proper logging
4. **Don't forget to handle exceptions** in Main

---

## 8. Common Pitfalls

### ❌ Pitfall 1: Wrong Method Signature

```csharp
// WRONG - Must be static
void Main()  // Error!

// CORRECT
static void Main()
```

### ❌ Pitfall 2: Multiple Entry Points

```csharp
// File1.cs
static void Main() { }

// File2.cs
static void Main() { }  // ERROR: Multiple entry points

// SOLUTION: Keep only one Main method
```

### ❌ Pitfall 3: Forgetting to Handle Null Args

```csharp
// WRONG - Could throw NullReferenceException
foreach (var arg in args)  // args could be null

// CORRECT - Check for null
foreach (var arg in args ?? Array.Empty<string>())
```

### ❌ Pitfall 4: Not Using Async Properly

```csharp
// WRONG - Blocking in async Main
static async Task Main()
{
    Thread.Sleep(1000);  // Blocks thread!
}

// CORRECT - Use await
static async Task Main()
{
    await Task.Delay(1000);  // Non-blocking
}
```

---

## 9. Mini Exercises

### Exercise 1: Basic Structure
- [ ] Create a program with proper using, namespace, class, Main
- [ ] Print "My first C# program"

### Exercise 2: Command Line Args
- [ ] Accept two numbers as args
- [ ] Print their sum

### Exercise 3: Modern Style
- [ ] Convert to top-level statements
- [ ] Add global usings file

---

## 10. Interview Questions

### Q1: What is the entry point of a C# application?

**Answer:**
The `Main` method is the entry point. It's the first method executed when the application starts. In C# 9+, you can also use top-level statements which the compiler converts to a Main method.

### Q2: What are the valid signatures for Main method?

**Answer:**
```csharp
static void Main()
static void Main(string[] args)
static async Task Main()
static async Task Main(string[] args)
```

### Q3: What is the difference between top-level statements and traditional Main method?

**Answer:**
Top-level statements (C# 9+) are syntactic sugar - the compiler generates a Main method and class automatically. They're ideal for simple programs but traditional Main is better for complex applications needing more control.

### Q4: What are global usings?

**Answer:**
Global usings (C# 10+) apply `using` directives across the entire project. They reduce repetitive using statements in every file.

```csharp
global using System;
global using System.Collections.Generic;
```

### Q5: What is a namespace?

**Answer:**
A namespace is a logical container that organizes classes and other types. It prevents naming conflicts and provides hierarchical organization.

```csharp
namespace Company.Product.Module
{
    public class MyClass { }
}
```

---

## 📚 Additional Resources

- [Main() and Command-Line Arguments](https://docs.microsoft.com/dotnet/csharp/fundamentals/program-structure/main-command-line)
- [Global Usings](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/namespaces#84-using-alias-directives)

---

## ✅ Next Topic

Continue to: [Topic 03 - Variables and Data Types](./Topic_03_Variables_DataTypes.md)
