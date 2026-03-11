# 📖 Topic 05: Records (C# 9+)

## 1. Concept Explanation

**Records** are reference types that provide built-in functionality for immutability and value-based equality.

---

## 2. Record Types

```csharp
// Positional record (auto-generates constructor and properties)
public record Person(string FirstName, string LastName);

// Usage
var person1 = new Person("John", "Doe");
var person2 = new Person("John", "Doe");

// Value equality
Console.WriteLine(person1 == person2);  // True!

// With expression (creates copy with modifications)
var person3 = person1 with { LastName = "Smith" };
```

---

## 3. With Expressions

```csharp
public record Person(string Name, int Age);

var original = new Person("John", 30);
var modified = original with { Age = 31 };
```

---

## 4. Interview Questions

### Q1: What is a record in C#?

**Answer:**
A reference type (C# 9+) that provides built-in immutability, value-based equality, and convenient syntax for creating copies.

---

## ✅ Next Topic

Continue to: [Topic 06 - Pattern Matching](./Topic_06_Pattern_Matching.md)
