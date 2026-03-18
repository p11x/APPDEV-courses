# Standard Library Tour

## What You'll Learn

- Quick tour of useful stdlib modules

## Prerequisites

- Read [02_creating_modules.md](./02_creating_modules.md) first

## Useful Modules

```python
# os - operating system
import os
os.listdir(".")  # List files

# sys - system
import sys
sys.argv  # Command line args

# pathlib - file paths (modern)
from pathlib import Path
p = Path(".")
list(p.glob("*.py"))

# datetime - dates and times
from datetime import datetime
datetime.now()

# json - JSON handling
import json
json.dumps({"key": "value"})

# re - regular expressions
import re
re.match(r"\d+", "123")

# random - random numbers
import random
random.randint(1, 10)

# math - math functions
import math
math.sqrt(16)
```

## Summary

Python's "batteries included" philosophy provides rich modules for common tasks.

## Next Steps

Continue to **[06_Modules_and_Packages/02_Packages/01_package_structure.md](../02_Packages/01_package_structure.md)**
