# Example32.py
# Topic: Control Flow — If, Elif, and Else

# The if statement lets your program make decisions
# Based on whether a condition is True or False

# === Basic if Statement ===
# The if runs the code block ONLY if the condition is True

age = 18                       # int  — person's age

if age >= 18:
    print("You are an adult")  # This runs because age >= 18 is True

# === if with else ===
# else runs when the if condition is False

age = 16                       # int  — person's age

if age >= 18:
    print("You are an adult")
else:
    print("You are a minor")   # This runs because age >= 18 is False

# === if with elif and else ===
# elif checks multiple conditions in order

score = 85                     # int  — test score

if score >= 90:
    print("A - Excellent!")
elif score >= 80:
    print("B - Good job!")
elif score >= 70:
    print("C - Satisfactory")
elif score >= 60:
    print("D - Needs improvement")
else:
    print("F - Failed")

# The conditions are checked in order from top to bottom
# Once one condition is True, the rest are skipped

# === How elif works ===
# If score is 85:
#   - score >= 90? No (False) → continue
#   - score >= 80? Yes (True) → print "B - Good job!" → STOP

# === Multiple elif examples ===
score = 95                    # int  — higher score

if score >= 90:
    print("A - Excellent!")   # This one prints
elif score >= 80:
    print("B - Good job!")
elif score >= 70:
    print("C - Satisfactory")

score = 55                    # int  — lower score

if score >= 90:
    print("A - Excellent!")
elif score >= 80:
    print("B - Good job!")
elif score >= 70:
    print("C - Satisfactory")
elif score >= 60:
    print("D - Needs improvement")
else:
    print("F - Failed")       # This one prints

# === Real-world example: Age categories ===
age = 45

if age < 13:
    print("Child")
elif age < 20:
    print("Teenager")
elif age < 65:
    print("Adult")
else:
    print("Senior citizen")

# === Real-world example: Shipping cost ===
order_total = 75.0             # float  — order amount in dollars

if order_total >= 100.0:
    print("Free shipping!")
elif order_total >= 50.0:
    print("Shipping: $5.00")
else:
    print("Shipping: $10.00")

# === Real-world example: Temperature advice ===
temperature = 72              # int  — temperature in Fahrenheit

if temperature >= 90:
    print("It's hot! Stay hydrated.")
elif temperature >= 70:
    print("Nice weather!")
elif temperature >= 50:
    print("A bit chilly, bring a jacket.")
else:
    print("It's cold! Bundle up.")

# === Multiple conditions with elif ===
# Each elif is a separate check

user_role = "admin"            # str  — user's role

if user_role == "admin":
    print("Full access granted")
elif user_role == "editor":
    print("Can edit content")
elif user_role == "viewer":
    print("Can view content only")
else:
    print("Unknown role")
