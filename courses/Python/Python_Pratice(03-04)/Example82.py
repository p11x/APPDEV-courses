# Example82.py
# Topic: Common Mistakes with Pure Functions

# This file shows common mistakes when writing pure functions.


# ============================================================
# Example 1: Modifying Arguments
# ============================================================
print("=== Mistake: Modifying Arguments ===")

# BAD: Impure - modifies input
def add_item_bad(items, item):
    items.append(item)  # Modifies original list
    return items

# GOOD: Pure - returns new list
def add_item_pure(items, item):
    return items + [item]

# Test
original = [1, 2, 3]
result = add_item_bad(original, 4)
print(f"Original after BAD: {original}")  # Modified!

original2 = [1, 2, 3]
result2 = add_item_pure(original2, 4)
print(f"Original after GOOD: {original2}")  # Unchanged
print(f"Result: {result2}")


# ============================================================
# Example 2: Using Global State
# ============================================================
print("\n=== Mistake: Using Global State ===")

# BAD: Depends on global variable
tax_rate = 0.1

def calculate_tax_bad(amount):
    return amount * tax_rate  # Depends on global

# GOOD: Pass as parameter
def calculate_tax_pure(amount, tax_rate):
    return amount * tax_rate

# Test
print(f"With global: {calculate_tax_bad(100)}")
print(f"With param: {calculate_tax_pure(100, 0.1)}")


# ============================================================
# Example 3: Side Effects (I/O)
# ============================================================
print("\n=== Mistake: Side Effects ===")

# BAD: Prints to console
def log_message_bad(message):
    print(f"LOG: {message}")  # Side effect

# BAD: Reads from file
def read_file_bad(filename):
    with open(filename, 'r') as f:  # Side effect
        return f.read()

# GOOD: Pure - no I/O
def format_message_pure(message, level):
    return f"{level}: {message}"

# If I/O needed, separate from pure logic
def process_and_log(message):
    formatted = format_message_pure(message, "INFO")
    # Log separately (in main code)
    return formatted


# ============================================================
# Example 4: Random Numbers
# ============================================================
print("\n=== Mistake: Random Numbers ===")

import random

# BAD: Uses random - not deterministic
def roll_dice_bad():
    return random.randint(1, 6)  # Different each time

# GOOD: Pass seed or use random module properly
def roll_dice_pure(seed):
    random.seed(seed)
    return random.randint(1, 6)

# Or use random module's functions
def generate_random_pure(lower, upper):
    import random
    return random.randint(lower, upper)

# Test determinism with seed
print(f"With seed 42: {roll_dice_pure(42)}")
print(f"With seed 42 again: {roll_dice_pure(42)}")


# ============================================================
# Example 5: Time-Dependent Behavior
# ============================================================
print("\n=== Mistake: Time-Dependent Behavior ===")

import time

# BAD: Depends on current time
def is_expired_bad(timestamp):
    return time.time() > timestamp  # Different each call

# GOOD: Pass current time as parameter
def is_expired_pure(timestamp, current_time):
    return current_time > timestamp

# Test
future = time.time() + 1000
print(f"With current time: {is_expired_pure(future, time.time())}")
print(f"Past timestamp: {is_expired_pure(time.time() - 100, time.time())}")


# ============================================================
# Example 6: Mixing Pure and Impure
# ============================================================
print("\n=== Mistake: Mixing Pure and Impure ===")

# BAD: Mixed concerns
def process_and_save_bad(data):
    # I/O mixed with logic
    validated = validate(data)  # Pure
    result = transform(validated)  # Pure
    save_to_file(result)  # Impure - I/O
    return result

# GOOD: Separate concerns
def validate(data):
    return data

def transform(data):
    return data.upper()

def save_to_file(data):
    print(f"Saving: {data}")

def process(data):
    """Pure orchestration."""
    validated = validate(data)
    return transform(validated)

def main(data):
    """Impure - calls pure and handles I/O."""
    result = process(data)
    save_to_file(result)
    return result


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMMON MISTAKES: PURE FUNCTIONS")
print("=" * 50)
print("""
AVOID:
- Modifying function arguments
- Using global variables
- Side effects (print, file I/O)
- Random numbers without seeding
- Time-dependent behavior

REMEMBER:
- Same input → Same output
- No side effects
- Don't modify arguments
- Pass dependencies as parameters
""")
