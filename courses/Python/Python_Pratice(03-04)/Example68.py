# Example68.py
# Topic: itertools - groupby, compress, takewhile, dropwhile

# This file demonstrates additional itertools functions.


# ============================================================
# Example 1: itertools.groupby()
# ============================================================
print("=== itertools.groupby() ===")

from itertools import groupby

# Group consecutive items
data = [1, 1, 1, 2, 2, 3, 3, 3, 3, 1]

for key, group in groupby(data):
    print(f"Key: {key}, Group: {list(group)}")

# Group by attribute
people = [
    {'name': 'Alice', 'dept': 'IT'},
    {'name': 'Bob', 'dept': 'IT'},
    {'name': 'Charlie', 'dept': 'HR'},
    {'name': 'Diana', 'dept': 'HR'},
    {'name': 'Eve', 'dept': 'IT'},
]

print("\nGrouped by department:")
for dept, group in groupby(people, key=lambda x: x['dept']):
    print(f"  {dept}: {[p['name'] for p in group]}")


# ============================================================
# Example 2: groupby with sorting
# ============================================================
print("\n=== groupby requires sorting ===")

from itertools import groupby

# Not sorted - groups consecutive items
data = [('IT', 'Alice'), ('HR', 'Bob'), ('IT', 'Charlie'), ('HR', 'Diana')]
print("Without sorting:")
for key, group in groupby(data, key=lambda x: x[0]):
    print(f"  {key}: {list(group)}")

# Sorted first
sorted_data = sorted(data, key=lambda x: x[0])
print("\nWith sorting:")
for key, group in groupby(sorted_data, key=lambda x: x[0]):
    print(f"  {key}: {list(group)}")


# ============================================================
# Example 3: itertools.compress()
# ============================================================
print("\n=== itertools.compress() ===")

from itertools import compress

# Select items based on selectors
data = ['A', 'B', 'C', 'D', 'E']
selectors = [1, 0, 1, 0, 1]

result = list(compress(data, selectors))
print(f"compress(['A','B','C','D','E'], [1,0,1,0,1]): {result}")

# Using booleans
selectors = [True, False, True, True, False]
result = list(compress(data, selectors))
print(f"compress with booleans: {result}")


# ============================================================
# Example 4: itertools.takewhile()
# ============================================================
print("\n=== itertools.takewhile() ===")

from itertools import takewhile

numbers = [1, 3, 5, 6, 7, 8, 9]

# Take while condition is true
result = list(takewhile(lambda x: x < 7, numbers))
print(f"takewhile(x < 7, {numbers}): {result}")

# Take positive numbers
result = list(takewhile(lambda x: x > 0, [5, 4, 3, -1, -2]))
print(f"takewhile positive: {result}")


# ============================================================
# Example 5: itertools.dropwhile()
# ============================================================
print("\n=== itertools.dropwhile() ===")

from itertools import dropwhile

numbers = [1, 3, 5, 6, 7, 8, 9]

# Drop while condition is true, then take rest
result = list(dropwhile(lambda x: x < 7, numbers))
print(f"dropwhile(x < 7, {numbers}): {result}")

# Drop until condition breaks
result = list(dropwhile(lambda x: x < 7, [1, 2, 6, 7, 8]))
print(f"dropwhile on [1,2,6,7,8]: {result}")


# ============================================================
# Example 6: Real-world - Data processing
# ============================================================
print("\n=== Real-world: Data Processing ===")

from itertools import groupby, takewhile, dropwhile

# Process log entries until error
logs = [
    {'time': 1, 'msg': 'Started'},
    {'time': 2, 'msg': 'Processing'},
    {'time': 3, 'msg': 'Working'},
    {'time': 4, 'msg': 'Error'},
    {'time': 5, 'msg': 'Failed'},
]

successful = list(takewhile(lambda x: 'Error' not in x['msg'], logs))
print(f"Successful logs: {[l['msg'] for l in successful]}")

# Group sales by month
sales = [
    {'month': 'Jan', 'amount': 100},
    {'month': 'Jan', 'amount': 150},
    {'month': 'Feb', 'amount': 200},
    {'month': 'Feb', 'amount': 175},
    {'month': 'Mar', 'amount': 300},
]

sales_sorted = sorted(sales, key=lambda x: x['month'])
monthly_totals = []
for month, group in groupby(sales_sorted, key=lambda x: x['month']):
    total = sum(s['amount'] for s in group)
    monthly_totals.append((month, total))

print(f"Monthly totals: {monthly_totals}")


# ============================================================
# Example 7: Filter with compress
# ============================================================
print("\n=== Real-world: Filter with compress ===")

from itertools import compress

# Filter products by availability
products = ['Laptop', 'Phone', 'Tablet', 'Watch', 'Speaker']
available = [True, True, False, True, False]

in_stock = list(compress(products, available))
print(f"In stock: {in_stock}")

# Filter by multiple criteria
price = [1000, 500, 300, 200, 150]
budget = 500
affordable = list(compress(products, [p <= budget for p in price]))
print(f"Affordable (≤$500): {affordable}")


# ============================================================
# Example 8: Split data with dropwhile
# ============================================================
print("\n=== Real-world: Split with dropwhile ===")

from itertools import dropwhile, takewhile

# Parse header and data
lines = [
    '=== Header ===',
    'Name,Age',
    'Alice,25',
    'Bob,30',
    '=== Footer ===',
]

header = list(takewhile(lambda x: not x.startswith('==='), lines))
print(f"Header lines: {header}")

# Find data section
data_start = dropwhile(lambda x: not x.startswith('==='), lines)
data_section = list(dropwhile(lambda x: x.startswith('==='), data_start))
print(f"Data section: {data_section}")


# ============================================================
# Example 9: Advanced groupby
# ============================================================
print("\n=== Advanced groupby ===")

from itertools import groupby

# Group by multiple keys (composite)
transactions = [
    {'type': 'credit', 'amount': 100, 'day': 'Mon'},
    {'type': 'debit', 'amount': 50, 'day': 'Mon'},
    {'type': 'credit', 'amount': 200, 'day': 'Tue'},
    {'type': 'debit', 'amount': 30, 'day': 'Tue'},
    {'type': 'credit', 'amount': 150, 'day': 'Mon'},
]

# Group by day, then calculate totals
sorted_tx = sorted(transactions, key=lambda x: x['day'])
for day, day_group in groupby(sorted_tx, key=lambda x: x['day']):
    credits = sum(t['amount'] for t in day_group if t['type'] == 'credit')
    print(f"{day}: Credits = ${credits}")


# ============================================================
# Example 10: Chaining iterators
# ============================================================
print("\n=== Chaining Iterators ===")

from itertools import chain, takewhile, dropwhile, islice

# Complex pipeline
data = range(20)

pipeline = islice(
    dropwhile(lambda x: x < 5,
        takewhile(lambda x: x < 15, data)),
    5
)

result = list(pipeline)
print(f"Pipeline result: {result}")
print("(Drop <5, take <15, then first 5)")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Additional itertools")
print("=" * 50)
print("""
groupby(iterable, key):
  - Groups consecutive items
  - Data must be sorted first
  - Returns (key, group) pairs

compress(data, selectors):
  - Filter by corresponding selectors
  - Like filter but with separate selectors

takewhile(predicate, iterable):
  - Take items while predicate is True
  - Stop at first False

dropwhile(predicate, iterable):
  - Drop items while predicate is True
  - Take rest after first False
""")
