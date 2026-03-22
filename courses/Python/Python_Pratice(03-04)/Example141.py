# Example141.py
# Topic: Working with Slices


# ============================================================
# Example 1: Basic Slicing
# ============================================================
print("=== Basic Slicing ===")

data = list(range(10))
print(f"Data: {data}")
print(f"data[2:5]: {data[2:5]}")
print(f"data[:3]: {data[:3]}")
print(f"data[5:]: {data[5:]}")
print(f"data[::2]: {data[::2]}")
print(f"data[::-1]: {data[::-1]}")


# ============================================================
# Example 2: Slice Objects
# ============================================================
print("\n=== Slice Objects ===")

s = slice(2, 5)
data = list(range(10))
print(f"data[s]: {data[s]}")

s2 = slice(None, None, 2)
print(f"data[::2] via slice: {data[s2]}")


# ============================================================
# Example 3: slice.indices
# ============================================================
print("\n=== slice.indices ===")

s = slice(2, 8, 2)
start, stop, step = s.indices(10)
print(f"slice(2, 8, 2).indices(10): start={start}, stop={stop}, step={step}")

data = list(range(10))
print(f"Equivalent to data[2:8:2]: {data[start:stop:step]}")


# ============================================================
# Example 4: Negative Slicing
# ============================================================
print("\n=== Negative Indices ===")

data = list("abcdefgh")
print(f"Data: {data}")
print(f"data[-1]: {data[-1]}")
print(f"data[-3:]: {data[-3:]}")
print(f"data[:-3]: {data[:-3]}")
print(f"data[-4:-1]: {data[-4:-1]}")


# ============================================================
# Example 5: Modify with Slices
# ============================================================
print("\n=== Modify with Slices ===")

data = list(range(10))
print(f"Original: {data}")

data[2:5] = [20, 30, 40]
print(f"After data[2:5] = [20,30,40]: {data}")

data[::2] = [0, 0, 0, 0, 0]
print(f"After data[::2] = 0s: {data}")


# ============================================================
# Example 6: Real-World: Pagination
# ============================================================
print("\n=== Real-World: Pagination ===")

def paginate(items, page, page_size):
    start = (page - 1) * page_size
    end = start + page_size
    return items[start:end]

items = list(range(1, 26))

for page in range(1, 4):
    page_items = paginate(items, page, 10)
    print(f"Page {page}: {page_items}")


# ============================================================
# Example 7: Matrix Slicing
# ============================================================
print("\n=== Matrix Slicing ===")

matrix = [
    [1, 2, 3, 4],
    [5, 6, 7, 8],
    [9, 10, 11, 12],
    [13, 14, 15, 16],
]

print("Middle 2x2:")
rows = slice(1, 3)
cols = slice(1, 3)
print([row[cols] for row in matrix[rows]])

print("First column:")
print([row[0] for row in matrix])
