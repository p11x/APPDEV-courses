# 📖 Topic 10: Enums and Nullable Types

## 1. Concept Explanation

### Enums (Enumerations)

An **enum** is a distinct type consisting of a set of named constants. Enums make code more readable and maintainable.

```csharp
// Basic enum
public enum Day
{
    Monday,    // 0
    Tuesday,   // 1
    Wednesday, // 2
    Thursday,  // 3
    Friday,    // 4
    Saturday,  // 5
    Sunday     // 6
}

// Enum with explicit values
public enum Status
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}

// Enum with underlying type
public enum ByteValue : byte
{
    Low = 100,
    Medium = 200,
    High = 300
}
```

---

## 2. Enum Usage

```csharp
Day today = Day.Monday;

Console.WriteLine(today);           // Monday
Console.WriteLine((int)today);      // 0

// Parse from string
Day parsed = Enum.Parse<Day>("Tuesday");

// Convert to string
string dayString = today.ToString();

// Switch on enum
switch (today)
{
    case Day.Monday:
    case Day.Tuesday:
    case Day.Wednesday:
    case Day.Thursday:
    case Day.Friday:
        Console.WriteLine("Weekday");
        break;
    case Day.Saturday:
    case Day.Sunday:
        Console.WriteLine("Weekend");
        break;
}
```

---

## 3. Flags Enum

Use `[Flags]` attribute for bitwise operations:

```csharp
[Flags]
public enum Permissions
{
    None = 0,
    Read = 1,      // 0001
    Write = 2,     // 0010
    Execute = 4,   // 0100
    Delete = 8     // 1000
}

// Usage
Permissions perms = Permissions.Read | Permissions.Write;
Console.WriteLine(perms);  // Read, Write

bool hasWrite = perms.HasFlag(Permissions.Write);  // true
```

---

## 4. Nullable Types

Nullable types allow value types to have a null value.

```csharp
// Nullable value type
int? nullableInt = null;
double? nullableDouble = 3.14;
bool? nullableBool = null;

// Null coalescing
int value = nullableInt ?? 0;

// Null conditional
string? name = null;
int? length = name?.Length;  // null

// Null coalescing assignment (C# 8+)
nullableInt ??= 10;
```

---

## 5. Interview Questions

### Q1: What is an enum?

**Answer:**
An enum is a distinct type that represents a set of named constant values. It makes code more readable than using magic numbers.

### Q2: What is the default underlying type of enum?

**Answer:**
`int` by default. Can be changed using `enum Name : underlyingType`.

### Q3: What are nullable value types?

**Answer:**
Nullable value types (C# 2+) add null capability to value types using `Nullable<T>` or the shorthand `T?`.

---

## 📚 Additional Resources

- [Enums](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/enums)
- [Nullable types](https://docs.microsoft.com/dotnet/csharp/language-reference/nullable-types)

---

## ✅ End of Module

Return to: [README](./README.md)
