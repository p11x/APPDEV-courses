# 📖 Topic 03: Access Modifiers

## 1. Concept Explanation

Access modifiers control the visibility of types and members.

---

## 2. Access Modifier Types

| Modifier | Description |
|----------|-------------|
| `public` | Accessible everywhere |
| `private` | Accessible only in declaring class |
| `protected` | Accessible in declaring class and derived classes |
| `internal` | Accessible within the same assembly |
| `protected internal` | Accessible in same assembly OR derived classes |
| `private protected` | Accessible in same assembly AND derived classes (C# 7.2+) |

---

## 3. Examples

```csharp
public class MyClass
{
    public int PublicField;      // Accessible everywhere
    private int PrivateField;    // Only in MyClass
    protected int ProtectedField; // MyClass + derived classes
    internal int InternalField;  // Same assembly only
    
    // Property with controlled access
    private string _name;
    public string Name
    {
        get { return _name; }
        private set { _name = value; }  // Only accessible within class
    }
}
```

---

## ✅ Next Topic

Continue to: [Topic 04 - Encapsulation](./Topic_04_Encapsulation.md)
