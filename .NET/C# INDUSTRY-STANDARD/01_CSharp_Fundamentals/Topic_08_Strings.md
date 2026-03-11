# 📖 Topic 08: Strings in C#

## 1. Concept Explanation

A **string** is a sequence of Unicode characters. In C#, `string` is an alias for `System.String` and is a reference type, but it behaves like a value type for many operations.

### String Characteristics

- Immutable (cannot be changed after creation)
- Unicode support
- Null-terminated internally
- Extensive built-in methods

---

## 2. String Creation

```csharp
// Various ways to create strings
string s1 = "Hello";                    // Literal
string s2 = new string('A', 5);        // Character array repeated
char[] chars = { 'H', 'e', 'l', 'l', 'o' };
string s3 = new string(chars);         // From char array
string s4 = new string(new char[] { 'A', 'B', 'C' });
```

---

## 3. String Methods

```csharp
string text = "Hello, World!";

// Length
Console.WriteLine(text.Length);  // 13

// Case conversion
Console.WriteLine(text.ToUpper());   // HELLO, WORLD!
Console.WriteLine(text.ToLower());   // hello, world!

// Search
Console.WriteLine(text.Contains("World"));  // True
Console.WriteLine(text.IndexOf("World"));    // 7
Console.WriteLine(text.StartsWith("Hello")); // True
Console.WriteLine(text.EndsWith("!"));      // True

// Substring
Console.WriteLine(text.Substring(0, 5));  // Hello
Console.WriteLine(text.Substring(7));     // World!

// Replace
Console.WriteLine(text.Replace("World", "C#"));  // Hello, C#!

// Split
string csv = "apple,banana,cherry";
string[] fruits = csv.Split(',');  // ["apple", "banana", "cherry"]

// Trim
string padded = "  hello  ";
Console.WriteLine(padded.Trim());  // hello
```

---

## 4. String Interpolation

```csharp
string name = "John";
int age = 30;

// String interpolation (recommended)
string message = $"Hello, {name}! You are {age} years old.";

// With formatting
double price = 19.99;
string formatted = $"Price: {price:C2}";  // Price: $19.99

// Expressions in interpolation
string calc = $"2 + 2 = {2 + 2}";  // 2 + 2 = 4
```

---

## 5. StringBuilder

For frequent modifications, use `StringBuilder`:

```csharp
using System.Text;

StringBuilder sb = new StringBuilder();
sb.Append("Hello");
sb.AppendLine(" World");  // Adds newline
sb.AppendFormat("Count: {0}", 42);
sb.Replace("World", "C#");

string result = sb.ToString();
```

---

## 6. Interview Questions

### Q1: Why are strings immutable?

**Answer:**
Strings are immutable for security, thread safety, and optimization (string interning). When you "modify" a string, a new string is created.

### Q2: What is string interning?

**Answer:**
The compiler stores string literals in a pool. Multiple references to the same literal point to the same memory location, saving memory.

### Q3: Difference between string and StringBuilder?

**Answer:**
- `string`: Immutable, creates new instance on modification
- `StringBuilder`: Mutable, modifies in place, better for frequent changes

---

## 📚 Additional Resources

- [String class](https://docs.microsoft.com/dotnet/api/system.string)

---

## ✅ Next Topic

Continue to: [Topic 09 - Structs vs Classes](./Topic_09_Structs_Classes.md)
