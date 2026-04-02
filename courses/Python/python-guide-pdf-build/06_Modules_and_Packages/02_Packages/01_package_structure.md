# Package Structure

## What You'll Learn

- What makes a folder a package
- __init__.py
- Namespace packages

## Prerequisites

- Read [03_standard_library_tour.md](./03_standard_library_tour.md) first

## Package Structure

```
mypackage/
├── __init__.py
├── module1.py
└── module2.py
```

## __init__.py

Makes a folder a Python package:

```python
# __init__.py
from .module1 import func1
from .module2 import func2
```

## Namespace Packages (Python 3.3+)

No __init__.py needed:

```
namespacepkg/
module1.py
module2.py
```

## Summary

- Package = folder with __init__.py
- Import using dot notation: package.module

## Next Steps

Continue to **[02_pip_and_venv.md](./02_pip_and_venv.md)**
