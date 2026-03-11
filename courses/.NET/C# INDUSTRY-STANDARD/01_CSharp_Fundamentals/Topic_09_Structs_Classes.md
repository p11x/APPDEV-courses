# 📖 Topic 09: Structs vs Classes

## 1. Concept Explanation

In C#, both structs and classes are used to create custom data types, but they have fundamental differences in memory management and behavior.

### Key Differences

```
┌─────────────────────────────────────────────────────────────────┐
│                  STRUCT vs CLASS                                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────────┐    ┌──────────────────────┐          │
│  │       STRUCT        │    │       CLASS          │          │
│  │    (Value Type)     │    │   (Reference Type)  │          │
│  ├──────────────────────┤    ├──────────────────────┤          │
│  │ Stored on Stack      │    │ Stored on Heap       │          │
│  │                     │    │                      │          │
│  │ Copy on assignment  │    │ Copy reference       │          │
│  │                     │    │                      │          │
│  │ No inheritance      │    │ Can inherit          │          │
│  │                     │    │                      │          │
│  │ Default constructor │    │ Can have destructor  │          │
│  │ always runs         │    │                      │          │
│  └──────────────────────┘    └──────────────────────┘          │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. Struct Example

```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public void Display()
    {
        Console.WriteLine($"({X}, {Y})");
    }
}

// Usage
Point p1 = new Point(10, 20);
Point p2 = p1;  // Creates a COPY

p2.X = 30;

Console.WriteLine(p1.X);  // 10 (unchanged - different copy)
Console.WriteLine(p2.X);  // 30
```

---

## 3. Class Example

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// Usage
Person p1 = new Person("John", 30);
Person p2 = p1;  // Both point to SAME object

p2.Name = "Jane";

Console.WriteLine(p1.Name);  // Jane (both reference same object)
Console.WriteLine(p2.Name);  // Jane
```

---

## 4. When to Use Each

### Use Struct When:
- Small data containers (< 16 bytes typically)
- Data-centric, no polymorphism needed
- Immutable data
- Frequent creation/destruction

### Use Class When:
- Complex objects with behavior
- Need inheritance
- Reference semantics needed
- Larger objects

---

## 5. Interview Questions

### Q1: What is the main difference between struct and class?

**Answer:**
- **Struct**: Value type, stored on stack, copied on assignment
- **Class**: Reference type, stored on heap, reference copied on assignment

### Q2: Can structs have constructors?

**Answer:**
Yes, but parameterless constructors are not allowed in structs (C# 10+ allows it).

### Q3: Do structs support inheritance?

**Answer:**
No, structs cannot inherit from classes or other structs. They implicitly inherit from `System.ValueType`.

---

## 📚 Additional Resources

- [Structs](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/structs)

---

## ✅ Next Topic

Continue to: [Topic 10 - Enums and Nullables](./Topic_10_Enums_Nullables.md)
