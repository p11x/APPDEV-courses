# Example149.py
# Topic: For Loops


# ============================================================
# Example 1: Basic For Loop
# ============================================================
print("=== Basic For Loop ===")

for i in range(5):
    print(f"i = {i}")


# ============================================================
# Example 2: Iterating Over List
# ============================================================
print("\n=== Iterate Over List ===")

fruits: list[str] = ["apple", "banana", "cherry"]

for fruit in fruits:
    print(f"Fruit: {fruit}")


# ============================================================
# Example 3: Iterating Over String
# ============================================================
print("\n=== Iterate Over String ===")

for char in "Python":
    print(f"Character: {char}")


# ============================================================
# Example 4: Iterating Over Dictionary
# ============================================================
print("\n=== Iterate Over Dict ===")

person: dict[str, str] = {"name": "Alice", "age": "30", "city": "NYC"}

for key in person:
    print(f"{key}: {person[key]}")

print("\nUsing items():")
for key, value in person.items():
    print(f"{key} = {value}")


# ============================================================
# Example 5: Using Enumerate
# ============================================================
print("\n=== Using Enumerate ===")

fruits: list[str] = ["apple", "banana", "cherry"]

for index, fruit in enumerate(fruits):
    print(f"{index}: {fruit}")

print("\nWith start index:")
for index, fruit in enumerate(fruits, start=1):
    print(f"{index}: {fruit}")


# ============================================================
# Example 6: For Loop with Else
# ============================================================
print("\n=== For-Else ===")

numbers: list[int] = [1, 2, 3, 4, 5]

for num in numbers:
    if num > 10:
        print("Found number > 10")
        break
else:
    print("No number > 10 found")


# ============================================================
# Example 7: Nested For Loops
# ============================================================
print("\n=== Nested For Loops ===")

for i in range(3):
    for j in range(3):
        print(f"({i}, {j})", end=" ")
    print()


# ============================================================
# Example 8: List Comprehension with For
# ============================================================
print("\n=== List Comprehension ===")

squares: list[int] = [x**2 for x in range(5)]
print(f"Squares: {squares}")

even: list[int] = [x for x in range(10) if x % 2 == 0]
print(f"Even numbers: {even}")


# ============================================================
# Example 9: Real-World: Processing Data
# ============================================================
print("\n=== Real-World: Process Data ===")

products: list[dict] = [
    {"name": "Laptop", "price": 999},
    {"name": "Mouse", "price": 29},
    {"name": "Keyboard", "price": 79},
]

total: float = 0
for product in products:
    total += product["price"]
    print(f"{product['name']}: ${product['price']}")

print(f"Total: ${total}")
