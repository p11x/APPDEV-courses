# Example234: itertools - groupby
import itertools

# groupby(iterable, key) - groups consecutive elements by key
data = [('a', 1), ('a', 2), ('b', 3), ('b', 4), ('c', 5)]

print("groupby - group consecutive elements:")
for key, group in itertools.groupby(data, key=lambda x: x[0]):
    print(f"Key: {key}, Values: {list(group)}")

# Group by length
print("\nGroup by length:")
words = ['one', 'two', 'three', 'four', 'five', 'six']
for length, group in itertools.groupby(words, key=len):
    print(f"Length {length}: {list(group)}")

# Grouping with sorted data (important - groupby only groups CONSECUTIVE items)
print("\nImportant - groupby with sorted data:")
data = [1, 2, 2, 3, 3, 3, 4, 4]
for key, group in itertools.groupby(data):
    print(f"Key: {key}, Count: {len(list(group))}")

# Using groupby for counting consecutive duplicates
print("\nCount consecutive duplicates:")
data = [1, 1, 1, 2, 2, 3, 3, 3, 3, 1]
result = [(key, len(list(group))) for key, group in itertools.groupby(data)]
print(f"Data: {data}")
print(f"Consecutive counts: {result}")

# Practical: finding runs in data
print("\nFind runs of increasing numbers:")
data = [1, 2, 3, 5, 4, 6, 7, 1, 2]
current_run = []
runs = []
for key, group in itertools.groupby(enumerate(data), key=lambda x: x[1] - x[0]):
    run = list(group)
    current_run.append(run)
    if len(run) > 0:
        print(f"Run: {[x[1] for x in run]}")

# Grouping with object
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age

people = [
    Person('Alice', 25), Person('Bob', 25), 
    Person('Charlie', 30), Person('Dave', 30), Person('Eve', 25)
]

print("\nGroup people by age:")
for age, group in itertools.groupby(people, key=lambda p: p.age):
    print(f"Age {age}: {[p.name for p in group]}")
