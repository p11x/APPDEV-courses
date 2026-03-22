# Example207.py
# Topic: Advanced List Comprehensions - Filtering Patterns

# This file demonstrates advanced filtering patterns with list comprehensions.


# ============================================================
# Example 1: Filter Prime Numbers
# ============================================================
print("=== Filter Primes ===")

def is_prime(n):
    if n < 2: return False
    for i in range(2, int(n**0.5) + 1):
        if n % i == 0: return False
    return True

numbers = range(2, 20)
primes = [x for x in numbers if is_prime(x)]
print(f"Primes: {primes}")


# ============================================================
# Example 2: Filter Palindromes
# ============================================================
print("\n=== Filter Palindromes ===")

words = ["radar", "hello", "level", "world", "civic"]
palindromes = [w for w in words if w == w[::-1]]
print(f"Palindromes: {palindromes}")


# ============================================================
# Example 3: Filter Anagrams
# ============================================================
print("\n=== Filter Anagrams ===")

words = ["listen", "silent", "hello", "world", "enlist"]
sorted_words = {tuple(sorted(w)): w for w in words}
anagrams = [sorted_words[k] for k in sorted_words if words.count(sorted_words[k]) > 1]
print(f"Anagrams: {anagrams}")


# ============================================================
# Example 4: Filter by Sum
# ============================================================
print("\n=== Filter by Digit Sum ===")

numbers = [11, 22, 33, 44, 55, 123, 456]
digit_sum = lambda x: sum(int(d) for d in str(x))
filtered = [x for x in numbers if digit_sum(x) > 5]
print(f"Digit sum > 5: {filtered}")


# ============================================================
# Example 5: Filter Nested List
# ============================================================
print("\n=== Filter Nested ===")

nested = [[1, 2, 3], [4, 5], [6, 7, 8, 9]]
filtered = [x for sublist in nested for x in sublist if x > 4]
print(f"Values > 4: {filtered}")


# ============================================================
# Example 6: Filter Dictionary
# ============================================================
print("\n=== Filter Dict ===")

d = {"apple": 5, "banana": 2, "cherry": 7, "date": 1}
filtered = {k: v for k, v in d.items() if v > 3}
print(f"Values > 3: {filtered}")


# ============================================================
# Example 7: Filter Multiple Conditions
# ============================================================
print("\n=== Multiple Conditions ===")

numbers = range(-10, 11)
filtered = [x for x in numbers if x > 0 and x % 2 == 0 and x % 3 == 0]
print(f"Positive multiples of 2 and 3: {filtered}")


# ============================================================
# Example 8: Filter with Any/All
# ============================================================
print("\n=== Any/All Filter ===")

words = ["hello", "world", "python", "good"]
has_o = [w for w in words if any(c in 'aeiou' for c in w)]
print(f"Has vowel: {has_o}")


# ============================================================
# Example 9: Filter Unique
# ============================================================
print("\n=== Unique Filter ===")

items = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4]
unique = [x for i, x in enumerate(items) if x not in items[:i]]
print(f"Unique: {unique}")


# ============================================================
# Example 10: Filter by Function Result
# ============================================================
print("\n=== Filter by Function ===")

strings = ["hello", "world", "python", "hi"]
filtered = [s for s in strings if len(s) > 4 and s[0] in 'hp']
print(f"Long, starts with h/p: {filtered}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ADVANCED FILTERING:
- Complex conditions with and/or
- Use any()/all() for membership
- Filter nested structures
- Filter dict by value
""")
