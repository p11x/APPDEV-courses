# Example37.py
# Topic: Control Flow — When to Use Match vs If/Elif

# Match is great for comparing ONE value against MANY options
# If/elif is better for multiple different conditions

# === Use Match When ===
# 1. Comparing one value against many literal options
# 2. Pattern matching (structures, sequences)
# 3. Replacing complex if/elif chains
# 4. Clean, readable dispatch logic

# === Use If/Elif When ===
# 1. Multiple different conditions
# 2. Complex boolean expressions
# 3. Conditions that don't share a common subject
# 4. Simple true/false checks

# === Match Best For: Command Dispatch ===

# Using match — CLEAN
command = "start"

match command:
    case "start":
        print("Starting the system...")
    case "stop":
        print("Stopping the system...")
    case "restart":
        print("Restarting the system...")
    case "status":
        print("Checking status...")
    case _:
        print("Unknown command")

# Using if/elif — WORKS BUT LONGER
command = "start"

if command == "start":
    print("Starting the system...")
elif command == "stop":
    print("Stopping the system...")
elif command == "restart":
    print("Restarting the system...")
elif command == "status":
    print("Checking status...")
else:
    print("Unknown command")

# === If/Elif Best For: Complex Conditions ===

# Multiple different conditions — use if/elif
age = 25
has_income = True
credit_score = 720

# Complex boolean — if/elif is clearer
if age >= 18 and has_income:
    print("May qualify for credit")
elif credit_score >= 700:
    print("Good credit score")
elif age < 18:
    print("Must be 18 or older")
else:
    print("Does not qualify")

# Trying to do this with match would be messy!

# === Match vs If: Side-by-Side Examples ===

# Example 1: Grade calculator

# With match — great for exact matches
score = 85

match score:
    case 100:
        print("Perfect score!")
    case 90 | 91 | 92 | 93 | 94 | 95 | 96 | 97 | 98 | 99:
        print("A grade")
    case 80 | 81 | 82 | 83 | 84 | 85 | 86 | 87 | 88 | 89:
        print("B grade")
    case _:
        print("Other grade")

# With if/elif — can use ranges
score = 85

if score >= 90:
    print("A grade")
elif score >= 80:
    print("B grade")
elif score >= 70:
    print("C grade")
elif score >= 60:
    print("D grade")
else:
    print("F grade")

# Note: For ranges, if/elif is often simpler

# === Match with OR Patterns ===
# Cleaner than multiple OR in if/elif

status = "pending"

# Match with OR
match status:
    case "active" | "running" | "online":
        print("System is up")
    case "stopped" | "offline" | "error":
        print("System is down")
    case _:
        print("Unknown")

# Equivalent if/elif
if status == "active" or status == "running" or status == "online":
    print("System is up")
elif status == "stopped" or status == "offline" or status == "error":
    print("System is down")
else:
    print("Unknown")

# === Real-World Decision Guide ===

# Use MATCH when:
# - You have a menu system
# - Processing HTTP status codes
# - Parsing command-line arguments
# - Working with enum-like values
# - Data validation against known patterns

# Use IF/ELIF when:
# - Checking if a number is in a range
# - Validating multiple fields together
# - Complex business logic
# - When conditions don't share a common variable

# === Practical Example: Order Processing ===

# Using match for order type
order_type = "express"

match order_type:
    case "standard":
        delivery_days = 5
        cost = 5.0
    case "express":
        delivery_days = 2
        cost = 15.0
    case "overnight":
        delivery_days = 1
        cost = 30.0
    case _:
        delivery_days = 0
        cost = 0.0

print("Delivery in " + str(delivery_days) + " days, cost $" + str(cost))

# Using if/elif for complex validation
order_total = 150.0
is_premium = True
coupon_code = "SAVE10"

discount = 0.0

if order_total >= 100.0:
    discount = 0.10
elif is_premium:
    discount = 0.15
elif coupon_code == "SAVE10":
    discount = 0.10

print("Discount: " + str(int(discount * 100)) + "%")

# === Summary ===
# Match is NOT a replacement for if/elif in all cases
# Choose the right tool for the job!
