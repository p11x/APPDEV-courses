# String Manipulation Techniques

## What You'll Learn

- Advanced string operations
- Text formatting
- String building patterns

## Prerequisites

- Completed `07-log-parsing.md`

## String Formatting

```python
# f-strings with advanced formatting
name = "Alice"
age = 30
price = 19.99

# Multiple expressions
f"{name} is {age} years old"  # "Alice is 30 years old"

# Formatting numbers
f"Price: ${price:.2f}"        # "Price: $19.99"
f"Binary: {age:b}"            # "Binary: 11110"
f"Hex: {age:x}"               # "Hex: 1e"

# Padding and alignment
f"{name:>10}"                # "     Alice" (right align)
f"{name:<10}"                # "Alice     " (left align)
f"{name:^10}"                # "  Alice   " (center)
f"{age:05d}"                 # "00030" (zero-padded)

# DateTime formatting
from datetime import datetime
now = datetime.now()
f"{now:%Y-%m-%d}"            # "2024-01-15"
f"{now:%H:%M:%S}"            # "14:30:00"
```

## String Building Patterns

```python
# Using join (efficient)
parts = ["a", "b", "c"]
result = ", ".join(parts)    # "a, b, c"

# Using list + join for complex building
parts = []
for i in range(5):
    parts.append(f"item {i}")
result = "\n".join(parts)

# Using generator expression with join
words = ["hello", "world"]
result = " ".join(w.upper() for w in words)  # "HELLO WORLD"
```

## Slicing Operations

```python
text = "Hello, World!"

# Basic slicing
text[0:5]     # "Hello"
text[7:]      # "World!"
text[:5]      # "Hello"
text[::2]     # "Hlo,Wrd" (every 2nd char)
text[::-1]    # "!dlroW ,olleH" (reversed)

# Extract parts
filename = "document.pdf"
extension = filename[filename.rfind("."):]  # ".pdf"
name = filename[:filename.rfind(".")]       # "document"
```

## Common Transformations

```python
import re

# Remove duplicates
def remove_duplicates(text: str) -> str:
    seen = set()
    result = []
    for char in text:
        if char not in seen:
            seen.add(char)
            result.append(char)
    return "".join(result)

# Capitalize words
text = "hello world"
text.title()                  # "Hello World"

# Swap case
text.swapcase()               # "HELLO WORLD"

# Strip HTML tags
def strip_tags(text: str) -> str:
    return re.sub(r'<[^>]+>', '', text)

# Remove extra whitespace
def normalize_whitespace(text: str) -> str:
    return re.sub(r'\s+', ' ', text).strip()
```

## Template Strings

```python
from string import Template

# Simple template
template = Template("Hello, $name!")
result = template.substitute(name="Alice")  # "Hello, Alice!"

# With dictionary
data = {"name": "Bob", "age": "25"}
template = Template("Name: $name, Age: $age")
result = template.substitute(data)  # "Name: Bob, Age: 25"

# Safe substitution (leave placeholders)
template = Template("Hello, $name!")
result = template.safe_substitute(name="Alice")  # "Hello, Alice!"
```

## Unicode and Encoding

```python
# Encode to bytes
text = "Hello, World!"
encoded = text.encode("utf-8")
print(encoded)  # b'Hello, World!'

# Decode from bytes
decoded = encoded.decode("utf-8")
print(decoded)  # "Hello, World!"

# Handle encoding errors
text = "Hello, 世界"
encoded = text.encode("utf-8", errors="replace")
decoded = encoded.decode("utf-8", errors="replace")
```

## Summary

- Use f-strings for advanced formatting
- Use join for efficient string building
- Handle encoding properly

## Next Steps

Continue to `09-advanced-regex.md`.
