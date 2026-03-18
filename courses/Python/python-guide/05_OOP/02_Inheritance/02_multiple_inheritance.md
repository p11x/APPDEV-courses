# Multiple Inheritance

## What You'll Learn

- MRO (Method Resolution Order)
- C3 linearization
- Mixins pattern
- __mro__ attribute

## Prerequisites

- Read [01_inheritance_basics.md](./01_inheritance_basics.md) first

## MRO

Python determines method resolution order:

```python
class A:
    def greet(self) -> str:
        return "A"

class B(A):
    def greet(self) -> str:
        return "B"

class C(A):
    def greet(self) -> str:
        return "C"

class D(B, C):  # Multiple inheritance
    pass

d = D()
print(d.greet())  # B (B comes before C in MRO)
print(D.__mro__)  # Show full MRO
```

## Mixins

```python
class LoggerMixin:
    def log(self, message: str) -> None:
        print(f"LOG: {message}")

class Dog(LoggerMixin):
    def bark(self) -> str:
        self.log("Barking!")
        return "Woof!"
```

## Summary

- **MRO**: Order Python looks for methods
- **Mixins**: Add functionality via multiple inheritance
- Check with `ClassName.__mro__`

## Next Steps

Continue to **[03_abstract_classes.md](./03_abstract_classes.md)**
