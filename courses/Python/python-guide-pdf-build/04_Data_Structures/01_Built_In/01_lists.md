# Lists

## What You'll Learn

- Creating lists
- Indexing and slicing
- List methods (append, extend, insert, pop, remove, sort, reverse)
- Using lists as stacks and queues

## Prerequisites

- Read [03_functools.md](../../03_Functions/03_Functional_Tools/03_functools.md) first

## Creating Lists

```python
# Empty list
empty: list = []

# With items
fruits: list[str] = ["apple", "banana", "cherry"]

# Mixed types
mixed: list = [1, "hello", True, 3.14]

# List from range
numbers: list[int] = list(range(5))  # [0, 1, 2, 3, 4]
```

## Indexing

```python
fruits: list[str] = ["apple", "banana", "cherry"]

# Access by index
first: str = fruits[0]   # "apple"
last: str = fruits[-1]   # "cherry"

# Negative indexing
second_last: str = fruits[-2]  # "banana"
```

## Slicing

```python
numbers: list[int] = [0, 1, 2, 3, 4, 5]

# Basic slicing
subset: list[int] = numbers[1:4]   # [1, 2, 3]
first_three: list[int] = numbers[:3]  # [0, 1, 2]
last_two: list[int] = numbers[-2:]  # [4, 5]

# With step
evens: list[int] = numbers[::2]  # [0, 2, 4]
reversed_list: list[int] = numbers[::-1]  # [5, 4, 3, 2, 1, 0]
```

## List Methods

```python
my_list: list[int] = [1, 2, 3]

# Add items
my_list.append(4)      # [1, 2, 3, 4]
my_list.extend([5, 6]) # [1, 2, 3, 4, 5, 6]
my_list.insert(0, 0)   # [0, 1, 2, 3, 4, 5, 6]

# Remove items
my_list.pop()         # Removes last: returns 6
my_list.remove(3)     # Removes first occurrence of 3

# Sort
my_list.sort()        # In-place sort
sorted_list: list[int] = sorted(my_list)  # Returns new sorted list

# Reverse
my_list.reverse()     # In-place reverse
```

## Annotated Example

```python
# list_demo.py

def main() -> None:
    # Create a list
    numbers: list[int] = [5, 2, 8, 1, 9]
    print(f"Original: {numbers}")
    
    # Sort
    numbers.sort()
    print(f"Sorted: {numbers}")
    
    # Reverse
    numbers.reverse()
    print(f"Reversed: {numbers}")
    
    # Append
    numbers.append(10)
    print(f"After append: {numbers}")
    
    # Insert at index
    numbers.insert(0, 0)
    print(f"After insert: {numbers}")
    
    # Pop
    popped: int = numbers.pop()
    print(f"Popped: {popped}, List: {numbers}")


if __name__ == "__main__":
    main()
```

## Summary

- Lists are ordered, mutable collections
- Index from 0, negative indexing from -1
- Use `.sort()` for in-place, `sorted()` for new list

## Next Steps

Continue to **[02_tuples_and_sets.md](./02_tuples_and_sets.md)**
