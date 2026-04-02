# Iterators Protocol

## What You'll Learn

- __iter__ and __next__
- Build custom iterator class
- StopIteration

## Prerequisites

- Read [03_rich_for_cli.md](../06_Modules_and_Packages/03_Popular_Third_Party/03_rich_for_cli.md) first

## Iterator Protocol

```python
class Counter:
    def __init__(self, limit: int) -> None:
        self.limit = limit
        self.current = 0
    
    def __iter__(self):
        return self
    
    def __next__(self):
        if self.current >= self.limit:
            raise StopIteration
        self.current += 1
        return self.current - 1


for i in Counter(5):
    print(i)  # 0, 1, 2, 3, 4
```

## Summary

- **__iter__**: Returns iterator
- **__next__**: Returns next item
- **StopIteration**: Signals end

## Next Steps

Continue to **[02_generators.md](./02_generators.md)**
