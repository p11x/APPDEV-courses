# Comparison and Logical Operators

## What You'll Learn

- Comparison operators: ==, !=, <, >, <=, >=
- Logical operators: and, or, not
- Truth tables for logical operations
- Short-circuit evaluation

## Prerequisites

- Read [01_arithmetic_operators.md](./01_arithmetic_operators.md) first

## Comparison Operators

Comparison operators compare two values and return `True` or `False`.

### The Six Comparison Operators

| Operator | Meaning | Example | Result |
|----------|---------|---------|--------|
| `==` | Equal to | `5 == 5` | `True` |
| `!=` | Not equal to | `5 != 3` | `True` |
| `<` | Less than | `3 < 5` | `True` |
| `>` | Greater than | `5 > 3` | `True` |
| `<=` | Less than or equal | `3 <= 5` | `True` |
| `>=` | Greater than or equal | `5 >= 5` | `True` |

### Examples

```python
# Equal (==) - note: TWO equals signs!
result: bool = 5 == 5      # True
result: bool = 5 == 3      # False
result: bool = "hello" == "hello"  # True

# Not equal (!=)
result: bool = 5 != 3      # True
result: bool = "cat" != "dog"  # True

# Less than (<)
result: bool = 3 < 5       # True
result: bool = 5 < 3       # False

# Greater than (>)
result: bool = 5 > 3       # True
result: bool = 3 > 5       # False

# Less than or equal (<=)
result: bool = 3 <= 5      # True
result: bool = 5 <= 5     # True

# Greater than or equal (>=)
result: bool = 5 >= 3      # True
result: bool = 5 >= 5      # True
```

### Comparing Different Types

```python
# Numbers work as expected
result: bool = 10 > 5      # True
result: bool = 3.14 < 10   # True

# Strings compare alphabetically (lexicographically)
result: bool = "apple" < "banana"  # True (a < b)
result: bool = "cat" > "bird"     # True (c > b)

# Be careful with different types!
result: bool = 5 == "5"  # False (int != str)
```

## Logical Operators

Logical operators combine boolean values.

### The Three Logical Operators

| Operator | Description | Meaning |
|----------|-------------|---------|
| `and` | Both must be True | True if A and B are both True |
| `or` | At least one must be True | True if A or B (or both) are True |
| `not` | Inverts the value | True becomes False, False becomes True |

### Truth Tables

#### `and` — Both Must Be True

| A | B | A and B |
|---|---|---------|
| True | True | **True** |
| True | False | False |
| False | True | False |
| False | False | False |

```python
result: bool = True and True    # True
result: bool = True and False   # False
result: bool = False and True    # False
result: bool = False and False   # False
```

#### `or` — At Least One Must Be True

| A | B | A or B |
|---|---|--------|
| True | True | **True** |
| True | False | **True** |
| False | True | **True** |
| False | False | False |

```python
result: bool = True or True     # True
result: bool = True or False    # True
result: bool = False or True    # True
result: bool = False or False   # False
```

#### `not` — Inverts the Value

| A | not A |
|---|-------|
| True | False |
| False | True |

```python
result: bool = not True    # False
result: bool = not False   # True
```

## Combining Comparisons and Logical Operators

```python
age: int = 25

# Is age between 18 and 65?
# Must be >= 18 AND <= 65
is_adult: bool = age >= 18 and age <= 65  # True

score: int = 85

# Is score above 50 OR below 0?
is_valid: bool = score > 50 or score < 0  # True

is_active: bool = True

# Not of True is False
can_access: bool = is_active and not False  # True
```

## Short-Circuit Evaluation

Python doesn't evaluate more than it needs to:

### `and` Short-Circuit

If the first value is `False`, Python doesn't check the second:

```python
# First is False - don't need to check second
result: bool = False and print("This won't run")  # False

# First is True - must check second
result: bool = True and print("This WILL run")  # This WILL run, then True
```

### `or` Short-Circuit

If the first value is `True`, Python doesn't check the second:

```python
# First is True - don't need to check second
result: bool = True or print("This won't run")  # True

# First is False - must check second
result: bool = False or print("This WILL run")  # This WILL run, then True
```

### Practical Use

```python
# Safe division - only divide if divisor is not zero
def safe_divide(dividend: float, divisor: float) -> float | None:
    # If divisor is 0, return None immediately (don't divide!)
    if divisor == 0:
        return None
    return dividend / divisor


# Default value pattern
def greet(name: str | None) -> str:
    # If name is None or empty string, use "Guest"
    return f"Hello, {name if name else 'Guest'}!"


# Early return pattern
def process(data: list[int] | None) -> int:
    # Check for None first
    if data is None or len(data) == 0:
        return 0
    
    # If we get here, data is valid
    return sum(data)
```

## Annotated Example: Grade Calculator

```python
# grade_checker.py
# Determine letter grade based on score

def main() -> None:
    # Score from 0 to 100
    score: int = 85
    
    # Check if score is valid (0-100)
    # Use and to require BOTH conditions
    is_valid: bool = score >= 0 and score <= 100
    
    # Determine letter grade
    # Using if/elif/else (covered in Control Flow)
    # For now, just show the boolean operations
    
    # A: 90-100
    is_grade_a: bool = score >= 90 and score <= 100
    
    # B: 80-89
    is_grade_b: bool = score >= 80 and score < 90
    
    # C: 70-79
    is_grade_c: bool = score >= 70 and score < 70
    
    # F: Below 70
    is_grade_f: bool = score < 70
    
    print(f"Score: {score}")
    print(f"Is valid: {is_valid}")
    print(f"Grade A: {is_grade_a}")
    print(f"Grade B: {is_grade_b}")
    print(f"Grade C: {is_grade_c}")
    print(f"Grade F: {is_grade_f}")
    
    # Using not
    is_failing: bool = not is_grade_a and not is_grade_b and not is_grade_c
    print(f"Failing: {is_failing}")
    
    # Using or
    is_passing: bool = is_grade_a or is_grade_b or is_grade_c
    print(f"Passing: {is_passing}")


if __name__ == "__main__":
    main()
```

### Output

```
Score: 85
Is valid: True
Grade A: False
Grade B: True
Grade C: False
Grade F: False
Failing: False
Passing: True
```

## Common Mistakes

### ❌ Using = instead of ==

```python
# WRONG - assignment, not comparison!
if age = 18:  # SyntaxError!
    ...

# CORRECT - comparison
if age == 18:
    ...
```

### ❌ Confusing and/or with &&/||

```python
# WRONG - this is Python, not C/Java!
if age > 18 && age < 65:  # SyntaxError!
    ...

# CORRECT - use words
if age > 18 and age < 65:
    ...
```

### ❌ Not Using Parentheses

```python
# Can be confusing without parentheses
result: bool = True or False and False
# Evaluates as: True or (False and False) = True or False = True

# Better to be explicit
result: bool = True or (False and False)
```

## Summary

- **Comparison operators**: `==`, `!=`, `<`, `>`, `<=`, `>=`
- **Logical operators**: `and`, `or`, `not`
- **Truth tables**: Help understand how logical operators work
- **Short-circuit**: Python stops evaluating when result is determined
- **Use `==` for comparison**, `=` for assignment

## Next Steps

Now let's learn about **assignment operators** in **[03_assignment_operators.md](./03_assignment_operators.md)**.
