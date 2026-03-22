# Example233: itertools - accumulate
import itertools
import operator

# accumulate(iterable, func=operator.add) - running accumulation
print("accumulate - running sum:")
numbers = [1, 2, 3, 4, 5]
result = list(itertools.accumulate(numbers))
print(f"Input: {numbers}")
print(f"Running sum: {result}")

# Different operation: multiplication
print("\naccumulate with multiplication:")
result = list(itertools.accumulate(numbers, operator.mul))
print(f"Running product: {result}")

# Max accumulation
print("\naccumulate with max:")
result = list(itertools.accumulate([3, 1, 4, 1, 5], max))
print(f"Running max: {result}")

# Using subtract
print("\naccumulate with subtraction:")
result = list(itertools.accumulate([10, 5, 3, 2], lambda a, b: a - b))
print(f"Running subtraction: {result}")

# Practical: running total / balance
print("\nPractical - bank balance:")
transactions = [1000, -200, -150, 500, -100]
balance = list(itertools.accumulate(transactions))
print(f"Transactions: {transactions}")
print(f"Balance after each: {balance}")

# Find when balance goes below threshold
print("\nFind first negative balance:")
transactions = [100, -50, -60, 200, -100]
balance = list(itertools.accumulate(transactions))
print(f"Transactions: {transactions}")
print(f"Balance: {balance}")
for i, bal in enumerate(balance):
    if bal < 0:
        print(f"First negative at transaction {i+1}: {bal}")
        break

# Calculate percentage change
print("\nPercentage changes:")
prices = [100, 110, 105, 115, 120]
changes = list(itertools.accumulate(prices, lambda a, b: (b - a) / a * 100))
print(f"Prices: {prices}")
print(f"% change from original: {[round(p - 100, 2) for p in changes]}")
