# Example33.py
# Topic: Control Flow — Nested Conditionals

# Nested conditionals means putting if statements inside other if statements
# This is useful when you need to check multiple levels of conditions

# === Basic Nested If ===
# Check age first, then check if they have a license

age = 25                      # int  — person's age
has_license = True           # bool  — whether they have a license

if age >= 18:
    # This block only runs if age >= 18
    if has_license:
        print("You can drive!")     # Both conditions are True
    else:
        print("You're old enough but need a license")
else:
    print("You're too young to drive")

# === Another nested example ===
# Age 16, no license
age = 16
has_license = False

if age >= 18:
    if has_license:
        print("You can drive!")
    else:
        print("You're old enough but need a license")
else:
    print("You're too young to drive")  # This prints

# === Better version: Using Logical Operators ===
# The nested version can be simplified with 'and', 'or', 'not'

age = 25
has_license = True

if age >= 18 and has_license:
    print("You can drive!")
elif age >= 18 and not has_license:
    print("You're old enough but need a license")
else:
    print("You're too young to drive")

# Both versions work, but the second is cleaner for simple cases
# Nested ifs are better when you need to do different things at each level

# === Real-world example: Online Store Discount ===
# First check if user is logged in, then check membership status

is_logged_in = True           # bool  — user logged in
is_premium = True              # bool  — premium membership
order_amount = 150.0          # float  — order total

if is_logged_in:
    # User is logged in, check membership
    if is_premium:
        discount = 0.20       # 20% off for premium
        print("Premium member: 20% discount!")
    else:
        discount = 0.10       # 10% off for regular members
        print("Regular member: 10% discount")
else:
    discount = 0.0            # No discount for guests
    print("Guest: no discount")

final_price = order_amount * (1 - discount)
print("Final price: $" + str(final_price))

# === Real-world example: Access Control System ===
# Check role and specific permissions

user_role = "editor"          # str  — user role
is_active = True              # bool  — account is active

if user_role == "admin":
    if is_active:
        print("Full access: can manage users and content")
    else:
        print("Account is disabled")
elif user_role == "editor":
    if is_active:
        print("Can edit and publish content")
    else:
        print("Account is disabled")
elif user_role == "viewer":
    if is_active:
        print("Can only view content")
    else:
        print("Account is disabled")
else:
    print("Unknown role - access denied")

# === Real-world example: Shipping Options ===
# Check destination first, then weight

destination = "international"  # str  — domestic or international
weight = 5.0                  # float  — package weight in kg

if destination == "domestic":
    if weight < 1.0:
        print("Standard shipping: $5")
    elif weight < 5.0:
        print("Express shipping: $10")
    else:
        print("Heavy item shipping: $20")
else:
    # International shipping
    if weight < 1.0:
        print("International standard: $15")
    elif weight < 5.0:
        print("International express: $30")
    else:
        print("International heavy: $50")

# === Real-world example: Game Score System ===
# Check player level first, then score ranges

player_level = 2              # int  — player level (1, 2, or 3)
score = 850                   # int  — player's score

if player_level == 1:
    if score >= 500:
        print("Beginner champion!")
    else:
        print("Keep playing, beginner!")
elif player_level == 2:
    if score >= 700:
        print("Intermediate champion!")
    else:
        print("Almost there, intermediate!")
elif player_level == 3:
    if score >= 900:
        print("Expert champion!")
    else:
        print("Expert level - keep trying!")
