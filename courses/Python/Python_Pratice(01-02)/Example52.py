# Example52.py
# Topic: Loops — While vs For Loop

# When to use while loop vs for loop

# === Comparison ===

# For loop - known number of iterations
print("For loop:")
for i in range(5):
    print(i)
# 0, 1, 2, 3, 4

# While loop - same result, different syntax
print("While loop:")
count = 0
while count < 5:
    print(count)
    count += 1
# 0, 1, 2, 3, 4

# === When to use FOR loop ===
# - Known number of iterations
# - Iterating over a collection
# - Using range()

# Example: iterate a fixed number of times
for i in range(3):
    print("Round " + str(i + 1))
# Round 1, Round 2, Round 3

# === When to use WHILE loop ===
# - Unknown number of iterations
# - Repeat until a condition changes
# - User input validation
# - Game loops

# Example: repeat until user quits
# (simulated without actual input)
status = "running"

while status == "running":
    print("Game is running...")
    status = "stopped"  # Exit condition

print("Game stopped!")

# === Example: Read until end of file ===
# (simulated)
lines = ["line 1", "line 2", "END"]
index = 0

while index < len(lines):
    line = lines[index]
    if line == "END":
        break
    print("Read: " + line)
    index += 1

# === Example: Waiting for condition ===
health = 0

while health < 100:
    print("Healing... health is " + str(health))
    health += 25

print("Fully healed!")
# Healing... health is 0
# Healing... health is 25
# Healing... health is 50
# Healing... health is 75
# Fully healed!

# === For vs While Summary ===
# Use FOR when:
# - You know how many times to loop
# - Iterating over items

# Use WHILE when:
# - You don't know when it'll end
# - Waiting for something

# === Real-world: Menu system ===
# For fixed menu options
print("Using for:")
menu_items = ["Start", "Settings", "Quit"]

for item in menu_items:
    print("- " + item)
# - Start
# - Settings
# - Quit

# While for repeating menu
menu_active = True

while menu_active:
    print("Menu: 1=Play, 2=Settings, 3=Quit")
    choice = "3"  # Simulated input
    
    if choice == "1":
        print("Starting game...")
    elif choice == "2":
        print("Opening settings...")
    elif choice == "3":
        menu_active = False
        print("Goodbye!")
    else:
        print("Invalid choice")

# === Real-world: Retry logic ===
# Using for (fixed attempts)
attempts = 3

for attempt in range(attempts):
    print("Attempt " + str(attempt + 1) + " of " + str(attempts))
    # Try something

print("Done with fixed attempts")

# Using while (unknown attempts)
attempts = 0
max_attempts = 5

while attempts < max_attempts:
    print("Attempt " + str(attempts + 1))
    attempts += 1
    # Try something

print("Done after max attempts")
