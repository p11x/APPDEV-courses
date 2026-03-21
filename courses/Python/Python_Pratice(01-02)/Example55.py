# Example55.py
# Topic: Loops — Infinite Loops (Danger!)

# An infinite loop never ends - this is usually a BUG!
# Must be very careful with while loops

# === WRONG: Infinite loop! ===
# count = 0
# while count < 5:
#     print(count)
#     # Forgot to update count - runs forever!

# === Must-Update Pattern ===
# Always update the variable that controls the loop!

# WRONG - count never changes
# count = 0
# while count < 5:
#     print(count)
#     # Infinite loop!

# CORRECT - update count
count = 0
while count < 5:
    print(count)
    count = count + 1
# 0, 1, 2, 3, 4

# === Safe Infinite Loop ===
# Use while True with break to control when to exit

# Safe pattern
while True:
    print("Running...")
    # Do something
    should_stop = True  # Exit condition
    
    if should_stop:
        break
# Runs once then exits

# === Real-world: Game loop ===
game_running = True

while game_running:
    print("Game is running...")
    
    # Simulate game logic
    player_quit = True
    
    if player_quit:
        print("Player quit the game")
        break

print("Game loop ended")

# === Real-world: Menu with while True ===
print("Menu system:")

while True:
    print("1. Play")
    print("2. Settings")
    print("3. Quit")
    choice = "3"  # Simulated input
    
    if choice == "1":
        print("Starting game...")
    elif choice == "2":
        print("Opening settings...")
    elif choice == "3":
        print("Goodbye!")
        break
    else:
        print("Invalid choice, try again")

print("Exited menu")

# === Danger: Forgetting to update ===
# WRONG
# number = 10
# while number > 0:
#     print(number)
#     # Forgot: number -= 1 - infinite loop!

# CORRECT
number = 10
while number > 0:
    print(number)
    number -= 1

print("Countdown done!")

# === Using input with while True ===
# This pattern is common but be careful!
# In real code:
# while True:
#     user_input = input("Enter command: ")
#     if user_input == "quit":
#         break
#     # process input

# === Counter never reaching condition ===
# WRONG - condition already met
# count = 10
# while count < 5:
#     print(count)
#     count += 1

# This won't run at all because count=10 is already not < 5

count = 10
while count < 5:
    print(count)
else:
    print("Condition was already False, loop didn't run")

# === Real-world: Waiting for event ===
# Better approach: use a maximum wait time
max_wait = 5
wait_count = 0
event_happened = False

while wait_count < max_wait:
    print("Waiting... " + str(wait_count + 1))
    wait_count += 1
    
    # Simulate checking for event
    if wait_count == 3:
        event_happened = True
    
    if event_happened:
        print("Event happened!")
        break
else:
    print("Timed out waiting for event")

# === Always have exit condition ===
# Good practice: always have a way to exit

# Safe template
attempts = 0
max_attempts = 3

while attempts < max_attempts:
    print("Attempt " + str(attempts + 1))
    attempts += 1
    
    # Check success
    success = attempts == 3
    
    if success:
        print("Success!")
        break
else:
    print("Max attempts reached")

# === Infinite loop protection ===
# Some editors/IDEs will kill the process if it runs too long
# But don't rely on that - fix your code!
