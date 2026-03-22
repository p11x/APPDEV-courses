# Example72.py
# Topic: functools - partial

# This file demonstrates functools.partial for currying.


# ============================================================
# Example 1: Basic partial
# ============================================================
print("=== functools.partial - Basic ===")

from functools import partial

def power(base, exponent):
    return base ** exponent

# Create specialized functions
square = partial(power, exponent=2)
cube = partial(power, exponent=3)
power_of_4 = partial(power, exponent=4)

print(f"square(5): {square(5)}")      # 5^2 = 25
print(f"cube(5): {cube(5)}")          # 5^3 = 125
print(f"power_of_4(5): {power_of_4(5)}")  # 5^4 = 625


# ============================================================
# Example 2: partial with positional args
# ============================================================
print("\n=== partial with positional args ===")

def greet(greeting, name, punctuation):
    return f"{greeting}, {name}{punctuation}"

# Pre-fill first argument
say_hello = partial(greet, "Hello")
print(f"say_hello('World', '!'): {say_hello('World', '!')}")

# Pre-fill multiple arguments
excited = partial(greet, "Hi", "Alice", "!!!")
print(f"excited(): {excited()}")


# ============================================================
# Example 3: partial for event handlers
# ============================================================
print("\n=== Real-world: Event Handlers ===")

from functools import partial

def on_click(button_id, callback, data):
    print(f"Button {button_id} clicked with callback {callback}, data: {data}")

# Create handlers for specific buttons
button1_handler = partial(on_click, "btn_submit", callback="submit_form")
button2_handler = partial(on_click, "btn_cancel", callback="reset_form")

# Simulate clicks
button1_handler(data={"form": "contact"})
button2_handler(data={"form": "contact"})


# ============================================================
# Example 4: partial with map
# ============================================================
print("\n=== partial with map ===")

from functools import partial

def multiply(a, b):
    return a * b

# Create doubling function
double = partial(multiply, 2)

# Use with map
numbers = [1, 2, 3, 4, 5]
doubled = list(map(double, numbers))
print(f"Double {numbers}: {doubled}")

# Triple
triple = partial(multiply, 3)
tripled = list(map(triple, numbers))
print(f"Triple {numbers}: {tripled}")


# ============================================================
# Example 5: partial for formatting
# ============================================================
print("\n=== Real-world: Formatting ===")

from functools import partial

def format_currency(value, symbol, decimal_places):
    return f"{symbol}{value:.{decimal_places}f}"

# Create formatters
usd = partial(format_currency, symbol="$", decimal_places=2)
eur = partial(format_currency, symbol="€", decimal_places=2)
jpy = partial(format_currency, symbol="¥", decimal_places=0)

print(f"USD 1234.5: {usd(1234.5)}")
print(f"EUR 1234.5: {eur(1234.5)}")
print(f"JPY 1234.5: {jpy(1234.5)}")


# ============================================================
# Example 6: partial with sorting
# ============================================================
print("\n=== partial with sorting ===")

from functools import partial

def sort_key(item, field, reverse=False):
    value = item[field]
    return -value if reverse else value

# Create sorters
sort_by_name = partial(sort_key, field='name')
sort_by_age = partial(sort_key, field='age', reverse=True)

people = [
    {'name': 'Alice', 'age': 25},
    {'name': 'Bob', 'age': 30},
    {'name': 'Charlie', 'age': 20},
]

# Sort by name
print("By name:", sorted(people, key=lambda x: x['name']))

# Sort by age (descending) using partial
print("By age (desc):", sorted(people, key=partial(sort_key, field='age', reverse=True)))


# ============================================================
# Example 7: partial for function wrapping
# ============================================================
print("\n=== partial for wrapping ===")

from functools import partial

def send_email(to, subject, body, priority):
    print(f"To: {to}")
    print(f"Subject: {subject}")
    print(f"Body: {body}")
    print(f"Priority: {priority}")

# Pre-configured email functions
send_notification = partial(send_email, subject="Notification", priority="high")
send_alert = partial(send_email, subject="ALERT!", priority="urgent")

send_notification(to="user@example.com", body="You have a new message")
print()
send_alert(to="admin@example.com", body="System error detected")


# ============================================================
# Example 8: partial vs lambda
# ============================================================
print("\n=== partial vs lambda ===")

from functools import partial

# Using partial
power_of_2 = partial(pow, 2)
print(f"partial: {power_of_2(5)}")

# Using lambda
power_of_2_lambda = lambda x: pow(2, x)
print(f"lambda: {power_of_2_lambda(5)}")


# ============================================================
# Example 9: partial with class methods
# ============================================================
print("\n=== partial with classes ===")

from functools import partial

class Calculator:
    def __init__(self, base):
        self.base = base
    
    def add(self, value):
        return self.base + value

# Create instances
calc_10 = Calculator(10)
calc_100 = Calculator(100)

# Create bound operations
add_to_10 = calc_10.add
add_to_100 = calc_100.add

print(f"add_to_10(5): {add_to_10(5)}")
print(f"add_to_100(5): {add_to_100(5)}")


# ============================================================
# Example 10: Chaining partials
# ============================================================
print("\n=== Chaining partials ===")

from functools import partial

def process(a, b, c, d):
    return a + b + c + d

# Chain partials
step1 = partial(process, 1)  # a=1
step2 = partial(step1, 2)     # a=1, b=2
step3 = partial(step2, 3)     # a=1, b=2, c=3

print(f"step3(4): {step3(4)}")  # 1+2+3+4 = 10


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: functools.partial")
print("=" * 50)
print("""
partial:
  - Creates new function with pre-filled arguments
  - Useful for specializing generic functions
  - Can use positional or keyword args
  - Good for callbacks, event handlers
  - Alternative to lambda for argument binding
""")
