# Strings as Sequences

## What You'll Learn

- String iteration and slicing
- String methods in depth
- Formatted strings (f-strings)
- Working with Unicode

## Prerequisites

- Read [04_sets_and_frozensets.md](./04_sets_and_frozensets.md) first

## String Iteration and Slicing

Strings are sequences, supporting indexing and slicing like lists.

```python
# string_slicing.py

text: str = "Hello, World!"

# Indexing
first_char: str = text[0]      # 'H'
last_char: str = text[-1]      # '!'

# Slicing
substring: str = text[0:5]     # 'Hello'
reversed_text: str = text[::-1]  # '!dlroW ,olleH'

# Step slicing
every_other: str = text[::2]   # 'HloWrd'
```

## String Methods

Python strings have numerous built-in methods.

```python
# string_methods.py

text: str = "  Hello, World!  "

# Case conversion
print(text.lower())       # "  hello, world!  "
print(text.upper())       # "  HELLO, WORLD!  "
print(text.title())       # "  Hello, World!  "

# Whitespace handling
print(text.strip())       # "Hello, World!"
print(text.lstrip())      # "Hello, World!  "
print(text.rstrip())      # "  Hello, World!"

# Search and replace
print(text.find("World"))      # 9
print(text.replace("World", "Python"))

# Split and join
parts: list[str] = "a,b,c".split(",")
joined: str = "-".join(parts)
```

## F-Strings (Formatted String Literals)

F-strings provide a concise way to embed expressions in strings.

```python
# fstrings_demo.py

name: str = "Alice"
age: int = 30
height: float = 5.6

# Basic formatting
print(f"My name is {name}, I'm {age} years old")

# Expressions in f-strings
print(f"In 5 years, I'll be {age + 5}")

# Format specifiers
print(f"Height: {height:.2f}")
print(f"Pi: {3.14159:.4f}")

# F-strings with dictionaries
data = {"name": "Bob", "score": 95}
print(f"Player: {data['name']}, Score: {data['score']}")
```

## Annotated Full Example

```python
# strings_demo.py
"""Complete demonstration of string operations."""

from typing import List


def word_reverse(text: str) -> str:
    """Reverse each word in a sentence."""
    return " ".join(word[::-1] for word in text.split())


def main() -> None:
    text = "Hello, World!"
    
    # String as sequence
    print(f"First char: {text[0]}")
    print(f"Last char: {text[-1]}")
    print(f"Sliced [0:5]: {text[0:5]}")
    
    # Methods
    print(f"\nUpper: {text.upper()}")
    print(f"Replace: {text.replace('World', 'Python')}")
    
    # F-strings with formatting
    price = 19.99
    quantity = 3
    print(f"\nTotal: ${price * quantity:.2f}")
    
    # Word reverse
    sentence = "Hello World"
    print(f"\nReversed words: {word_reverse(sentence)}")


if __name__ == "__main__":
    main()
```

## Summary

- String iteration and slicing
- String methods in depth
- Formatted strings (f-strings)

## Next Steps

Continue to **[06_bytearray_and_memoryview.md](./06_bytearray_and_memoryview.md)**
