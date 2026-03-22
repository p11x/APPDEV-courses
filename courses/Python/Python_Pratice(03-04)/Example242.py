# Example242: Bisect - Custom Classes and Insertion Points
import bisect

# Basic bisect
print("Basic bisect_left/bisect_right:")
numbers = [1, 3, 5, 5, 5, 7, 9]
print(f"Array: {numbers}")
print(f"bisect_left(5): {bisect.bisect_left(numbers, 5)}")
print(f"bisect_right(5): {bisect.bisect_right(numbers, 5)}")

# Finding insertion point
print("\nInsertion points:")
key = 6
left = bisect.bisect_left(numbers, key)
right = bisect.bisect_right(numbers, key)
print(f"Insert {key} at left: {left}")
print(f"Insert {key} at right: {right}")

# Inserting with insort
print("\nInserting with insort:")
data = [1, 3, 5, 7, 9]
bisect.insort(data, 5)
print(f"After insort(5): {data}")

data = [1, 3, 5, 7, 9]
bisect.insort_left(data, 5)
print(f"After insort_left(5): {data}")

# Custom class with __lt__ for bisect
print("\nCustom class with bisect:")
class Number:
    def __init__(self, value):
        self.value = value
    
    def __lt__(self, other):
        return self.value < other.value
    
    def __repr__(self):
        return f"Number({self.value})"

numbers = [Number(1), Number(3), Number(5), Number(7)]
idx = bisect.bisect_left(numbers, Number(4))
print(f"Insert point for 4: {idx}")

# Practical: maintain sorted inventory
print("\nPractical - sorted inventory:")
class Product:
    def __init__(self, name, price):
        self.name = name
        self.price = price
    
    def __lt__(self, other):
        return self.price < other.price
    
    def __repr__(self):
        return f"{self.name} (${self.price})"

inventory = [Product("Apple", 1.0), Product("Banana", 0.5), Product("Orange", 1.5)]
inventory.sort()

def add_product(inventory, product):
    bisect.insort(inventory, product)

add_product(inventory, Product("Grape", 0.75))
print("Inventory (sorted by price):")
for p in inventory:
    print(f"  {p}")

# Using bisect for range queries
print("\nRange queries:")
data = [1, 3, 5, 7, 9, 11, 13, 15]
low, high = 5, 12
left = bisect.bisect_left(data, low)
right = bisect.bisect_right(data, high)
print(f"Values in range [{low}, {high}]: {data[left:right]}")
