# Example146.py
# Topic: If, Elif, and Else Basics


# ============================================================
# Example 1: Basic If Statement
# ============================================================
print("=== Basic If ===")

age: int = 18

if age >= 18:
    print("You are an adult")


# ============================================================
# Example 2: If-Else Statement
# ============================================================
print("\n=== If-Else ===")

age: int = 16

if age >= 18:
    print("You are an adult")
else:
    print("You are a minor")


# ============================================================
# Example 3: If-Elif-Else Chain
# ============================================================
print("\n=== If-Elif-Else ===")

score: int = 85

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


# ============================================================
# Example 4: Nested Conditionals
# ============================================================
print("\n=== Nested Conditionals ===")

age: int = 25
has_license: bool = True

if age >= 18:
    if has_license:
        print("You can drive!")
    else:
        print("You're old enough but need a license")
else:
    print("You're too young to drive")


# ============================================================
# Example 5: Logical Operators in Conditionals
# ============================================================
print("\n=== Logical Operators ===")

age: int = 25
has_license: bool = True

if age >= 18 and has_license:
    print("You can drive!")
elif age >= 18 and not has_license:
    print("You're old enough but need a license")
else:
    print("You're too young to drive")


# ============================================================
# Example 6: Ternary Operator
# ============================================================
print("\n=== Ternary Operator ===")

age: int = 20

status: str = "adult" if age >= 18 else "minor"
print(f"Status: {status}")

a: int = 5
b: int = 10
max_val: int = a if a > b else b
print(f"Max: {max_val}")

x: int = -5
abs_val: int = x if x > 0 else -x
print(f"Absolute: {abs_val}")

num: int = 7
result: str = "even" if num % 2 == 0 else "odd"
print(f"{num} is {result}")


# ============================================================
# Example 7: Real-World: Grade Calculator
# ============================================================
print("\n=== Grade Calculator ===")

def calculate_grade(score: int) -> str:
    if score < 0 or score > 100:
        return "Invalid score"
    
    if score == 100:
        return "A+ (Perfect!)"
    elif score >= 90:
        return "A - Excellent!"
    elif score >= 80:
        return "B - Good job!"
    elif score >= 70:
        return "C - Satisfactory"
    elif score >= 60:
        return "D - Needs improvement"
    else:
        return "F - Failed"

test_scores: list[int] = [100, 95, 85, 75, 65, 55, -5, 105]

for score in test_scores:
    grade: str = calculate_grade(score)
    print(f"Score: {score:3d} → {grade}")


# ============================================================
# Example 8: Multiple Conditions with Or
# ============================================================
print("\n=== Multiple Conditions ===")

day: str = "saturday"

if day == "monday" or day == "tuesday" or day == "wednesday" or day == "thursday" or day == "friday":
    print("Weekday")
elif day == "saturday" or day == "sunday":
    print("Weekend")
else:
    print("Not a day")
