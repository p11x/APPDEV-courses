# Generators

## What You'll Learn

- yield keyword
- Generator functions
- Generator expressions

## Prerequisites

- Read [01_iterators_protocol.md](./01_iterators_protocol.md) first

## Generator Functions

```python
def count_up_to(n: int):
    i = 1
    while i <= n:
        yield i
        i += 1


for i in count_up_to(5):
    print(i)  # 1, 2, 3, 4, 5
```

## Generator Expressions

```python
# Like list comprehension but lazy
gen = (x * 2 for x in range(5))
print(list(gen))  # [0, 2, 4, 6, 8]
```

## Summary

- **yield**: Pauses function, returns value
- **Generator**: Lazy evaluation
- **Memory efficient**: Doesn't create full list

## Next Steps

Continue to **[03_yield_from.md](./03_yield_from.md)**
