# Descriptors

## What You'll Learn

- Understanding descriptors
- Property implementation
- Custom descriptors

## Prerequisites

- Completed `08-iterators-and-generators.md`

## Descriptors

Descriptors are classes that implement __get__, __set__, or __delete__.

```python
class Property:
    def __init__(self, fget=None, fset=None, fdel=None, doc=None):
        self.fget = fget
        self.fset = fset
        self.fdel = fdel
        self.doc = doc
    
    def __get__(self, obj, objtype=None):
        if obj is None:
            return self
        return self.fget(obj)
    
    def __set__(self, obj, value):
        self.fset(obj, value)
```

## Summary

- Descriptors implement property access
- @property uses descriptors

## Next Steps

Continue to `10-metaclasses.md`.
