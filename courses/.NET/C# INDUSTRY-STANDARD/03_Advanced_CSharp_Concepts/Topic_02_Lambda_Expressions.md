# 📖 Topic 02: Lambda Expressions

## 1. Concept Explanation

A **lambda expression** is an anonymous function that can be used to create delegates or expression tree types.

---

## 2. Lambda Syntax

```csharp
// Traditional method
int Add(int a, int b) { return a + b; }

// Lambda expression
(int a, int b) => a + b;

// With type inference
(a, b) => a + b;

// Single parameter (parentheses optional)
x => x * 2;

// No parameters
() => Console.WriteLine("Hello");
```

---

## 3. Lambda with LINQ

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };

// Filter with Where
var even = numbers.Where(n => n % 2 == 0);

// Transform with Select
var doubled = numbers.Select(n => n * 2);
```

---

## ✅ Next Topic

Continue to: [Topic 03 - Generics](./Topic_03_Generics.md)
