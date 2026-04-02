# The Module System

## What You'll Learn

- Understanding sys.modules
- Module objects and attributes
- The __file__ attribute
- Module caching

## Prerequisites

- Read [01_importing_modules.md](./01_importing_modules.md) first

## sys.modules

Python caches imported modules in sys.modules.

```python
# module_system.py

import sys
import math

# Check if module is cached
print("math" in sys.modules)  # True

# List all loaded modules
print(list(sys.modules.keys())[:10])

# Module attributes
print(math.__name__)
print(math.__file__)
print(math.__doc__[:50])
```

## The __name__ Attribute

Every module has a __name__ that identifies how it was loaded.

```python
# name_attribute.py

# When run directly: __name__ is "__main__"
# When imported: __name__ is the module name

print(f"This module is named: {__name__}")

if __name__ == "__main__":
    print("This code runs when the file is executed directly")
else:
    print("This code runs when the file is imported")
```

## Annotated Full Example

```python
# module_system_demo.py
"""Complete demonstration of module system."""

import sys
import os


def show_module_info(module_name: str) -> None:
    """Show information about a module."""
    if module_name in sys.modules:
        mod = sys.modules[module_name]
        print(f"Module: {mod.__name__}")
        if hasattr(mod, "__file__"):
            print(f"  File: {mod.__file__}")
        print(f"  Loaded: True")
    else:
        print(f"Module {module_name} not loaded")


def main() -> None:
    # Check built-in modules
    import math
    import json
    
    show_module_info("math")
    show_module_info("json")
    
    # Current module info
    print(f"\nCurrent module: {__name__}")
    print(f"File: {__file__}")
    print(f"Package: {__package__}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding sys.modules
- Module objects and attributes
- The __file__ attribute

## Next Steps

Continue to **[03_main_guard.md](./03_main_guard.md)**
