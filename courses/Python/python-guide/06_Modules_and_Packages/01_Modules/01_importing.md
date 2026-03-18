# Importing

## What You'll Learn

- import statements
- from...import
- Aliases with as
- __name__ == "__main__" guard
- Relative vs absolute imports

## Prerequisites

- Read [03_descriptors.md](../05_OOP/03_Modern_OOP/03_descriptors.md) first

## Basic Import

```python
import math
print(math.sqrt(16))  # 4.0
```

## from...import

```python
from math import sqrt, pi
print(sqrt(16))  # 4.0
print(pi)  # 3.14159...
```

## Aliases

```python
import numpy as np
from typing import Optional as Opt
```

## __name__ == "__main__"

```python
# mymodule.py
def main():
    print("Running!")

if __name__ == "__main__":
    main()
```

## Summary

- **import module**: Import entire module
- **from module import name**: Import specific name
- **as**: Create alias
- **__name__ == "__main__"**: Guard for direct execution

## Next Steps

Continue to **[02_creating_modules.md](./02_creating_modules.md)**
