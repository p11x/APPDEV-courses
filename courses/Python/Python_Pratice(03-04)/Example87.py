# Example87.py
# Topic: Common Mistakes with Function Signatures

# This file shows common mistakes when using function signatures.


# ============================================================
# Example 1: Wrong Order of Parameters
# ============================================================
print("=== Mistake: Wrong Order ===")

# BAD: Putting defaults before required
# def bad_func(a=1, b):  # SyntaxError!
#     pass

# GOOD: Required before defaults
def good_func(a, b=1, c=2):
    return f"a={a}, b={b}, c={c}"

print(good_func(1))
print(good_func(1, 2))
print(good_func(1, 2, 3))


# ============================================================
# Example 2: Confusing Positional and Keyword
# ============================================================
print("\n=== Mistake: Confusing Positional/Keyword ===")

# BAD: Using wrong type
def greet_bad(name, message):
    return f"{message}, {name}!"

# Call with unclear intent
# greet_bad(message="Hello", "John")  # SyntaxError!

# GOOD: Clear intent
def greet_good(message, name):
    return f"{message}, {name}!"

print(greet_good("Hello", "John"))
print(greet_good(message="Hello", name="John"))


# ============================================================
# Example 3: Not Understanding / and *
# ============================================================
print("\n=== Mistake: Not Understanding / and * ===")

# Using *args where * alone is needed
def kw_only_wrong(*, a, b=1):
    """Keyword-only parameters."""
    return f"a={a}, b={b}"

# This works
print(kw_only_wrong(a=5))
print(kw_only_wrong(a=5, b=10))

# Using / incorrectly
def pos_only_wrong(a, /, b):
    """Positional-only for a."""
    return f"a={a}, b={b}"

print(pos_only_wrong(1, 2))
# print(pos_only_wrong(a=1, b=2))  # Error!


# ============================================================
# Example 4: Mutable Default Arguments
# ============================================================
print("\n=== Mistake: Mutable Default Arguments ===")

# BAD: Using mutable as default
def add_item_bad(item, items=[]):
    items.append(item)
    return items

# This causes issues - list persists between calls
print(add_item_bad("a"))
print(add_item_bad("b"))  # "a" still there!

# GOOD: Use None
def add_item_good(item, items=None):
    if items is None:
        items = []
    items.append(item)
    return items

print(add_item_good("a"))
print(add_item_good("b"))  # Fresh list each time


# ============================================================
# Example 5: Too Many Parameters
# ============================================================
print("\n=== Mistake: Too Many Parameters ===")

# BAD: Too many parameters
def create_user_bad(name, email, age, city, country, phone, status):
    pass

# GOOD: Use data class or dictionary
def create_user_good(**user_data):
    return user_data

user = create_user_good(
    name="John",
    email="john@example.com",
    age=25,
    city="NYC",
    country="USA",
    phone="123456",
    status="active"
)
print(f"User created: {user}")


# ============================================================
# Example 6: Not Using *args/**kwargs Properly
# ============================================================
print("\n=== Mistake: *args/**kwargs Issues ===")

# BAD: Not unpacking
def sum_bad(a, b, c):
    return a + b + c

numbers = [1, 2, 3]
# sum_bad(numbers)  # Error!

# GOOD: Unpack
def sum_good(*numbers):
    return sum(numbers)

print(sum_good(1, 2, 3))
print(sum_good(*numbers))  # Unpack list


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMMON MISTAKES: Function Signatures")
print("=" * 50)
print("""
AVOID:
- Defaults before required parameters
- Confusing positional/keyword
- Not understanding / and *
- Mutable default arguments
- Too many parameters
- Not unpacking *args

REMEMBER:
- / separates positional-only
- * separates keyword-only
- Use None for mutable defaults
- Use **kwargs for many parameters
""")
