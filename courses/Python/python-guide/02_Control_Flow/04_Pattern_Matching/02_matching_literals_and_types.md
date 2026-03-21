# Matching Literals and Types

## What You'll Learn
- Match literal values
- Match by type
- Use guards

## Prerequisites
- Read 01_match_statement.md first

## Overview
Match against specific values and types.

## Literal Matching
Match exact values

```python
x = "yes"
match x:
    case "yes" | "y": print("yes!")
```

## Type Matching
Match using isinstance

```python
val = "hello"
match val:
    case str(): print("string")
```

## Common Mistakes
- Quotes
- Case sensitivity

## Summary
- Literals match exactly
- Case sensitive
- | combines

## Next Steps
Continue to **[03_matching_sequences.md](./03_matching_sequences.md)**
