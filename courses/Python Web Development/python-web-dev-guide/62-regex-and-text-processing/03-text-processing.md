# Text Processing

## What You'll Learn

- String manipulation
- Text parsing
- Processing large text

## Prerequisites

- Completed `02-pattern-matching.md`

## String Methods

```python
# Split and join
text = "hello,world,test"
parts = text.split(",")  # ['hello', 'world', 'test']
joined = "-".join(parts)  # "hello-world-test"

# Strip whitespace
"  hello  ".strip()   # "hello"
"hello!".rstrip("!")  # "hello"

# Replace
"hello world".replace("world", "there")  # "hello there"

# Case conversion
"HELLO".lower()   # "hello"
"hello".upper()   # "HELLO"
"hello world".title()  # "Hello World"
"Hello World".swapcase()  # "hELLO wORLD"

# Check methods
"hello".startswith("hel")  # True
"hello".endswith("lo")    # True
"123".isdigit()           # True
"abc".isalpha()           # True
```

## F-strings

```python
name = "Alice"
age = 30

# Simple
f"Hello, {name}"  # "Hello, Alice"

# Expressions
f"In 5 years: {age + 5}"  # "In 5 years: 35"

# Formatting
f"Pi: {3.14159:.2f}"  # "Pi: 3.14"
f"Price: ${10:.2f}"   # "Price: $10.00"

# DateTime
from datetime import datetime
now = datetime.now()
f"Date: {now:%Y-%m-%d}"  # "Date: 2024-01-15"
```

## Text Parsing

```python
import re

def parse_log_line(line: str) -> dict:
    """Parse a log line."""
    pattern = r"(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2})\s+(\w+)\s+(.+)"
    match = re.match(pattern, line)
    
    if match:
        return {
            "date": match.group(1),
            "time": match.group(2),
            "level": match.group(3),
            "message": match.group(4)
        }
    return {}

# Parse CSV-like text
def parse_csv(text: str) -> list[dict]:
    """Parse CSV text."""
    lines = text.strip().split("\n")
    headers = lines[0].split(",")
    
    result = []
    for line in lines[1:]:
        values = line.split(",")
        result.append(dict(zip(headers, values)))
    
    return result
```

## Processing Large Text

```python
# Read file line by line
def process_large_file(filepath: str) -> None:
    with open(filepath, "r") as f:
        for line in f:
            # Process each line
            pass

# Using generator for memory efficiency
def read_lines(filepath: str):
    with open(filepath, "r") as f:
        for line in f:
            yield line.strip()

# Process in chunks
def process_in_chunks(filepath: str, chunk_size: int = 8192):
    with open(filepath, "r") as f:
        while True:
            chunk = f.read(chunk_size)
            if not chunk:
                break
            yield chunk
```

## Text Normalization

```python
import re

def normalize_text(text: str) -> str:
    """Normalize text for comparison."""
    # Lowercase
    text = text.lower()
    
    # Remove extra whitespace
    text = re.sub(r"\s+", " ", text)
    
    # Remove punctuation (keep alphanumeric)
    text = re.sub(r"[^\w\s]", "", text)
    
    return text.strip()

def remove_html(text: str) -> str:
    """Remove HTML tags."""
    return re.sub(r"<[^>]+>", "", text)

def extract_words(text: str) -> list[str]:
    """Extract words from text."""
    return re.findall(r"\b[a-zA-Z]+\b", text.lower())
```

## Summary

- Use string methods for basic manipulation
- Use regex for complex patterns
- Process large files with generators

## Next Steps

Continue to `04-input-validation.md`.
