# Common Errors

## What You'll Learn

- Common Python errors
- How to fix them
- Prevention strategies

## Prerequisites

- Completed `03-logging-debugging.md`

## Common Errors

### SyntaxError

```python
# Missing colon
if x > 5  # SyntaxError: expected ':'
    print(x)
```

### NameError

```python
# Using undefined variable
print(undefined_var)  # NameError

# Fix: Define the variable first
undefined_var = 10
print(undefined_var)
```

### TypeError

```python
# Wrong type operation
"hello" + 5  # TypeError

# Fix: Convert types
"hello" + str(5)  # "hello5"
```

### IndexError

```python
# List index out of range
items = [1, 2, 3]
print(items[5])  # IndexError

# Fix: Check bounds
if len(items) > 5:
    print(items[5])
```

## Summary

- Read error messages carefully
- Check types before operations

## Next Steps

Continue to `05-debugging-web-apps.md`.
