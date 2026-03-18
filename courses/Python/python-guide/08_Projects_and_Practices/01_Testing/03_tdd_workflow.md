# TDD Workflow

## What You'll Learn

- Red-Green-Refactor cycle
- Write failing test first
- Make it pass
- Refactor

## Prerequisites

- Read [02_pytest_guide.md](./02_pytest_guide.md) first

## TDD Cycle

```
1. RED: Write failing test
2. GREEN: Make test pass
3. REFACTOR: Clean up code
```

## Example: Calculator

```python
# Step 1: Write failing test
def test_add():
    assert add(1, 2) == 3

# Step 2: Make pass
def add(a, b):
    return a + b

# Step 3: Refactor (if needed)
```

## Summary

- **Red**: Write failing test
- **Green**: Make pass
- **Refactor**: Clean up

## Next Steps

Continue to **[08_Projects_and_Practices/02_Best_Practices/01_pep8_and_style.md](../02_Best_Practices/01_pep8_and_style.md)**
