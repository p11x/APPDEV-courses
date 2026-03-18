# Your First Python Script

## What You'll Learn

- How to create and run your first Python script
- Understanding every part of a Python program
- What `print()`, strings, and f-strings are
- How to use comments to document your code

## Prerequisites

- Read [02_installing_python.md](./02_installing_python.md) first

## Creating hello_world.py

Let's create your first Python program. This is a tradition in programming — every beginner starts with a "Hello, World!" program.

### Step 1: Create the File

Create a new file called `hello_world.py` in your text editor or IDE. The `.py` extension tells your computer this is a Python file.

### Step 2: Add the Code

Copy and paste this code into your file:

```python
# hello_world.py
# This is your first Python program!

# Define the main function - this is the entry point of our program
def main() -> None:
    # Create a variable to store the name
    name: str = "World"
    
    # Use f-string (formatted string literal) to create a greeting
    message: str = f"Hello, {name}! Welcome to Python 3.12+"
    
    # Print the message to the console
    print(message)
    
    # Demonstrate f-string with multiple variables
    version: int = 3      # Major version number
    minor: int = 12       # Minor version number
    
    # f-strings can include expressions inside {}
    print(f"You're learning Python {version}.{minor}+")


# This special block only runs when we execute the file directly
# It prevents the code from running if we import this file as a module
if __name__ == "__main__":
    # Call the main function to start the program
    main()
```

### Step 3: Run the Script

Open your terminal and run:

```cmd
# On Windows
python hello_world.py
```

```bash
# On macOS/Linux
python3 hello_world.py
```

### Expected Output

```
Hello, World! Welcome to Python 3.12+
You're learning Python 3.12+
```

## Line-by-Line Breakdown

Let's understand every part of this program:

### Comments

```python
# This is your first Python program!
```

The `#` symbol starts a **comment**. Comments are for humans to read — Python ignores them. Use comments to explain what your code does!

### The `def` Keyword and Functions

```python
def main() -> None:
```

| Part | Meaning |
|------|---------|
| `def` | Keyword that starts a function definition |
| `main` | The name of our function (we can choose any name) |
| `()` | parentheses — where we would put parameters |
| `-> None` | Type hint saying this function returns nothing |
| `:` | Marks the start of the function body |

### Variable Declaration with Type Hints

```python
name: str = "World"
```

| Part | Meaning |
|------|---------|
| `name` | Variable name (our label for this data) |
| `: str` | Type hint: this variable should hold a string |
| `=` | Assignment operator: put the right side into the left side |
| `"World"` | A string literal — text inside quotes |

### F-Strings (Python 3.12+)

```python
message: str = f"Hello, {name}! Welcome to Python 3.12+"
```

The `f` before the quotes makes it an **f-string** (formatted string literal). Inside the curly braces `{}`, you can put:
- Variable names: `{name}` becomes "World"
- Expressions: `{2 + 2}` becomes "4"

### The `print()` Function

```python
print(message)
```

| Part | Meaning |
|------|---------|
| `print` | Built-in Python function that displays text |
| `(` | Start of function arguments |
| `message` | The variable we want to display |
| `)` | End of function arguments |

### The `if __name__ == "__main__":` Block

```python
if __name__ == "__main__":
    main()
```

This is a Python best practice:
- When you run a file directly, `__name__` equals `"__main__"`
- When you import the file as a module, `__name__` equals the module name
- This lets you have code that only runs when the file is executed directly

## A Simpler Version

Here's the simplest possible Hello World in Python:

```python
# The absolute minimum to print text
print("Hello, World!")
```

That's it! Just one line. But the longer version above is better practice because:
- Uses a function (better organization)
- Uses type hints (helps catch errors)
- Uses f-strings (more flexible)

## Practice: Modify Your Script

Try these modifications:

### 1. Change the Name

```python
def main() -> None:
    name: str = "Your Name"
    print(f"Hello, {name}!")
```

### 2. Add More Output

```python
def main() -> None:
    print("Line 1")
    print("Line 2")
    print("Line 3")
```

### 3. Calculate Something

```python
def main() -> None:
    x: int = 10
    y: int = 5
    print(f"{x} + {y} = {x + y}")
    print(f"{x} * {y} = {x * y}")
```

Output:
```
10 + 5 = 15
10 * 5 = 50
```

## Common Mistakes to Avoid

### ❌ Forgetting Quotes

```python
# WRONG - this will cause an error
print(Hello World)
```

```python
# CORRECT - string needs quotes
print("Hello World")
```

### ❌ Missing Parentheses

```python
# WRONG
print "Hello"

# CORRECT
print("Hello")
```

### ❌ Wrong File Extension

```python
# WRONG - .txt is not a Python file
hello.txt

# CORRECT - .py is the Python extension
hello.py
```

## Summary

- **Create** a `.py` file with your code
- **Run** it with `python filename.py`
- **Comments** start with `#` and are ignored by Python
- **Variables** store data with a name and optional type hint
- **Strings** are text inside quotes (`"..."` or `'...'`)
- **F-strings** (`f"..."`) let you embed variables in text with `{variable}`
- **`print()`** displays output to the console
- **`def`** defines a function
- **`if __name__ == "__main__":`** runs code only when file is executed directly

## Next Steps

Great job writing your first Python script! Now head to **[01_Foundations/02_Variables_and_Types/01_variables_explained.md](../02_Variables_and_Types/01_variables_explained.md)** to learn about variables in depth.
