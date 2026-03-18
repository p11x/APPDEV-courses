# Descriptors

## What You'll Learn

- __get__, __set__, __delete__
- Data vs non-data descriptors
- Validated descriptor example

## Prerequisites

- Read [02_slots_and_performance.md](./02_slots_and_performance.md) first

## Creating Descriptors

```python
class Validated:
    def __set_name__(self, owner: type, name: str) -> None:
        self.name = name
    
    def __get__(self, obj: object, objtype: type | None = None) -> int:
        return getattr(obj, f"_{self.name}", 0)
    
    def __set__(self, obj: object, value: int) -> None:
        if value < 0:
            raise ValueError("Must be positive!")
        obj[f"_{self.name}"] = value


class Counter:
    count = Validated()


c = Counter()
c.count = 10
print(c.count)  # 10
c.count = -5  # Raises ValueError!
```

## Summary

- **Descriptors**: Objects that control attribute access
- **__get__**: Get value
- **__set__**: Set value
- **__delete__**: Delete value

## Next Steps

This concludes OOP. Move to **[06_Modules_and_Packages/01_Modules/01_importing.md](../06_Modules_and_Packages/01_Modules/01_importing.md)**
