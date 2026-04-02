# Closures and Scope

## What You'll Learn

- The LEGB rule (Local, Enclosing, Global, Built-in)
- The `nonlocal` keyword
- Creating closures (functions that remember their environment)
- Real example: a counter factory

## Prerequisites

- Read [01_lambda_functions.md](./01_lambda_functions.md) first

## Understanding Scope

Scope determines where variables are accessible in your code:

```
┌─────────────────────────────────────────────┐
│              SCOPE HIERARCHY                 │
├─────────────────────────────────────────────┤
│                                             │
│   Built-in   ← Python's built-in names       │
│       │                                      │
│   Global    ← Module-level variables         │
│       │                                      │
│   Enclosing← Outer function (for closures)   │
│       │                                      │
│   Local    ← Current function                │
│                                             │
└─────────────────────────────────────────────┘
```

## The LEGB Rule

Python looks for variables in this order:

1. **L**ocal — current function
2. **E**nclosing — outer functions
3. **G**lobal — module level
4. **B**uilt-in — Python's built-ins

### Examples

```python
# Global variable
x: int = 10


def test() -> None:
    # Local variable
    x: int = 20
    print(x)  # Prints 20 (local)


test()
print(x)  # Prints 10 (global)
```

## Closures

A closure is a function that "remembers" variables from its enclosing scope:

```python
def make_adder(n: int) -> Callable[[int], int]:
    """Create a function that adds n to its input."""
    
    def adder(x: int) -> int:
        return x + n  # 'n' comes from the enclosing scope!
    
    return adder


# Create adders
add_5: Callable[[int], int] = make_adder(5)
add_10: Callable[[int], int] = make_adder(10)

# Use them
print(add_5(3))    # 8
print(add_10(3))   # 13
```

## The nonlocal Keyword

Use `nonlocal` to modify variables from an enclosing scope:

```python
def counter() -> Callable[[], int]:
    """Create a counter that increments each call."""
    count: int = 0
    
    def increment() -> int:
        nonlocal count  # Allows modification of 'count'
        count += 1
        return count
    
    return increment


# Create and use counter
c: Callable[[], int] = counter()
print(c())  # 1
print(c())  # 2
print(c())  # 3
```

## Annotated Example: Counter Factory

```python
# closure_demo.py
# Demonstrates closures and scope

from typing import Callable


def make_counter() -> Callable[[], int]:
    """Create a counter function.
    
    Returns:
        A function that increments and returns a count
    """
    # This variable is in the enclosing scope
    count: int = 0
    
    def increment() -> int:
        # nonlocal allows us to modify 'count' from enclosing scope
        nonlocal count
        count += 1
        return count
    
    return increment


def make_multiplier(factor: int) -> Callable[[int], int]:
    """Create a function that multiplies by factor.
    
    Args:
        factor: The multiplier
    
    Returns:
        A function that multiplies by factor
    """
    def multiply(x: int) -> int:
        # 'factor' is captured from the enclosing scope
        return x * factor
    
    return multiply


def make_logger(name: str) -> Callable[[str], None]:
    """Create a logger with a custom name.
    
    Args:
        name: Logger name
    
    Returns:
        A logging function
    """
    def log(message: str) -> None:
        # 'name' is captured from the enclosing scope
        print(f"[{name}] {message}")
    
    return log


def main() -> None:
    # Example 1: Counter
    print("=== Counter ===")
    counter_a: Callable[[], int] = make_counter()
    counter_b: Callable[[], int] = make_counter()
    
    print(f"counter_a: {counter_a()}")  # 1
    print(f"counter_a: {counter_a()}")  # 2
    print(f"counter_b: {counter_b()}")  # 1 (separate counter!)
    
    # Example 2: Multiplier
    print("\n=== Multiplier ===")
    double: Callable[[int], int] = make_multiplier(2)
    triple: Callable[[int], int] = make_multiplier(3)
    
    print(f"double(5) = {double(5)}")   # 10
    print(f"triple(5) = {triple(5)}")  # 15
    print(f"double(10) = {double(10)}")  # 20
    
    # Example 3: Logger
    print("\n=== Logger ===")
    error_log: Callable[[str], None] = make_logger("ERROR")
    debug_log: Callable[[str], None] = make_logger("DEBUG")
    
    error_log("Something went wrong")  # [ERROR] Something went wrong
    debug_log("Debug info here")       # [DEBUG] Debug info here
    
    # Example 4: Multiple closures
    print("\n=== Multiple Closures ===")
    multipliers: list[Callable[[int], int]] = [
        make_multiplier(i) for i in range(1, 4)
    ]
    
    for i, func in enumerate(multipliers):
        print(f"multiply by {i+1}: {func(5)}")
    
    # Example 5: Demonstrating scope
    print("\n=== Scope Demonstration ===")
    
    x: int = "global"  # Global variable
    
    def outer() -> None:
        x: str = "enclosing"  # Enclosing variable
        
        def inner() -> None:
            x: str = "local"  # Local variable
            print(f"Inner: x = {x}")  # local
        
        inner()
        print(f"Outer: x = {x}")  # enclosing
    
    outer()
    print(f"Global: x = {x}")  # global


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
=== Counter ===
counter_a: 1
counter_a: 2
counter_b: 1

=== Multiplier ===
double(5) = 10
triple(5) = 15
double(10) = 20

=== Logger ===
[ERROR] Something went wrong
[DEBUG] Debug info here

=== Multiple Closures ===
multiply by 1: 5
multiply by 2: 10
multiply by 3: 15

=== Scope Demonstration ===
Inner: x = local
Outer: x = enclosing
Global: x = global
```

## Common Mistakes

### ❌ Forgetting nonlocal

```python
# WRONG - trying to modify without nonlocal
def counter() -> Callable[[], int]:
    count: int = 0
    
    def increment() -> int:
        count += 1  # UnboundLocalError!
        return count
    
    return increment

# CORRECT - use nonlocal
def counter() -> Callable[[], int]:
    count: int = 0
    
    def increment() -> int:
        nonlocal count  # Now it works!
        count += 1
        return count
    
    return increment
```

### ❌ Modifying Global Variables

```python
# WRONG - modifying global without declaration
x: int = 10

def modify() -> None:
    x = 20  # Creates local variable, doesn't modify global

# CORRECT - use global keyword
def modify() -> None:
    global x
    x = 20
```

## Summary

- **LEGB Rule**: Local → Enclosing → Global → Built-in
- **Closure**: Function that remembers variables from enclosing scope
- **`nonlocal`**: Modify variables from enclosing (non-global) scope
- **`global`**: Modify variables from global scope

## Next Steps

Now let's learn about **decorators** in **[03_decorators.md](./03_decorators.md)**.
