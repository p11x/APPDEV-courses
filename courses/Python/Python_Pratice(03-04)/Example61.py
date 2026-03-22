# Example61.py
# Topic: filter() Function - Selecting Items

# This file demonstrates filter() function for selecting items from iterables.


# ============================================================
# Example 1: Basic filter() with lambda
# ============================================================
print("=== Basic filter() with lambda ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

evens = list(filter(lambda x: x % 2 == 0, numbers))
print(f"Original: {numbers}")
print(f"Even numbers: {evens}")

odds = list(filter(lambda x: x % 2 != 0, numbers))
print(f"Odd numbers: {odds}")


# ============================================================
# Example 2: filter() with comparison conditions
# ============================================================
print("\n=== filter() with comparison conditions ===")

numbers = [-5, -3, 0, 2, 5, 8, 10, 15]

positive = list(filter(lambda x: x > 0, numbers))
print(f"Positive: {positive}")

non_zero = list(filter(lambda x: x != 0, numbers))
print(f"Non-zero: {non_zero}")

greater_than_5 = list(filter(lambda x: x > 5, numbers))
print(f"Greater than 5: {greater_than_5}")


# ============================================================
# Example 3: filter() with strings
# ============================================================
print("\n=== filter() with strings ===")

words = ['hello', 'world', 'python', 'hi', 'code', 'programming']

long_words = list(filter(lambda w: len(w) > 5, words))
print(f"Words > 5 chars: {long_words}")

starts_with_p = list(filter(lambda w: w.startswith('p'), words))
print(f"Starts with 'p': {starts_with_p}")

contains_o = list(filter(lambda w: 'o' in w, words))
print(f"Contains 'o': {contains_o}")


# ============================================================
# Example 4: filter() with None (identity)
# ============================================================
print("\n=== filter() with None ===")

values = [0, 1, '', 'hello', None, [], [1, 2], False, True]

# filter(None) keeps truthy values
truthy = list(filter(None, values))
print(f"Truthy values: {truthy}")
print(f"Original: {values}")


# ============================================================
# Example 5: filter() with complex conditions
# ============================================================
print("\n=== filter() with complex conditions ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Divisible by both 2 and 3
div_2_and_3 = list(filter(lambda x: x % 2 == 0 and x % 3 == 0, numbers))
print(f"Divisible by 2 and 3: {div_2_and_3}")

# In range [3, 7]
in_range = list(filter(lambda x: 3 <= x <= 7, numbers))
print(f"In range [3, 7]: {in_range}")

# Multiple of 3 or 5
mult_3_or_5 = list(filter(lambda x: x % 3 == 0 or x % 5 == 0, numbers))
print(f"Multiple of 3 or 5: {mult_3_or_5}")


# ============================================================
# Example 6: filter() with dictionaries
# ============================================================
print("\n=== filter() with dictionaries ===")

users = [
    {'name': 'Alice', 'age': 25, 'active': True},
    {'name': 'Bob', 'age': 17, 'active': True},
    {'name': 'Charlie', 'age': 30, 'active': False},
    {'name': 'Diana', 'age': 22, 'active': True},
]

active_users = list(filter(lambda u: u['active'], users))
print(f"Active users: {[u['name'] for u in active_users]}")

adults = list(filter(lambda u: u['age'] >= 18, users))
print(f"Adult users: {[u['name'] for u in adults]}")


# ============================================================
# Example 7: filter() with named functions
# ============================================================
print("\n=== filter() with named functions ===")

def is_palindrome(s):
    return s == s[::-1]

def is_prime(n):
    if n < 2:
        return False
    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False
    return True

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]

prime_numbers = list(filter(is_prime, numbers))
print(f"Prime numbers: {prime_numbers}")

words = ['radar', 'hello', 'level', 'world', 'civic']
palindromes = list(filter(is_palindrome, words))
print(f"Palindromes: {palindromes}")


# ============================================================
# Example 8: filter() vs list comprehension
# ============================================================
print("\n=== filter() vs comprehension ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Using filter
filter_result = list(filter(lambda x: x > 5, numbers))

# Using comprehension
comp_result = [x for x in numbers if x > 5]

print(f"filter() result: {filter_result}")
print(f"Comprehension: {comp_result}")
print(f"Equal: {filter_result == comp_result}")


# ============================================================
# Example 9: filter() with indices
# ============================================================
print("\n=== filter() with indices ===")

data = [10, 20, 30, 40, 50]

# Keep only items at even indices
even_index = list(filter(lambda x: data.index(x) % 2 == 0, data))
print(f"Even index values: {even_index}")

# Using enumerate
even_index2 = [v for i, v in enumerate(data) if i % 2 == 0]
print(f"Even index (enumerate): {even_index2}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: filter()")
print("=" * 50)
print("""
- filter() keeps items that satisfy a condition
- Returns an iterator (use list() to materialize)
- Use with lambda or named functions
- filter(None) keeps all truthy values
- Often replaceable with list comprehensions
""")
