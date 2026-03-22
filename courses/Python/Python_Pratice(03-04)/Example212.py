# Example212.py
# Topic: Counter Advanced Patterns

# This file demonstrates advanced Counter patterns for data analysis.


# ============================================================
# Example 1: Most Common
# ============================================================
print("=== Most Common ===")

from collections import Counter

c = Counter("abracadabra")
print(f"Top 3: {c.most_common(3)}")


# ============================================================
# Example 2: Elements
# ============================================================
print("\n=== Elements ===")

c = Counter({"a": 3, "b": 1})
print(f"Elements: {list(c.elements())}")


# ============================================================
# Example 3: Counter Arithmetic
# ============================================================
print("\n=== Arithmetic ===")

c1 = Counter({"a": 3, "b": 1})
c2 = Counter({"a": 1, "b": 2, "c": 1})
print(f"Add: {c1 + c2}")
print(f"Subtract: {c1 - c2}")


# ============================================================
# Example 4: Intersection
# ============================================================
print("\n=== Intersection ===")

c1 = Counter({"a": 3, "b": 2})
c2 = Counter({"a": 1, "b": 3})
print(f"Min: {c1 & c2}")


# ============================================================
# Example 5: Union
# ============================================================
print("\n=== Union ===")

c1 = Counter({"a": 3, "b": 2})
c2 = Counter({"a": 1, "b": 3})
print(f"Max: {c1 | c2}")


# ============================================================
# Example 6: Update Counter
# ============================================================
print("\n=== Update ===")

c = Counter()
c.update("abracadabra")
print(f"Updated: {c}")


# ============================================================
# Example 7: Filter Counter
# ============================================================
print("\n=== Filter ===")

c = Counter({"a": 5, "b": -1, "c": 3})
print(f"Positive: {+c}")


# ============================================================
# Example 8: Word Frequency
# ============================================================
print("\n=== Word Frequency ===")

text = "the quick brown fox jumps over the lazy dog"
words = text.split()
c = Counter(words)
print(f"Most common: {c.most_common(2)}")


# ============================================================
# Example 9: Character Frequency
# ============================================================
print("\n=== Character Frequency ===")

from collections import Counter
c = Counter("hello world")
print(f"Chars: {c}")


# ============================================================
# Example 10: Counter to Dict
# ============================================================
print("\n=== To Dict ===")

c = Counter({"a": 3, "b": 1})
print(f"Dict: {dict(c)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
COUNTER ADVANCED:
- most_common(n): Top n
- elements(): Repeat elements
- +, -: Add/subtract
- &: Min, |: Max
- +c: Filter positive
""")
