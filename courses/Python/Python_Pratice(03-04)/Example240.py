# Example240: Advanced Sorting - stable sort, multiple keys
from operator import itemgetter, attrgetter

# Stable sort - preserves order of equal elements
print("Stable sort - preserves order:")
data = [('apple', 3), ('banana', 1), ('cherry', 2), ('date', 1)]
result = sorted(data, key=lambda x: x[1])
print(f"Original: {data}")
print(f"Sorted by number: {result}")

# Multiple keys with tuples
print("\nMultiple sort keys:")
students = [
    ('Alice', 'A', 90),
    ('Bob', 'B', 85),
    ('Charlie', 'A', 85),
    ('Diana', 'B', 90)
]
result = sorted(students, key=lambda x: (x[1], x[2]))
print("Sorted by grade then score:")
for s in result:
    print(f"  {s}")

# Using itemgetter for multiple keys
print("\nUsing itemgetter:")
result = sorted(students, key=itemgetter(1, 2))
for s in result:
    print(f"  {s}")

# Descending sort
print("\nDescending sort:")
numbers = [5, 2, 8, 1, 9]
result = sorted(numbers, reverse=True)
print(f"Descending: {result}")

# Sort with multiple conditions
print("\nComplex sorting:")
products = [
    ('Laptop', 'Electronics', 1000),
    ('Phone', 'Electronics', 800),
    ('Shirt', 'Clothing', 30),
    ('Pants', 'Clothing', 50),
    ('Tablet', 'Electronics', 500)
]
# Sort by category (asc), then by price (desc)
result = sorted(products, key=lambda x: (x[1], -x[2]))
print("Category (asc), Price (desc):")
for p in result:
    print(f"  {p}")

# nlargest and nsmallest
print("\nnlargest and nsmallest:")
from heapq import nlargest, nsmallest
numbers = [5, 2, 8, 1, 9, 3, 7, 4, 6]
print(f"Largest 3: {nlargest(3, numbers)}")
print(f"Smallest 3: {nsmallest(3, numbers)}")

# Sort with key for complex objects
print("\nSort by attribute:")
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __repr__(self):
        return f"Person({self.name}, {self.age})"

people = [Person('Alice', 30), Person('Bob', 25), Person('Charlie', 30)]
result = sorted(people, key=attrgetter('age', 'name'))
print(result)
