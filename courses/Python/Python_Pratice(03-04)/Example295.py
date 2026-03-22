# Example295: Lambda Functions
# Basic lambda
print("Lambda Functions:")
square = lambda x: x ** 2
print(f"Square of 5: {square(5)}")

# Lambda with multiple args
add = lambda x, y: x + y
print(f"3 + 4: {add(3, 4)}")

# Lambda with sorting
print("\nLambda with sorting:")
points = [(1, 2), (3, 1), (2, 3)]
sorted_by_y = sorted(points, key=lambda p: p[1])
print(f"Sorted by y: {sorted_by_y}")

# Lambda with filter
print("\nLambda with filter:")
nums = [1, 2, 3, 4, 5, 6, 7, 8]
evens = list(filter(lambda x: x % 2 == 0, nums))
print(f"Evens: {evens}")

# Lambda with map
print("\nLambda with map:")
doubled = list(map(lambda x: x * 2, nums))
print(f"Doubled: {doubled}")

# Lambda with reduce
print("\nLambda with reduce:")
from functools import reduce
product = reduce(lambda x, y: x * y, nums)
print(f"Product: {product}")

# Conditional in lambda
print("\nConditional lambda:")
grade = lambda score: "Pass" if score >= 60 else "Fail"
print(f"Score 75: {grade(75)}")
print(f"Score 50: {grade(50)}")

# Lambda with sorted key
print("\nComplex lambda:")
employees = [
    {"name": "Alice", "dept": "IT"},
    {"name": "Bob", "dept": "Sales"},
    {"name": "Charlie", "dept": "IT"}
]
sorted_emp = sorted(employees, key=lambda e: (e["dept"], e["name"]))
print(f"Sorted by dept then name: {sorted_emp}")
