# Lambda Functions

## What You'll Learn

- Lambda syntax and usage
- When to use lambdas vs regular functions
- Common uses with sorted(), map(), filter()
- When NOT to use lambdas

## Prerequisites

- Read [03_type_hints_in_functions.md](../01_Defining_Functions/03_type_hints_in_functions.md) first

## What Is a Lambda?

A lambda is an **anonymous function** — a function without a name:

```python
# Regular function
def add(a: int, b: int) -> int:
    return a + b


# Lambda equivalent
add_lambda = lambda a, b: a + b

# Both work the same way!
result: int = add(5, 3)        # 8
result = add_lambda(5, 3)        # 8
```

### Syntax

```python
lambda parameters: expression
```

- `lambda` — keyword to create lambda
- `parameters` — like function parameters (optional)
- `expression` — single expression that returns a value

## When to Use Lambda

### 1. With sorted()

```python
# Sort by name length
names: list[str] = ["Alice", "Bob", "Charlie", "Diana"]

# Using lambda as key function
sorted_names: list[str] = sorted(names, key=lambda x: len(x))
print(sorted_names)  # ['Bob', 'Alice', 'Diana', 'Charlie']

# Sort by last character
sorted_names = sorted(names, key=lambda x: x[-1])
print(sorted_names)  # ['Alice', 'Bob', 'Diana', 'Charlie']
```

### 2. With map()

```python
# Double all numbers
numbers: list[int] = [1, 2, 3, 4, 5]

# Using lambda with map
doubled: list[int] = list(map(lambda x: x * 2, numbers))
print(doubled)  # [2, 4, 6, 8, 10]
```

### 3. With filter()

```python
# Keep only even numbers
numbers: list[int] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Using lambda with filter
evens: list[int] = list(filter(lambda x: x % 2 == 0, numbers))
print(evens)  # [2, 4, 6, 8, 10]
```

## When NOT to Use Lambda

### ❌ Complex Logic

```python
# WRONG - too complex for lambda
result = lambda x: [i for i in range(x) if i % 2 == 0 and (i % 3 == 0 or i % 5 == 0)]

# CORRECT - use a regular function
def complex_logic(n: int) -> list[int]:
    return [
        i for i in range(n)
        if i % 2 == 0 and (i % 3 == 0 or i % 5 == 0)
    ]
```

### ❌ Multi-statement

```python
# WRONG - lambdas can't have multiple statements
result = lambda x: if x > 0: return x else: return -x  # SyntaxError!

# CORRECT - use regular function
def abs_value(x: int) -> int:
    if x > 0:
        return x
    return -x
```

### ❌ Reusable Functions

```python
# WRONG - lambdas aren't reusable by name
result = lambda x: x ** 2  # No good way to call this again!

# CORRECT - use a regular function
def square(x: int) -> int:
    return x ** 2
```

## Annotated Example: Data Processing

```python
# lambda_demo.py
# Demonstrates lambda functions in practical scenarios

from typing import Any


def main() -> None:
    # --- Basic Lambda ---
    print("=== Basic Lambda ===")
    
    # Simple lambda
    add: Callable[[int, int], int] = lambda a, b: a + b
    print(f"add(5, 3) = {add(5, 3)}")
    
    # Lambda with no parameters
    get_random: Callable[[], int] = lambda: 42
    print(f"get_random() = {get_random()}")
    
    # --- Lambda with sorted() ---
    print("\n=== Lambda with sorted() ===")
    
    # Data: list of dictionaries
    users: list[dict[str, Any]] = [
        {"name": "Alice", "age": 25},
        {"name": "Bob", "age": 30},
        {"name": "Charlie", "age": 20},
    ]
    
    # Sort by age
    by_age: list[dict[str, Any]] = sorted(users, key=lambda u: u["age"])
    print("Sorted by age:")
    for user in by_age:
        print(f"  {user['name']}: {user['age']}")
    
    # Sort by name length
    by_name_len: list[dict[str, Any]] = sorted(users, key=lambda u: len(u["name"]))
    print("Sorted by name length:")
    for user in by_name_len:
        print(f"  {user['name']} ({len(user['name'])} chars)")
    
    # --- Lambda with map() ---
    print("\n=== Lambda with map() ===")
    
    prices: list[float] = [10.99, 25.50, 5.00, 15.75]
    
    # Apply 10% discount
    discounted: list[float] = list(map(lambda p: p * 0.9, prices))
    print(f"Prices after 10% off: {[f'${p:.2f}' for p in discounted]}")
    
    # --- Lambda with filter() ---
    print("\n=== Lambda with filter() ===")
    
    numbers: list[int] = list(range(1, 21))
    
    # Filter: multiples of 3
    multiples_of_3: list[int] = list(filter(lambda x: x % 3 == 0, numbers))
    print(f"Multiples of 3: {multiples_of_3}")
    
    # Filter: numbers greater than 10
    greater_than_10: list[int] = list(filter(lambda x: x > 10, numbers))
    print(f"Greater than 10: {greater_than_10}")
    
    # --- Lambda with max() ---
    print("\n=== Lambda with max() ===")
    
    # Find longest word
    words: list[str] = ["cat", "elephant", "dog", "bird"]
    longest: str = max(words, key=lambda w: len(w))
    print(f"Longest word: {longest}")
    
    # --- Combining Lambda with Comprehensions ---
    print("\n=== Lambda Alternative (Comprehensions) ===")
    
    # Map equivalent
    doubled: list[int] = [x * 2 for x in range(5)]
    print(f"Doubled: {doubled}")
    
    # Filter equivalent
    evens: list[int] = [x for x in range(10) if x % 2 == 0]
    print(f"Evens: {evens}")
    
    # --- Complex Example ---
    print("\n=== Complex Example ===")
    
    # Data transformation pipeline
    data: list[int] = list(range(1, 11))
    
    # Filter evens, square them, get first 3
    result: list[int] = list(
        map(
            lambda x: x ** 2,
            filter(lambda x: x % 2 == 0, data)
        )
    )[:3]
    
    print(f"First 3 squared evens: {result}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Basic Lambda ===
add(5, 3) = 8
get_random() = 42

=== Lambda with sorted() ===
Sorted by age:
  Charlie: 20
  Alice: 25
  Bob: 30
Sorted by name length:
  Bob (3 chars)
  Alice (5 chars)
  Charlie (7 chars)

=== Lambda with map() ===
Prices after 10% off: ['$9.89', '$22.95', '$4.50', '$14.18']

=== Lambda with filter() ===
Multiples of 3: [3, 6, 9, 12, 15, 18]
Greater than 10: [11, 12, 13, 14, 15, 16, 17, 18, 19, 20]

=== Lambda with max() ===
Longest word: elephant

=== Lambda Alternative (Comprehensions) ===
Doubled: [0, 2, 4, 6, 8]
Evens: [0, 2, 4, 6, 8]

=== Complex Example ===
First 3 squared evens: [4, 16, 36]
```

## Summary

- **Lambda**: Anonymous function: `lambda x: x * 2`
- **Use with**: `sorted()`, `map()`, `filter()`, `max()`, `min()`
- **Don't use**: Complex logic, multiple statements, reusable functions

## Next Steps

Now let's learn about **closures and scope** in **[02_closures_and_scope.md](./02_closures_and_scope.md)**.
