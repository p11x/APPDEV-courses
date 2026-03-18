# Regex Fundamentals

## What You'll Learn

- Basic regex patterns
- Character classes
- Quantifiers

## Prerequisites

- Basic Python knowledge

## What Is Regex

Regular expressions (regex) are patterns used to match text. They're powerful for validation, extraction, and text manipulation.

Think of regex like a search pattern with wildcards: "find all words that start with 'a' and end with 't'".

## Basic Patterns

```python
import re

# Simple match
pattern = r"hello"
text = "hello world"
match = re.search(pattern, text)
print(match.group() if match else "No match")  # "hello"

# Match at beginning
pattern = r"^hello"
re.match(pattern, "hello world")  # Match object

# Match at end
pattern = r"world$"
re.search(pattern, "hello world")  # Match object
```

## Character Classes

```python
# Any digit \d
re.search(r"\d", "item 123")  # Matches "1"

# Any non-digit \D
re.search(r"\D", "123abc")  # Matches "a"

# Any word character \w
re.search(r"\w+", "hello_world")  # Matches "hello_world"

# Any whitespace \s
re.search(r"\s", "hello world")  # Matches " "

# Any character .
re.search(r".", "abc")  # Matches "a"

# Character set []
re.search(r"[aeiou]", "hello")  # Matches "e"
re.search(r"[0-9]", "abc123")  # Matches "1"

# Negation [^]
re.search(r"[^0-9]", "123abc")  # Matches "a"
```

## Quantifiers

```python
# Zero or more *
re.search(r"ab*c", "ac")    # Matches "ac"
re.search(r"ab*c", "abc")   # Matches "abc"
re.search(r"ab*c", "abbc")  # Matches "abbc"

# One or more +
re.search(r"ab+c", "abc")   # Matches "abc"
re.search(r"ab+c", "ac")    # No match

# Zero or one ?
re.search(r"colou?r", "color")   # Matches "color"
re.search(r"colou?r", "colour")  # Matches "colour"

# Exact count {n}
re.search(r"\d{4}", "12345")  # Matches "1234"

# Range {n,m}
re.search(r"\d{2,4}", "12345")  # Matches "1234"
```

## Groups and Capturing

```python
# Capturing groups ()
match = re.search(r"(\d{3})-(\d{4})", "phone: 555-1234")
print(match.group(1))  # "555"
print(match.group(2))  # "1234"
print(match.groups())  # ("555", "1234")

# Non-capturing groups (?:)
re.search(r"(?:ab)+", "ababab")  # Matches "ababab"

# Named groups (?P<name>)
match = re.search(r"(?P<area>\d{3})-(?P<num>\d{4})", "555-1234")
print(match.group("area"))  # "555"
print(match.group("num") )  # "1234"
```

## Summary

- Regex patterns match text
- Character classes match types of characters
- Quantifiers specify how many

## Next Steps

Continue to `02-pattern-matching.md`.
