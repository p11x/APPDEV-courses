# pathlib Deep Dive

## What You'll Learn

- Path() for file paths
- Path operations
- Reading/writing files

## Prerequisites

- Read [01_requests_and_httpx.md](./01_requests_and_httpx.md) first

## Basic Usage

```python
from pathlib import Path

p = Path(".")

# List files
list(p.glob("*.py"))

# Check existence
p.exists()

# Is file or directory
p.is_file()
p.is_dir()

# Read/write
p.read_text()
p.write_text("content")
```

## Path Operations

```python
from pathlib import Path

# Join paths
p = Path("folder") / "file.txt"

# Parent
p.parent

# Name
p.name  # "file.txt"
p.stem  # "file"
p.suffix  # ".txt"
```

## Summary

- **pathlib**: Modern path handling
- Replaces os.path

## Next Steps

Continue to **[03_rich_for_cli.md](./03_rich_for_cli.md)**
