# Example54.py
# Topic: Loops — Break and Continue

# break - Exit the loop immediately
# continue - Skip to the next iteration

# === Break - Exit the loop ===
print("Using break:")
count = 0

while count < 10:
    if count == 5:
        break  # Exit loop when count is 5
    print(count)
    count += 1

print("Loop exited at count = " + str(count))
# 0, 1, 2, 3, 4, 5
# Loop exited at count = 5

# === Break in for loop ===
print("\nBreak in for loop:")
for i in range(10):
    if i == 3:
        break
    print(i)
# 0, 1, 2

# === Continue - Skip iteration ===
print("\nUsing continue:")
count = 0

while count < 5:
    count += 1
    if count == 3:
        continue  # Skip when count is 3
    print(count)
# 1, 2, 4, 5 (3 is skipped)

# === Continue in for loop ===
print("\nContinue in for loop:")
for i in range(5):
    if i == 2:
        continue
    print(i)
# 0, 1, 3, 4 (2 is skipped)

# === Real-world: Menu with break ===
# Simulating menu selection
print("\nMenu system:")
menu_active = True

while menu_active:
    print("1. Start game")
    print("2. Load game")
    print("3. Quit")
    choice = "3"  # Simulated input
    
    if choice == "1":
        print("Starting game...")
    elif choice == "2":
        print("Loading game...")
    elif choice == "3":
        print("Quitting...")
        break
    else:
        print("Invalid choice")

print("Back to main menu")

# === Real-world: Search and stop ===
print("\nSearching:")
items = [1, 2, 3, 4, 5]
target = 3

for item in items:
    if item == target:
        print("Found " + str(target) + "! Stopping.")
        break
    print("Checking " + str(item) + "...")
# Checking 1...
# Checking 2...
# Found 3! Stopping.

# === Real-world: Skip invalid data ===
print("\nProcessing valid items:")
data = [10, 20, 0, 40, 0, 60]
total = 0

for value in data:
    if value == 0:
        print("Skipping zero value")
        continue  # Skip zeros
    total += value
    print("Added " + str(value))

print("Total: " + str(total))
# Added 10
# Added 20
# Skipping zero value
# Added 40
# Skipping zero value
# Added 60
# Total: 130

# === Break vs Continue ===
# Break: exits the entire loop
# Continue: skips only this iteration

print("\nBreak example:")
for i in range(5):
    if i == 2:
        break
    print(i)
# 0, 1

print("\nContinue example:")
for i in range(5):
    if i == 2:
        continue
    print(i)
# 0, 1, 3, 4

# === Real-world: Password validation ===
print("\nPassword check:")
password = "wrong"
attempts = 0
max_attempts = 3

while attempts < max_attempts:
    attempts += 1
    print("Attempt " + str(attempts))
    
    if password == "secret":
        print("Access granted!")
        break
    else:
        print("Wrong password")

else:
    print("Account locked!")

# === Real-world: Filter even numbers ===
print("\nFilter odd numbers:")
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
odd_numbers = []

for num in numbers:
    if num % 2 == 0:
        continue  # Skip even
    odd_numbers.append(num)

print("Odd numbers: " + str(odd_numbers))
# Odd numbers: [1, 3, 5, 7, 9]
