# ConApp1 - Pattern Printing

This document covers pattern printing examples taught in ConApp1.

## Table of Contents
1. [Number Patterns](#number-patterns)
2. [Star Patterns](#star-patterns)
3. [Alphabet Patterns](#alphabet-patterns)
4. [Advanced Patterns](#advanced-patterns)

---

## Number Patterns

### Pattern 1: Simple Numbers

```
1
12
123
1234
12345
```

**Source:** [Class9.cs](../../ConsoleApplication/ConApp1/Class9.cs)

```csharp
for (int i = 1; i <= 5; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write(j);
    }
    Console.WriteLine();
}
```

### Pattern 2: Repeated Numbers

```
1
22
333
4444
55555
```

```csharp
for (int i = 1; i <= 5; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write(i);
    }
    Console.WriteLine();
}
```

### Pattern 3: Pyramid

```
    1
   212
  32123
 4321234
543212345
```

**Source:** [Class14.cs](../../ConsoleApplication/ConApp1/Class14.cs)

```csharp
for (int i = 1; i <= 5; i++)
{
    // Spaces
    for (int j = 5; j > i; j--)
    {
        Console.Write(" ");
    }
    
    // Left side descending
    for (int k = i; k >= 1; k--)
    {
        Console.Write(k);
    }
    
    // Right side ascending
    for (int l = 2; l <= i; l++)
    {
        Console.Write(l);
    }
    
    Console.WriteLine();
}
```

---

## Star Patterns

### Star Triangle 1

```
*
**
***
****
*****
```

```csharp
for (int i = 1; i <= 5; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write("*");
    }
    Console.WriteLine();
}
```

### Star Triangle 2 (Inverted)

```
*****
****
***
**
*
```

```csharp
for (int i = 5; i >= 1; i--)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write("*");
    }
    Console.WriteLine();
}
```

### Star Pyramid

```
    *
   ***
  *****
 *******
*********
```

**Source:** [Class12.cs](../../ConsoleApplication/ConApp1/Class12.cs)

```csharp
for (int i = 1; i <= 5; i++)
{
    // Spaces
    for (int j = 5; j > i; j--)
    {
        Console.Write(" ");
    }
    
    // Stars
    for (int k = 1; k <= (2 * i - 1); k++)
    {
        Console.Write("*");
    }
    
    Console.WriteLine();
}
```

### Diamond Pattern

```
    *
   ***
  *****
 *******
*********
 *******
  *****
   ***
    *
```

```csharp
// Top half
for (int i = 1; i <= 5; i++)
{
    for (int j = 5; j > i; j--)
        Console.Write(" ");
    for (int k = 1; k <= (2 * i - 1); k++)
        Console.Write("*");
    Console.WriteLine();
}

// Bottom half
for (int i = 4; i >= 1; i--)
{
    for (int j = 5; j > i; j--)
        Console.Write(" ");
    for (int k = 1; k <= (2 * i - 1); k++)
        Console.Write("*");
    Console.WriteLine();
}
```

---

## Alphabet Patterns

### Alphabet Triangle

```
A
AB
ABC
ABCD
ABCDE
```

```csharp
for (int i = 1; i <= 5; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write((char)('A' + j - 1));
    }
    Console.WriteLine();
}
```

### Alphabet Pyramid

```
    A
   ABA
  ABCBA
 ABCDCBA
ABCDEDCBA
```

**Source:** [Class15.cs](../../ConsoleApplication/ConApp1/Class15.cs)

```csharp
int letter = 65;

for (int i = 0; i < 5; i++)
{
    // Spaces
    for (int j = 0; j < 5 - i - 1; j++)
    {
        Console.Write(" ");
    }
    
    // Left side ascending
    for (int j = 0; j <= i; j++)
    {
        Console.Write((char)(letter + j));
    }
    
    // Right side descending
    for (int j = i - 1; j >= 0; j--)
    {
        Console.Write((char)(letter + j));
    }
    
    Console.WriteLine();
}
```

---

## Advanced Patterns

### Floyd's Triangle

```
1
2 3
4 5 6
7 8 9 10
```

**Source:** [Class10.cs](../../ConsoleApplication/ConApp1/Class10.cs)

```csharp
int num = 1;
for (int i = 1; i <= 4; i++)
{
    for (int j = 1; j <= i; j++)
    {
        Console.Write(num + " ");
        num++;
    }
    Console.WriteLine();
}
```

### Pascal's Triangle

```
       1
      1 1
     1 2 1
    1 3 3 1
   1 4 6 4 1
```

```csharp
for (int i = 0; i < 5; i++)
{
    int val = 1;
    
    // Spaces
    for (int j = 0; j < 5 - i; j++)
    {
        Console.Write(" ");
    }
    
    for (int j = 0; j <= i; j++)
    {
        Console.Write(val + " ");
        val = val * (i - j) / (j + 1);
    }
    
    Console.WriteLine();
}
```

### Number Diamond

```
    1
   121
  12321
 1234321
123454321
 1234321
  12321
   121
    1
```

**Source:** [Class16.cs](../../ConsoleApplication/ConApp1/Class16.cs)

---

## Key Takeaways

### Pattern Printing Tips

1. **Analyze the structure** - Count rows and columns
2. **Break into parts** - Spaces, left side, right side
3. **Use nested loops** - Outer for rows, inner for columns
4. **Print by logic** - Ascending/descending sequences

### Common Patterns Summary

| Pattern Type | Key Technique |
|--------------|---------------|
| Triangle | Inner loop goes to `i` |
| Inverted | Inner loop goes to `n-i+1` |
| Pyramid | Add space loop + 2*i-1 stars |
| Diamond | Combine pyramid + inverted pyramid |

---

*Last Updated: 2026-03-11*