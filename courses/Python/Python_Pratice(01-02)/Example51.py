# Example51.py
# Topic: Loops — Basic While Loop

# A while loop repeats as long as a condition is True

# === Basic while loop ===
count = 0

while count < 5:
    print(count)
    count = count + 1

# Prints: 0, 1, 2, 3, 4

# === While loop syntax ===
# while condition:
#     # Code to repeat
#     # Must eventually make condition False

# === Counting up ===
counter = 1

while counter <= 10:
    print(counter)
    counter = counter + 1
# Prints: 1, 2, 3, ..., 10

# === Counting down ===
countdown = 5

while countdown > 0:
    print(countdown)
    countdown = countdown - 1

print("Blast off!")
# 5, 4, 3, 2, 1, Blast off!

# === Using shorthand += ===
number = 0

while number < 5:
    print(number)
    number += 1
# 0, 1, 2, 3, 4

# === While with strings ===
message = "Hello"
index = 0

while index < len(message):
    print(message[index])
    index += 1
# H, e, l, l, o

# === Real-world example: Simple timer ===
time_left = 3

while time_left > 0:
    print("Time left: " + str(time_left))
    time_left -= 1

print("Time's up!")
# Time left: 3
# Time left: 2
# Time left: 1
# Time's up!

# === Real-world example: Deposit until goal ===
balance = 0
goal = 100
deposit = 25

while balance < goal:
    balance += deposit
    print("Deposited $" + str(deposit) + ", balance: $" + str(balance))

print("Goal reached!")
# Deposited $25, balance: $25
# Deposited $25, balance: $50
# Deposited $25, balance: $75
# Deposited $25, balance: $100
# Goal reached!
