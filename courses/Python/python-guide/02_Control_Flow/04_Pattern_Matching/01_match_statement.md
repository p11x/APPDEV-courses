# Match Statement

## What You'll Learn
- Understand match/case syntax introduced in Python 3.10
- Replace long if/elif chains with pattern matching
- Match against literal values and types
- Use wildcard patterns and guards

## Prerequisites
- Read 02_match_statements.md first

## Overview
The match statement lets you compare a value against several patterns. It's like a supercharged switch statement that can destructure data and bind variables as it matches.

## Basic Match Syntax
A match statement compares a subject expression against one or more case patterns. The first matching case executes its block.

```python
status = "active"
match status:
    case "pending":
        print("Waiting...")
    case "active":
        print("Running!")
    case _:
        print("Unknown")
```

## Matching Multiple Patterns
Use the | operator to match multiple values in a single case.

```python
code = 200
match code:
    case 200 | 201 | 202:
        print("Success")
    case 400 | 401 | 403:
        print("Client error")
    case 500 | 502 | 503:
        print("Server error")
```

## Wildcard and Default
The underscore _ matches anything and is useful as a catch-all default case.

```python
point = (10, 20)
match point:
    case (0, 0):
        print("Origin")
    case (x, 0):
        print(f"On X-axis: {x}")
    case (0, y):
        print(f"On Y-axis: {y}")
    case _:
        print("Somewhere else")
```

## Common Mistakes
- Forgetting the wildcard case and getting MatchError when no pattern matches
- Using == instead of | for OR patterns (case 1 | 2, not case 1 or 2)

## Summary
- Match statements replace switch/case from other languages
- Patterns can bind variables and destructure data
- Always include a wildcard case for handling unexpected values

## Next Steps
Continue to **[02_matching_literals_and_types.md](./02_matching_literals_and_types.md)**
