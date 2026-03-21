# Example47.py
# Topic: Loops — Using zip()

# zip() lets you iterate over multiple sequences at once

# === Basic zip ===
names = ["Alice", "Bob", "Charlie"]
ages = [25, 30, 35]

for name, age in zip(names, ages):
    print(name + " is " + str(age) + " years old")
# Alice is 25 years old
# Bob is 30 years old
# Charlie is 35 years old

# === Zip with three lists ===
first_names = ["John", "Jane", "Bob"]
last_names = ["Smith", "Doe", "Jones"]
ages = [28, 24, 35]

for first, last, age in zip(first_names, last_names, ages):
    print(first + " " + last + ", age " + str(age))
# John Smith, age 28
# Jane Doe, age 24
# Bob Jones, age 35

# === Unequal length lists ===
# zip() stops at the shortest list
short_list = ["a", "b"]
long_list = [1, 2, 3, 4, 5]

for letter, number in zip(short_list, long_list):
    print(letter + ": " + str(number))
# a: 1
# b: 2

# === Real-world example: Creating a dictionary ===
keys = ["name", "age", "city"]
values = ["Alice", 25, "New York"]

result = dict(zip(keys, values))
print(result)  # {'name': 'Alice', 'age': 25, 'city': 'New York'}

# === Real-world example: Shopping list prices ===
items = ["Apple", "Banana", "Orange"]
prices = [0.50, 0.30, 0.75]
quantities = [3, 2, 4]

total = 0

for item, price, qty in zip(items, prices, quantities):
    item_total = price * qty
    print(item + ": $" + str(price) + " x " + str(qty) + " = $" + str(item_total))
    total = total + item_total

print("Total: $" + str(total))
# Apple: $0.5 x 3 = $1.5
# Banana: $0.3 x 2 = $0.6
# Orange: $0.75 x 4 = $3.0
# Total: $5.1

# === Real-world example: Combining user data ===
usernames = ["alice", "bob", "charlie"]
emails = ["alice@email.com", "bob@email.com", "charlie@email.com"]
roles = ["admin", "user", "editor"]

users = []

for username, email, role in zip(usernames, emails, roles):
    user = {"username": username, "email": email, "role": role}
    users.append(user)

print(users)
# [{'username': 'alice', 'email': 'alice@email.com', 'role': 'admin'}, ...]

# === Real-world example: Parallel iteration for comparison ===
prices_store_a = [100, 200, 300, 400]
prices_store_b = [90, 210, 290, 410]

print("Price comparison:")
for i, (price_a, price_b) in enumerate(zip(prices_store_a, prices_store_b)):
    diff = price_a - price_b
    if diff > 0:
        print("Item " + str(i) + ": Store B is cheaper by $" + str(diff))
    elif diff < 0:
        print("Item " + str(i) + ": Store A is cheaper by $" + str(-diff))
    else:
        print("Item " + str(i) + ": Same price")
# Item 0: Store B is cheaper by $10
# Item 1: Store A is cheaper by $10
# ...

# === Using zip with enumerate ===
students = ["Alice", "Bob", "Charlie", "Diana"]
grades = [85, 92, 78, 90]

for rank, (student, grade) in enumerate(zip(students, grades), start=1):
    print("#" + str(rank) + " " + student + ": " + str(grade))
# #1 Alice: 85
# #2 Bob: 92
# #3 Charlie: 78
# #4 Diana: 90

# === Converting to list of tuples ===
list1 = [1, 2, 3]
list2 = ["a", "b", "c"]

zipped = list(zip(list1, list2))
print(zipped)  # [(1, 'a'), (2, 'b'), (3, 'c')]
