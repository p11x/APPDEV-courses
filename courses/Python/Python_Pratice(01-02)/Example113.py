# Example113.py
# Topic: Iteration Tools — Reverse Parameter and Practical Uses

# Using reverse parameter and practical examples

# === Basic reverse sort ===
numbers = [1, 5, 2, 8, 3]
print("Original: " + str(numbers))
print("Reverse: " + str(sorted(numbers, reverse=True)))

# === Reverse with strings ===
words = ["banana", "apple", "cherry"]
print("Reverse alphabetical: " + str(sorted(words, reverse=True)))

# === Reverse with custom key ===
words = ["banana", "apple", "cherry", "date"]
print("By length (longest first): " + str(sorted(words, key=len, reverse=True)))

# === Practical: Sort by price (descending) ===
products = [
    {"name": "Laptop", "price": 999},
    {"name": "Phone", "price": 599},
    {"name": "Tablet", "price": 399},
    {"name": "Watch", "price": 199}
]

# Sort by price descending
sorted_products = sorted(products, key=lambda x: x["price"], reverse=True)
print("\nProducts by price (high to low):")
for p in sorted_products:
    print("  " + p["name"] + ": $" + str(p["price"]))

# === Practical: Sort by date ===
from datetime import datetime

events = [
    {"name": "Party", "date": "2024-01-15"},
    {"name": "Meeting", "date": "2024-01-10"},
    {"name": "Conference", "date": "2024-02-01"}
]

# Convert to datetime and sort
sorted_events = sorted(events, key=lambda x: datetime.strptime(x["date"], "%Y-%m-%d"))
print("\nEvents (chronological):")
for e in sorted_events:
    print("  " + e["date"] + ": " + e["name"])

# === Practical: Sort by priority ===
tasks = [
    {"task": "Fix bug", "priority": 3},
    {"task": "Write docs", "priority": 1},
    {"task": "Deploy", "priority": 4},
    {"task": "Review code", "priority": 2}
]

# Sort by priority (higher priority first)
sorted_tasks = sorted(tasks, key=lambda x: x["priority"], reverse=True)
print("\nTasks by priority:")
for t in sorted_tasks:
    print("  Priority " + str(t["priority"]) + ": " + t["task"])

# === Sort with lambda - multiple conditions ===
employees = [
    {"name": "Alice", "dept": "Sales", "salary": 50000},
    {"name": "Bob", "dept": "IT", "salary": 60000},
    {"name": "Carol", "dept": "Sales", "salary": 55000}
]

# Sort by department, then by salary (descending)
sorted_emp = sorted(employees, key=lambda x: (x["dept"], x["salary"]), reverse=True)
print("\nEmployees (dept, then salary desc):")
for e in sorted_emp:
    print("  " + e["dept"] + ": " + e["name"] + " - $" + str(e["salary"]))

# === Reverse with negative key ===
# Equivalent to reverse=True with same key
numbers = [1, 2, 3, 4, 5]
print("\nSort descending via negative key: " + str(sorted(numbers, key=lambda x: -x)))

# === Practical: Sort file names by extension ===
files = ["report.pdf", "image.png", "data.csv", "script.py", "photo.jpg"]

# Sort by extension (last part after dot)
sorted_files = sorted(files, key=lambda x: x.split(".")[-1])
print("\nSorted by extension:")
print("  " + str(sorted_files))

# === Practical: Sort by last character ===
words = ["hello", "world", "python", "code"]
print("\nSort by last character: " + str(sorted(words, key=lambda x: x[-1])))

# === Combining reverse with different keys ===
data = ["apple", "banana", "cherry", "date"]

# Longest first (no reverse needed)
print("Longest first: " + str(sorted(data, key=len, reverse=True)))

# Shortest first (default)
print("Shortest first: " + str(sorted(data, key=len)))

# === Reversed vs reverse=True ===
# reversed() returns an iterator
# sorted(..., reverse=True) returns a list

print("\nreversed() gives iterator:")
rev = reversed([1, 2, 3])
print("  " + str(list(rev)))

print("sorted(reverse=True) gives list:")
print("  " + str(sorted([1, 2, 3], reverse=True)))
