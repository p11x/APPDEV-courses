# 📖 Topic 07: Abstract Classes

## 1. Concept Explanation

An **abstract class** cannot be instantiated directly. It serves as a base class and may contain abstract methods (without implementation).

---

## 2. Abstract Class Example

```csharp
public abstract class Shape
{
    // Abstract property
    public abstract double Area { get; }
    
    // Abstract method
    public abstract void Draw();
    
    // Concrete method
    public void Display()
    {
        Console.WriteLine($"Area: {Area}");
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double Area => Math.PI * Radius * Radius;
    
    public override void Draw()
    {
        Console.WriteLine("Drawing circle");
    }
}
```

---

## 3. Interview Questions

### Q1: What is an abstract class?

**Answer:**
A class that cannot be instantiated and may contain abstract members (methods without implementation). Used as a base for derived classes.

---

## ✅ Next Topic

Continue to: [Topic 08 - Interfaces](./Topic_08_Interfaces.md)
