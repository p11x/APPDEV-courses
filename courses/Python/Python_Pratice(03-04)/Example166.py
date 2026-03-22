# Example166.py
# Topic: More Control Flow Patterns


# ============================================================
# Example 1: Walrus Operator in Loops
# ============================================================
print("=== Walrus Operator ===")

data = [1, 2, 3, 4, 5]

if (n := len(data)) > 3:
    print(f"List has {n} items (more than 3)")

while (line := "test") != "quit":
    print(f"Processing: {line}")
    break

results = [y for x in range(5) if (y := x * 2) > 4]
print(f"Results: {results}")


# ============================================================
# Example 2: Named Expressions for Caching
# ============================================================
print("\n=== Caching with Walrus ===")

def expensive_operation():
    import time
    time.sleep(0.01)
    return 42

items = [1, 2, 1, 3, 2, 1]

processed = []
for item in items:
    if (result := expensive_operation()) > 0:
        processed.append((item, result))

print(f"Processed: {len(processed)} items")


# ============================================================
# Example 3: Exception Groups (Python 3.11+)
# ============================================================
print("\n=== Exception Groups ===")

try:
    raise ExceptionGroup("group", [
        ValueError("invalid value"),
        TypeError("wrong type")
    ])
except* ValueError as eg:
    print(f"ValueError group: {eg}")
except* TypeError as eg:
    print(f"TypeError group: {eg}")


# ============================================================
# Example 4: Notes for Else
# ============================================================
print("\n=== For-Else ===")

def find_prime_factors(n):
    factors = []
    d = 2
    while d * d <= n:
        while n % d == 0:
            factors.append(d)
            n //= d
        d += 1
    else:
        if n > 1:
            factors.append(n)
    return factors

print(f"Prime factors of 12: {find_prime_factors(12)}")
print(f"Prime factors of 7: {find_prime_factors(7)}")


# ============================================================
# Example 5: Loop with Index and Value
# ============================================================
print("\n=== Index and Value ===")

fruits = ["apple", "banana", "cherry"]

for i, fruit in enumerate(fruits):
    print(f"{i}: {fruit}")

data = {"a": 1, "b": 2, "c": 3}
for i, (key, value) in enumerate(data.items()):
    print(f"{i}: {key}={value}")


# ============================================================
# Example 6: Multiple Iteration Variables
# ============================================================
print("\n=== Multiple Variables ===")

points = [(1, 2), (3, 4), (5, 6)]

for x, y in points:
    print(f"x={x}, y={y}")

data = [
    {"name": "Alice", "age": 30},
    {"name": "Bob", "age": 25},
]

for i, {"name": name, "age": age} in enumerate(data):
    print(f"{i}: {name}, {age}")


# ============================================================
# Example 7: Transpose Matrix
# ============================================================
print("\n=== Transpose Matrix ===")

matrix = [
    [1, 2, 3],
    [4, 5, 6],
]

transposed = list(map(list, zip(*matrix)))
print(f"Original: {matrix}")
print(f"Transposed: {transposed}")


# ============================================================
# Example 8: Chunk List
# ============================================================
print("=== Chunk List ===")

def chunk_list(lst, size):
    for i in range(0, len(lst), size):
        yield lst[i:i + size]

data = list(range(10))
print(f"Original: {data}")
print(f"Chunked: {list(chunk_list(data, 3))}")
