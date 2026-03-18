# functools Module

## What You'll Learn

- functools.lru_cache for memoization
- functools.cache (Python 3.9+)
- functools.partial for currying
- functools.cached_property

## Prerequisites

- Read [02_itertools.md](./02_itertools.md) first

## lru_cache — Memoization

```python
from functools import lru_cache

@lru_cache(maxsize=128)
def fibonacci(n: int) -> int:
    if n < 2:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

# Now fast!
print(fibonacci(100))  # 354224848179261915075
```

## cache — Simple Caching (Python 3.9+)

```python
from functools import cache

@cache
def expensive_computation(x: int) -> int:
    print(f"Computing for {x}...")
    return x * x

expensive_computation(5)  # Computes
expensive_computation(5)  # Cached!
```

## partial — Create Specialized Functions

```python
from functools import partial

def power(base: int, exponent: int) -> int:
    return base ** exponent

# Create specialized functions
square = partial(power, exponent=2)
cube = partial(power, exponent=3)

print(square(5))  # 25
print(cube(5))    # 125
```

## cached_property — Cached Property (Python 3.8+)

```python
from functools import cached_property

class Circle:
    def __init__(self, radius: float) -> None:
        self.radius = radius
    
    @cached_property
    def diameter(self) -> float:
        print("Computing diameter...")
        return self.radius * 2

c = Circle(5)
print(c.diameter)  # Computes once, then cached
print(c.diameter)  # Cached
```

## Summary

- **lru_cache**: Memoization with size limit
- **cache**: Simple infinite cache (Python 3.9+)
- **partial**: Create specialized functions
- **cached_property**: Cached version of @property

## Next Steps

Now let's move to **[04_Data_Structures/01_Built_In/01_lists.md](../04_Data_Structures/01_Built_In/01_lists.md)**
