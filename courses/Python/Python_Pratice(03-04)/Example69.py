# Example69.py
# Topic: Real-World Examples with itertools

# This file provides practical real-world examples using itertools.


# ============================================================
# Example 1: Data analysis pipeline
# ============================================================
print("=== Real-world: Data Analysis ===")

from itertools import groupby, islice, chain

# Sales data by region
sales = [
    {'region': 'North', 'sales': 100, 'month': 'Jan'},
    {'region': 'North', 'sales': 150, 'month': 'Feb'},
    {'region': 'South', 'sales': 200, 'month': 'Jan'},
    {'region': 'South', 'sales': 180, 'month': 'Feb'},
    {'region': 'East', 'sales': 120, 'month': 'Jan'},
    {'region': 'East', 'sales': 140, 'month': 'Feb'},
]

# Sort by region for groupby
sales_sorted = sorted(sales, key=lambda x: x['region'])

# Calculate totals by region
regional_totals = {}
for region, group in groupby(sales_sorted, key=lambda x: x['region']):
    total = sum(item['sales'] for item in group)
    regional_totals[region] = total

print(f"Regional totals: {regional_totals}")

# Get top 3 months (simple approach)
all_sales = sorted(sales, key=lambda x: x['sales'], reverse=True)
top_3 = list(islice(all_sales, 3))
print(f"Top 3 sales: {top_3}")


# ============================================================
# Example 2: Batch processing
# ============================================================
print("\n=== Real-world: Batch Processing ===")

from itertools import islice

def batch_process(items, batch_size):
    """Process items in batches."""
    it = iter(items)
    while True:
        batch = list(islice(it, batch_size))
        if not batch:
            break
        yield batch

# Process large dataset in batches
data = range(25)
for i, batch in enumerate(batch_process(data, 7)):
    print(f"Batch {i+1}: {batch}")


# ============================================================
# Example 3: Game development
# ============================================================
print("\n=== Real-world: Game Board Generation ===")

from itertools import product

# Generate game board positions
rows = 3
cols = 4

positions = list(product(range(rows), range(cols)))
print(f"Board positions ({rows}x{cols}): {len(positions)} positions")
print(f"Sample: {positions[:5]}")

# Adjacent cells (including diagonals)
def get_adjacent(row, col, rows, cols):
    return [(r, c) for r, c in product(range(rows), range(cols))
            if (r, c) != (row, col) and abs(r - row) <= 1 and abs(c - col) <= 1]

adj = get_adjacent(1, 1, 3, 4)
print(f"Adjacent to (1,1): {adj}")


# ============================================================
# Example 4: Test case generation
# ============================================================
print("\n=== Real-world: Test Case Generation ===")

from itertools import product, combinations

# Generate test combinations
configs = {
    'os': ['Windows', 'Linux', 'Mac'],
    'browser': ['Chrome', 'Firefox', 'Safari'],
    'screen': ['Desktop', 'Mobile']
}

# All combinations
test_cases = []
for values in product(*configs.values()):
    test_cases.append(dict(zip(configs.keys(), values)))

print(f"Total test cases: {len(test_cases)}")
print(f"Sample tests: {test_cases[:3]}")

# Boundary value testing
boundary_values = [0, 1, 100, 999, 1000]
pairs = list(combinations(boundary_values, 2))
print(f"Boundary pairs: {pairs}")


# ============================================================
# Example 5: Report generation
# ============================================================
print("\n=== Real-world: Report Generation ===")

from itertools import groupby, chain

employees = [
    {'name': 'Alice', 'dept': 'IT', 'salary': 90000},
    {'name': 'Bob', 'dept': 'IT', 'salary': 85000},
    {'name': 'Charlie', 'dept': 'HR', 'salary': 70000},
    {'name': 'Diana', 'dept': 'HR', 'salary': 65000},
    {'name': 'Eve', 'dept': 'Sales', 'salary': 75000},
]

# Group by department
employees_sorted = sorted(employees, key=lambda x: x['dept'])
report = []
for dept, group in groupby(employees_sorted, key=lambda x: x['dept']):
    dept_employees = list(group)
    avg_salary = sum(e['salary'] for e in dept_employees) / len(dept_employees)
    report.append({
        'department': dept,
        'count': len(dept_employees),
        'avg_salary': avg_salary,
        'employees': [e['name'] for e in dept_employees]
    })

print("Department Report:")
for r in report:
    print(f"  {r['department']}: {r['count']} employees, avg ${r['avg_salary']:.0f}")


# ============================================================
# Example 6: Permutations for scheduling
# ============================================================
print("\n=== Real-world: Task Scheduling ===")

from itertools import permutations

tasks = ['A', 'B', 'C', 'D']
duration = {'A': 3, 'B': 2, 'C': 5, 'D': 1}

# Find optimal order (shortest total time)
def total_time(order):
    return sum(duration[t] for t in order)

# All permutations
all_orders = list(permutations(tasks))
best_order = min(all_orders, key=total_time)
print(f"Optimal order: {best_order} = {total_time(best_order)} hours")


# ============================================================
# Example 7: Data validation
# ============================================================
print("\n=== Real-world: Data Validation ===")

from itertools import compress

# Validate user registration
user = {
    'username': 'john_doe',
    'email': 'john@example.com',
    'age': 25,
    'password': 'secret123'
}

fields = ['username', 'email', 'age', 'password']
validators = [
    lambda u: len(u.get('username', '')) >= 3,
    lambda u: '@' in u.get('email', ''),
    lambda u: u.get('age', 0) >= 18,
    lambda u: len(u.get('password', '')) >= 6,
]

errors = list(compress(fields, [not v(user) for v in validators]))
print(f"Validation errors: {errors if errors else 'None'}")


# ============================================================
# Example 8: CSV parsing simulation
# ============================================================
print("\n=== Real-world: CSV-like Processing ===")

from itertools import islice, takewhile

# Simulate CSV with header
csv_data = [
    'Name,Age,City',
    'Alice,25,NYC',
    'Bob,30,LA',
    'Charlie,35,Chicago',
]

# Parse header
header = next(iter(csv_data)).split(',')
print(f"Header: {header}")

# Parse rows
rows = (line.split(',') for line in islice(csv_data, 1, None))
print("Data rows:")
for row in rows:
    print(f"  {dict(zip(header, row))}")


# ============================================================
# Example 9: Pagination with groupby
# ============================================================
print("\n=== Real-world: Pagination ===")

from itertools import islice

def paginate(data, page_size):
    """Simple pagination generator."""
    it = iter(data)
    page = list(islice(it, page_size))
    page_num = 1
    while page:
        yield page_num, page
        page = list(islice(it, page_size))
        page_num += 1

# Paginate results
results = list(range(23))
for page_num, items in paginate(results, 7):
    print(f"Page {page_num}: {items}")


# ============================================================
# Example 10: Complex data transformation
# ============================================================
print("\n=== Real-world: Complex Transformation ===")

from itertools import groupby, chain, islice

# Transform nested structure
raw_data = [
    {'date': '2024-01-01', 'type': 'sale', 'amount': 100},
    {'date': '2024-01-01', 'type': 'return', 'amount': 20},
    {'date': '2024-01-02', 'type': 'sale', 'amount': 150},
    {'date': '2024-01-02', 'type': 'sale', 'amount': 80},
    {'date': '2024-01-03', 'type': 'return', 'amount': 30},
]

# Calculate daily net
raw_sorted = sorted(raw_data, key=lambda x: x['date'])
daily_net = []
for date, group in groupby(raw_sorted, key=lambda x: x['date']):
    items = list(group)
    sales = sum(i['amount'] for i in items if i['type'] == 'sale')
    returns = sum(i['amount'] for i in items if i['type'] == 'return')
    daily_net.append({'date': date, 'net': sales - returns})

print("Daily net:")
for d in daily_net:
    print(f"  {d['date']}: ${d['net']}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD itertools USES")
print("=" * 50)
print("""
- Data analysis: Grouping, aggregation
- Batch processing: Chunk large datasets
- Game development: Board generation, adjacency
- Testing: Generate test combinations
- Reporting: Group and summarize data
- Scheduling: Find optimal orders
- Validation: Filter with multiple criteria
- CSV parsing: Header/row processing
- Pagination: Divide data into pages
""")
