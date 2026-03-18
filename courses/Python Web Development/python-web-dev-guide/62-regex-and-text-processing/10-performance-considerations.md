# Performance Considerations

## What You'll Learn

- Optimizing regex
- Efficient string operations
- Memory considerations

## Prerequisites

- Completed `09-practical-regex-recipes.md`

## Compiling Patterns

```python
import re

# Don't compile inside loops
# BAD:
for text in texts:
    match = re.search(r"\d+", text)

# GOOD - compile once:
pattern = re.compile(r"\d+")
for text in texts:
    match = pattern.search(text)
```

## Avoid Catastrophic Backtracking

```python
# BAD - can cause catastrophic backtracking
pattern = r"(a+)+$"
re.match(pattern, "a" * 30)

# GOOD - use atomic groups or possessive quantifiers
# Instead, use:
pattern = r"a+$"
```

## Use Specific Patterns

```python
# BAD - .* is greedy, matches everything
re.search(r".*hello", text)

# GOOD - be specific
re.search(r"[^h]*hello", text)

# BAD - alternation order matters
re.search(r"foo|foobar", "foobar")  # Matches "foo"

# GOOD - put longer patterns first
re.search(r"foobar|foo", "foobar")  # Matches "foobar"
```

## String Operation Performance

```python
# Use str methods when possible (faster than regex)
text = "hello world"
text.find("world")  # Faster than re.search
text.startswith("hello")  # Faster than re.match
text.count("o")  # Faster than re.findall

# Use set for membership testing
words = {"hello", "world", "python"}
if "hello" in words:  # O(1)
    pass
```

## Caching and Memoization

```python
from functools import lru_cache
import re

@lru_cache(maxsize=128)
def get_compiled_pattern(pattern: str):
    """Cache compiled regex patterns."""
    return re.compile(pattern)

def search_text(pattern: str, text: str):
    """Search with caching."""
    compiled = get_compiled_pattern(pattern)
    return compiled.search(text)
```

## Processing Large Text

```python
# BAD - loads entire file into memory
with open("large.txt") as f:
    content = f.read()
    matches = re.findall(r"\d+", content)

# GOOD - process line by line
matches = []
with open("large.txt") as f:
    for line in f:
        matches.extend(re.findall(r"\d+", line))

# BETTER - use generator
def find_matches(filepath: str, pattern: str):
    """Generator that yields matches."""
    compiled = re.compile(pattern)
    with open(filepath) as f:
        for line in f:
            yield from compiled.findall(line)
```

## Memory-Efficient Processing

```python
# Use re.finditer for large text (doesn't create full list)
text = "some very long text with numbers 123 456 789"

# BAD - creates list of all matches
matches = re.findall(r"\d+", text)

# GOOD - iterator, doesn't create full list
match_iter = re.finditer(r"\d+", text)
for match in match_iter:
    print(match.group())
```

## Summary

- Compile patterns outside loops
- Avoid catastrophic backtracking
- Use string methods when possible
- Process large files line by line

## Next Steps

This concludes the Regex and Text Processing folder. Continue to other topics in your learning journey.
