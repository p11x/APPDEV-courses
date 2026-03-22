# Example115.py
# Topic: String Operations Deep Dive

# Advanced string operations.


# ============================================================
# Example 1: String Formatting
# ============================================================
print("=== String Formatting ===")

name = "Alice"
age = 30
price = 19.99

# F-strings
print(f"Name: {name}, Age: {age}")
print(f"Price: ${price:.2f}")

# Format spec
print(f"Pad: {42:05d}")
print(f"Float: {3.14159:.4f}")


# ============================================================
# Example 2: String Methods
# ============================================================
print("\n=== Methods ===")

s = "  Hello, World!  "

# Case
print(f"Upper: {s.upper()}")
print(f"Lower: {s.lower()}")
print(f"Title: {s.title()}")

# Whitespace
print(f"Strip: '{s.strip()}'")
print(f"Replace: {s.replace('World', 'Python')}")

# Split/Join
print(f"Split: {'a,b,c'.split(',')}")
print(f"Join: {','.join(['a', 'b', 'c'])}")


# ============================================================
# Example 3: String Searching
# ============================================================
print("\n=== Searching ===")

s = "Hello, World!"

print(f"Find: {s.find('World')}")
print(f"Index: {s.index('World')}")
print(f"Starts with: {s.startswith('Hello')}")
print(f"Ends with: {s.endswith('!')}")


# ============================================================
# Example 4: String Slicing
# ============================================================
print("\n=== Slicing ===")

s = "Hello, World!"

print(f"First 5: {s[:5]}")
print(f"Last 6: {s[-6:]}")
print(f"Reverse: {s[::-1]}")
print(f"Every 2nd: {s[::2]}")


# ============================================================
# Example 5: Practical String Operations
# ============================================================
print("\n=== Practical ===")

# Palindrome check
def is_palindrome(s):
    return s == s[::-1]

print(f"'radar' palindrome: {is_palindrome('radar')}")

# Word count
text = "hello world hello python"
words = text.split()
print(f"Word count: {len(words)}")

# Reverse words
def reverse_words(s):
    return ' '.join(s.split()[::-1])

print(f"Reverse words: {reverse_words(text)}")
