# Data Types

## What You'll Learn

- The five fundamental data types: int, float, str, bool, NoneType
- How to use `type()` to inspect types
- Implicit vs explicit type conversion
- A reference table of types and their use cases

## Prerequisites

- Read [01_variables_explained.md](./01_variables_explained.md) first

## What Are Data Types?

Every piece of data in Python has a **type** — the kind of data it is. Python uses types to:
- Know how to process the data
- Allocate the right amount of memory
- Validate operations (you can't divide a string!)

## The Five Fundamental Types

### 1. Integer (`int`) — Whole Numbers

Integers are whole numbers without decimals:

```python
# Integer examples
count: int = 42           # Positive integer
negative: int = -17       # Negative integer
zero: int = 0             # Zero is also an integer

# Operations work as expected
sum: int = 10 + 5        # 15
difference: int = 10 - 5 # 5
product: int = 10 * 5    # 50
```

### 2. Float (`float`) — Decimal Numbers

Floats represent numbers with decimal points:

```python
# Float examples
price: float = 19.99       # Positive decimal
temperature: float = -5.5  # Negative decimal
pi_approx: float = 3.14159  # Mathematical constant

# Operations
result: float = 10 / 4     # 2.5 (division always gives float)
```

### 3. String (`str`) — Text

Strings hold text — sequences of characters:

```python
# String examples
greeting: str = "Hello"                 # Double quotes
name: str = 'Alice'                     # Single quotes work too
message: str = "It's a great day!"     # Mix quotes
multiline: str = """Line 1
Line 2"""                               # Triple quotes for multi-line

# String operations
combined: str = "Hello " + "World"     # Concatenation
repeated: str = "Ha" * 3              # "HaHaHa" - repetition
```

### 4. Boolean (`bool`) — True or False

Booleans represent truth values — either `True` or `False`:

```python
# Boolean examples
is_active: bool = True       # True (capital T)
is_complete: bool = False   # False (capital F)

# Boolean from comparisons
is_greater: bool = 10 > 5    # True
is_equal: bool = 5 == 5      # True (note: == for comparison, = for assignment)
```

### 5. NoneType (`None`) — Nothing

`None` represents the absence of a value — it's Python's "nothing":

```python
# None examples
nothing: None = None         # The only value of NoneType
result: None = None          # Used when no value is returned

# Checking for None
if result is None:
    print("No result available")
```

## Using `type()` to Inspect Types

Python can tell you what type any value is:

```python
# Check the type of values
print(type(42))           # <class 'int'>
print(type(3.14))         # <class 'float'>
print(type("hello"))      # <class 'str'>
print(type(True))         # <class 'bool'>
print(type(None))         # <class 'NoneType'>
```

### Practical Example

```python
def demonstrate_types() -> None:
    # Create variables of different types
    count: int = 42
    price: float = 19.99
    name: str = "Alice"
    is_member: bool = True
    data: None = None
    
    # Print each variable with its type
    print(f"count: {count} is {type(count)}")
    print(f"price: {price} is {type(price)}")
    print(f"name: {name} is {type(name)}")
    print(f"is_member: {is_member} is {type(is_member)}")
    print(f"data: {data} is {type(data)}")

# Output:
# count: 42 is <class 'int'>
# price: 19.99 is <class 'float'>
# name: Alice is <class 'str'>
# is_member: True is <class 'bool'>
# data: None is <class 'NoneType'>
```

## Type Conversion (Casting)

### Implicit Conversion (Automatic)

Python automatically converts some types in expressions:

```python
# Integer + Integer = Integer
result: int = 10 + 5  # 15

# Float + Float = Float  
result: float = 3.14 + 2.86  # 6.0

# Integer + Float = Float (integer is "promoted" to float)
result: float = 10 + 5.5  # 15.5
```

### Explicit Conversion (Manual)

You can manually convert between types:

```python
# String to Integer
age_str: str = "25"
age_int: int = int(age_str)  # 25

# String to Float
price_str: str = "19.99"
price_float: float = float(price_str)  # 19.99

# Integer to String
count: int = 42
count_str: str = str(count)  # "42"

# Float to Integer (truncates decimal)
pi: float = 3.14159
pi_int: int = int(pi)  # 3 (loses the decimal part!)

# Integer to Float
count: int = 42
count_float: float = float(count)  # 42.0
```

### Conversion Functions

| Function | What It Does | Example |
|----------|--------------|---------|
| `int(x)` | Convert to integer | `int("42")` → `42` |
| `float(x)` | Convert to float | `float("3.14")` → `3.14` |
| `str(x)` | Convert to string | `str(42)` → `"42"` |
| `bool(x)` | Convert to boolean | `bool(1)` → `True` |
| `list(x)` | Convert to list | `list("abc")` → `['a', 'b', 'c']` |

### Common Conversion Mistakes

```python
# ❌ WRONG - can't convert random text to number
number: int = int("hello")  # ValueError!

# ✅ CORRECT - only convert numeric strings
number: int = int("42")  # 42
number: int = int("3.14")  # ValueError! Use float first
number: float = float("3.14")  # 3.14
```

## Data Type Reference Table

| Type | Name | Example | Use Case |
|------|------|---------|----------|
| `int` | Integer | `42`, `-17`, `0` | Counting, indexing, whole numbers |
| `float` | Float | `3.14`, `-0.5`, `100.0` | Measurements, prices, decimals |
| `str` | String | `"hello"`, `'world'` | Text, messages, names |
| `bool` | Boolean | `True`, `False` | Flags, conditions, yes/no |
| `None` | NoneType | `None` | Missing values, placeholder |

## Annotated Example: Type Conversion Program

```python
# data_types_demo.py
# Demonstrates all data types and conversions

def main() -> None:
    # String input from user
    number_str: str = "42"
    
    # Convert string to integer using int()
    number_int: int = int(number_str)
    
    # Integer to float
    number_float: float = float(number_int)
    
    # Float back to string
    final_string: str = str(number_float)
    
    # Print all values and their types
    print(f"String: {number_str} - Type: {type(number_str)}")
    print(f"Integer: {number_int} - Type: {type(number_int)}")
    print(f"Float: {number_float} - Type: {type(number_float)}")
    print(f"Back to String: {final_string} - Type: {type(final_string)}")
    
    # Demonstrate implicit conversion
    # When you mix int and float, Python converts to float
    mixed: float = 10 + 5.5  # int 10 becomes 10.0 automatically
    
    # Demonstrate boolean conversion
    # Most values are "truthy" (True), some are "falsy" (False)
    print(f"bool(1): {bool(1)}")      # True
    print(f"bool(0): {bool(0)}")      # False
    print(f"bool('text'): {bool('text')}")  # True
    print(f"bool(''): {bool('')}")    # False (empty string)
    print(f"bool(None): {bool(None)}")  # False
    
    # Using type hints throughout
    user_id: int = 1001
    username: str = "alice"
    balance: float = 150.75
    is_premium: bool = True
    
    print(f"User {user_id}: {username}, Balance: ${balance}, Premium: {is_premium}")


# Run the program
if __name__ == "__main__":
    main()
```

## Summary

- **Five main types**: `int` (whole numbers), `float` (decimals), `str` (text), `bool` (True/False), `None` (nothing)
- **`type()`** reveals what type a value is
- **Implicit conversion**: Python converts int + float to float automatically
- **Explicit conversion**: Use `int()`, `float()`, `str()`, `bool()` to convert manually
- **Type hints** make code clearer: `age: int = 25`

## Next Steps

Now let's explore **type hints in depth** in **[03_type_hints_and_annotations.md](./03_type_hints_and_annotations.md)** — learn how to use them effectively in Python 3.12+.
