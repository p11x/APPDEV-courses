# Example179.py
# Topic: key Parameter Deep Dive

# This file demonstrates the key parameter for custom sorting.
# The key function transforms each element for comparison,
# enabling flexible and efficient sorting strategies.


# ============================================================
# Example 1: Sort by Length
# ============================================================
print("=== Sort by Length ===")

words = ["apple", "hi", "banana", "cat", "a"]

# Sort by string length
by_length = sorted(words, key=len)    # list — sorted by length
print(f"By length: {by_length}")    # By length: ['a', 'hi', 'cat', 'apple', 'banana']

# Sort by length descending
by_length_desc = sorted(words, key=len, reverse=True)    # list — longest first
print(f"By length desc: {by_length_desc}")    # By length desc: ['banana', 'apple', 'cat', 'hi', 'a']


# ============================================================
# Example 2: Sort by Absolute Value
# ============================================================
print("\n=== Sort by Absolute Value ===")

numbers = [-5, 2, -3, 1, -8, 4, 7]

# Sort by absolute value
by_abs = sorted(numbers, key=abs)    # list — sorted by absolute value
print(f"By absolute: {by_abs}")    # By absolute: [1, 2, -3, 4, -5, 7, -8]


# ============================================================
# Example 3: Sort Dictionary by Value
# ============================================================
print("\n=== Sort Dictionary by Value ===")

prices = {"apple": 5, "banana": 2, "cherry": 7, "date": 1}

# Sort items by value
by_value = sorted(prices.items(), key=lambda x: x[1])    # list of tuples
print(f"By value: {by_value}")    # By value: [('date', 1), ('banana', 2), ('apple', 5), ('cherry', 7)]

# Sort keys by value
by_value_keys = sorted(prices.keys(), key=lambda k: prices[k])    # list
print(f"Keys by value: {by_value_keys}")    # Keys by value: ['date', 'banana', 'apple', 'cherry']


# ============================================================
# Example 4: Sort by Multiple Criteria
# ============================================================
print("\n=== Sort by Multiple Criteria ===")

data = [
    ("Alice", "B", 85),
    ("Bob", "A", 92),
    ("Carol", "A", 78),
    ("David", "B", 90),
    ("Eve", "C", 88)
]

# Sort by grade (desc), then name (asc)
sorted_multi = sorted(data, key=lambda x: (-x[2], x[0]))    # list — multi-key sort
print(f"Multi-criteria: {sorted_multi}")


# ============================================================
# Example 5: Sort Strings by Last Character
# ============================================================
print("\n=== Sort by Last Character ===")

words = ["abc", "def", "ghi", "jkl", "mno"]

# Sort by last character
by_last = sorted(words, key=lambda w: w[-1])    # list — sorted by last char
print(f"By last char: {by_last}")    # By last char: ['abc', 'def', 'ghi', 'jkl', 'mno']

# Sort by second character
by_second = sorted(words, key=lambda w: w[1] if len(w) > 1 else '')    # list
print(f"By second char: {by_second}")    # By second char: ['abc', 'def', 'ghi', 'jkl', 'mno']


# ============================================================
# Example 6: Sort with Method Reference
# ============================================================
print("\n=== Sort with Method Reference ===")

class Person:
    def __init__(self, name: str, age: int):
        self.name = name
        self.age = age
    
    def __repr__(self):
        return f"{self.name}({self.age})"

people = [
    Person("Alice", 30),
    Person("Bob", 25),
    Person("Carol", 35)
]

# Sort by age using method reference
by_age = sorted(people, key=lambda p: p.age)    # list — sorted by age
print(f"By age: {by_age}")    # By age: [Bob(25), Alice(30), Carol(35)]

# Sort by name length
by_name_len = sorted(people, key=lambda p: len(p.name))    # list
print(f"By name length: {by_name_len}")    # By name length: [Bob(25), Alice(30), Carol(35)]


# ============================================================
# Example 7: Case-Insensitive Sorting
# ============================================================
print("\n=== Case-Insensitive Sorting ===")

words = ["Banana", "apple", "CHERRY", "date"]

# Case-insensitive sort
case_insensitive = sorted(words, key=str.lower)    # list — ignore case
print(f"Case-insensitive: {case_insensitive}")    # Case-insensitive: ['apple', 'Banana', 'CHERRY', 'date']

# Original case preserved
print(f"Original: {words}")    # Original unchanged


# ============================================================
# Example 8: Sort Tuples by Multiple Elements
# ============================================================
print("\n=== Sort Tuples ===")

pairs = [(1, 3), (2, 1), (3, 2), (1, 1), (2, 3)]

# Sort by first element, then second
sorted_pairs = sorted(pairs)    # list — natural tuple sort
print(f"Default: {sorted_pairs}")    # Default: [(1, 1), (1, 3), (2, 1), (2, 3), (3, 2)]

# Sort by second element only
by_second = sorted(pairs, key=lambda p: p[1])    # list — by second element
print(f"By second: {by_second}")    # By second: [(1, 1), (2, 1), (3, 2), (1, 3), (2, 3)]


# ============================================================
# Example 9: Custom Key Functions
# ============================================================
print("\n=== Custom Key Functions ===")

def get_weekday(name: str) -> int:
    days = {"Monday": 0, "Tuesday": 1, "Wednesday": 2, 
            "Thursday": 3, "Friday": 4, "Saturday": 5, "Sunday": 6}
    return days.get(name, -1)

weekdays = ["Monday", "Friday", "Wednesday", "Sunday"]
sorted_days = sorted(weekdays, key=get_weekday)    # list — by weekday order
print(f"By weekday: {sorted_days}")    # By weekday: ['Monday', 'Wednesday', 'Friday', 'Sunday']


# ============================================================
# Example 10: Sort with Transform
# ============================================================
print("\n=== Sort with Transform ===")

files = ["file1.txt", "file10.txt", "file2.txt", "file21.txt", "file3.txt"]

# Sort naturally (file1, file2, file3 instead of file1, file10, file2)
import re

def natural_sort_key(s: str) -> tuple:
    return [int(c) if c.isdigit() else c.lower() for c in re.split(r'(\d+)', s)]

natural = sorted(files, key=natural_sort_key)    # list — natural sort
print(f"Natural sort: {natural}")    # Natural sort: ['file1.txt', 'file2.txt', 'file3.txt', 'file10.txt', 'file21.txt']


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
KEY PARAMETER:
- key=function transforms elements for comparison
- Key function called once per element
- Can use lambda, methods, or custom functions

COMMON USES:
- len: sort by length
- abs: sort by absolute value
- str.lower: case-insensitive
- lambda x: x[1]: sort tuples/dicts by index
- Multiple criteria: (-score, name) for desc/asc

TIPS:
- Keep key function simple for performance
- Use negative for descending sort
- Method references (str.lower) work as keys
""")
