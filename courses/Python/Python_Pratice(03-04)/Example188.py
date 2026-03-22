# Example188.py
# Topic: List Comprehensions - With Conditionals

# This file demonstrates list comprehensions with conditional filtering
# using if clauses to filter elements during list creation.


# ============================================================
# Example 1: Filter Even Numbers
# ============================================================
print("=== Filter Even Numbers ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
evens = [x for x in numbers if x % 2 == 0]
print(f"Evens: {evens}")    # [2, 4, 6, 8, 10]


# ============================================================
# Example 2: Filter by Length
# ============================================================
print("\n=== Filter by Length ===")

words = ["cat", "elephant", "dog", "mouse"]
long_words = [w for w in words if len(w) > 3]
print(f"Long words: {long_words}")    # ['elephant', 'mouse']


# ============================================================
# Example 3: Filter Strings
# ============================================================
print("\n=== Filter Strings ===")

items = [1, "hello", 2.5, "world", 3, "python"]
strings = [s for s in items if isinstance(s, str)]
print(f"Strings: {strings}")    # ['hello', 'world', 'python']


# ============================================================
# Example 4: Filter with Multiple Conditions
# ============================================================
print("\n=== Multiple Conditions ===")

numbers = range(-5, 6)
positive_even = [x for x in numbers if x > 0 and x % 2 == 0]
print(f"Positive even: {positive_even}")    # [2, 4]


# ============================================================
# Example 5: Filter with OR Condition
# ============================================================
print("\n=== OR Condition ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
divisible = [x for x in numbers if x % 3 == 0 or x % 5 == 0]
print(f"Divisible by 3 or 5: {divisible}")    # [3, 5, 6, 9, 10]


# ============================================================
# Example 6: Filter with Negation
# ============================================================
print("\n=== Negation ===")

words = ["start", "end", "beginning", "finish"]
short = [w for w in words if not len(w) > 5]
print(f"Not long: {short}")    # ['start', 'end']


# ============================================================
# Example 7: Complex Filter with Function
# ============================================================
print("\n=== Complex Filter ===")

def is_prime(n):
    if n < 2:
        return False
    for i in range(2, int(n**0.5) + 1):
        if n % i == 0:
            return False
    return True

numbers = range(2, 20)
primes = [x for x in numbers if is_prime(x)]
print(f"Primes: {primes}")    # [2, 3, 5, 7, 11, 13, 17, 19]


# ============================================================
# Example 8: Filter and Transform
# ============================================================
print("\n=== Filter and Transform ===")

numbers = [1, 2, 3, 4, 5, 6]
squared_evens = [x**2 for x in numbers if x % 2 == 0]
print(f"Squared evens: {squared_evens}")    # [4, 16, 36]


# ============================================================
# Example 9: Conditional Expression with if-else
# ============================================================
print("\n=== Conditional Expression ===")

numbers = [1, 2, 3, 4, 5]
labeled = ["even" if x % 2 == 0 else "odd" for x in numbers]
print(f"Labeled: {labeled}")    # ['odd', 'even', 'odd', 'even', 'odd']


# ============================================================
# Example 10: Nested Conditionals
# ============================================================
print("\n=== Nested Conditionals ===")

data = [(1, "a"), (2, "b"), (3, "a"), (4, "c"), (5, "b")]
filtered = [(n, ch) for n, ch in data if n > 2 if ch in "ab"]
print(f"Filtered: {filtered}")    # [(3, 'a'), (5, 'b')]


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
CONDITIONAL LIST COMPREHENSION:
- Filter: [x for x in list if condition]
- Multiple: [x for x in list if cond1 and cond2]
- Transform: [x**2 for x in list if cond]
- If-else: [a if cond else b for x in list]

KEY POINTS:
- if filters after transform
- if-else transforms based on condition
- Can chain multiple ifs (AND)
""")
