# Example86.py
# Topic: Function Signatures - Positional-Only and Keyword-Only

# This file demonstrates function signature features in Python.


# ============================================================
# Example 1: Positional-Only Parameters (/)
# ============================================================
print("=== Positional-Only Parameters (/) ===")

# Parameters before / must be passed by position
def positional_only(a, b, c):
    """All positional parameters."""
    return f"a={a}, b={b}, c={c}"

# Using /
def pos_only(a, b, /, c):
    """a and b are positional-only, c can be positional or keyword."""
    return f"a={a}, b={b}, c={c}"

# Test
print(positional_only(1, 2, 3))
print(pos_only(1, 2, 3))  # All positional
print(pos_only(1, 2, c=3))  # c as keyword


# ============================================================
# Example 2: Keyword-Only Parameters (*)
# ============================================================
print("\n=== Keyword-Only Parameters (*) ===")

# Parameters after * must be passed by keyword
def keyword_only(*, a, b, c):
    """All keyword-only parameters."""
    return f"a={a}, b={b}, c={c}"

# Using *
def kw_only(a, *, b, c):
    """a can be positional or keyword, b and c must be keyword."""
    return f"a={a}, b={b}, c={c}"

# Test
print(keyword_only(a=1, b=2, c=3))
print(kw_only(1, b=2, c=3))  # a positional, b,c keyword
# print(kw_only(1, 2, 3))  # Error: b and c must be keyword


# ============================================================
# Example 3: Combined (/)
# ============================================================
print("\n=== Combined: Positional-Only and Keyword-Only ===")

# Both combined
def combined(a, /, b, *, c):
    """a: pos-only, b: either, c: kw-only."""
    return f"a={a}, b={b}, c={c}"

# Valid calls
print(combined(1, 2, c=3))      # a, b positional, c keyword
print(combined(1, b=2, c=3))     # a positional, b, c keyword

# Invalid calls
# combined(a=1, b=2, c=3)  # Error: a cannot be keyword


# ============================================================
# Example 4: Why Use Positional-Only?
# ============================================================
print("\n=== Why Positional-Only? ===")

# Makes function more flexible for internal use
# Protects parameter names from being used as keywords

# Example: Mathematical functions
def power(base, /, exponent):
    """base is positional-only."""
    return base ** exponent

# Valid
print(power(2, 3))  # Both positional
print(power(2, exponent=3))  # exponent can be keyword

# Can't do: power(base=2, exponent=3)


# ============================================================
# Example 5: Why Use Keyword-Only?
# ============================================================
print("\n=== Why Keyword-Only? ===")

# Makes intent clearer
# Avoids confusion with positional arguments

def create_user(name, /, *, email, age):
    """name positional, email and age keyword-only."""
    return {"name": name, "email": email, "age": age}

# Clear what's what
user = create_user("John", email="john@example.com", age=25)
print(f"User: {user}")

# More readable than:
# create_user("John", "john@example.com", 25)


# ============================================================
# Example 6: Default Values with Special Markers
# ============================================================
print("\n=== Default Values ===")

def func_with_defaults(a, b=10, /, c=20, *, d=30):
    """Various defaults."""
    return f"a={a}, b={b}, c={c}, d={d}"

# Test defaults
print(func_with_defaults(1))              # All defaults
print(func_with_defaults(1, 2))           # a=1, b=2, rest default
print(func_with_defaults(1, 2, 3))       # a=1, b=2, c=3
print(func_with_defaults(1, 2, 3, d=4))  # All specified


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Function Signatures")
print("=" * 50)
print("""
Positional-Only (before /):
  - Must be passed by position
  - Cannot use keyword argument
  - Good for: protecting parameter names

Keyword-Only (after *):
  - Must be passed by keyword
  - Good for: required options, clarity

Combined:
  def f(a, /, b, *, c):
    - a: positional-only
    - b: either
    - c: keyword-only
""")
