# Assignment Operators

## What You'll Learn

- Simple assignment: `=`
- Augmented assignment operators: +=, -=, *=, /=, //=, **=
- The walrus operator (`:=`) deep dive
- Using walrus in while loops and list comprehensions

## Prerequisites

- Read [02_comparison_and_logical.md](./02_comparison_and_logical.md) first

## Simple Assignment

The basic assignment operator is `=`:

```python
# Assign a value to a variable
x: int = 5
name: str = "Alice"
is_active: bool = True
```

## Augmented Assignment Operators

These operators combine an operation with assignment in one step:

| Operator | Example | Equivalent To |
|----------|---------|---------------|
| `+=` | `x += 3` | `x = x + 3` |
| `-=` | `x -= 3` | `x = x - 3` |
| `*=` | `x *= 3` | `x = x * 3` |
| `/=` | `x /= 3` | `x = x / 3` |
| `//=` | `x //= 3` | `x = x // 3` |
| `**=` | `x **= 3` | `x = x ** 3` |
| `%=` | `x %= 3` | `x = x % 3` |

### Examples

```python
# Addition assignment
x: int = 10
x += 5      # x is now 15 (same as x = x + 5)

# Subtraction assignment
x: int = 10
x -= 3      # x is now 7

# Multiplication assignment
x: int = 4
x *= 3      # x is now 12

# Division assignment (always returns float)
x: float = 10
x /= 2      # x is now 5.0

# Floor division assignment
x: int = 10
x //= 3     # x is now 3

# Exponentiation assignment
x: int = 2
x **= 4     # x is now 16

# Modulo assignment
x: int = 10
x %= 3      # x is now 1
```

### Why Use Augmented Assignment?

1. **Concise**: Less typing than `x = x + 1`
2. **Clearer intent**: Shows you're updating, not comparing
3. **Slightly faster**: In some languages (not Python, but good habit)

## The Walrus Operator (`:=`) — Deep Dive

The **walrus operator** (`:=`) was introduced in Python 3.8. It assigns a value to a variable **as part of an expression**.

### Without Walrus

```python
# Calculate a value, store it, then use it
result: int = len("hello")
if result > 3:
    print(f"Length is {result}")
```

### With Walrus

```python
# Assign and use in the same expression
if (result := len("hello")) > 3:
    print(f"Length is {result}")
```

### Why Is It Called Walrus?

The `:=` looks like walrus eyes and tusks:

```
    (  :=  )
     \    /
      \  /
       \/
```

## Walrus in While Loops

The walrus is especially useful in while loops:

### Without Walrus

```python
# Must calculate before the loop
command: str = input("Enter command: ")
while command != "quit":
    print(f"Processing: {command}")
    command = input("Enter command: ")  # Repeat at end
```

### With Walrus

```python
# Calculate and check in one expression
while (command := input("Enter command: ")) != "quit":
    print(f"Processing: {command}")
```

### Another Example: Read Until Condition

```python
# Collect numbers until user enters 0
numbers: list[int] = []

# walrus assigns and checks in one line
while (num := int(input("Enter number (0 to stop): "))) != 0:
    numbers.append(num)

print(f"You entered: {numbers}")
```

## Walrus in List Comprehensions

### Without Walrus

```python
# Get all even squares
squares: list[int] = []
for i in range(10):
    square: int = i ** 2
    if square % 2 == 0:
        squares.append(square)
```

### With Walrus (Python 3.8+)

```python
# walrus in list comprehension - compute once, use twice
squares: list[int] = [s for i in range(10) if (s := i ** 2) % 2 == 0]
```

## Annotated Example: Complete Program

```python
# assignment_demo.py
# Demonstrates all assignment operators

def main() -> None:
    # Simple assignment
    counter: int = 0
    print(f"Initial counter: {counter}")
    
    # Augmented assignment - addition
    counter += 5  # Same as: counter = counter + 5
    print(f"After += 5: {counter}")
    
    # Augmented assignment - multiplication
    counter *= 2  # Same as: counter = counter * 2
    print(f"After *= 2: {counter}")
    
    # More examples
    price: float = 100.0
    price -= 10    # Apply discount: 90.0
    price *= 0.9   # Apply tax: 81.0
    print(f"Final price: {price}")
    
    # Demonstrate walrus operator
    print("\n--- Walrus Operator Demo ---")
    
    # In a conditional - assign and test at once
    if (length := len("Hello World")) > 5:
        print(f"The string '{'Hello World'}' has {length} characters")
    
    # In a while loop - classic use case
    print("\n--- Input Loop with Walrus ---")
    print("Enter commands (type 'quit' to exit):")
    
    # This is cleaner than assigning before the loop
    while (command := input("> ")) != "quit":
        print(f"Processing: {command}")
    
    # In list comprehension - avoids recomputing
    print("\n--- Walrus in Comprehension ---")
    # Find all numbers where square is divisible by 4
    numbers: list[int] = [sq for i in range(10) if (sq := i ** 2) % 4 == 0]
    print(f"Squares divisible by 4: {numbers}")
    
    # Explanation: i=0, sq=0, 0%4=0 ✓
    #             i=1, sq=1, 1%4=1 ✗
    #             i=2, sq=4, 4%4=0 ✓
    #             i=3, sq=9, 9%4=1 ✗
    #             i=4, sq=16, 16%4=0 ✓
    
    # Multiple walrus in one expression
    # Warning: can get confusing!
    x: int = 5
    y: int = 10
    
    # Both assign and return values
    result: bool = (x := 3) < (y := 7)
    print(f"\nResult of (x := 3) < (y := 7): {result}")
    print(f"x is now {x}, y is now {y}")


# Run the program
if __name__ == "__main__":
    main()
```

### Output

```
Initial counter: 0
After += 5: 5
After *= 2: 10
Final price: 81.0

--- Walrus Operator Demo ---
The string 'Hello World' has 11 characters

--- Input Loop with Walrus ---
Enter commands (type 'quit' to exit):
> hello
Processing: hello
> test
Processing: test
> quit

--- Walrus in Comprehension ---
Squares divisible by 4: [0, 4, 16, 36, 64]

Result of (x := 3) < (y := 7): True
x is now 3, y is now 7
```

## Common Mistakes

### ❌ Forgetting the Colon

```python
# WRONG - missing colon in walrus
if x = 5:   # SyntaxError!
    ...

# CORRECT
if (x := 5) > 3:
    ...
```

### ❌ Using Walrus Without Parentheses

```python
# WRONG - ambiguous, may cause issues
if x := 5 > 3:  # Evaluates as: if x := (5 > 3)
    ...

# CORRECT - use parentheses for clarity
if (x := 5) > 3:
    ...
```

### ❌ Using Walrus at Module Level

```python
# WRONG - walrus is for expressions, not statements
x := 5  # SyntaxError at module level!

# CORRECT - walrus must be part of an expression
if (x := 5):
    ...
```

## Summary

- **Simple assignment** (`=`): Assigns a value to a variable
- **Augmented assignment** (`+=`, `-=`, `*=`, etc.): Combines operation and assignment
- **Walrus operator** (`:=`): Assigns as part of an expression
- **Walrus in while loops**: Assigns and checks in one line
- **Walrus in comprehensions**: Avoids recomputing expensive operations

## Next Steps

Great job with the Foundations section! Now let's move to **[02_Control_Flow/01_Conditionals/01_if_elif_else.md](../02_Control_Flow/01_Conditionals/01_if_elif_else.md)** to learn how to make decisions in your code.
