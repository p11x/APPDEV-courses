# Importing Modules

## What You'll Learn

- Basic import statements
- From...import syntax
- Aliases with as
- Common import pitfalls

## Prerequisites

- Read [07_class_and_static_methods.md](../../05_OOP/01_Classes_Basics/07_class_and_static_methods.md) first

## Basic Imports

```python
# basic_imports.py

# Import entire module
import math
print(math.pi)
print(math.sqrt(16))

# Import specific items
from math import pi, sqrt
print(pi)
print(sqrt(16))

# Import with alias
import numpy as np
print(np.array([1, 2, 3]))
```

## From...import

```python
# from_import.py

# Import multiple items
from collections import deque, defaultdict, namedtuple

# Import all (generally discouraged)
# from module import *  # Bad practice!

# Import with alias
from typing import Optional as Opt
from pathlib import Path as P
```

## Annotated Full Example

```python
# importing_demo.py
"""Demonstrates various import patterns."""

# Standard library imports
import json
from pathlib import Path

# Third-party imports (if available)
# import requests
# from fastapi import FastAPI

# Local imports
# from . import local_module


def read_json_file(path: str) -> dict:
    """Read and parse a JSON file."""
    with open(path, "r") as f:
        return json.load(f)


def main() -> None:
    # Using imports
    data = {"name": "Alice", "age": 30}
    json_str = json.dumps(data)
    print(f"JSON: {json_str}")
    
    # Using Path
    print(f"Current dir: {Path.cwd()}")


if __name__ == "__main__":
    main()
```

## Summary

- Basic import statements
- From...import syntax
- Aliases with as

## Next Steps

Continue to **[02_the_module_system.md](./02_the_module_system.md)**
