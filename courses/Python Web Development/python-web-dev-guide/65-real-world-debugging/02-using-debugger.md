# Using Debugger

## What You'll Learn

- pdb commands
- Breakpoints
- Step-by-step debugging

## Prerequisites

- Completed `01-debugging-fundamentals.md`

## PDB Commands

| Command | Description |
|---------|-------------|
| n | Next line |
| s | Step into function |
| c | Continue |
| p variable | Print variable |
| l | List source |
| w | Show call stack |
| u | Go up stack |
| d | Go down stack |

## Using breakpoints

```python
# Set breakpoint
def process_data(data: list) -> dict:
    result = {"sum": sum(data), "count": len(data)}
    
    # Check intermediate values
    if result["count"] > 0:
        result["average"] = result["sum"] / result["count"]
    
    return result

# Test
data = [1, 2, 3, 4, 5]
print(process_data(data))
```

## Summary

- Use debugger to step through code
- Inspect variables at breakpoints

## Next Steps

Continue to `03-logging-debugging.md`.
