# The Main Guard

## What You'll Learn

- Understanding if __name__ == "__main__"
- Script vs module behavior
- Common patterns
- Entry points

## Prerequisites

- Read [02_the_module_system.md](./02_the_module_system.md) first

## The Main Guard

The main guard prevents code from running when a module is imported.

```python
# main_guard.py

def main() -> None:
    print("Running main function!")
    # Your code here


if __name__ == "__main__":
    main()
```

## Script vs Module Behavior

```python
# script_vs_module.py

def process():
    print("Processing data...")

print("This runs on import!")

if __name__ == "__main__":
    print("This runs when executed directly!")
    process()
else:
    print("This module was imported")
```

## Annotated Full Example

```python
# main_guard_demo.py
"""Demonstrates the main guard pattern."""

from typing import List


def validate_data(data: List[int]) -> bool:
    """Validate input data."""
    return len(data) > 0 and all(isinstance(x, int) for x in data)


def process(data: List[int]) -> int:
    """Process data and return sum."""
    return sum(data)


def main() -> None:
    """Main entry point."""
    data = [1, 2, 3, 4, 5]
    
    if validate_data(data):
        result = process(data)
        print(f"Result: {result}")
    else:
        print("Invalid data")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding if __name__ == "__main__"
- Script vs module behavior
- Common patterns

## Next Steps

Continue to **[04_relative_imports.md](./04_relative_imports.md)**
