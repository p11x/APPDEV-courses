# While Loops

## What You'll Learn

- How to use `while` loops
- Infinite loop dangers
- Using `break`, `continue`, and `else` with loops
- Real example: user input validation

## Prerequisites

- Read [01_for_loops.md](./01_for_loops.md) first

## What Is a While Loop?

A `while` loop repeats as long as a condition is `True`:

```python
# Basic while loop
count: int = 0

while count < 5:
    print(count)
    count += 1  # Must update count or loop runs forever!

# Prints: 0, 1, 2, 3, 4
```

### Syntax

```python
while condition:
    # Code to repeat
    # Must eventually make condition False
```

## While Loop vs For Loop

| For Loop | While Loop |
|----------|------------|
| Known number of iterations | Unknown iterations |
| `for i in range(5):` | `while i < 5:` |
| Automatically stops | You control the condition |

### When to Use While

- **User input**: Repeat until valid input
- **Game loops**: Repeat until game ends
- **File reading**: Read until end of file
- **Waiting**: Wait for condition to change

## The Else Block

Python has a unique feature: `else` on loops. The `else` block runs when the loop **completes normally** (not when `break` is used):

```python
# While with else
count: int = 0

while count < 3:
    print(f"Count: {count}")
    count += 1
else:
    print("Loop completed!")

# Output:
# Count: 0
# Count: 1
# Count: 2
# Loop completed!
```

### Why Use Else?

The `else` block is useful when you want to know if a loop finished naturally or was broken:

```python
# Search for a number
numbers: list[int] = [1, 2, 3, 4, 5]
target: int = 7

index: int = 0
while index < len(numbers):
    if numbers[index] == target:
        print(f"Found {target} at index {index}")
        break  # Exit loop early
    index += 1
else:
    # Only runs if loop wasn't broken
    print(f"{target} not found in list")
```

## Break and Continue

### Break — Exit the Loop

```python
# Exit loop early with break
while True:
    user_input: str = input("Enter 'quit' to exit: ")
    if user_input == "quit":
        break  # Exit the loop
    print(f"You entered: {user_input}")

print("Goodbye!")
```

### Continue — Skip to Next Iteration

```python
# Skip even numbers
count: int = 0

while count < 10:
    count += 1
    if count % 2 == 0:
        continue  # Skip the rest of this iteration
    print(count)  # Only prints odd numbers

# Output: 1, 3, 5, 7, 9
```

## Infinite Loops (Danger!)

A loop that never ends is a **bug**:

```python
# WRONG - infinite loop!
while True:
    print("This runs forever!")
```

### Safe Infinite Loop Pattern

```python
# Safe way to use infinite loop
while True:
    user_input: str = input("Enter command: ")
    
    if user_input == "quit":
        break  # Exit the loop
    
    # Process input...
    print(f"Processing: {user_input}")
```

### Must-Update Pattern

```python
# WRONG - infinite loop (count never changes!)
count: int = 0
while count < 5:
    print(count)
    # Forgot: count += 1

# CORRECT - update the condition variable
count: int = 0
while count < 5:
    print(count)
    count += 1  # Now it will eventually stop
```

## Annotated Example: Input Validation

```python
# input_validation.py
# Demonstrates while loops with user input validation

def get_valid_age() -> int:
    """Get a valid age from user.
    
    Returns:
        A valid age (positive integer)
    """
    while True:  # Loop until we get valid input
        user_input: str = input("Enter your age: ")
        
        # Try to convert to integer
        try:
            age: int = int(user_input)
            
            # Check if age is valid
            if age < 0:
                print("Age cannot be negative. Try again.")
                continue  # Skip to next iteration
            
            if age > 150:
                print("That's unrealistic. Try again.")
                continue
            
            # If we get here, age is valid
            return age  # Exit the function with valid age
            
        except ValueError:
            # Input was not a valid integer
            print(f"'{user_input}' is not a number. Try again.")
            # Loop continues to ask again


def get_menu_choice() -> str:
    """Get a valid menu choice from user.
    
    Returns:
        A valid menu choice (A, B, C, or D)
    """
    valid_choices: set[str] = {"A", "B", "C", "D"}
    
    while True:
        user_input: str = input("Choose (A/B/C/D): ").upper().strip()
        
        if user_input in valid_choices:
            return user_input  # Valid choice, exit
        
        print(f"Invalid choice. Must be A, B, C, or D.")
        # Loop continues


def count_to_zero() -> None:
    """Count down from a number to zero."""
    start: int = 5
    
    while start > 0:
        print(start)
        start -= 1  # Decrement (same as: start = start - 1)
    else:
        print("Blast off! 🚀")


def main() -> None:
    print("=== While Loop Examples ===\n")
    
    # Example 1: Get valid age
    print("--- Example 1: Age Validation ---")
    age: int = get_valid_age()
    print(f"You entered age: {age}\n")
    
    # Example 2: Menu choice
    print("--- Example 2: Menu Choice ---")
    choice: str = get_menu_choice()
    print(f"You chose: {choice}\n")
    
    # Example 3: Countdown
    print("--- Example 3: Countdown ---")
    count_to_zero()


# Run the program
if __name__ == "__main__":
    main()
```

### Sample Output

```
=== While Loop Examples ===

--- Example 1: Age Validation ---
Enter your age: -5
Age cannot be negative. Try again.
Enter your age: twenty
'twenty' is not a number. Try again.
Enter your age: 25
You entered age: 25

--- Example 2: Menu Choice ---
Choose (A/B/C/D): x
Invalid choice. Must be A, B, C, or D.
Choose (A/B/C/D): b
You chose: B

--- Example 3: Countdown ---
5
4
3
2
1
Blast off! 🚀
```

## Common Mistakes

### ❌ Forgetting to Update

```python
# WRONG - infinite loop
while count < 10:
    print(count)
    # Missing: count += 1
```

### ❌ Wrong Comparison Operator

```python
# WRONG - uses = instead of ==
count: int = 0
while count = 5:  # SyntaxError!
    print(count)
```

### ❌ Not Handling Exceptions

```python
# WRONG - crashes on invalid input
age: int = int(input("Age: "))  # crashes if user types "abc"

# CORRECT - use try/except (covered in Exception Handling)
while True:
    try:
        age: int = int(input("Age: "))
        break
    except ValueError:
        print("Invalid input")
```

## Summary

- **`while` loop**: Repeats while condition is True
- **`break`**: Exit the loop immediately
- **`continue`**: Skip to next iteration
- **`else`**: Runs when loop completes normally
- **Infinite loops**: Use `while True` with `break` to control
- **Always update** the condition variable to avoid infinite loops

## Next Steps

Now let's learn about **comprehensions** in **[03_comprehensions.md](./03_comprehensions.md)** — a powerful way to create lists, dictionaries, and sets.
