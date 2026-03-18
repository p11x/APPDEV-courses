# Iterators and Generators

## What You'll Learn

- Iterator protocol
- Generator functions
- Memory efficiency

## Prerequisites

- Completed `07-data-model.md`

## Iterator Protocol

```python
class Counter:
    def __init__(self, max: int):
        self.current = 0
        self.max = max
    
    def __iter__(self):
        return self
    
    def __next__(self) -> int:
        if self.current >= self.max:
            raise StopIteration
        self.current += 1
        return self.current - 1
```

## Generators

```python
def count_to(n: int):
    for i in range(n):
        yield i

for num in count_to(5):
    print(num)
```

## Summary

- Generators are memory-efficient
- Use yield to create generators

## Next Steps

Continue to `09-descriptors.md`.
