# Scope and Namespaces

## What You'll Learn

- Understanding the LEGB rule
- Using global and nonlocal keywords
- Understanding Python's namespace system
- Avoiding common scope pitfalls

## Prerequisites

- Read [06_docstrings_and_annotations.md](./06_docstrings_and_annotations.md) first

## The LEGB Rule

Python resolves variable names using the LEGB rule: Local, Enclosing, Global, Built-in.

```python
# legb_rule.py

# Built-in scope
print(len([1, 2, 3]))  # len is built-in

# Global scope
global_var = "I'm global"

def outer():
    # Enclosing scope
    enclosing_var = "I'm enclosing"
    
    def inner():
        # Local scope
        local_var = "I'm local"
        print(enclosing_var)  # Found in enclosing
        print(global_var)    # Found in global
    
    inner()
```

## Global and Nonlocal

Use `global` to modify global variables, and `nonlocal` to modify enclosing scope variables.

```python
# global_nonlocal.py

counter = 0

def increment_global() -> None:
    """Increment the global counter."""
    global counter
    counter += 1


def outer():
    count = 0
    
    def inner():
        """Increment the enclosing count."""
        nonlocal count
        count += 1
        print(f"Inner count: {count}")
    
    inner()
    print(f"Outer count: {count}")
```

## Understanding Namespaces

A namespace is a dictionary that maps names to objects. Different namespaces exist simultaneously.

```python
# namespaces.py

# Each module has its own global namespace
import math
import builtins

# Access different namespaces
print(globals()["math"])  # Module namespace
print(dir(builtins))       # Built-in namespace
```

## Annotated Full Example

```python
# scope_demo.py
"""Demonstrates scope and namespace concepts."""

# Global variable
module_level = "I'm global"


def outer_function(text: str) -> str:
    """Outer function with enclosing scope."""
    enclosing_var = text.upper()
    
    def inner_function() -> str:
        """Inner function accessing enclosing scope."""
        nonlocal enclosing_var
        enclosing_var = enclosing_var + "!"
        return enclosing_var
    
    return inner_function()


def modify_global() -> None:
    """Demonstrate global keyword."""
    global module_level
    module_level = "Global was modified!"


def main() -> None:
    # Test LEGB rule
    print(f"Module level: {module_level}")
    
    # Test enclosing scope
    result = outer_function("hello")
    print(f"Outer result: {result}")
    
    # Test global modification
    modify_global()
    print(f"After modification: {module_level}")
    
    # Show namespace
    print(f"Local namespace: {locals().keys()}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding the LEGB rule
- Using global and nonlocal keywords
- Understanding Python's namespace system

## Next Steps

Continue to **[01_closures.md](../02_Advanced_Functions/01_closures.md)**
