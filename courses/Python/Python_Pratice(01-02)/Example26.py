# Example26.py
# Topic: Operators — Logical Operators (and, or, not)

# Logical operators combine boolean values (True/False)
# They help you check multiple conditions at once

# The 'and' operator — BOTH must be True for the result to be True
# True and True = True
# True and False = False
# False and True = False
# False and False = False

result = True and True      # bool  — both must be True
print(True and True)        # True

result = True and False     # bool  — one is False
print(True and False)       # False

result = False and True     # bool  — one is False
print(False and True)       # False

result = False and False    # bool  — both are False
print(False and False)      # False

# The 'or' operator — AT LEAST ONE must be True for the result to be True
# True or True = True
# True or False = True
# False or True = True
# False or False = False

result = True or True       # bool  — at least one is True
print(True or True)         # True

result = True or False      # bool  — one is True
print(True or False)        # True

result = False or True      # bool  — one is True
print(False or True)        # True

result = False or False     # bool  — both are False
print(False or False)       # False

# The 'not' operator — INVERTS the value (flips True to False, False to True)
# not True = False
# not False = True

result = not True           # bool  — flips True to False
print(not True)             # False

result = not False          # bool  — flips False to True
print(not False)            # True

# Double negation — not not True brings you back to True
result = not not True        # bool  — flips twice, back to original
print(not not True)          # True

# Real-world example: checking if user can access a premium feature
# User must be logged in AND have premium status
is_logged_in = True
has_premium = True
can_access = is_logged_in and has_premium

print(is_logged_in)    # True
print(has_premium)     # True
print(can_access)      # True

# Real-world example: checking if a student can enroll
# Student must have completed prerequisites OR have instructor permission
completed_prereqs = False
has_instructor_permission = True
can_enroll = completed_prereqs or has_instructor_permission

print(completed_prereqs)            # False
print(has_instructor_permission)    # True
print(can_enroll)                   # True

# Real-world example: checking if access should be denied
# If user is NOT active, deny access
is_active = True
should_deny = not is_active

print(is_active)      # True
print(should_deny)    # False

# Combining 'not' with comparison operators
age = 25
is_minor = not (age >= 18)   # bool  — True if under 18

print(age)           # 25
print(is_minor)      # False

# Real-world example: checking if a discount applies
# No discount if user is a VIP OR already got a promotion
is_vip = False
got_promotion = True
no_discount = is_vip or got_promotion

print(is_vip)          # False
print(got_promotion)   # True
print(no_discount)     # True
