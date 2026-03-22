# Example194.py
# Topic: Generator Expressions

# This file demonstrates generator expressions - memory-efficient
# iterables that yield values on demand.


# ============================================================
# Example 1: Basic Generator
# ============================================================
print("=== Basic Generator ===")

gen = (x**2 for x in range(5))
print(f"Generator: {gen}")    # generator object
print(f"List: {list(gen)}")    # [0, 1, 4, 9, 16]


# ============================================================
# Example 2: Lazy Evaluation
# ============================================================
print("\n=== Lazy Evaluation ===")

gen = (x**2 for x in range(1000000))
print(f"First: {next(gen)}")    # 0 - doesn't create full list


# ============================================================
# Example 3: Memory Efficient
# ============================================================
print("\n=== Memory Efficient ===")

import sys

list_comp = [x**2 for x in range(1000)]
gen_expr = (x**2 for x in range(1000))

print(f"List size: {sys.getsizeof(list_comp)} bytes")
print(f"Generator size: {sys.getsizeof(gen_expr)} bytes")


# ============================================================
# Example 4: With Function
# ============================================================
print("\n=== With Function ===")

def process(nums):
    return sum(nums)

numbers = (x for x in range(10))
result = process(numbers)
print(f"Sum: {result}")    # 45


# ============================================================
# Example 5: Generator with Condition
# ============================================================
print("\n=== With Condition ===")

evens = (x for x in range(10) if x % 2 == 0)
print(f"Evens: {list(evens)}")    # [0, 2, 4, 6, 8]


# ============================================================
# Example 6: Generator vs List Comprehension
# ============================================================
print("\n=== Generator vs List ===")

import time

start = time.time()
result = sum([x**2 for x in range(100000)])
print(f"List: {time.time() - start:.4f}s")

start = time.time()
result = sum(x**2 for x in range(100000))
print(f"Generator: {time.time() - start:.4f}s")


# ============================================================
# Example 7: Generator with zip
# ============================================================
print("\n=== With zip ===")

names = ["Alice", "Bob", "Carol"]
ages = [25, 30, 35]
combined = (f"{n}: {a}" for n, a in zip(names, ages))
print(f"Combined: {list(combined)}")


# ============================================================
# Example 8: Infinite Generator
# ============================================================
print("\n=== Counter Generator ===")

def counter():
    n = 0
    while True:
        yield n
        n += 1

gen = counter()
for _ in range(5):
    print(next(gen), end=" ")
print()


# ============================================================
# Example 9: Generator Pipeline
# ============================================================
print("\n=== Pipeline ===")

data = (x for x in range(10))
filtered = (x for x in data if x > 3)
doubled = (x * 2 for x in filtered)
print(f"Pipeline: {list(doubled)}")    # [8, 10, 12, 14, 16, 18]


# ============================================================
# Example 10: Sending Values to Generator
# ============================================================
print("\n=== Send to Generator ===")

def echo():
    while True:
        received = yield
        print(f"Received: {received}")

gen = echo()
next(gen)
gen.send("Hello")
gen.send("World")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
GENERATOR EXPRESSION:
(x**2 for x in range(5))

ADVANTAGES:
- Lazy evaluation
- Memory efficient
- Can be infinite

VS LIST:
- Generators: O(1) memory
- Lists: O(n) memory
""")
