# Example148.py
# Topic: Truthiness and Falsiness


# ============================================================
# Example 1: Falsy Values
# ============================================================
print("=== Falsy Values ===")

falsy_values: list = [False, None, 0, 0.0, "", [], (), {}, set(), range(0)]

for val in falsy_values:
    print(f"{repr(val):15} → bool: {bool(val)}")


# ============================================================
# Example 2: Truthy Values
# ============================================================
print("\n=== Truthy Values ===")

truthy_values: list = [True, 1, -1, 0.1, "hello", ["item"], (1, 2), {"key": "value"}, {1, 2, 3}, range(1)]

for val in truthy_values:
    print(f"{repr(val):15} → bool: {bool(val)}")


# ============================================================
# Example 3: Checking Empty Collections
# ============================================================
print("\n=== Checking Empty Collections ===")

my_list: list[int] = [1, 2, 3]
if my_list:
    print("List has items")

my_dict: dict[str, int] = {"a": 1}
if my_dict:
    print("Dict has items")

empty_list: list = []
if not empty_list:
    print("Empty list is falsy")


# ============================================================
# Example 4: Default Value Pattern
# ============================================================
print("\n=== Default Value Pattern ===")

name: str | None = None
name = name or "Guest"
print(f"Hello, {name}")

name2: str | None = "Alice"
name2 = name2 or "Guest"
print(f"Hello, {name2}")


# ============================================================
# Example 5: Checking None
# ============================================================
print("\n=== Checking None ===")

value: int | None = 5

if value is not None:
    print(f"Value is {value}")

value = None
if value:
    print(f"Value is truthy: {value}")
else:
    print("Value is falsy (None)")


# ============================================================
# Example 6: Numeric Examples
# ============================================================
print("\n=== Numeric Truthiness ===")

if 0:
    print("This won't print")
else:
    print("0 is falsy")

if -1:
    print("-1 is truthy (negative is still truthy!)")

if 42:
    print("42 is truthy")


# ============================================================
# Example 7: Short-Circuit with And
# ============================================================
print("\n=== Short-Circuit ===")

my_list: list[str] = ["item"]
my_list and print(my_list[0])

empty_list: list = []
empty_list and print(empty_list[0])
print("(nothing printed for empty list)")


# ============================================================
# Example 8: Real-World: Input Processing
# ============================================================
print("\n=== Input Processing ===")

def process_input(user_input: str | None) -> str:
    if user_input:
        cleaned: str = user_input.strip().lower()
        return f"Processing: '{cleaned}'"
    else:
        return "No input provided"

print(process_input("  Hello World  "))
print(process_input(""))
print(process_input(None))


# ============================================================
# Example 9: Validate Number
# ============================================================
print("\n=== Number Validation ===")

def validate_number(number: int | None) -> str:
    if not number:
        return "No number provided or number is zero"
    return f"Valid number: {number}"

print(validate_number(42))
print(validate_number(0))
print(validate_number(None))
