# Example46.py
# Topic: Loops — Using enumerate()

# enumerate() gives you both index and value when iterating

# === Basic enumerate ===
fruits = ["apple", "banana", "cherry"]

for index, fruit in enumerate(fruits):
    print(str(index) + ": " + fruit)
# 0: apple
# 1: banana
# 2: cherry

# === Starting index at 1 ===
for index, fruit in enumerate(fruits, start=1):
    print(str(index) + ": " + fruit)
# 1: apple
# 2: banana
# 3: cherry

# === With conditions ===
colors = ["red", "green", "blue", "yellow"]

for index, color in enumerate(colors):
    if index % 2 == 0:
        print("Position " + str(index) + ": " + color + " (even)")
    else:
        print("Position " + str(index) + ": " + color + " (odd)")
# Position 0: red (even)
# Position 1: green (odd)
# Position 2: blue (even)
# Position 3: yellow (odd)

# === Finding index of specific value ===
animals = ["cat", "dog", "bird", "fish", "cat"]
target = "bird"
target_index = -1

for index, animal in enumerate(animals):
    if animal == target:
        target_index = index
        break

print(target + " found at index " + str(target_index))  # bird found at index 2

# === Real-world example: Leaderboard ===
players = [
    {"name": "Alice", "score": 1500},
    {"name": "Bob", "score": 1200},
    {"name": "Charlie", "score": 1800},
    {"name": "Diana", "score": 900},
]

for rank, player in enumerate(players, start=1):
    print("#" + str(rank) + " " + player["name"] + ": " + str(player["score"]) + " points")
# #1 Alice: 1500 points
# #2 Bob: 1200 points
# #3 Charlie: 1800 points
# #4 Diana: 900 points

# === Real-world example: To-do list with numbers ===
tasks = ["Buy groceries", "Clean house", "Walk dog", "Call mom", "Pay bills"]

for index, task in enumerate(tasks, start=1):
    print(str(index) + ". [ ] " + task)
# 1. [ ] Buy groceries
# 2. [ ] Clean house
# 3. [ ] Walk dog
# 4. [ ] Call mom
# 5. [ ] Pay bills

# === Real-world example: Updating values by index ===
numbers = [10, 20, 30, 40, 50]

for index, num in enumerate(numbers):
    numbers[index] = num * 2

print(numbers)  # [20, 40, 60, 80, 100]

# === Real-world example: Finding and replacing ===
words = ["cat", "dog", "bird", "cat", "fish", "cat"]
old_word = "cat"
new_word = "wolf"

for index, word in enumerate(words):
    if word == old_word:
        words[index] = new_word

print(words)  # ['wolf', 'dog', 'bird', 'wolf', 'fish', 'wolf']

# === Using enumerate with multiple data ===
products = ["Laptop", "Phone", "Tablet"]
prices = [999, 599, 399]

for index, product in enumerate(products):
    price = prices[index]
    print(product + ": $" + str(price))
# Laptop: $999
# Phone: $599
# Tablet: $399

# Note: prefer zip() for this use case (see next example)
