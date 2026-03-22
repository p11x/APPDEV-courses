# Example163.py
# Topic: More Complex Control Flow


# ============================================================
# Example 1: Switch Dictionary Pattern
# ============================================================
print("=== Switch Dictionary ===")

def calculate(operation: str, a: float, b: float) -> float:
    operations = {
        "add": lambda: a + b,
        "subtract": lambda: a - b,
        "multiply": lambda: a * b,
        "divide": lambda: a / b if b != 0 else None,
    }
    
    if operation not in operations:
        raise ValueError(f"Unknown operation: {operation}")
    
    return operations[operation]()

print(f"10 + 5 = {calculate('add', 10, 5)}")
print(f"10 - 5 = {calculate('subtract', 10, 5)}")
print(f"10 * 5 = {calculate('multiply', 10, 5)}")
print(f"10 / 5 = {calculate('divide', 10, 5)}")


# ============================================================
# Example 2: Elif vs Dictionary
# ============================================================
print("\n=== Elif vs Dictionary ===")

def get_day_type(day: str) -> str:
    if day == "saturday" or day == "sunday":
        return "weekend"
    elif day == "monday":
        return "start of week"
    elif day == "friday":
        return "end of week"
    else:
        return "weekday"

print(get_day_type("monday"))
print(get_day_type("saturday"))

def get_day_type_dict(day: str) -> str:
    return {
        "saturday": "weekend",
        "sunday": "weekend",
        "monday": "start of week",
        "friday": "end of week",
    }.get(day, "weekday")

print(get_day_type_dict("monday"))
print(get_day_type_dict("saturday"))


# ============================================================
# Example 3: Conditional Expression Chain
# ============================================================
print("=== Conditional Chain ===")

def get_grade(score: int) -> str:
    return (
        "A" if score >= 90 else
        "B" if score >= 80 else
        "C" if score >= 70 else
        "D" if score >= 60 else
        "F"
    )

for score in [95, 85, 75, 65, 55]:
    print(f"Score {score}: {get_grade(score)}")


# ============================================================
# Example 4: Short-Circuit Evaluation
# ============================================================
print("=== Short-Circuit ===")

def expensive_check():
    print("Expensive check called!")
    return True

result = False or expensive_check()
print(f"Result: {result}")

result = True and expensive_check()
print(f"Result: {result}")


# ============================================================
# Example 5: Default Dict for Counting
# ============================================================
print("=== Counting Pattern ===")

from collections import defaultdict

def count_items(items: list) -> dict:
    counts = defaultdict(int)
    for item in items:
        counts[item] += 1
    return dict(counts)

fruits = ["apple", "banana", "apple", "cherry", "banana", "apple"]
print(count_items(fruits))


# ============================================================
# Example 6: Partition Pattern
# ============================================================
print("=== Partition Pattern ===")

numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

evens = []
odds = []
for n in numbers:
    if n % 2 == 0:
        evens.append(n)
    else:
        odds.append(n)

print(f"Evens: {evens}")
print(f"Odds: {odds}")

evens, odds = zip(*[(n, n) for n in numbers if n % 2 == 0] or ([], [])
print(f"Using zip: evens={list(evens)}, odds={list(odds)}")


# ============================================================
# Example 7: Window Iterator
# ============================================================
print("=== Window Iterator ===")

def window(iterable, size):
    it = iter(iterable)
    window = []
    for _ in range(size):
        window.append(next(it, None))
    yield tuple(window)
    for item in it:
        window = window[1:] + [item]
        yield tuple(window)

data = [1, 2, 3, 4, 5]
print(f"Data: {data}")
print(f"Window size 3:")
for w in window(data, 3):
    print(f"  {w}")


# ============================================================
# Example 8: Reduce Pattern
# ============================================================
print("=== Reduce Pattern ===")

from functools import reduce

def reduce_pattern(data, operation, initial=None):
    if initial is None:
        return reduce(operation, data)
    return reduce(operation, data, initial)

numbers = [1, 2, 3, 4, 5]

sum_all = reduce_pattern(numbers, lambda a, b: a + b)
print(f"Sum: {sum_all}")

max_val = reduce_pattern(numbers, lambda a, b: a if a > b else b)
print(f"Max: {max_val}")

product = reduce_pattern(numbers, lambda a, b: a * b, 1)
print(f"Product: {product}")
