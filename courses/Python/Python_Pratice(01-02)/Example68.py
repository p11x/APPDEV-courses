# Example68.py
# Topic: Exception Handling — The Else Block

# The else block runs ONLY if no exception occurred

# === Basic else ===
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Can't divide by zero")
else:
    print("Division successful: " + str(result))
# Output: Division successful: 5.0

# === Else with error ===
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero")
else:
    print("Division successful: " + str(result))
# Output: Can't divide by zero
# (else block doesn't run!)

# === Real-world: Get user input ===
def get_positive_number():
    while True:
        try:
            value = int(input("Enter a positive number: "))
            if value <= 0:
                print("Must be positive!")
                continue
        except ValueError:
            print("That's not a number!")
        else:
            # Only runs if no exception AND value is valid
            return value
            break

# Note: This is simplified - in practice would need actual input

# === Practical example ===
def divide(a, b):
    try:
        result = a / b
    except ZeroDivisionError:
        return "Error: Division by zero"
    else:
        print("Division completed successfully")
        return result

print(divide(10, 2))   # prints message, returns 5.0
print(divide(10, 0))   # returns error, doesn't print

# === File reading example ===
# def read_number(filename):
#     try:
#         f = open(filename, "r")
#         number = int(f.read())
#     except FileNotFoundError:
#         return "File not found"
#     except ValueError:
#         return "Invalid number in file"
#     else:
#         print("File read successfully")
#         return number

# === Use case: Validation ===
def validate_and_process(data):
    try:
        name = data["name"]
        age = data["age"]
    except KeyError as e:
        return "Missing key: " + str(e)
    else:
        # Only runs if no KeyError
        return "Valid data: " + name + ", " + str(age)

print(validate_and_process({"name": "Alice", "age": 25}))
print(validate_and_process({"name": "Bob"}))

# === When else is useful ===
# - Code that should run ONLY on success
# - Distinguishing success from failure
# - Clean separation of try/except/else

# Without else - always runs
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Failed")
print("Done")  # Always runs

# With else - only on success
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Failed")
else:
    print("Success: " + str(result))  # Only runs on success
