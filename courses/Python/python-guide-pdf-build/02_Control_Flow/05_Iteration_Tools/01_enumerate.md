# Enumerate

## What You'll Learn
- Use enumerate() for index-value
- Start from custom index
- Replace counters

## Prerequisites
- Read 02_Loops.md first

## Overview
enumerate() adds counters to iterables.

## Basic Enumerate
enumerate wraps iterables

```python
for i, v in enumerate(["a","b"]):
    print(i, v)
```

## Custom Start
Start parameter

```python
for i, v in enumerate(["a","b"], 1):
    print(i, v)
```

## Common Mistakes
- Not unpacking
- Wrong start

## Summary
- Yields (index, value)
- start= sets index
- Replaces manual i+=1

## Next Steps
Continue to **[02_zip_and_zip_longest.md](./02_zip_and_zip_longest.md)**
