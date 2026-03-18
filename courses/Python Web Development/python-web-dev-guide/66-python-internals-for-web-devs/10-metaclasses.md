# Metaclasses

## What You'll Learn

- Understanding metaclasses
- Creating metaclasses
- Use cases

## Prerequisites

- Completed `09-descriptors.md`

## Metaclasses

Metaclasses are classes that create classes.

```python
class Meta(type):
    def __new__(mcs, name, bases, namespace):
        cls = super().__new__(mcs, name, bases, namespace)
        cls.attribute = "value"
        return cls

class MyClass(metaclass=Meta):
    pass

print(MyClass.attribute)  # "value"
```

## Summary

- Metaclasses control class creation
- Rarely needed in practice

## Next Steps

This concludes the Python Internals folder. Continue to other topics in your learning journey.
