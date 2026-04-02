# Docstrings and Annotations

## What You'll Learn

- Writing clear docstrings in various styles
- Using type annotations for better code
- Accessing docstrings at runtime
- Generating documentation automatically

## Prerequisites

- Read [05_args_and_kwargs.md](./05_args_and_kwargs.md) first

## Writing Docstrings

Docstrings provide documentation for functions, classes, and modules. They're defined as the first statement in a function body.

```python
# docstring_basics.py

def calculate_area(radius: float) -> float:
    """Calculate the area of a circle.
    
    Args:
        radius: The radius of the circle in units.
        
    Returns:
        The area of the circle in square units.
    """
    import math
    return math.pi * radius ** 2
```

## Docstring Styles

There are several popular docstring styles: Google, NumPy, and reStructuredText.

```python
# docstring_styles.py

# Google Style
def google_style(param1: int, param2: str) -> bool:
    """Short description.
    
    Args:
        param1: Description of param1.
        param2: Description of param2.
        
    Returns:
        Description of return value.
    """
    pass

# NumPy Style
def numpy_style(param1: int, param2: str) -> bool:
    """
    Short description.
    
    Parameters
    ----------
    param1 : int
        Description of param1.
    param2 : str
        Description of param2.
        
    Returns
    -------
    bool
        Description of return value.
    """
    pass
```

## Type Annotations

Type hints make code more readable and enable static analysis tools.

```python
# type_annotations.py

from typing import Optional, Union


def greet(name: str, greeting: Optional[str] = None) -> str:
    """Greet a person with optional custom greeting."""
    prefix = greeting if greeting else "Hello"
    return f"{prefix}, {name}!"


def process_items(items: list[int], multiplier: float = 1.0) -> list[float]:
    """Process a list of integers with a multiplier."""
    return [item * multiplier for item in items]
```

## Annotated Full Example

```python
# docstrings_annotations_demo.py
"""Demonstrates docstrings and type annotations."""

import math
from typing import Optional


def calculate_circle_properties(radius: float) -> dict[str, float]:
    """Calculate various properties of a circle.
    
    Args:
        radius: The radius of the circle (must be positive).
        
    Returns:
        Dictionary containing area, circumference, and diameter.
        
    Raises:
        ValueError: If radius is negative.
    """
    if radius < 0:
        raise ValueError("Radius cannot be negative")
    
    return {
        "area": math.pi * radius ** 2,
        "circumference": 2 * math.pi * radius,
        "diameter": radius * 2
    }


class Circle:
    """Represents a circle with a given radius."""
    
    def __init__(self, radius: float) -> None:
        """Initialize circle with radius."""
        self.radius = radius
    
    @property
    def area(self) -> float:
        """Calculate the area of the circle."""
        return math.pi * self.radius ** 2
    
    def __str__(self) -> str:
        """Return string representation."""
        return f"Circle(radius={self.radius})"


def main() -> None:
    # Access docstrings at runtime
    print(f"calculate_circle_properties docstring:\n{calculate_circle_properties.__doc__}")
    
    # Use the function
    props = calculate_circle_properties(5)
    print(f"Properties: {props}")
    
    # Use the class
    circle = Circle(3)
    print(circle)
    print(f"Area: {circle.area}")


if __name__ == "__main__":
    main()
```

## Summary

- Writing clear docstrings in various styles
- Using type annotations for better code
- Accessing docstrings at runtime

## Next Steps

Continue to **[07_scope_and_namespaces.md](./07_scope_and_namespaces.md)**
