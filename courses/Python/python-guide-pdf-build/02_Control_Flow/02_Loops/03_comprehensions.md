# Comprehensions

## What You'll Learn

- List comprehensions
- Dictionary comprehensions
- Set comprehensions
- Generator expressions
- When to use comprehensions vs loops

## Prerequisites

- Read [02_while_loops.md](./02_while_loops.md) first

## What Are Comprehensions?

Comprehensions are a concise way to create collections. They're like **one-line loops** that build lists, dictionaries, or sets.

### Traditional Loop

```python
# Create a list of squares using a for loop
squares: list[int] = []

for i in range(10):
    squares.append(i ** 2)

print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
```

### List Comprehension

```python
# Same result, one line!
squares: list[int] = [i ** 2 for i in range(10)]

print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
```

## List Comprehension Syntax

```python
[expression for item in iterable if condition]
```

| Part | Description |
|------|-------------|
| `expression` | What to do with each item |
| `for item in iterable` | Loop through items |
| `if condition` | Optional filter |

### Examples

```python
# Basic - double each number
numbers: list[int] = [1, 2, 3, 4, 5]
doubled: list[int] = [n * 2 for n in numbers]
print(doubled)  # [2, 4, 6, 8, 10]

# With filter - only even numbers
numbers: list[int] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
evens: list[int] = [n for n in numbers if n % 2 == 0]
print(evens)  # [2, 4, 6, 8, 10]

# Transform strings
words: list[str] = ["hello", "world"]
upper_words: list[str] = [word.upper() for word in words]
print(upper_words)  # ["HELLO", "WORLD"]
```

## Dictionary Comprehension

Create dictionaries concisely:

```python
# Basic dictionary comprehension
squares_dict: dict[int, int] = {i: i ** 2 for i in range(5)}
print(squares_dict)  # {0: 0, 1: 1, 2: 4, 3: 9, 4: 16}

# From two lists
keys: list[str] = ["a", "b", "c"]
values: list[int] = [1, 2, 3]

combined: dict[str, int] = {k: v for k, v in zip(keys, values)}
print(combined)  # {"a": 1, "b": 2, "c": 3}

# With filter
numbers: list[int] = [1, 2, 3, 4, 5]
squares_over_5: dict[int, int] = {n: n ** 2 for n in numbers if n ** 2 > 5}
print(squares_over_5)  # {3: 9, 4: 16, 5: 25}
```

## Set Comprehension

Create sets (unique values):

```python
# Basic set comprehension
numbers: list[int] = [1, 2, 2, 3, 3, 3, 4]
unique_squares: set[int] = {n ** 2 for n in numbers}
print(unique_squares)  # {1, 4, 9, 16}

# With filter
text: str = "hello world"
unique_letters: set[str] = {char for char in text if char.isalpha()}
print(unique_letters)  # {'h', 'e', 'l', 'o', 'w', 'r', 'd'}
```

## Generator Expressions

Generators are **lazy** — they produce values one at a time, not all at once:

```python
# List comprehension - creates entire list in memory
squares_list: list[int] = [x ** 2 for x in range(1000000)]

# Generator expression - produces values on demand
squares_gen = (x ** 2 for x in range(1000000))  # Note parentheses!

# You can iterate through a generator
for square in squares_gen:
    print(square)
    if square > 100:
        break  # Only calculated what we needed
```

### Generator vs List Performance

```python
# When to use generator:
# - Working with large datasets
# - Only need first few items
# - Memory is a concern

# When to use list:
# - Need to use the data multiple times
# - Need random access (list[5])
# - Need to know the length
```

## Nested Comprehensions

You can nest comprehensions (use carefully!):

```python
# Matrix (list of lists)
matrix: list[list[int]] = [[i * j for j in range(3)] for i in range(3)]
print(matrix)
# [[0, 0, 0],
#  [0, 1, 2],
#  [0, 2, 4]]

# Flatten a matrix
flattened: list[int] = [num for row in matrix for num in row]
print(flattened)  # [0, 0, 0, 0, 1, 2, 0, 2, 4]

# ⚠️ WARNING: Avoid deeply nested comprehensions!
# They're hard to read. Use regular loops instead.
```

## Annotated Example: Data Processing

```python
# comprehensions_demo.py
# Demonstrates various comprehension types

from typing import Generator


def main() -> None:
    # --- List Comprehension ---
    print("=== List Comprehensions ===")
    
    # Create list of squares
    squares: list[int] = [x ** 2 for x in range(10)]
    print(f"Squares: {squares}")
    
    # Filter: only even squares
    even_squares: list[int] = [x for x in squares if x % 2 == 0]
    print(f"Even squares: {even_squares}")
    
    # Transform: extract names
    users: list[dict[str, str]] = [
        {"name": "Alice", "city": "NYC"},
        {"name": "Bob", "city": "LA"},
    ]
    names: list[str] = [user["name"] for user in users]
    print(f"Names: {names}")
    
    # --- Dictionary Comprehension ---
    print("\n=== Dictionary Comprehensions ===")
    
    # Create lookup from list
    items: list[str] = ["apple", "banana", "cherry"]
    item_lengths: dict[str, int] = {item: len(item) for item in items}
    print(f"Item lengths: {item_lengths}")
    
    # Filter dictionary
    scores: dict[str, int] = {"Alice": 95, "Bob": 72, "Charlie": 88, "Diana": 60}
    passing: dict[str, int] = {name: score for name, score in scores.items() if score >= 70}
    print(f"Passing scores: {passing}")
    
    # --- Set Comprehension ---
    print("\n=== Set Comprehensions ===")
    
    # Extract unique first letters
    words: list[str] = ["apple", "apricot", "banana", "blueberry"]
    first_letters: set[str] = {word[0] for word in words}
    print(f"First letters: {first_letters}")
    
    # --- Generator Expression ---
    print("\n=== Generator Expressions ===")
    
    # Create a generator (lazy evaluation)
    large_squares: Generator[int, None, None] = (x ** 2 for x in range(1000000))
    
    # Only compute what we need
    print("First 5 squares from generator:")
    for i, square in enumerate(large_squares):
        if i >= 5:
            break
        print(f"  {square}")
    
    # --- Nested Comprehension ---
    print("\n=== Nested Comprehension ===")
    
    # Create a multiplication table
    # 3x3 table
    mult_table: list[list[int]] = [[i * j for j in range(1, 4)] for i in range(1, 4)]
    
    # Print nicely
    for row in mult_table:
        print(row)
    
    # Flatten
    flat: list[int] = [num for row in mult_table for num in row]
    print(f"Flattened: {flat}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== List Comprehensions ===
Squares: [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
Even squares: [0, 4, 16, 36, 64]
Names: ['Alice', 'Bob']

=== Dictionary Comprehensions ===
Item lengths: {'apple': 5, 'banana': 6, 'cherry': 6}
Passing scores: {'Alice': 95, 'Bob': 72, 'Charlie': 88}

=== Set Comprehensions ===
First letters: {'a', 'b', 'c'}

=== Generator Expressions ===
First 5 squares from generator:
  0
  1
  4
  9
  16

=== Nested Comprehension ===
[1, 2, 3]
[2, 4, 6]
[3, 6, 9]
Flattened: [1, 2, 3, 2, 4, 6, 3, 6, 9]
```

## Common Mistakes

### ❌ List vs Generator Confusion

```python
# WRONG - creating list when you need a generator
large_data: list[int] = [x for x in range(10000000)]  # Uses lots of memory!

# CORRECT - use generator for large data
large_data = (x for x in range(10000000))  # Memory efficient
```

### ❌ Modifying in Place

```python
# WRONG - can't use comprehension to modify original list directly
numbers: list[int] = [1, 2, 3]
squares: list[int] = [n ** 2 for n in numbers]  # Creates NEW list
print(numbers)  # Still [1, 2, 3]!

# If you want to modify original, use a loop
numbers: list[int] = [1, 2, 3]
for i in range(len(numbers)):
    numbers[i] **= 2
print(numbers)  # [1, 4, 9]
```

## Summary

- **List comprehension**: `[expr for x in iterable if cond]`
- **Dictionary comprehension**: `{k: v for k, v in items if cond}`
- **Set comprehension**: `{x for x in iterable if cond}`
- **Generator**: `(x for x in iterable)` — lazy evaluation
- **Nested comprehensions**: Powerful but use carefully

## Next Steps

Now let's learn about **exception handling** in **[03_Exception_Handling/01_try_except_basics.md](../03_Exception_Handling/01_try_except_basics.md)**.
