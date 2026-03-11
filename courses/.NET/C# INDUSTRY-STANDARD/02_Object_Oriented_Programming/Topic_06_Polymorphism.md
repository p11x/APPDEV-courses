# 📖 Topic 06: Polymorphism

## 1. Concept Explanation

**Polymorphism** allows one interface to be used for different data types. In C#, this is achieved through method overriding.

---

## 2. Method Overriding

```csharp
public class Animal
{
    public virtual void Speak()
    {
        Console.WriteLine("Animal speaks");
    }
}

public class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Dog barks");
    }
}

public class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Cat meows");
    }
}
```

---

## 3. Runtime Polymorphism

```csharp
Animal animal = new Dog();
animal.Speak();  // Outputs: Dog barks

animal = new Cat();
animal.Speak();  // Outputs: Cat meows
```

---

## 4. Interview Questions

### Q1: What is polymorphism?

**Answer:**
The ability of objects to take on many forms. In C#, achieved through method overriding (runtime) and method overloading (compile-time).

### Q2: Difference between method overloading and overriding?

**Answer:**
- **Overloading**: Same method name, different parameters (compile-time)
- **Overriding**: Replaces base class method (runtime)

---

## ✅ Next Topic

Continue to: [Topic 07 - Abstract Classes](./Topic_07_Abstract_Classes.md)
