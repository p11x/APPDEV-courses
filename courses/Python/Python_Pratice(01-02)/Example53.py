# Example53.py
# Topic: Loops — Else Block with Loops

# Python has a unique feature: else block on loops
# The else block runs when the loop completes NORMALLY (not when break is used)

# === Basic while with else ===
count = 0

while count < 3:
    print("Count: " + str(count))
    count += 1
else:
    print("Loop completed!")

# Output:
# Count: 0
# Count: 1
# Count: 2
# Loop completed!

# === For loop with else ===
for i in range(3):
    print(i)
else:
    print("For loop done!")

# 0
# 1
# 2
# For loop done!

# === Why use else? ===
# The else block tells you if the loop finished naturally or was broken

# Example: Search for a number
numbers = [1, 2, 3, 4, 5]
target = 7
index = 0
found = False

while index < len(numbers):
    if numbers[index] == target:
        found = True
        print("Found " + str(target) + " at index " + str(index))
        break
    index += 1

if not found:
    print(str(target) + " not found in list")

# Using else to simplify:
numbers = [1, 2, 3, 4, 5]
target = 7
index = 0

while index < len(numbers):
    if numbers[index] == target:
        print("Found " + str(target) + " at index " + str(index))
        break
    index += 1
else:
    # Only runs if loop wasn't broken
    print(str(target) + " not found in list")
# 7 not found in list

# === Real-world example: Login attempt ===
# Try to find a valid username in database
usernames = ["alice", "bob", "charlie"]
search_for = "david"
index = 0

while index < len(usernames):
    if usernames[index] == search_for:
        print("User found: " + search_for)
        break
    index += 1
else:
    print("User " + search_for + " not found")
# User david not found

# === Real-world example: Process items until condition ===
items = [1, 2, 3, 4, 5]
threshold = 3
processed = []

while items:
    item = items.pop(0)
    if item > threshold:
        print("Stopping - " + str(item) + " exceeds threshold")
        break
    processed.append(item)

else:
    print("Processed all items: " + str(processed))

# === Real-world example: Wait for event ===
attempts = 0
max_attempts = 3

while attempts < max_attempts:
    print("Attempt " + str(attempts + 1))
    attempts += 1
    # Simulate checking for event
    event_ready = attempts == 2
    
    if event_ready:
        print("Event found!")
        break
else:
    print("No event after " + str(max_attempts) + " attempts")

# === Practical: Prime checker ===
def is_prime(n):
    if n < 2:
        return False
    
    divisor = 2
    
    while divisor * divisor <= n:
        if n % divisor == 0:
            return False
        divisor += 1
    else:
        # Loop completed normally - no divisor found
        return True

print("Is 7 prime?", is_prime(7))  # True
print("Is 10 prime?", is_prime(10))  # False

# === Difference between break and else ===
# With break - else doesn't run
numbers = [1, 2, 3]

for n in numbers:
    if n == 2:
        print("Found 2!")
        break
else:
    print("Loop completed")

# Output: Found 2!

# Without break - else runs
numbers = [1, 2, 3]

for n in numbers:
    print(n)
else:
    print("Loop completed")

# Output: 1, 2, 3, Loop completed
