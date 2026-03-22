# Example186.py
# Topic: Additional Sorting Patterns

# This file demonstrates advanced sorting patterns including stable sort,
# sorting with multiple keys, and real-world sorting scenarios.


# ============================================================
# Example 1: Stable Sort Demonstration
# ============================================================
print("=== Stable Sort ===")

data = [
    ("apple", 3),
    ("banana", 1),
    ("cherry", 2),
    ("date", 1),
    ("elderberry", 3)
]

# First sort by number
by_num = sorted(data, key=lambda x: x[1])    # list — by number
print("By number:", by_num)

# Then sort by name (stable - keeps number order)
by_name = sorted(by_num, key=lambda x: x[0])    # list — stable sort
print("By name (stable):", by_name)


# ============================================================
# Example 2: Sort with Lambda vs Operator
# ============================================================
print("\n=== Lambda vs Operator ===")

from operator import itemgetter

pairs = [(1, 3), (2, 1), (3, 2), (1, 1)]

# Using lambda
lambda_sort = sorted(pairs, key=lambda x: x[1])    # list — lambda
print(f"Lambda: {lambda_sort}")

# Using itemgetter (faster)
itemgetter_sort = sorted(pairs, key=itemgetter(1))    # list — itemgetter
print(f"itemgetter: {itemgetter_sort}")


# ============================================================
# Example 3: Sort with Reverse
# ============================================================
print("\n=== Sort with Reverse ===")

data = [3, 1, 4, 1, 5, 9, 2, 6]

# Simple reverse
reverse_sorted = sorted(data, reverse=True)    # list — descending
print(f"Descending: {reverse_sorted}")

# Key with reverse
by_abs = sorted([-3, 1, -4, 1, -5], key=abs)    # list — by abs value
print(f"By abs: {by_abs}")


# ============================================================
# Example 4: Complex Multi-Key Sort
# ============================================================
print("\n=== Multi-Key Sort ===")

class Employee:
    def __init__(self, name: str, dept: str, salary: int):
        self.name = name
        self.dept = dept
        self.salary = salary
    
    def __repr__(self):
        return f"{self.name}({self.dept}, {self.salary})"

employees = [
    Employee("Alice", "IT", 70000),
    Employee("Bob", "HR", 60000),
    Employee("Carol", "IT", 80000),
    Employee("David", "HR", 65000),
    Employee("Eve", "Finance", 75000)
]

# Sort by department, then salary descending
sorted_emp = sorted(employees, key=lambda e: (e.dept, -e.salary))    # list
print("By dept, salary desc:")
for emp in sorted_emp:
    print(f"  {emp}")


# ============================================================
# Example 5: Partial Sort (Heapq)
# ============================================================
print("\n=== Partial Sort with heapq ===")

import heapq

data = [5, 2, 8, 1, 9, 3, 7, 4, 6]

# Get 3 smallest without full sort
smallest_3 = heapq.nsmallest(3, data)    # list — 3 smallest
print(f"3 smallest: {smallest_3}")

# Get 3 largest
largest_3 = heapq.nlargest(3, data)    # list — 3 largest
print(f"3 largest: {largest_3}")


# ============================================================
# Example 6: Sort Strings with Numbers
# ============================================================
print("\n=== Natural Sort for Strings ===")

import re

files = ["file1.txt", "file10.txt", "file2.txt", "file21.txt", "file3.txt"]

def natural_key(s: str):
    return [int(c) if c.isdigit() else c.lower() for c in re.split(r'(\d+)', s)]

natural = sorted(files, key=natural_key)    # list — natural order
print(f"Natural sort: {natural}")


# ============================================================
# Example 7: Sorting with NULL Values
# ============================================================
print("\n=== Handling NULL/None Values ===")

data = [
    ("Alice", 85),
    ("Bob", None),
    ("Carol", 92),
    ("David", None),
    ("Eve", 78)
]

# None values last
sorted_data = sorted(data, key=lambda x: (x[1] is None, x[1] or 0))    # list
print("None values last:", sorted_data)

# None values first
sorted_data_first = sorted(data, key=lambda x: (x[1] is not None, x[1] or 0))    # list
print("None values first:", sorted_data_first)


# ============================================================
# Example 8: Sorting Dates
# ============================================================
print("\n=== Sorting Dates ===")

from datetime import datetime

dates = [
    "2023-01-15",
    "2023-03-10",
    "2023-01-05",
    "2023-02-20",
    "2023-03-01"
]

sorted_dates = sorted(dates)    # list — string sort
print(f"String sort: {sorted_dates}")

# Parse and sort
parsed = sorted(dates, key=lambda d: datetime.strptime(d, "%Y-%m-%d"))    # list
print(f"Date sort: {parsed}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
SORTING PATTERNS:
- Stable sort preserves equal element order
- Multi-key: tuple of keys, negative for desc
- Natural sort for file names with numbers
- Handle None with custom key
- Use heapq for partial sort (top-k)

BEST PRACTICES:
- Use itemgetter over lambda when possible
- Consider memory for large datasets
- Use reverse parameter, not negative key
- Handle edge cases (None, empty)
""")
