# 📖 Topic 05: Control Flow Statements

## 1. Concept Explanation

Control flow statements determine the order in which your code executes. They allow you to make decisions, repeat actions, and control program flow.

### Types of Control Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    CONTROL FLOW CATEGORIES                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────┐    ┌──────────────────┐                 │
│  │ DECISION MAKING  │    │     LOOPING      │                 │
│  │                  │    │                  │                 │
│  │ • if-else       │    │ • for            │                 │
│  │ • switch        │    │ • foreach        │                 │
│  │ • ternary       │    │ • while          │                 │
│  └──────────────────┘    │ • do-while      │                 │
│                          └──────────────────┘                 │
│                                                                  │
│  ┌──────────────────┐    ┌──────────────────┐                 │
│  │     JUMP         │    │    EXCEPTION     │                 │
│  │                  │    │     HANDLING     │                 │
│  │ • break         │    │                  │                 │
│  │ • continue     │    │ • try-catch      │                 │
│  │ • return       │    │ • throw          │                 │
│  │ • goto         │    │ • finally        │                 │
│  └──────────────────┘    └──────────────────┘                 │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. If-Else Statement

### Syntax

```csharp
if (condition)
{
    // code executed if condition is true
}
else if (anotherCondition)
{
    // code executed if anotherCondition is true
}
else
{
    // code executed if no conditions are true
}
```

### Example

```csharp
int score = 85;

if (score >= 90)
{
    Console.WriteLine("Grade: A");
}
else if (score >= 80)
{
    Console.WriteLine("Grade: B");
}
else if (score >= 70)
{
    Console.WriteLine("Grade: C");
}
else
{
    Console.WriteLine("Grade: F");
}
```

---

## 3. Switch Statement

### Traditional Switch

```csharp
int day = 3;
string dayName;

switch (day)
{
    case 1:
        dayName = "Monday";
        break;
    case 2:
        dayName = "Tuesday";
        break;
    case 3:
        dayName = "Wednesday";
        break;
    default:
        dayName = "Unknown";
        break;
}
```

### Switch Expression (C# 8+)

```csharp
int day = 3;
string dayName = day switch
{
    1 => "Monday",
    2 => "Tuesday",
    3 => "Wednesday",
    4 => "Thursday",
    5 => "Friday",
    6 or 7 => "Weekend",
    _ => "Unknown"
};
```

---

## 4. Loops

### For Loop

```csharp
// Classic for loop
for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"Iteration: {i}");
}

// For with multiple variables
for (int i = 0, j = 10; i < j; i++, j--)
{
    Console.WriteLine($"i={i}, j={j}");
}
```

### Foreach Loop

```csharp
string[] fruits = { "Apple", "Banana", "Cherry" };

foreach (string fruit in fruits)
{
    Console.WriteLine(fruit);
}
```

### While Loop

```csharp
int count = 0;

while (count < 5)
{
    Console.WriteLine(count);
    count++;
}
```

### Do-While Loop

```csharp
int number;
do
{
    Console.WriteLine("Enter a number (0 to exit): ");
    number = int.Parse(Console.ReadLine());
} while (number != 0);
```

---

## 5. Jump Statements

```csharp
// Break - exits loop
for (int i = 0; i < 10; i++)
{
    if (i == 5) break;
    Console.WriteLine(i);
}

// Continue - skip iteration
for (int i = 0; i < 5; i++)
{
    if (i == 2) continue;
    Console.WriteLine(i);
}

// Return - exit method
// Goto - jump to label (avoid in modern code)
```

---

## 6. Interview Questions

### Q1: What is the difference between while and do-while?

**Answer:**
- `while`: Checks condition first, might not execute at all
- `do-while`: Checks condition after execution, always executes at least once

### Q2: What is the difference between break and continue?

**Answer:**
- `break`: Exits the loop entirely
- `continue`: Skips current iteration and continues with next

### Q3: Can you use switch expressions with multiple values?

**Answer:**
Yes, use `or` to combine values:
```csharp
string dayType = day switch
{
    6 or 7 => "Weekend",
    _ => "Weekday"
};
```

---

## 📚 Additional Resources

- [Control Flow in C#](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/statements)

---

## ✅ Next Topic

Continue to: [Topic 06 - Methods](./Topic_06_Methods.md)
