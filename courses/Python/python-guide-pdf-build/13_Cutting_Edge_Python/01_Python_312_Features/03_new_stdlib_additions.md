# 📚 Python 3.12 Standard Library Additions

## 🎯 What You'll Learn

- Reading TOML config files natively with `tomllib`
- New pathlib features including `Path.walk()`
- F-string improvements: debugging with `=`, nested f-strings
- itertools.batched() for chunking iterables
- Understanding the new sys.monitoring API concept

## 📦 Prerequisites

- Familiarity with Python standard library modules (pathlib, itertools)
- Understanding of file handling in Python

---

## tomllib: Read TOML Files Natively

Python 3.11+ includes `tomllib` in the standard library — no third-party library needed!

### Basic TOML Reading

```python
import tomllib
from pathlib import Path

# Sample pyproject.toml content for demonstration
pyproject_content = """
[project]
name = "my-awesome-app"
version = "1.2.3"
authors = [{name = "Alice", email = "alice@example.com"}]

[project.optional-dependencies]
dev = ["pytest", "ruff"]

[tool.ruff]
line-length = 100
target-version = "py312"
"""

# Write sample file
config_file = Path("pyproject.toml")
config_file.write_text(pyproject_content)

# Read and parse TOML
with open(config_file, "rb") as f:  # Note: binary mode!
    config = tomllib.load(f)

# Access parsed data like a dictionary
print(config["project"]["name"])  # my-awesome-app
print(config["project"]["version"])  # 1.2.3
print(config["tool.ruff"]["line-length"])  # 100
```

### 💡 Line-by-Line Breakdown

```python
import tomllib                              # Built-in TOML parser (Python 3.11+)
from pathlib import Path

pyproject_content = """..."""              # Sample TOML content as a string
config_file = Path("pyproject.toml")        # Create Path object
config_file.write_text(pyproject_content)   # Write sample to file

with open(config_file, "rb") as f:          # Open in BINARY mode (required!)
    config = tomllib.load(f)                # Load and parse TOML into dict

print(config["project"]["name"])            # Access nested data: my-awesome-app
print(config["project"]["version"])         # Access nested data: 1.2.3
print(config["tool.ruff"]["line-length"]) # Access nested data: 100
```

### Type-Safe Access with TomlDict

```python
import tomllib
from pathlib import Path
from typing import Any

def load_config(path: Path) -> dict[str, Any]:
    """Load a TOML config file and return parsed contents."""
    with open(path, "rb") as f:
        return tomllib.load(f)

# Access with type hints (tomllib returns a special dict-like)
config = load_config(Path("pyproject.toml"))

# Check if a key exists
if "project" in config and "version" in config["project"]:
    name: str = config["project"]["name"]
    version: str = config["project"]["version"]
    print(f"Loaded {name} version {version}")
```

---

## pathlib Improvements: Path.walk()

Python 3.12 adds `Path.walk()` as a simpler alternative to `os.walk()`:

```python
from pathlib import Path

# Create sample directory structure for demonstration
root = Path("demo_folder")
(root / "docs").mkdir(parents=True, exist_ok=True)
(root / "docs" / "readme.txt").write_text("Readme content")
(root / "src").mkdir(parents=True, exist_ok=True)
(root / "src" / "main.py").write_text("print('hello')")
(root / "src" / "utils.py").write_text("# utilities")

# Walk through directories - simpler than os.walk()!
for dirpath, dirnames, filenames in root.walk():
    print(f"📁 {dirpath}")
    print(f"   📂 Subdirs: {dirnames}")
    print(f"   📄 Files: {filenames}")
    print()
```

### 💡 Line-by-Line Breakdown

```python
from pathlib import Path

root = Path("demo_folder")                               # Create root path
(root / "docs").mkdir(parents=True, exist_ok=True)      # Make demo directories
(root / "docs" / "readme.txt").write_text("Readme content")
(root / "src").mkdir(parents=True, exist_ok=True)
(root / "src" / "main.py").write_text("print('hello')")
(root / "src" / "utils.py").write_text("# utilities")

for dirpath, dirnames, filenames in root.walk():       # Iterate using pathlib
    print(f"📁 {dirpath}")                               # Current directory path
    print(f"   📂 Subdirs: {dirnames}")                  # List of subdirectories
    print(f"   📄 Files: {filenames}")                   # List of files in this dir
```

### Using walk() with Filtering

```python
from pathlib import Path

# Find all .py files in a directory tree
root = Path("my_project")

python_files = []
for dirpath, dirnames, filenames in root.walk():
    # Filter to only Python files
    for filename in filenames:
        if filename.endswith(".py"):
            python_files.append(dirpath / filename)

print("Found Python files:")
for f in python_files:
    print(f"  {f}")
```

---

## F-String Improvements: Debugging with `=`

Python 3.12 makes f-strings incredibly powerful for debugging:

### The `=` Debugging Trick

```python
name = "Alice"
age = 30
score = 95.5

# Before: Had to write both variable and its value
print(f"name = {name}, age = {age}, score = {score}")

# Python 3.12+: Just use = after the expression!
print(f"{name=}")      # name='Alice'
print(f"{age=}")       # age=30
print(f"{score=}")     # score=95.5

# Works with expressions too!
print(f"{name.upper()=}")  # name.upper()='ALICE'
print(f"{age * 2=}")       # age * 2=60
```

### 💡 Line-by-Line Breakdown

```python
name = "Alice"                    # Define some variables
age = 30
score = 95.5

print(f"{name=}")                 # Prints: name='Alice' - shows both name and value
print(f"{age=}")                  # Prints: age=30
print(f"{score=}")                # Prints: score=95.5

print(f"{name.upper()=}")         # Shows expression AND result: name.upper()='ALICE'
print(f"{age * 2=}")              # Shows computation: age * 2=60
```

### Nested F-Strings (Python 3.12+)

```python
outer = "hello"
inner = "world"

# Nested f-strings now fully supported!
result = f"{outer} {inner}"
print(result)  # hello world

# Even more complex nesting
template = "The {adjective} brown {noun}"
adjective = "quick"
noun = "fox"

formatted = f"The {adjective} brown {noun}"
print(formatted)  # The quick brown fox
```

---

## itertools.batched()

Python 3.12 adds `itertools.batched()` to chunk iterables into fixed-size groups:

```python
import itertools

# Sample data - a list of items to process in batches
items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Batch into groups of 3
batches = list(itertools.batched(items, 3))

print(batches)
# Output: [(1, 2, 3), (4, 5, 6), (7, 8, 9), (10,)]

# Perfect for processing in chunks!
def process_items(items: list[int]) -> int:
    """Process a batch of items and return the sum."""
    print(f"Processing: {items}")
    return sum(items)

# Process each batch
total = sum(process_items(batch) for batch in itertools.batched(range(17), 5))
print(f"Total: {total}")
```

### 💡 Line-by-Line Breakdown

```python
import itertools

items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]  # Sample list

batches = list(itertools.batched(items, 3))  # Chunk into groups of 3

print(batches)  # [(1, 2, 3), (4, 5, 6), (7, 8, 9), (10,)] - last batch is smaller

# Process each batch
def process_items(items: list[int]) -> int:
    print(f"Processing: {items}")  # Show what we're processing
    return sum(items)

total = sum(process_items(batch) for batch in itertools.batched(range(17), 5))
print(f"Total: {total}")
```

### Use Cases for batched()

- **API calls**: Send items in groups to respect rate limits
- **Database inserts**: Batch records for bulk inserts
- **File processing**: Process files in chunks to manage memory

---

## Other Notable Additions

### datetime.fromisoformat() Improvements

```python
from datetime import datetime

# Now handles more ISO 8601 formats!
dt1 = datetime.fromisoformat("2024-01-15T10:30:00")  # Standard format
dt2 = datetime.fromisoformat("2024-01-15T10:30:00+05:30")  # With timezone
dt3 = datetime.fromisoformat("2024-01-15")  # Date only

print(dt1, dt2, dt3)
```

### calendar Module Improvements

```python
import calendar

# New locale-aware month names
print(calendar.month_name[1])   # January
print(calendar.month_abbr[1])  # Jan

# Generate calendar with custom settings
cal = calendar.TextCalendar()
print(cal.formatmonth(2024, 1))
```

### sys.monitoring: New Monitoring API (Concept)

Python 3.12 introduces a new low-overhead monitoring API (`sys.monitoring`) that's still evolving. It allows tools to track code execution without the overhead of traditional profiling.

```python
# This is a conceptual example - API is still evolving
# import sys.monitoring as sm

# Create a monitoring scope
# scope = sm.start()

# Register event handlers
# sm.register_callback(sm.Events.CALL, my_call_handler)

# Your code runs with monitoring
# do_work()

# sm.stop(scope)
```

---

## ✅ Summary

- `tomllib` provides native TOML parsing without third-party dependencies
- `Path.walk()` simplifies directory traversal compared to `os.walk()`
- F-string `=` syntax makes debugging output incredibly easy
- `itertools.batched()` chunks iterables into fixed-size groups efficiently
- Various datetime/calendar improvements enhance date handling

## ➡️ Next Steps

Continue to [../02_Python_313_Preview/01_free_threaded_mode.md](../02_Python_313_Preview/01_free_threaded_mode.md) to learn about Python 3.13's revolutionary free-threaded mode that removes the GIL.

## 🔗 Further Reading

- [PEP 680: tomllib — Support for parsing TOML](https://peps.python.org/pep-0680/)
- [What's New in Python 3.12: F-strings](https://docs.python.org/3.12/whatsnew/3.12.html#f-strings-support-for-self-documenting-expressions-and-debugging)
- [itertools.batched()](https://docs.python.org/3.12/library/itertools.html#itertools.batched)
- [pathlib — Object-oriented filesystem paths](https://docs.python.org/3.12/library/pathlib.html)
