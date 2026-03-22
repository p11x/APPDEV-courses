# Example155.py
# Topic: Break, Continue, and Pass


# ============================================================
# Example 1: Break Statement
# ============================================================
print("=== Break ===")

for i in range(10):
    if i == 5:
        break
    print(i, end=" ")
print()
print("Loop ended early")


# ============================================================
# Example 2: Continue Statement
# ============================================================
print("\n=== Continue ===")

for i in range(10):
    if i % 2 == 0:
        continue
    print(i, end=" ")
print()
print("Printed only odd numbers")


# ============================================================
# Example 3: Pass Statement
# ============================================================
print("\n=== Pass ===")

def placeholder_function():
    pass

class EmptyClass:
    pass

if True:
    pass

print("Pass allows empty blocks")


# ============================================================
# Example 4: For-Else Pattern
# ============================================================
print("\n=== For-Else ===")

numbers: list[int] = [1, 2, 3, 4, 5]

for num in numbers:
    if num > 10:
        print("Found number > 10")
        break
else:
    print("No number > 10 found")


# ============================================================
# Example 5: While-Else Pattern
# ============================================================
print("\n=== While-Else ===")

counter: int = 0

while counter < 3:
    print(counter)
    counter += 1
else:
    print("While loop completed normally")


# ============================================================
# Example 6: Searching with Break-Else
# ============================================================
print("\n=== Search Pattern ===")

def find_first_even(numbers: list[int]) -> int | None:
    for i, num in enumerate(numbers):
        if num % 2 == 0:
            return i
    else:
        return None

print(f"First even in [1,3,5,7]: {find_first_even([1,3,5,7])}")
print(f"First even in [1,2,3,4]: {find_first_even([1,2,3,4])}")


# ============================================================
# Example 7: Skip Invalid Data
# ============================================================
print("\n=== Skip Invalid Data ===")

data: list = [1, 2, "invalid", 4, 5, "error", 7]

valid_numbers: list = []
for item in data:
    if isinstance(item, str):
        print(f"Skipping invalid: {item}")
        continue
    valid_numbers.append(item)

print(f"Valid numbers: {valid_numbers}")


# ============================================================
# Example 8: Real-World: Menu Loop
# ============================================================
print("\n=== Menu Loop ===")

def menu_system():
    while True:
        print("\n1. Add")
        print("2. View")
        print("3. Delete")
        print("4. Exit")
        
        choice = input("Choice: ") if False else "1"
        
        match choice:
            case "1":
                print("Adding...")
            case "2":
                print("Viewing...")
            case "3":
                print("Deleting...")
            case "4":
                print("Goodbye!")
                break
            case _:
                print("Invalid choice")

# menu_system()
print("(Menu loop simulated)")
