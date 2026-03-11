# 📖 Topic 08: Interfaces

## 1. Concept Explanation

An **interface** defines a contract. Classes that implement an interface must provide implementations for all members.

---

## 2. Interface Example

```csharp
public interface IAnimal
{
    string Name { get; set; }
    void Speak();
}

public class Dog : IAnimal
{
    public string Name { get; set; }
    
    public void Speak()
    {
        Console.WriteLine($"{Name} barks");
    }
}
```

---

## 3. Interface vs Abstract Class

| Feature | Interface | Abstract Class |
|---------|-----------|----------------|
| Inheritance | Multiple allowed | Single only |
| Methods | No implementation | Can have both |
| Fields | Not allowed | Allowed |
| Constructors | Not allowed | Allowed |

---

## 4. Interview Questions

### Q1: What is an interface?

**Answer:**
A contract that defines a set of methods, properties, or events without implementation. Classes must implement all members.

### Q2: Can C# support multiple interfaces?

**Answer:**
Yes! A class can implement multiple interfaces: `class Dog : IAnimal, IComparable`

---

## ✅ End of Module

Return to: [README](./README.md)
