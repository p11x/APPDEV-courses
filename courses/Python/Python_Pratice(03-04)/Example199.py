# Example199.py
# Topic: Sorting - Basics, keys, stable

# This file demonstrates sorting basics including sorted(), .sort(),
# key functions, and stability.


# ============================================================
# Example 1: sorted() vs .sort()
# ============================================================
print("=== sorted() vs .sort() ===")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

sorted_nums = sorted(numbers)
print(f"sorted(): {sorted_nums}")    # New list
print(f"Original: {numbers}")    # Unchanged

numbers.sort()
print(f".sort(): {numbers}")    # Modified in place


# ============================================================
# Example 2: Reverse Sort
# ============================================================
print("\n=== Reverse ===")

numbers = [3, 1, 4, 1, 5]
desc = sorted(numbers, reverse=True)
print(f"Descending: {desc}")    # [5, 4, 3, 1, 1]


# ============================================================
# Example 3: Key Function
# ============================================================
print("\n=== Key Function ===")

words = ["apple", "hi", "banana", "cat"]
by_len = sorted(words, key=len)
print(f"By length: {by_len}")    # ['hi', 'cat', 'apple', 'banana']


# ============================================================
# Example 4: Key with abs
# ============================================================
print("\n=== Key abs ===")

numbers = [-5, 2, -3, 1, -8, 4]
by_abs = sorted(numbers, key=abs)
print(f"By abs: {by_abs}")    # [1, 2, -3, 4, -5, -8]


# ============================================================
# Example 5: Multiple Keys
# ============================================================
print("\n=== Multiple Keys ===")

data = [("apple", 3), ("banana", 1), ("cherry", 2)]
sorted_data = sorted(data, key=lambda x: x[1])
print(f"By second: {sorted_data}")    # [('banana', 1), ('cherry', 2), ('apple', 3)]


# ============================================================
# Example 6: Descending with Key
# ============================================================
print("\n=== Descending Key ===")

words = ["apple", "banana", "cherry"]
by_len_desc = sorted(words, key=len, reverse=True)
print(f"By length desc: {by_len_desc}")    # ['banana', 'cherry', 'apple']


# ============================================================
# Example 7: Stable Sort
# ============================================================
print("\n=== Stable Sort ===")

data = [("a", 3), ("b", 1), ("c", 3), ("d", 2)]
sorted_data = sorted(data, key=lambda x: x[1])
print(f"Stable: {sorted_data}")    # Order preserved for equal keys


# ============================================================
# Example 8: Tuple Sorting
# ============================================================
print("\n=== Tuple Sorting ===")

pairs = [(1, 3), (2, 1), (3, 2), (1, 1)]
print(f"Default: {sorted(pairs)}")    # [(1, 1), (1, 3), (2, 1), (3, 2)]
print(f"By 2nd: {sorted(pairs, key=lambda x: x[1])}")    # [(1,1), (2,1), (3,2), (1,3)]


# ============================================================
# Example 9: Case Insensitive
# ============================================================
print("\n=== Case Insensitive ===")

words = ["Apple", "banana", "CHERRY"]
sorted_words = sorted(words, key=str.lower)
print(f"Case-insensitive: {sorted_words}")    # ['Apple', 'banana', 'CHERRY']


# ============================================================
# Example 10: Sort Dict
# ============================================================
print("\n=== Sort Dict ===")

d = {"banana": 3, "apple": 1, "cherry": 2}
by_key = sorted(d.keys())
by_value = sorted(d.items(), key=lambda x: x[1])
print(f"By key: {by_key}")    # ['apple', 'banana', 'cherry']
print(f"By value: {by_value}")    # [('apple', 1), ('cherry', 2), ('banana', 3)]


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
SORTING:
- sorted(list): Returns new sorted list
- list.sort(): Sorts in place
- key=function: Custom sort
- reverse=True: Descending
- Stable: Equal keys preserve order
""")
