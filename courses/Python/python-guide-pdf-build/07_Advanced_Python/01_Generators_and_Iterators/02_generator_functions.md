# Generator Functions

## What You'll Learn

- Understanding yield
- Generator state
- Lazy evaluation
- Infinite generators

## Prerequisites

- Read [01_iterators_protocol.md](./01_iterators_protocol.md) first

## Understanding yield

Generator functions use yield to produce values lazily.

```python
# yield_basics.py

def count_up_to(n: int):
    """Generator that yields numbers from 1 to n."""
    current = 1
    while current <= n:
        yield current
        current += 1

gen = count_up_to(5)
print(next(gen))  # 1
print(next(gen))  # 2
print(list(gen))  # [3, 4, 5]
```

## Lazy Evaluation

Generators produce values on-demand, making them memory-efficient.

```python
# lazy_eval.py

def infinite_range(start: int = 0):
    """Generate numbers infinitely."""
    while True:
        yield start
        start += 1

def fibonacci_gen():
    """Generate Fibonacci numbers infinitely."""
    a, b = 0, 1
    while True:
        yield a
        a, b = b, a + b

# Memory efficient - doesn't create infinite list!
fib = fibonacci_gen()
for i in range(10):
    print(next(fib), end=" ")
```

## Annotated Full Example

```python
# generator_functions_demo.py
"""Complete demonstration of generator functions."""

from typing import Generator


def range_generator(start: int, stop: int, step: int = 1) -> Generator[int, None, None]:
    """Generate numbers in a range."""
    current = start
    while current < stop:
        yield current
        current += step


def running_total(numbers: list[int]) -> Generator[int, None, None]:
    """Yield running totals."""
    total = 0
    for num in numbers:
        total += num
        yield total


def main() -> None:
    print("Range generator:")
    for num in range_generator(0, 10, 2):
        print(num, end=" ")
    print()
    
    print("\nRunning total:")
    for total in running_total([1, 2, 3, 4, 5]):
        print(total, end=" ")
    print()


if __name__ == "__main__":
    main()
```

## Summary

- Understanding yield
- Generator state
- Lazy evaluation

## Next Steps

Continue to **[03_generator_expressions.md](./03_generator_expressions.md)**
