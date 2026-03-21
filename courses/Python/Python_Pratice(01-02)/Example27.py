# Example27.py
# Topic: Operators — Combining Comparisons and Logical Operators

# You can combine comparison operators with logical operators
# This lets you check complex conditions in a single expression

# Combining comparisons with 'and' — both conditions must be True
age = 25

# Is age between 18 and 65 (inclusive)?
# Must be >= 18 AND <= 65
is_working_age = age >= 18 and age <= 65

print(age)              # 25
print(is_working_age)   # True

# Combining comparisons with 'or' — at least one must be True
score = 85

# Is score above passing OR above bonus threshold?
# Either condition being True gives a positive result
is_good_score = score > 50 or score > 90

print(score)            # 85
print(is_good_score)    # True

# Using 'not' with comparisons — inverts the result
temperature = 30

# Is temperature NOT in the cold range?
is_not_cold = not (temperature < 10)

print(temperature)      # 30
print(is_not_cold)      # True

# Real-world example: determining if a user can rent a car
# Must be 21 or older AND have a valid license
age = 23
has_valid_license = True

can_rent_car = age >= 21 and has_valid_license

print(age)                  # 23
print(has_valid_license)    # True
print(can_rent_car)         # True

# Real-world example: checking if a package can be shipped
# Must weigh less than max limit AND not be fragile
package_weight = 5          # in pounds
is_fragile = False
max_weight = 20             # maximum allowed weight

can_ship = package_weight <= max_weight and not is_fragile

print(package_weight)    # 5
print(is_fragile)       # False
print(can_ship)         # True

# Real-world example: checking loan eligibility
# Must have income above threshold OR have collateral
annual_income = 45000
has_collateral = True
min_income_required = 50000

eligible = annual_income >= min_income_required or has_collateral

print(annual_income)       # 45000
print(has_collateral)      # True
print(eligible)            # True

# Real-world example: determining shipping speed
# Free shipping: order >= $100
# Express: order < $100 but user is premium
order_total = 75
is_premium = True

free_shipping = order_total >= 100
express_shipping = order_total < 100 and is_premium

print(order_total)     # 75
print(is_premium)      # True
print(free_shipping)   # False
print(express_shipping)  # True

# Using parentheses to group conditions
# Without parentheses, 'and' is evaluated before 'or'
x = True
y = False
z = False

# (x and y) or z = False or False = False
result1 = (x and y) or z

# x and (y or z) = True and False = False
result2 = x and (y or z)

print(result1)    # False
print(result2)    # False

# Different results when not using parentheses!
# and has higher precedence than or
result3 = x and y or z   # same as (x and y) or z
result4 = x or y and z   # same as x or (y and z)

print(result3)    # False
print(result4)   # True
