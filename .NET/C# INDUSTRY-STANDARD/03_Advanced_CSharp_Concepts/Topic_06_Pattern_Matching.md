# 📖 Topic 06: Pattern Matching

## 1. Concept Explanation

**Pattern matching** allows you to test expressions against different patterns.

---

## 2. Pattern Types

```csharp
// Type pattern
object obj = "Hello";
if (obj is string s)
    Console.WriteLine($"String: {s}");

// Relational patterns (C# 9+)
int score = 85;
string grade = score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    _ => "F"
};

// Property patterns
if (person is { Age: > 18, Name: "John" })
    Console.WriteLine("Adult John");
```

---

## ✅ End of Module

Return to: [README](./README.md)
