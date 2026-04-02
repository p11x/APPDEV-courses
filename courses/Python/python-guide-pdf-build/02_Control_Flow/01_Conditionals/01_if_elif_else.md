# If, Elif, and Else

## What You'll Learn

- How to use `if`, `elif`, and `else` to make decisions
- Nested conditionals
- Ternary operator (one-liner)
- Real example: a grade calculator

## Prerequisites

- Read [03_assignment_operators.md](../../01_Foundations/03_Operators/03_assignment_operators.md) first

## The `if` Statement

The `if` statement lets your program make decisions:

```python
# Basic if statement
age: int = 18

if age >= 18:
    print("You are an adult")
```

### Syntax

```python
if condition:
    # Code to run if condition is True
    statement
```

**Important**: The code inside the `if` block must be **indented** (usually 4 spaces).

## The `else` Statement

The `else` block runs when the `if` condition is `False`:

```python
age: int = 16

if age >= 18:
    print("You are an adult")
else:
    print("You are a minor")
```

## The `elif` Statement

Use `elif` (else if) for multiple conditions:

```python
score: int = 85

if score >= 90:
    print("A - Excellent!")
elif score >= 80:
    print("B - Good job!")
elif score >= 70:
    print("C - Satisfactory")
elif score >= 60:
    print("D - Needs improvement")
else:
    print("F - Failed")
```

### Flow Chart

```
┌──────────────┐
│   Start      │
└──────┬───────┘
       ▼
┌──────────────┐
│ score >= 90? │──Yes──► "A"
└──────┬───────┘
       │ No
       ▼
┌──────────────┐
│ score >= 80? │──Yes──► "B"
└──────┬───────┘
       │ No
       ▼
┌──────────────┐
│ score >= 70? │──Yes──► "C"
└──────┬───────┘
       │ No
       ▼
┌──────────────┐
│ score >= 60? │──Yes──► "D"
└──────┬───────┘
       │ No
       ▼
    "F" (else)
```

## Nested Conditionals

You can put `if` statements inside other `if` statements:

```python
age: int = 25
has_license: bool = True

if age >= 18:
    if has_license:
        print("You can drive!")
    else:
        print("You're old enough but need a license")
else:
    print("You're too young to drive")
```

### Better Version (Using Logical Operators)

```python
age: int = 25
has_license: bool = True

if age >= 18 and has_license:
    print("You can drive!")
elif age >= 18 and not has_license:
    print("You're old enough but need a license")
else:
    print("You're too young to drive")
```

## Ternary Operator (One-Liner)

Python has a compact form for simple if/else:

```python
# Traditional way
age: int = 20
if age >= 18:
    status = "adult"
else:
    status = "minor"

# Ternary operator (one-liner)
status: str = "adult" if age >= 18 else "minor"
```

### Syntax

```python
value_if_true if condition else value_if_false
```

### More Examples

```python
# Maximum of two numbers
a: int = 5
b: int = 10
max_val: int = a if a > b else b

# Absolute value
x: int = -5
abs_val: int = x if x > 0 else -x

# Check even/odd
num: int = 7
result: str = "even" if num % 2 == 0 else "odd"
```

## Annotated Example: Grade Calculator

```python
# grade_calculator.py
# Calculate letter grade from score

def calculate_grade(score: int) -> str:
    """Calculate letter grade based on numeric score.
    
    Args:
        score: Numeric score from 0-100
    
    Returns:
        Letter grade as string
    """
    # Check if score is valid (0-100)
    if score < 0 or score > 100:
        return "Invalid score"
    
    # Determine letter grade using if/elif/else
    # First check for perfect score
    if score == 100:
        return "A+ (Perfect!)"
    
    # Then check A grade (90-99)
    elif score >= 90:
        return "A - Excellent!"
    
    # B grade (80-89)
    elif score >= 80:
        return "B - Good job!"
    
    # C grade (70-79)
    elif score >= 70:
        return "C - Satisfactory"
    
    # D grade (60-69)
    elif score >= 60:
        return "D - Needs improvement"
    
    # F grade (below 60)
    else:
        return "F - Failed"


def main() -> None:
    # Test with different scores
    test_scores: list[int] = [100, 95, 85, 75, 65, 55, -5, 105]
    
    # Loop through test scores
    for score in test_scores:
        # Calculate grade using our function
        grade: str = calculate_grade(score)
        
        # Print result
        print(f"Score: {score:3d} → {grade}")
    
    print("\n--- Using ternary for pass/fail ---")
    
    # Ternary operator for simple pass/fail
    score: int = 75
    result: str = "PASS" if score >= 60 else "FAIL"
    print(f"Score {score}: {result}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
Score: 100 → A+ (Perfect!)
Score:  95 → A - Excellent!
Score:  85 → B - Good job!
Score:  75 → C - Satisfactory
Score:  65 → D - Needs improvement
Score:  55 → F - Failed
Score:  -5 → Invalid score
Score: 105 → Invalid score

--- Using ternary for pass/fail ---
Score 75: PASS
```

## Common Mistakes

### ❌ Forgetting the Colon

```python
# WRONG - missing colon
if age >= 18
    print("Adult")

# CORRECT - colon required
if age >= 18:
    print("Adult")
```

### ❌ Wrong Indentation

```python
# WRONG - not indented
if age >= 18:
print("Adult")  # This runs always!

# CORRECT - indented
if age >= 18:
    print("Adult")  # This runs only if True
```

### ❌ Using = Instead of ==

```python
# WRONG - assignment, not comparison!
if age = 18:  # SyntaxError!
    ...

# CORRECT - comparison
if age == 18:
    ...
```

### ❌ Not Using `elif` When Needed

```python
# INEFFICIENT - multiple if statements
if score >= 90:
    print("A")
if score >= 80:  # This ALSO runs even if score >= 90!
    print("B")
if score >= 70:
    print("C")

# CORRECT - use elif
if score >= 90:
    print("A")
elif score >= 80:  # Only runs if first was False
    print("B")
elif score >= 70:
    print("C")
```

## Summary

- **`if`**: Runs code if condition is True
- **`else`**: Runs code if all previous conditions were False
- **`elif`**: Short for "else if" — checks another condition
- **Indentation**: All code in a block must be indented the same amount
- **Ternary**: `value if condition else value` for simple decisions

## Next Steps

Now let's learn about the **match statement** (Python 3.10+) in **[02_match_statements.md](./02_match_statements.md)** — a powerful way to handle multiple conditions.
