# 📖 Topic 03: Generics

## 1. Concept Explanation

**Generics** allow you to create reusable, type-safe code without committing to a specific data type.

---

## 2. Generic Class

```csharp
public class Box<T>
{
    public T Content { get; set; }
    
    public void Display()
    {
        Console.WriteLine($"Content: {Content}");
    }
}

// Usage
Box<int> intBox = new Box<int> { Content = 42 };
Box<string> strBox = new Box<string> { Content = "Hello" };
```

---

## 3. Generic Method

```csharp
public static void Swap<T>(ref T a, ref T b)
{
    T temp = a;
    a = b;
    b = temp;
}

// Usage
int x = 1, y = 2;
Swap(ref x, ref y);
```

---

## 4. Generic Constraints

```csharp
// Class constraint
public class Repository<T> where T : class { }

// Interface constraint
public class Service<T> where T : IComparable { }

// New constraint
public class Factory<T> where T : new() { }
```

---

## 5. Interview Questions

### Q1: What are generics?

**Answer:**
A feature that allows writing code that works with any data type while maintaining type safety at compile time.

---

## ✅ Next Topic

Continue to: [Topic 04 - Extension Methods](./Topic_04_Extension_Methods.md)
