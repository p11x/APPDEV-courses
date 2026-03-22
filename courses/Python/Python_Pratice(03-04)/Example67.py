# Example67.py
# Topic: itertools - combinations, permutations, product

# This file demonstrates combinatoric iterators.


# ============================================================
# Example 1: itertools.combinations()
# ============================================================
print("=== itertools.combinations() ===")

from itertools import combinations

# All combinations of 2 from [1,2,3]
result = list(combinations([1, 2, 3], 2))
print(f"combinations([1,2,3], 2): {result}")

# All combinations of 3 from [1,2,3,4]
result = list(combinations([1, 2, 3, 4], 3))
print(f"combinations([1,2,3,4], 3): {result}")

# Combinations of all elements
result = list(combinations('abc', 2))
print(f"combinations('abc', 2): {result}")

# Order doesn't matter - no duplicates
result = list(combinations([1, 1, 2], 2))
print(f"combinations with duplicates: {result}")


# ============================================================
# Example 2: itertools.combinations_with_replacement()
# ============================================================
print("\n=== combinations_with_replacement() ===")

from itertools import combinations_with_replacement

# With replacement (allows same element)
result = list(combinations_with_replacement([1, 2, 3], 2))
print(f"combinations_with_replacement([1,2,3], 2): {result}")

# Coin change problem (order doesn't matter)
coins = [1, 5, 10, 25]
result = list(combinations_with_replacement(coins, 3))
print(f"Coin combos (3 coins): {result[:5]}...")


# ============================================================
# Example 3: itertools.permutations()
# ============================================================
print("=== itertools.permutations() ===")

from itertools import permutations

# All permutations of 2 from [1,2,3]
result = list(permutations([1, 2, 3], 2))
print(f"permutations([1,2,3], 2): {result}")

# All permutations (r = len(iterable))
result = list(permutations('abc'))
print(f"permutations('abc'): {result}")

# Permutations with duplicates
result = list(permutations([1, 1, 2]))
print(f"permutations([1,1,2]): {result}")


# ============================================================
# Example 4: combinations vs permutations
# ============================================================
print("\n=== combinations vs permutations ===")

items = ['A', 'B', 'C']

# Combinations - order doesn't matter
combos = list(combinations(items, 2))
print(f"Combinations (2): {combos}")

# Permutations - order matters
perms = list(permutations(items, 2))
print(f"Permutations (2): {perms}")

print(f"Combinations: {len(combos)}, Permutations: {len(perms)}")


# ============================================================
# Example 5: itertools.product()
# ============================================================
print("=== itertools.product() ===")

from itertools import product

# Cartesian product
result = list(product([1, 2], ['a', 'b']))
print(f"product([1,2], ['a','b']): {result}")

# Multiple iterables
result = list(product([1, 2], [3, 4], [5, 6]))
print(f"product with 3 lists: {result}")

# Repeat for power set
result = list(product([0, 1], repeat=3))
print(f"product([0,1], repeat=3): {result}")


# ============================================================
# Example 6: Real-world - Password generation
# ============================================================
print("\n=== Real-world: Password Generation ===")

from itertools import product

# Generate all 2-digit passwords
digits = '0123456789'
passwords = list(product(digits, repeat=2))
print(f"2-digit passwords: {passwords[:10]}...")

# Lowercase + numbers (first 10)
chars = 'abcdefghijklmnopqrstuvwxyz0123456789'
passwords = list(product(chars, repeat=3))
print(f"3-char passwords: {len(passwords)} total")


# ============================================================
# Example 7: Real-world - Team selection
# ============================================================
print("\n=== Real-world: Team Selection ===")

from itertools import combinations

players = ['Alice', 'Bob', 'Charlie', 'Diana', 'Eve']

# Select team of 3
teams = list(combinations(players, 3))
print(f"Teams of 3 from 5 players: {len(teams)}")
print(f"Sample teams: {teams[:3]}")

# All possible pairs for doubles match
pairs = list(combinations(players, 2))
print(f"Possible pairs: {len(pairs)}")


# ============================================================
# Example 8: Real-world - Route planning
# ============================================================
print("\n=== Real-world: Route Planning ===")

from itertools import permutations

cities = ['A', 'B', 'C']

# All possible routes
routes = list(permutations(cities))
print(f"All routes between 3 cities: {len(routes)}")
print(f"Routes: {routes}")

# Shortest route (example with distances)
distances = {
    ('A', 'B'): 10,
    ('B', 'A'): 10,
    ('A', 'C'): 15,
    ('C', 'A'): 15,
    ('B', 'C'): 20,
    ('C', 'B'): 20,
}

def route_distance(route):
    total = 0
    for i in range(len(route) - 1):
        total += distances.get((route[i], route[i+1]), 0)
    return total

routes_with_dist = [(r, route_distance(r)) for r in routes]
shortest = min(routes_with_dist, key=lambda x: x[1])
print(f"Shortest route: {shortest[0]} = {shortest[1]} miles")


# ============================================================
# Example 9: Real-world - Menu combinations
# ============================================================
print("\n=== Real-world: Menu Combinations ===")

from itertools import combinations

appetizers = ['Soup', 'Salad', 'Bread']
mains = ['Chicken', 'Fish', 'Steak']
desserts = ['Cake', 'Ice Cream', None]

# All 3-course meals (excluding None dessert)
meals = list(product(appetizers, mains, desserts))
valid_meals = [m for m in meals if m[2] is not None]
print(f"3-course meals: {len(valid_meals)}")

# Budget combinations
prices = {
    'Soup': 5, 'Salad': 4, 'Bread': 3,
    'Chicken': 20, 'Fish': 18, 'Steak': 25,
    'Cake': 8, 'Ice Cream': 6
}

budget = 30
within_budget = []
for app, main, dess in valid_meals:
    total = prices[app] + prices[main] + prices[dess]
    if total <= budget:
        within_budget.append((app, main, dess, total))

print(f"Meals under ${budget}: {len(within_budget)}")
for meal in within_budget[:3]:
    print(f"  {meal[0]} + {meal[1]} + {meal[2]} = ${meal[3]}")


# ============================================================
# Example 10: Filtered combinations
# ============================================================
print("\n=== Filtered Combinations ===")

from itertools import combinations

# Get only combinations that sum to target
numbers = [1, 2, 3, 4, 5, 6]
target = 10

valid_combos = [c for c in combinations(numbers, 3) if sum(c) == target]
print(f"Combinations of 3 that sum to 10: {valid_combos}")

# Get combinations with minimum sum
min_sum = 8
valid_combos = [c for c in combinations(numbers, 3) if sum(c) >= min_sum]
print(f"Combinations with sum >= {min_sum}: {valid_combos}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: Combinatoric Iterators")
print("=" * 50)
print("""
combinations(iterable, r):
  - Order doesn't matter
  - No repetition

combinations_with_replacement(iterable, r):
  - Order doesn't matter
  - Allows repetition

permutations(iterable, r=None):
  - Order matters
  - No repetition

product(*iterables, repeat=1):
  - Cartesian product
  - Equivalent to nested loops
""")
