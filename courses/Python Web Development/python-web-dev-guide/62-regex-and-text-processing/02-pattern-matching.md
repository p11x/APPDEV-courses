# Pattern Matching

## What You'll Learn

- Finding and extracting matches
- Using re module functions
- Common patterns

## Prerequisites

- Completed `01-regex-fundamentals.md`

## re Module Functions

```python
import re

# Search - find first match
match = re.search(r"\d+", "abc 123 def 456")
print(match.group())  # "123"

# Find all - find all matches
matches = re.findall(r"\d+", "abc 123 def 456")
print(matches)  # ['123', '456']

# Find iter - iterator of matches
for match in re.finditer(r"\d+", "abc 123 def 456"):
    print(match.group(), match.start(), match.end())

# Sub - replace matches
result = re.sub(r"\d+", "X", "abc 123 def 456")
print(result)  # "abc X def X"

# Split - split by pattern
parts = re.split(r"\s+", "hello world  test")
print(parts)  # ['hello', 'world', 'test']
```

## Compiling Patterns

```python
# Compile for efficiency when using multiple times
pattern = re.compile(r"\d+")

match = pattern.search("abc 123")
print(match.group())  # "123"

matches = pattern.findall("abc 123 def 456")
print(matches)  # ['123', '456']
```

## Flags

```python
# Case insensitive
re.search(r"hello", "HELLO", re.IGNORECASE)  # Match

# Multiline
text = """first line
second line"""
re.search(r"^second", text, re.MULTILINE)  # Match

# Dotall - . matches newlines
re.search(r"a.b", "a\nb", re.DOTALL)  # Match
```

## Common Patterns

```python
# Email
email_pattern = r"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"
re.search(email_pattern, "contact@example.com")

# URL
url_pattern = r"https?://[^\s]+"
re.findall(url_pattern, "Visit https://example.com")

# Phone (US)
phone_pattern = r"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}"
re.search(phone_pattern, "(555) 123-4567")

# Date (YYYY-MM-DD)
date_pattern = r"\d{4}-\d{2}-\d{2}"
re.search(date_pattern, "2024-01-15")

# IP Address
ip_pattern = r"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"
re.search(ip_pattern, "192.168.1.1")
```

## Backreferences

```python
# Match repeated words
text = "the the cat"
pattern = r"(\w+)\s+\1"
re.search(pattern, text)  # Matches "the the"

# Match quoted strings
pattern = r"(['\"])(.*?)\1"
re.search(pattern, "'hello'")  # Matches "'hello'"
```

## Lookahead and Lookbehind

```python
# Positive lookahead (?=)
# Match if followed by something
pattern = r"\d+(?=px)"
re.search(pattern, "10px")   # Matches "10"
re.search(pattern, "10em")   # No match

# Negative lookahead (?!)
pattern = r"\d+(?!px)"
re.search(pattern, "10em")   # Matches "10"
re.search(pattern, "10px")   # No match

# Positive lookbehind (?<=)
pattern = r"(?<=\$)\d+"
re.search(pattern, "$100")   # Matches "100"

# Negative lookbehind (?<!)
pattern = r"(?<!\$)\d+"
re.search(pattern, "100")    # Matches "100"
```

## Summary

- Use re.search, findall, sub, split
- Compile patterns for efficiency
- Use flags for case sensitivity

## Next Steps

Continue to `03-text-processing.md`.
