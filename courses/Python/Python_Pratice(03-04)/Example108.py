# Example108.py
# Topic: Strings - Methods

# This file demonstrates string methods.


# ============================================================
# Example 1: Case Conversion
# ============================================================
print("=== Case Conversion ===")

text = "Hello, World!"

print(f"Original: {text}")
print(f"Upper: {text.upper()}")
print(f"Lower: {text.lower()}")
print(f"Title: {text.title()}")
print(f"Capitalize: {text.capitalize()}")


# ============================================================
# Example 2: Whitespace
# ============================================================
print("\n=== Whitespace ===")

text = "  Hello, World!  "

print(f"Original: '{text}'")
print(f"Strip: '{text.strip()}'")
print(f"Lstrip: '{text.lstrip()}'")
print(f"Rstrip: '{text.rstrip()}'")


# ============================================================
# Example 3: Find and Replace
# ============================================================
print("\n=== Find and Replace ===")

text = "Hello, World!"

print(f"Find 'World': {text.find('World')}")
print(f"Replace: {text.replace('World', 'Python')}")
print(f"Split: {text.split(',')}")
print(f"Join: {'-'.join(['a', 'b', 'c'])}")


# ============================================================
# Example 4: F-strings
# ============================================================
print("\n=== F-strings ===")

name = "Alice"
age = 30
price = 19.99

print(f"Name: {name}, Age: {age}")
print(f"In 5 years: {age + 5}")
print(f"Price: ${price:.2f}")
print(f"Price: {price:,.2f}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
CASE:
- upper(), lower(), title(), capitalize()

WHITESPACE:
- strip(), lstrip(), rstrip()

FIND/REPLACE:
- find(), replace(), split(), join()

F-STRINGS:
- f"Value: {variable}"
- f"Price: ${price:.2f}"
""")
