# For Loops

## What You'll Learn

- How to use `for` loops with `range()`
- Iterating over lists
- Using `enumerate()` for index and value
- Using `zip()` to iterate multiple sequences
- Unpacking in for loops

## Prerequisites

- Read [03_truthiness_and_falsy.md](../01_Conditionals/03_truthiness_and_falsy.md) first

## What Is a For Loop?

A `for` loop repeats code for each item in a sequence. It's like saying "do this for each item."

```python
# Basic for loop
for i in range(5):
    print(i)  # Prints 0, 1, 2, 3, 4
```

## Using `range()`

The `range()` function generates a sequence of numbers:

### `range(stop)`

```python
# 0 to stop-1
for i in range(5):      # 0, 1, 2, 3, 4
    print(i)
```

### `range(start, stop)`

```python
# start to stop-1
for i in range(2, 6):   # 2, 3, 4, 5
    print(i)
```

### `range(start, stop, step)`

```python
# start to stop-1, incrementing by step
for i in range(0, 10, 2):  # 0, 2, 4, 6, 8
    print(i)

# Negative step (counting down)
for i in range(5, 0, -1):   # 5, 4, 3, 2, 1
    print(i)
```

## Iterating Over Lists

```python
# Iterate through a list
fruits: list[str] = ["apple", "banana", "cherry"]

for fruit in fruits:
    print(fruit)

# Output:
# apple
# banana
# cherry
```

## Using `enumerate()`

When you need both the index and value:

```python
fruits: list[str] = ["apple", "banana", "cherry"]

# enumerate gives (index, value) pairs
for index, fruit in enumerate(fruits):
    print(f"{index}: {fruit}")

# Output:
# 0: apple
# 1: banana
# 2: cherry
```

### Starting Index at 1

```python
fruits: list[str] = ["apple", "banana", "cherry"]

for index, fruit in enumerate(fruits, start=1):
    print(f"{index}: {fruit}")

# Output:
# 1: apple
# 2: banana
# 3: cherry
```

## Using `zip()`

Iterate over multiple sequences simultaneously:

```python
names: list[str] = ["Alice", "Bob", "Charlie"]
ages: list[int] = [25, 30, 35]

for name, age in zip(names, ages):
    print(f"{name} is {age} years old")

# Output:
# Alice is 25 years old
# Bob is 30 years old
# Charlie is 35 years old
```

## Unpacking in For Loops

```python
# List of tuples
pairs: list[tuple[int, int]] = [(1, 2), (3, 4), (5, 6)]

for a, b in pairs:
    print(f"a={a}, b={b}")

# Output:
# a=1, b=2
# a=3, b=4
# a=5, b=6


# List of dictionaries
users: list[dict[str, str]] = [
    {"name": "Alice", "city": "NYC"},
    {"name": "Bob", "city": "LA"}
]

for user in users:
    # Access dictionary values
    print(f"{user['name']} lives in {user['city']}")
```

## Python 3.12+ Improved Error Messages

Python 3.12 introduced better error messages for common loop mistakes:

```python
# This used to give confusing errors, now it's clearer
for i in range(5)
    print(i)
# SyntaxError: expected ':'

# Another example - iterating over non-iterable
x: int = 5
for i in x:  # TypeError in 3.12+ now says "int is not iterable"
    print(i)
```

## Annotated Example: Shopping Cart

```python
# shopping_cart.py
# Demonstrates various for loop patterns

def main() -> None:
    # Cart items as (name, price, quantity)
    cart: list[tuple[str, float, int]] = [
        ("Apple", 0.50, 3),
        ("Banana", 0.30, 2),
        ("Orange", 0.75, 4),
    ]
    
    print("=== Shopping Cart ===\n")
    
    # Method 1: Simple iteration with index
    print("--- Method 1: enumerate ---")
    for index, (item, price, qty) in enumerate(cart, start=1):
        total: float = price * qty
        print(f"{index}. {item}: ${price:.2f} x {qty} = ${total:.2f}")
    
    # Method 2: Calculate grand total
    print("\n--- Calculate Total ---")
    grand_total: float = 0.0
    
    # Unpack each tuple into item, price, qty
    for item, price, qty in cart:
        grand_total += price * qty
    
    print(f"Grand Total: ${grand_total:.2f}")
    
    # Method 3: Using range
    print("\n--- Using range ---")
    for i in range(len(cert)):  # Note: prefer enumerate in practice
        item, price, qty = cart[i]  # Unpack from list
        print(f"Item {i}: {item}")
    
    # Method 4: Generate indices and values together
    print("\n--- With enumerate and conditions ---")
    for index, (item, price, qty) in enumerate(cart):
        if qty > 2:  # Only show items with quantity > 2
            print(f"Bulk item: {item} (qty: {qty})")
    
    # Method 5: Building a new list (covered more in comprehensions)
    print("\n--- Filter and transform ---")
    expensive_items: list[str] = [
        item for item, price, qty in cart if price > 0.4
    ]
    print(f"Items over $0.40: {expensive_items}")


# Run the program
if __name__ == "__main__":
    main()
```

## Common Mistakes

### ❌ Modifying List While Iterating

```python
# WRONG - don't modify list while iterating
numbers: list[int] = [1, 2, 3, 4, 5]
for n in numbers:
    if n % 2 == 0:
        numbers.remove(n)  # Can cause unexpected behavior!
print(numbers)

# CORRECT - iterate over a copy
numbers: list[int] = [1, 2, 3, 4, 5]
for n in numbers.copy():  # Iterate over a copy
    if n % 2 == 0:
        numbers.remove(n)
print(numbers)

# Or use list comprehension (preferred!)
numbers: list[int] = [1, 2, 3, 4, 5]
numbers = [n for n in numbers if n % 2 != 0]  # Keep odd numbers
print(numbers)
```

### ❌ Using Wrong Variable Name

```python
# WRONG - typo in variable name
fruits: list[str] = ["apple", "banana"]
for fruit in fruites:  # Typo!
    print(fruit)  # NameError: name 'fruites' is not defined

# CORRECT
for fruit in fruits:
    print(fruit)
```

### ❌ Forgetting Colon

```python
# WRONG
for i in range(5)
    print(i)  # SyntaxError!

# CORRECT
for i in range(5):
    print(i)
```

## Summary

- **`for` loop**: Repeats for each item in a sequence
- **`range(n)`**: Generates 0 to n-1
- **`range(start, stop, step)`**: Custom start, stop, and step
- **`enumerate()`**: Get both index and value
- **`zip()`**: Iterate multiple sequences together
- **Unpacking**: `for a, b in list_of_tuples:` extracts values

## Next Steps

Now let's learn about **while loops** in **[02_while_loops.md](./02_while_loops.md)**.
