# Pathlib In Depth

## What You'll Learn

- Creating and navigating paths
- Reading and writing files
- Glob patterns
- Path information

## Prerequisites

- Read [08_sqlalchemy_orm.md](../../15_Python_for_Platforms/02_Python_and_Databases/03_sqlalchemy_orm.md) first

## Creating Paths

```python
# pathlib_basics.py

from pathlib import Path

# Create paths
p = Path(".")
print(p.absolute())

# Path components
path = Path("/home/user/project/file.txt")
print(path.name)      # file.txt
print(path.stem)      # file
print(path.suffix)    # .txt
print(path.parent)    # /home/user/project
```

## Reading and Writing Files

```python
# file_operations.py

from pathlib import Path

# Read entire file
content = Path("file.txt").read_text()

# Write file
Path("output.txt").write_text("Hello, World!")

# Read lines
lines = Path("file.txt").read_text().splitlines()

# JSON
import json
data = json.loads(Path("data.json").read_text())
Path("output.json").write_text(json.dumps(data, indent=2))
```

## Glob Patterns

```python
# glob_patterns.py

from pathlib import Path

# All Python files
py_files = list(Path(".").glob("*.py"))

# Recursive - all files
all_files = list(Path(".").rglob("*"))

# Recursive Python files
py_files_recursive = list(Path(".").rglob("**/*.py"))

# With pattern
for p in Path(".").glob("*.txt"):
    print(p)
```

## Annotated Full Example

```python
# pathlib_demo.py
"""Complete demonstration of pathlib."""

from pathlib import Path


def main() -> None:
    # Current directory
    cwd = Path.cwd()
    print(f"Current directory: {cwd}")
    
    # Create a test file
    test_file = Path("test.txt")
    test_file.write_text("Hello, World!")
    
    # Read and display
    print(f"Contents: {test_file.read_text()}")
    
    # List files
    for f in Path(".").glob("*.txt"):
        print(f"Found: {f}")
    
    # Clean up
    test_file.unlink()


if __name__ == "__main__":
    main()
```

## Summary

- Creating and navigating paths
- Reading and writing files
- Glob patterns

## Next Steps

Continue to **[02_shutil_and_os.md](./02_shutil_and_os.md)**
