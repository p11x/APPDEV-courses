# The functools Module

## What You'll Learn

- Using functools.lru_cache for memoization
- Creating partial functions with partial
- Understanding functools.reduce
- Working with functools.wraps and other utilities

## Prerequisites

- Read [03_decorators.md](./03_decorators.md) first

## functools.lru_cache

The `@lru_cache` decorator caches function results to avoid recomputation.

```python
# lru_cache_demo.py

from functools import lru_cache


@lru_cache(maxsize=128)
def fibonacci(n: int) -> int:
    """Calculate nth Fibonacci number with caching."""
    if n < 2:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)

# First call - computed
result = fibonacci(100)  # Very fast due to caching!
print(f"Fibonacci(100): {result}")
print(f"Cache info: {fibonacci.cache_info()}")
```

## functools.partial

Create a new function with some arguments pre-filled.

```python
# partial_demo.py

from functools import partial


def power(base: int, exponent: int) -> int:
    """Raise base to exponent power."""
    return base ** exponent

# Create partial functions
square = partial(power, exponent=2)
cube = partial(power, exponent=3)

print(square(5))  # 25
print(cube(5))    # 125
```

## functools.reduce

Apply a function cumulatively to reduce a sequence to a single value.

```python
# reduce_demo.py

from functools import reduce


def multiply(x: int, y: int) -> int:
    """Multiply two numbers."""
    return x * y

numbers = [1, 2, 3, 4, 5]
result = reduce(multiply, numbers)
print(f"Product: {result}")  # 120
```

## Annotated Full Example

```python
# functools_demo.py
"""Complete demonstration of functools module utilities."""

from functools import lru_cache, partial, reduce, wraps


# Example 1: LRU Cache for expensive computation
@lru_cache(maxsize=100)
def expensive_computation(n: int) -> int:
    """Simulate expensive calculation."""
    print(f"Computing for {n}...")
    return n * n


# Example 2: Partial function for configuration
def create_logger(level: str, message: str, format_json: bool) -> str:
    """Create a formatted log message."""
    if format_json:
        return f'{{"level": "{level}", "message": "{message}"}}'
    return f"[{level}] {message}"


json_logger = partial(create_logger, format_json=True)
text_logger = partial(create_logger, format_json=False)


# Example 3: Reduce for aggregation
def combine_dicts(dict1: dict, dict2: dict) -> dict:
    """Merge two dictionaries, summing matching keys."""
    result = dict1.copy()
    for key, value in dict2.items():
        result[key] = result.get(key, 0) + value
    return result


def main() -> None:
    # Test lru_cache
    print("Testing lru_cache:")
    print(expensive_computation(10))  # Computed
    print(expensive_computation(10))  # Cached
    print(expensive_computation(20))   # Computed
    
    # Test partial functions
    print("\nTesting partial:")
    print(json_logger("INFO", "Application started"))
    print(text_logger("ERROR", "Connection failed"))
    
    # Test reduce
    print("\nTesting reduce:")
    sales = [{"apples": 10, "bananas": 5}, 
              {"apples": 15, "bananas": 3},
              {"apples": 8, "bananas": 7}]
    total = reduce(combine_dicts, sales)
    print(f"Total sales: {total}")


if __name__ == "__main__":
    main()
```

## Summary

- Using functools.lru_cache for memoization
- Creating partial functions with partial
- Understanding functools.reduce

## Next Steps

Continue to **[05_recursion_and_memoization.md](./05_recursion_and_memoization.md)**
