# Example157.py
# Topic: Advanced Iteration and Control Flow Patterns


# ============================================================
# Example 1: Generator Expression
# ============================================================
print("=== Generator Expression ===")

gen = (x**2 for x in range(5))
print(f"Generator: {gen}")
print(f"First call: {next(gen)}")
print(f"Second call: {next(gen)}")

for val in gen:
    print(val, end=" ")
print()


# ============================================================
# Example 2: Using iter() with Sentinel
# ============================================================
print("\n=== iter with Sentinel ===")

def read_chunks():
    chunks = ["chunk1", "chunk2", "chunk3", ""]
    for chunk in chunks:
        yield chunk

chunks_iter = iter(read_chunks())
sentinel = ""

for chunk in iter(lambda: next(chunks_iter), sentinel):
    print(f"Processing: {chunk}")


# ============================================================
# Example 3: Using next() with Default
# ============================================================
print("\n=== next with Default ===")

items = [1, 2, 3]
iterator = iter(items)

print(next(iterator, "default"))
print(next(iterator, "default"))
print(next(iterator, "default"))
print(next(iterator, "default"))


# ============================================================
# Example 4: Walrus Operator (:=)
# ============================================================
print("\n=== Walrus Operator ===")

data = [1, 2, 3, 4, 5]

if (n := len(data)) > 3:
    print(f"List has {n} items (more than 3)")

while (line := input("Enter: ") if False else "test") != "quit":
    print(f"Got: {line}")


# ============================================================
# Example 5: List Destructuring in Loop
# ============================================================
print("\n=== Destructuring in Loop ===")

pairs = [(1, 2), (3, 4), (5, 6)]

for a, b in pairs:
    print(f"a={a}, b={b}, sum={a+b}")

data = [
    {"name": "Alice", "age": 30},
    {"name": "Bob", "age": 25},
]

for {"name": name, "age": age} in data:
    print(f"{name}: {age}")


# ============================================================
# Example 6: Matrix Iteration
# ============================================================
print("\n=== Matrix Iteration ===")

matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9],
]

print("Row by row:")
for row in matrix:
    print(row)

print("\nElement by element:")
for row in matrix:
    for elem in row:
        print(elem, end=" ")
    print()


# ============================================================
# Example 7: Enumerate with Condition
# ============================================================
print("\n=== Enumerate with Condition ===")

fruits = ["apple", "banana", "cherry", "date"]

for i, fruit in enumerate(fruits):
    if i % 2 == 0:
        print(f"Index {i}: {fruit}")


# ============================================================
# Example 8: Real-World: Data Pipeline
# ============================================================
print("\n=== Data Pipeline ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

filtered = list(filter(lambda x: x % 2 == 0, numbers))
squared = list(map(lambda x: x**2, filtered))
result = sum(squared)

print(f"Original: {numbers}")
print(f"Even: {filtered}")
print(f"Squared: {squared}")
print(f"Sum: {result}")

pipeline = (
    numbers
    | (lambda x: filter(lambda n: n % 2 == 0, x))
    | (lambda x: map(lambda n: n**2, x))
    | sum
)
print(f"Pipeline sum: {pipeline}")
