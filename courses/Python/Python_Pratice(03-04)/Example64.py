# Example64.py
# Topic: Real-World Examples with map(), filter(), reduce()

# This file provides practical real-world examples.


# ============================================================
# Example 1: E-commerce - Order processing
# ============================================================
print("=== Real-world: E-commerce Order Processing ===")

from functools import reduce

orders = [
    {'id': 1, 'items': [{'price': 25, 'qty': 2}, {'price': 10, 'qty': 1}]},
    {'id': 2, 'items': [{'price': 50, 'qty': 1}]},
    {'id': 3, 'items': [{'price': 15, 'qty': 3}, {'price': 5, 'qty': 2}]},
]

# Calculate order totals
order_totals = list(map(
    lambda o: {'id': o['id'], 'total': reduce(lambda a, i: a + i['price'] * i['qty'], o['items'], 0)},
    orders
))
print("Order totals:")
for order in order_totals:
    print(f"  Order {order['id']}: ${order['total']}")


# ============================================================
# Example 2: Analytics - Report generation
# ============================================================
print("\n=== Real-world: Analytics Report ===")

products = [
    {'name': 'Laptop', 'category': 'Electronics', 'sales': 150, 'rating': 4.5},
    {'name': 'Shirt', 'category': 'Clothing', 'sales': 300, 'rating': 4.0},
    {'name': 'Phone', 'category': 'Electronics', 'sales': 200, 'rating': 4.8},
    {'name': 'Pants', 'category': 'Clothing', 'sales': 250, 'rating': 3.9},
    {'name': 'Tablet', 'category': 'Electronics', 'sales': 80, 'rating': 4.2},
]

# Sales by category
def summarize_by_category(acc, p):
    cat = p['category']
    acc[cat] = acc.get(cat, 0) + p['sales']
    return acc

sales_by_cat = reduce(summarize_by_category, products, {})
print(f"Sales by category: {sales_by_cat}")

# Top rated products
top_rated = list(filter(lambda p: p['rating'] >= 4.5, products))
print(f"Top rated: {[p['name'] for p in top_rated]}")


# ============================================================
# Example 3: Data cleaning pipeline
# ============================================================
print("\n=== Real-world: Data Cleaning ===")

raw_data = [
    "  john doe  ",
    "JANE SMITH",
    "bob wilson",
    None,
    "",
    "Alice Brown",
]

# Clean and filter
cleaned = list(filter(
    lambda s: s and s.strip(),
    map(
        lambda s: s.strip().title() if s else None,
        raw_data
    )
))
print(f"Cleaned names: {cleaned}")


# ============================================================
# Example 4: Employee database queries
# ============================================================
print("\n=== Real-world: Employee Database ===")

employees = [
    {'name': 'Alice', 'dept': 'Engineering', 'salary': 90000, 'years': 5},
    {'name': 'Bob', 'dept': 'Marketing', 'salary': 70000, 'years': 3},
    {'name': 'Charlie', 'dept': 'Engineering', 'salary': 85000, 'years': 7},
    {'name': 'Diana', 'dept': 'HR', 'salary': 65000, 'years': 2},
    {'name': 'Eve', 'dept': 'Engineering', 'salary': 95000, 'years': 4},
]

# Average salary in Engineering
eng_salaries = list(map(lambda e: e['salary'], filter(lambda e: e['dept'] == 'Engineering', employees)))
avg_eng_salary = reduce(lambda a, b: a + b, eng_salaries, 0) / len(eng_salaries)
print(f"Avg Engineering salary: ${avg_eng_salary:.2f}")

# Employees with 5+ years
veterans = list(filter(lambda e: e['years'] >= 5, employees))
print(f"Veterans: {[e['name'] for e in veterans]}")


# ============================================================
# Example 5: File processing
# ============================================================
print("\n=== Real-world: File Processing ===")

files = [
    {'name': 'report.txt', 'size': 1024, 'type': 'text'},
    {'name': 'image.png', 'size': 2048000, 'type': 'image'},
    {'name': 'data.csv', 'size': 5120, 'type': 'data'},
    {'name': 'video.mp4', 'size': 104857600, 'type': 'video'},
    {'name': 'script.py', 'size': 2048, 'type': 'code'},
]

# Total size by type
def size_by_type(acc, f):
    t = f['type']
    acc[t] = acc.get(t, 0) + f['size']
    return acc

total_by_type = reduce(size_by_type, files, {})
print(f"Size by type: {total_by_type}")

# Large files (> 1MB)
large_files = list(filter(lambda f: f['size'] > 1000000, files))
print(f"Large files: {[f['name'] for f in large_files]}")


# ============================================================
# Example 6: Survey analysis
# ============================================================
print("\n=== Real-world: Survey Analysis ===")

responses = [
    {'user': 1, 'score': 4, 'comments': 'Great!'},
    {'user': 2, 'score': 2, 'comments': 'Poor'},
    {'user': 3, 'score': 5, 'comments': 'Excellent!'},
    {'user': 4, 'score': 3, 'comments': 'Average'},
    {'user': 5, 'score': 5, 'comments': 'Love it!'},
]

# Average score
scores = list(map(lambda r: r['score'], responses))
avg_score = reduce(lambda a, b: a + b, scores) / len(scores)
print(f"Average score: {avg_score:.2f}")

# Positive responses (score >= 4)
positive = list(filter(lambda r: r['score'] >= 4, responses))
print(f"Positive responses: {len(positive)}/{len(responses)}")


# ============================================================
# Example 7: Inventory management
# ============================================================
print("\n=== Real-world: Inventory Management ===")

inventory = [
    {'item': 'Widget', 'stock': 100, 'cost': 5.00},
    {'item': 'Gadget', 'stock': 50, 'cost': 15.00},
    {'item': 'Gizmo', 'stock': 200, 'cost': 2.50},
    {'item': 'Thing', 'stock': 10, 'cost': 25.00},
]

# Total inventory value
total_value = reduce(
    lambda a, i: a + i['stock'] * i['cost'],
    inventory,
    0
)
print(f"Total inventory value: ${total_value:.2f}")

# Low stock items (stock < 20)
low_stock = list(filter(lambda i: i['stock'] < 20, inventory))
print(f"Low stock: {[i['item'] for i in low_stock]}")


# ============================================================
# Example 8: Log file analysis
# ============================================================
print("\n=== Real-world: Log Analysis ===")

logs = [
    {'timestamp': '2024-01-01', 'level': 'INFO', 'message': 'Server started'},
    {'timestamp': '2024-01-01', 'level': 'ERROR', 'message': 'Connection failed'},
    {'timestamp': '2024-01-01', 'level': 'WARNING', 'message': 'High memory'},
    {'timestamp': '2024-01-01', 'level': 'INFO', 'message': 'User logged in'},
    {'timestamp': '2024-01-01', 'level': 'ERROR', 'message': 'Timeout'},
]

# Count by level
def count_by_level(acc, log):
    level = log['level']
    acc[level] = acc.get(level, 0) + 1
    return acc

level_counts = reduce(count_by_level, logs, {})
print(f"Log level counts: {level_counts}")

# Error messages
errors = list(map(lambda l: l['message'], filter(lambda l: l['level'] == 'ERROR', logs)))
print(f"Error messages: {errors}")


# ============================================================
# Example 9: Grade book
# ============================================================
print("\n=== Real-world: Grade Book ===")

students = [
    {'name': 'Alice', 'grades': [85, 90, 92]},
    {'name': 'Bob', 'grades': [70, 75, 72]},
    {'name': 'Charlie', 'grades': [95, 88, 91]},
    {'name': 'Diana', 'grades': [60, 65, 68]},
]

# Calculate averages
averages = list(map(
    lambda s: {'name': s['name'], 'avg': reduce(lambda a, g: a + g, s['grades']) / len(s['grades'])},
    students
))
print("Student averages:")
for s in averages:
    print(f"  {s['name']}: {s['avg']:.2f}")

# Passing students (avg >= 70)
passing = list(filter(lambda s: s['avg'] >= 70, averages))
print(f"Passing: {[s['name'] for s in passing]}")


# ============================================================
# Example 10: URL processing
# ============================================================
print("\n=== Real-world: URL Processing ===")

urls = [
    'https://example.com',
    'http://test.org/path',
    'https://api.site.com/v1/users',
    'invalid-url',
]

# Parse URLs (simplified)
def parse_url(url):
    if not url.startswith(('http://', 'https://')):
        return None
    parts = url.replace('https://', '').replace('http://', '').split('/')
    return {'domain': parts[0], 'path': '/' + '/'.join(parts[1:]) if len(parts) > 1 else '/'}

parsed = list(filter(None, map(parse_url, urls)))
print("Parsed URLs:")
for p in parsed:
    print(f"  {p['domain']} -> {p['path']}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD USES OF map, filter, reduce")
print("=" * 50)
print("""
- E-commerce: Order totals, inventory value
- Analytics: Reports, aggregations
- Data cleaning: Transform and filter raw data
- Database queries: Filter and aggregate records
- File processing: Size calculations, filtering
- Log analysis: Counting, extracting errors
- Much more!
""")
