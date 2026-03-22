# Example152.py
# Topic: Exception Groups and Iteration Tools


# ============================================================
# Example 1: Exception Groups (Python 3.11+)
# ============================================================
print("=== Exception Groups ===")

try:
    raise ExceptionGroup("group", [ValueError(), TypeError()])
except* ValueError:
    print("Caught ValueError")
except* TypeError:
    print("Caught TypeError")


# ============================================================
# Example 2: Enumerate Basics
# ============================================================
print("\n=== Enumerate ===")

fruits: list[str] = ["apple", "banana", "cherry"]

for index, fruit in enumerate(fruits):
    print(f"{index}: {fruit}")

print("\nWith start index:")
for index, fruit in enumerate(fruits, start=1):
    print(f"{index}: {fruit}")


# ============================================================
# Example 3: Zip Basics
# ============================================================
print("\n=== Zip ===")

names: list[str] = ["Alice", "Bob", "Charlie"]
ages: list[int] = [25, 30, 35]

for name, age in zip(names, ages):
    print(f"{name}: {age}")

print("\nZip with different lengths:")
short: list = [1, 2]
long: list = [1, 2, 3, 4]
for a, b in zip(short, long):
    print(f"{a}, {b}")


# ============================================================
# Example 4: Zip Longest
# ============================================================
print("\n=== Zip Longest ===")

from itertools import zip_longest

a: list = [1, 2]
b: list = ["a", "b", "c", "d"]

for item in zip_longest(a, b, fillvalue="?"):
    print(item)


# ============================================================
# Example 5: Any and All
# ============================================================
print("\n=== Any and All ===")

numbers: list[int] = [1, 2, 3, 4, 5]

print(f"Any > 3: {any(x > 3 for x in numbers)}")
print(f"All > 0: {all(x > 0 for x in numbers)}")
print(f"Any > 10: {any(x > 10 for x in numbers)}")

empty: list = []
print(f"Any in empty: {any(x for x in empty)}")
print(f"All in empty: {all(x for x in empty)}")


# ============================================================
# Example 6: Sorted and Reversed
# ============================================================
print("\n=== Sorted and Reversed ===")

numbers: list[int] = [3, 1, 4, 1, 5, 9, 2, 6]

print(f"Sorted: {sorted(numbers)}")
print(f"Sorted reverse: {sorted(numbers, reverse=True)}")

print(f"Reversed: {list(reversed(numbers))}")

words: list[str] = ["banana", "apple", "cherry"]
print(f"Sorted by length: {sorted(words, key=len)}")


# ============================================================
# Example 7: Map and Filter
# ============================================================
print("\n=== Map and Filter ===")

numbers: list[int] = [1, 2, 3, 4, 5]

squared: list = list(map(lambda x: x**2, numbers))
print(f"Squared: {squared}")

evens: list = list(filter(lambda x: x % 2 == 0, numbers))
print(f"Evens: {evens}")


# ============================================================
# Example 8: Real-World: Data Processing
# ============================================================
print("\n=== Real-World: Data Processing ===")

users: list[dict] = [
    {"name": "Alice", "age": 25},
    {"name": "Bob", "age": 30},
    {"name": "Charlie", "age": 35},
]

names: list = list(map(lambda u: u["name"], users))
print(f"Names: {names}")

adults: list = list(filter(lambda u: u["age"] >= 30, users))
print(f"Adults: {adults}")

for i, user in enumerate(users, 1):
    print(f"{i}. {user['name']} - {user['age']}")
