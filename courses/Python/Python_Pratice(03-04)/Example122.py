# Example122.py
# Topic: Generator Expressions

# Generator expressions and lazy evaluation.


# ============================================================
# Example 1: Generator vs List
# ============================================================
print("=== Generator vs List ===")

# List - all in memory
squares_list = [x**2 for x in range(5)]
print(f"List: {squares_list}")

# Generator - lazy
squares_gen = (x**2 for x in range(5))
print(f"Generator: {squares_gen}")
print(f"Next: {next(squares_gen)}")


# ============================================================
# Example 2: Lazy Evaluation
# ============================================================
print("\n=== Lazy ===")

def expensive():
    print("Computing...")
    return 42

gen = (expensive() for x in [1])
print("Before next")
result = next(gen)
print(f"After: {result}")


# ============================================================
# Example 3: Generator to Collection
# ============================================================
print("\n=== Convert ===")

gen = (x**2 for x in range(5))

# To list
lst = list(gen)
print(f"To list: {lst}")

# To tuple
gen = (x**2 for x in range(5))
tup = tuple(gen)
print(f"To tuple: {tup}")


# ============================================================
# Example 4: sum with Generator
# ============================================================
print("\n=== Sum ===")

# Sum of first n squares
n = 1000000

# List
total = sum([x**2 for x in range(n)])

# Generator - more memory efficient
total = sum(x**2 for x in range(n))
print(f"Sum: {total}")


# ============================================================
# Example 5: Generator Functions
# ============================================================
print("\n=== Generator Function ===")

def count_up_to(n):
    i = 1
    while i <= n:
        yield i
        i += 1

for num in count_up_to(5):
    print(f"Num: {num}")
