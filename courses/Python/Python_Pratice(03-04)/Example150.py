# Example150.py
# Topic: While Loops


# ============================================================
# Example 1: Basic While Loop
# ============================================================
print("=== Basic While ===")

count: int = 0

while count < 5:
    print(f"count = {count}")
    count += 1


# ============================================================
# Example 2: While with User Input Simulation
# ============================================================
print("\n=== While with Condition ===")

password: str = ""
attempts: int = 0
correct: str = "secret"

while password != correct and attempts < 3:
    password = input("Enter password: ") if False else "wrong"
    attempts += 1
    print(f"Attempt {attempts}")

print("Access granted!" if password == correct else "Locked out!")


# ============================================================
# Example 3: While-Else
# ============================================================
print("\n=== While-Else ===")

numbers: list[int] = [1, 2, 3, 4, 5]
target: int = 10
index: int = 0

while index < len(numbers):
    if numbers[index] == target:
        print(f"Found {target} at index {index}")
        break
    index += 1
else:
    print(f"{target} not found in list")


# ============================================================
# Example 4: While with Break
# ============================================================
print("\n=== While with Break ===")

while True:
    user_input = input("Enter 'quit' to exit: ") if False else "continue"
    if user_input == "quit":
        print("Exiting...")
        break
    print(f"You entered: {user_input}")


# ============================================================
# Example 5: Infinite Loop with Flag
# ============================================================
print("\n=== Infinite Loop ===")

running: bool = True
counter: int = 0

while running:
    counter += 1
    print(f"Counter: {counter}")
    if counter >= 3:
        running = False

print("Done!")


# ============================================================
# Example 6: While for Input Validation
# ============================================================
print("\n=== Input Validation ===")

def get_positive_number() -> int:
    while True:
        user_input = input("Enter a positive number: ") if False else "abc"
        try:
            num = int(user_input)
            if num > 0:
                return num
            else:
                print("Number must be positive!")
        except ValueError:
            print("Invalid input. Please enter a number.")

# result = get_positive_number()
# print(f"You entered: {result}")


# ============================================================
# Example 7: Real-World: Menu System
# ============================================================
print("\n=== Menu System ===")

menu_options: list[str] = ["1. View Profile", "2. Edit Profile", "3. Settings", "4. Exit"]

choice: str = ""
while choice != "4":
    print("\nMenu:")
    for option in menu_options:
        print(option)
    
    choice = input("Select option: ") if False else "1"
    print(f"You selected: {choice}")
    
    match choice:
        case "1":
            print("Viewing profile...")
        case "2":
            print("Editing profile...")
        case "3":
            print("Opening settings...")
        case "4":
            print("Goodbye!")
        case _:
            print("Invalid option")


# ============================================================
# Example 8: While with Continue
# ============================================================
print("\n=== While with Continue ===")

counter: int = 0

while counter < 10:
    counter += 1
    if counter % 2 == 0:
        continue
    print(counter, end=" ")
print()
