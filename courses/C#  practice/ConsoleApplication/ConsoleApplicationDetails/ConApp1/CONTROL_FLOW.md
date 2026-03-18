# ConApp1 - Control Flow

This document covers control flow statements taught in ConApp1.

## Table of Contents
1. [Decision Making](#decision-making)
2. [Loops](#loops)
3. [Jump Statements](#jump-statements)

---

## Decision Making

### If-Else Statements

```csharp
// Simple if
if (condition)
{
    // Execute if true
}

// If-else
if (condition)
{
    // Execute if true
}
else
{
    // Execute if false
}

// Nested if-else
if (condition1)
{
    // Execute if condition1 is true
}
else if (condition2)
{
    // Execute if condition2 is true
}
else
{
    // Execute if all conditions are false
}
```

**Source:** [Class5.cs](../../ConsoleApplication/ConApp1/Class5.cs)

### Switch Statement

```csharp
switch (variable)
{
    case value1:
        // Code for value1
        break;
    case value2:
        // Code for value2
        break;
    default:
        // Default code
        break;
}
```

### Switch Expression (Modern C#)

```csharp
string result = variable switch
{
    value1 => "Result 1",
    value2 => "Result 2",
    _ => "Default"
};
```

---

## Loops

### For Loop

```csharp
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

// Nested for loop
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.WriteLine($"i={i}, j={j}");
    }
}
```

**Source:** [Class7.cs](../../ConsoleApplication/ConApp1/Class7.cs)

### While Loop

```csharp
while (condition)
{
    // Execute while condition is true
}

// Example: Print 1 to n
int i = 1;
while (i <= 10)
{
    Console.WriteLine(i);
    i++;
}
```

### Do-While Loop

```csharp
do
{
    // Execute at least once
} while (condition);
```

**Source:** [Class8.cs](../../ConsoleApplication/ConApp1/Class8.cs)

### Foreach Loop

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

foreach (int num in numbers)
{
    Console.WriteLine(num);
}

// Iterate through string
string name = "Sathesh";
foreach (char c in name)
{
    Console.WriteLine(c);
}
```

---

## Jump Statements

### Break

```csharp
for (int i = 0; i < 10; i++)
{
    if (i == 5)
        break;  // Exit loop
    Console.WriteLine(i);
}
```

### Continue

```csharp
for (int i = 0; i < 10; i++)
{
    if (i % 2 == 0)
        continue;  // Skip even numbers
    Console.WriteLine(i);
}
```

### Return

```csharp
public int Add(int a, int b)
{
    return a + b;  // Return value and exit method
}
```

---

## Loop Control Patterns

### Find Sum of Digits

```csharp
int num = 1234;
int sum = 0;

while (num > 0)
{
    int digit = num % 10;
    sum += digit;
    num = num / 10;
}

Console.WriteLine("Sum: " + sum);  // Output: 10
```

### Reverse Number

```csharp
int num = 123;
int reverse = 0;

while (num > 0)
{
    int digit = num % 10;
    reverse = reverse * 10 + digit;
    num = num / 10;
}

Console.WriteLine("Reverse: " + reverse);  // Output: 321
```

### Check Prime Number

```csharp
int num = 17;
bool isPrime = true;

for (int i = 2; i <= num / 2; i++)
{
    if (num % i == 0)
    {
        isPrime = false;
        break;
    }
}

Console.WriteLine(isPrime ? "Prime" : "Not Prime");
```

---

## Best Practices

1. **Avoid deeply nested if-else** - Consider switch or polymorphism
2. **Use meaningful variable names** - Improves readability
3. **Prefer foreach** - For iterating collections
4. **Be careful with break/continue** - Can make code hard to follow

---

## Key Takeaways

| Statement | Purpose |
|-----------|---------|
| if/else | Conditional execution |
| switch | Multiple case selection |
| for | Fixed iteration |
| while | Condition-based iteration |
| do-while | At-least-once iteration |
| foreach | Collection iteration |
| break | Exit loop/switch |
| continue | Skip iteration |

---

*Last Updated: 2026-03-11*