# Arithmetic Operators

## What You'll Learn

- The seven arithmetic operators: +, -, *, /, //, %, **
- Operator precedence (order of operations)
- How to calculate the area of a rectangle

## Prerequisites

- Read [03_type_hints_and_annotations.md](../../02_Variables_and_Types/03_type_hints_and_annotations.md) first

## What Are Arithmetic Operators?

Arithmetic operators perform mathematical calculations. They're the same symbols you use in math class.

## The Seven Arithmetic Operators

### 1. Addition (`+`)

Adds two numbers together:

```python
# Simple addition
result: int = 5 + 3      # 8
result: float = 2.5 + 3  # 5.5

# String concatenation (特殊!)
name: str = "Hello " + "World"  # "Hello World"
```

### 2. Subtraction (`-`)

Subtracts the right number from the left:

```python
result: int = 10 - 4     # 6
result: float = 5.5 - 2  # 3.5
result: int = -5 + 10    # 5 (unary minus)
```

### 3. Multiplication (`*`)

Multiplies two numbers:

```python
result: int = 6 * 7      # 42
result: float = 3.14 * 2 # 6.28

# String repetition (特殊!)
text: str = "Ha" * 3     # "HaHaHa"
```

### 4. Division (`/`)

Divides left by right — always returns a **float**:

```python
result: float = 10 / 2   # 5.0 (not 5!)
result: float = 7 / 3    # 2.333...
result: float = 10 / 4   # 2.5
```

### 5. Floor Division (`//`)

Divides and rounds **down** to the nearest integer:

```python
result: int = 10 // 3    # 3 (not 3.333...)
result: int = 7 // 2     # 3
result: int = -7 // 2    # -4 (rounds toward negative infinity)
```

### 6. Modulo (`%`)

Returns the **remainder** after division:

```python
result: int = 10 % 3     # 1 (10 = 3*3 + 1)
result: int = 7 % 2      # 1 (7 = 2*3 + 1)
result: int = 10 % 5     # 0 (no remainder)
```

### 7. Exponentiation (`**`)

Raises to a power:

```python
result: int = 2 ** 3     # 8 (2^3)
result: int = 5 ** 2     # 25 (5^2)
result: float = 2 ** 0.5 # 1.414... (square root)
```

## Operator Precedence

When you have multiple operators, Python follows this order:

| Priority | Operator | Description |
|----------|----------|-------------|
| 1 | `**` | Exponentiation |
| 2 | `-x` | Unary negation |
| 3 | `*`, `/`, `//`, `%` | Multiplication, division, floor, modulo |
| 4 | `+`, `-` | Addition, subtraction |

### Precedence Examples

```python
# Without parentheses - follows precedence
result: int = 2 + 3 * 4   # 14 (not 20!)
#   3 * 4 = 12
#   2 + 12 = 14

# Use parentheses to control order
result: int = (2 + 3) * 4 # 20
#   2 + 3 = 5
#   5 * 4 = 20

# Multiple operations
result: float = (10 - 2) ** 2 / 4  # 16.0
#   10 - 2 = 8
#   8 ** 2 = 64
#   64 / 4 = 16.0
```

### Precedence Table (Complete)

```
1. Parentheses: ()
2. Exponentiation: **
3. Unary: - (negation)
4. Multiplication/Division: *, /, //, %
5. Addition/Subtraction: +, -
```

## Practical Example: Rectangle Area Calculator

Let's build a program that calculates the area of a rectangle:

```python
# rectangle_area.py
# Calculate the area of a rectangle

def main() -> None:
    # Define rectangle dimensions (width and height)
    width: float = 5.0
    height: float = 3.0
    
    # Calculate area: width × height
    area: float = width * height
    
    # Print the result
    print(f"Rectangle dimensions: {width} x {height}")
    print(f"Area: {area}")
    
    # Let's try with integer dimensions too
    width_int: int = 10
    height_int: int = 4
    area_int: int = width_int * height_int
    
    print(f"\nInteger rectangle: {width_int} x {height_int}")
    print(f"Area: {area_int}")
    
    # More complex: calculate perimeter
    # Perimeter = 2 × (width + height)
    perimeter: float = 2 * (width + height)
    print(f"\nPerimeter: {perimeter}")
    
    # Calculate diagonal using Pythagorean theorem
    # diagonal = √(width² + height²)
    diagonal: float = (width ** 2 + height ** 2) ** 0.5
    print(f"Diagonal: {diagonal:.2f}")  # :.2f rounds to 2 decimal places


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
Rectangle dimensions: 5.0 x 3.0
Area: 15.0

Integer rectangle: 10 x 4
Area: 40

Perimeter: 16.0
Diagonal: 5.83
```

## Annotated Code Breakdown

```python
# rectangle_area.py - Fully annotated

def main() -> None:
    # Define the width of the rectangle (float for precision)
    width: float = 5.0
    
    # Define the height of the rectangle
    height: float = 3.0
    
    # Multiply width by height to get area
    # * is the multiplication operator
    area: float = width * height
    
    # Print using f-string to display results
    # f"..." allows embedding variables with {variable_name}
    print(f"Rectangle dimensions: {width} x {height}")
    print(f"Area: {area}")
    
    # Another example with integers
    width_int: int = 10
    height_int: int = 4
    
    # Area calculation with integers
    area_int: int = width_int * height_int
    
    # Print with newline for spacing
    print(f"\nInteger rectangle: {width_int} x {height_int}")
    print(f"Area: {area_int}")
    
    # Calculate perimeter: 2 × (width + height)
    # Multiplication (*) has higher precedence than addition (+)
    # So we use parentheses to add first, then multiply
    perimeter: float = 2 * (width + height)
    print(f"\nPerimeter: {perimeter}")
    
    # Calculate diagonal using Pythagorean theorem
    # diagonal = √(width² + height²)
    # Step 1: width ** 2 (5.0 squared = 25.0)
    # Step 2: height ** 2 (3.0 squared = 9.0)
    # Step 3: sum them (25.0 + 9.0 = 34.0)
    # Step 4: ** 0.5 (square root of 34.0)
    diagonal: float = (width ** 2 + height ** 2) ** 0.5
    
    # :.2f formats to 2 decimal places
    print(f"Diagonal: {diagonal:.2f}")


# Standard Python entry point
if __name__ == "__main__":
    main()
```

## Common Mistakes

### ❌ Forgetting Operator Precedence

```python
# WRONG - gets wrong answer
average: float = 10 + 20 / 2  # 20.0 (20/2 = 10, then 10+10 = 20)
# Should be 15.0!

# CORRECT - use parentheses
average: float = (10 + 20) / 2  # 15.0
```

### ❌ Confusing / and //

```python
# Division (/) always returns float
result1: float = 7 / 2   # 3.5

# Floor division (//) returns int (rounds down)
result2: int = 7 // 2    # 3
```

### ❌ Using * for Exponentiation

```python
# WRONG - this is multiplication, not power!
result: int = 2 * 3      # 6

# CORRECT - use ** for power
result: int = 2 ** 3     # 8
```

## Summary

- **Addition** (`+`): Adds numbers, or concatenates strings
- **Subtraction** (`-`): Subtracts numbers
- **Multiplication** (`*`): Multiplies numbers, or repeats strings
- **Division** (`/`): Always returns a float
- **Floor Division** (`//`): Divides and rounds down
- **Modulo** (`%`): Returns the remainder
- **Exponentiation** (`**`): Raises to a power
- **Precedence**: `**` → `* / // %` → `+ -` → Use `()` to control order

## Next Steps

Now let's learn about **comparison and logical operators** in **[02_comparison_and_logical.md](./02_comparison_and_logical.md)**.
