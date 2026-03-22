# Example136.py
# Topic: Operator Module


# ============================================================
# Example 1: itemgetter
# ============================================================
print("=== itemgetter ===")

from operator import itemgetter

data = [
    {"name": "Alice", "age": 30},
    {"name": "Bob", "age": 25},
]

print(sorted(data, key=itemgetter("age")))
print(sorted(data, key=itemgetter("name")))

point = (10, 20)
print(f"itemgetter(0)(point): {itemgetter(0)(point)}")


# ============================================================
# Example 2: attrgetter
# ============================================================
print("\n=== attrgetter ===")

from operator import attrgetter

class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __repr__(self):
        return f"Person({self.name}, {self.age})"

people = [Person("Alice", 30), Person("Bob", 25)]
print(sorted(people, key=attrgetter("age")))


# ============================================================
# Example 3: methodcaller
# ============================================================
print("\n=== methodcaller ===")

from operator import methodcaller

words = ["hello", "world", "python"]
sorted_by_len = sorted(words, key=methodcaller("__len__"))
print(f"Sorted by length: {sorted_by_len}")

upper_words = list(map(methodcaller("upper"), words))
print(f"Upper: {upper_words}")


# ============================================================
# Example 4: Arithmetic Operators
# ============================================================
print("\n=== Arithmetic Operators ===")

from operator import add, sub, mul, truediv, neg

print(f"add(1, 2): {add(1, 2)}")
print(f"sub(5, 3): {sub(5, 3)}")
print(f"mul(4, 5): {mul(4, 5)}")
print(f"truediv(10, 2): {truediv(10, 2)}")
print(f"neg(-5): {neg(-5)}")


# ============================================================
# Example 5: Comparison Operators
# ============================================================
print("\n=== Comparison ===")

from operator import eq, ne, lt, le, gt, ge

print(f"eq(1, 1): {eq(1, 1)}")
print(f"lt(1, 2): {lt(1, 2)}")
print(f"gt(2, 1): {gt(2, 1)}")


# ============================================================
# Example 6: Logical Operators
# ============================================================
print("\n=== Logical ===")

from operator import truth, not_

print(f"truth(1): {truth(1)}")
print(f"truth(0): {truth(0)}")
print(f"not_(True): {not_(True)}")
print(f"not_(False): {not_(False)}")


# ============================================================
# Example 7: Real-World: Sorting with Multiple Keys
# ============================================================
print("\n=== Real-World: Complex Sort ===")

from operator import itemgetter

data = [
    ("apple", 10, 100),
    ("banana", 5, 50),
    ("apple", 5, 80),
    ("banana", 10, 60),
]

sorted_data = sorted(data, key=itemgetter(0, 1, 2))
print("Sorted:")
for item in sorted_data:
    print(f"  {item}")
