# Example120.py
# Topic: Iteration Tools — Basic Filter

# Use filter() to select elements

# === Basic filter() with function ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

def is_even(x):
    return x % 2 == 0

result = filter(is_even, numbers)
print("Filter result: " + str(list(result)))

# === filter() returns iterator (lazy) ===
numbers = [1, 2, 3, 4]
result = filter(lambda x: x > 2, numbers)
print("Iterator: " + str(result))
print("As list: " + str(list(result)))

# === filter() with lambda ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Keep only even numbers
evens = list(filter(lambda x: x % 2 == 0, numbers))
print("Evens: " + str(evens))

# Keep only odd numbers
odds = list(filter(lambda x: x % 2 != 0, numbers))
print("Odds: " + str(odds))

# === filter() keeps elements where function returns True ===
numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Filter out falsy values
truthy = list(filter(None, numbers))
print("Truthy: " + str(truthy))

# === Practical: Filter by length ===
words = ["hi", "hello", "hey", "goodbye", "yo"]

# Keep words longer than 3
long_words = list(filter(lambda w: len(w) > 3, words))
print("Long words: " + str(long_words))

# Keep short words
short_words = list(filter(lambda w: len(w) <= 3, words))
print("Short words: " + str(short_words))

# === Practical: Filter by condition ===
ages = [12, 15, 18, 21, 25, 30]

# Keep adults (18+)
adults = list(filter(lambda a: a >= 18, ages))
print("Adults: " + str(adults))

# Keep teenagers (13-19)
teenagers = list(filter(lambda a: 13 <= a <= 19, ages))
print("Teenagers: " + str(teenagers))

# === filter() vs list comprehension ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# Same result:
filter_result = list(filter(lambda x: x > 5, numbers))
comp_result = [x for x in numbers if x > 5]
print("Filter: " + str(filter_result))
print("List comp: " + str(comp_result))

# === Practical: Filter objects ===
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __repr__(self):
        return self.name


people = [Person("Alice", 30), Person("Bob", 17), Person("Carol", 25)]

# Keep adults
adults = list(filter(lambda p: p.age >= 18, people))
print("Adults: " + str(adults))

# Keep specific age
exact = list(filter(lambda p: p.age == 25, people))
print("Age 25: " + str(exact))

# === filter() with empty list ===
result = list(filter(lambda x: x > 0, []))
print("Empty: " + str(result))

# === filter() keeps True values ===
# filter(func, iterable) keeps where func(x) is truthy
values = [0, 1, 2, "", "a", None, False, True]

# Keep truthy
truthy = list(filter(None, values))
print("Truthy filter(None): " + str(truthy))

# === Practical: Filter strings ===
emails = ["alice@example.com", "bob@", "@invalid", "carol@test.org", "dave"]

# Keep valid emails (have @ and .)
valid = list(filter(lambda e: "@" in e and "." in e, emails))
print("Valid emails: " + str(valid))

# === filter() returns iterator - lazy evaluation ===
def check(x):
    print("Checking " + str(x))
    return x > 3

numbers = [1, 2, 3, 4, 5]
result = filter(check, numbers)

print("Before list():")
list(result)
print("After list()")

# === filter() with complex condition ===
data = [
    {"name": "Alice", "active": True, "score": 85},
    {"name": "Bob", "active": False, "score": 90},
    {"name": "Carol", "active": True, "score": 75},
    {"name": "Dave", "active": True, "score": 55}
]

# Active users with score >= 80
high_scores = list(filter(lambda d: d["active"] and d["score"] >= 80, data))
print("Active high scores: " + str(high_scores))

# Inactive users
inactive = list(filter(lambda d: not d["active"], data))
print("Inactive: " + str(inactive))
