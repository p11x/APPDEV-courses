# Example45.py
# Topic: Loops — Iterating Over Lists

# For loops can iterate directly over list items

# === Basic list iteration ===
fruits = ["apple", "banana", "cherry"]

for fruit in fruits:
    print(fruit)
# apple
# banana
# cherry

# === Iterating with conditions ===
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

for num in numbers:
    if num % 2 == 0:
        print(str(num) + " is even")
    else:
        print(str(num) + " is odd")
# 1 is odd
# 2 is even
# 3 is odd
# ...

# === Summing a list ===
scores = [85, 90, 78, 92, 88]
total = 0

for score in scores:
    total = total + score

print("Total: " + str(total))  # 433

# === Finding maximum ===
prices = [29.99, 15.00, 45.50, 12.99, 99.99]
max_price = prices[0]

for price in prices:
    if price > max_price:
        max_price = price

print("Max price: $" + str(max_price))  # $99.99

# === Building a new list ===
names = ["alice", "bob", "charlie"]
capitalized = []

for name in names:
    capitalized.append(name.capitalize())

print(capitalized)  # ['Alice', 'Bob', 'Charlie']

# === Real-world example: Shopping cart total ===
cart_items = [
    {"name": "Apple", "price": 0.50, "quantity": 3},
    {"name": "Banana", "price": 0.30, "quantity": 2},
    {"name": "Orange", "price": 0.75, "quantity": 4},
]

total = 0

for item in cart_items:
    item_total = item["price"] * item["quantity"]
    print(item["name"] + ": $" + str(item["price"]) + " x " + str(item["quantity"]) + " = $" + str(item_total))
    total = total + item_total

print("Total: $" + str(total))
# Apple: $0.5 x 3 = $1.5
# Banana: $0.3 x 2 = $0.6
# Orange: $0.75 x 4 = $3.0
# Total: $5.1

# === Real-world example: Filter items ===
products = [
    {"name": "Laptop", "price": 999.99},
    {"name": "Mouse", "price": 29.99},
    {"name": "Keyboard", "price": 79.99},
    {"name": "Monitor", "price": 299.99},
]

expensive = []

for product in products:
    if product["price"] > 100:
        expensive.append(product["name"])

print("Expensive items: " + str(expensive))
# ['Laptop', 'Monitor']

# === Real-world example: Count occurrences ===
votes = ["Alice", "Bob", "Alice", "Charlie", "Alice", "Bob", "Alice"]
alice_count = 0

for vote in votes:
    if vote == "Alice":
        alice_count = alice_count + 1

print("Alice has " + str(alice_count) + " votes")  # 4 votes
