# Example70.py
# Topic: Comprehensive Review - itertools

# This file provides a comprehensive review of itertools module.


# ============================================================
# Example 1: Infinite iterators
# ============================================================
print("=== Infinite Iterators ===")

from itertools import count, cycle, repeat

# count() - infinite counter
print("count(1, 2):", list(islice(count(1, 2), 5)))

# cycle() - infinite repetition  
print("cycle('ABC'):", list(islice(cycle('ABC'), 7)))

# repeat() - repeat value
print("repeat('x', 3):", list(repeat('x', 3)))


# ============================================================
# Example 2: Iterators ending
# ============================================================
print("\n=== Iterators Ending ===")

from itertools import chain, islice

# chain() - chain iterables
print("chain([1,2], [3,4]):", list(chain([1,2], [3,4])))

# islice() - slice iterator
print("islice(range(10), 2, 8, 2):", list(islice(range(10), 2, 8, 2)))


# ============================================================
# Example 3: Combinatoric iterators
# ============================================================
print("\n=== Combinatoric Iterators ===")

from itertools import combinations, permutations, product

# combinations - order doesn't matter
print("combinations([1,2,3], 2):", list(combinations([1,2,3], 2)))

# permutations - order matters
print("permutations([1,2,3], 2):", list(permutations([1,2,3], 2)))

# product - cartesian product
print("product([1,2], ['a','b']):", list(product([1,2], ['a','b'])))


# ============================================================
# Example 4: Conditional iterators
# ============================================================
print("\n=== Conditional Iterators ===")

from itertools import filterfalse, dropwhile, takewhile

# filterfalse - opposite of filter
print("filterfalse(lambda x: x%2==0, range(5)):", 
      list(filterfalse(lambda x: x%2==0, range(5))))

# takewhile - take while true
print("takewhile(lambda x: x<5, range(10)):", 
      list(takewhile(lambda x: x<5, range(10))))

# dropwhile - drop while true
print("dropwhile(lambda x: x<5, [1,4,6,7,1]):", 
      list(dropwhile(lambda x: x<5, [1,4,6,7,1])))


# ============================================================
# Example 5: Grouping
# ============================================================
print("\n=== Grouping ===")

from itertools import groupby

data = [1, 1, 2, 2, 2, 3, 3]
groups = [(k, list(g)) for k, g in groupby(data)]
print(f"groupby({data}): {groups}")


# ============================================================
# Example 6: Pairwise
# ============================================================
print("\n=== Pairwise ===")

from itertools import pairwise

data = ['A', 'B', 'C', 'D', 'E']
pairs = list(pairwise(data))
print(f"pairwise({data}): {pairs}")


# ============================================================
# Example 7: Accumulate
# ============================================================
print("\n=== Accumulate ===")

from itertools import accumulate

numbers = [1, 2, 3, 4, 5]
print(f"accumulate({numbers}): {list(accumulate(numbers))}")
print(f"accumulate with mul: {list(accumulate(numbers, lambda a, b: a * b))}")


# ============================================================
# Example 8: compress
# ============================================================
print("\n=== Compress ===")

from itertools import compress

data = ['A', 'B', 'C', 'D', 'E']
selectors = [True, False, True, False, True]
print(f"compress({data}, {selectors}): {list(compress(data, selectors))}")


# ============================================================
# Example 9: All itertools functions overview
# ============================================================
print("\n=== All itertools Functions ===")

print("""
Infinite Iterators:
  count(start, step)     - Infinite counter
  cycle(iterable)        - Infinite repetition
  repeat(value, times)  - Repeat value

Iterators terminating on shortest input:
  chain(*iterables)              - Chain together
  islice(iterable, stop)         - Slice iterator
  pairwise(iterable)             - Consecutive pairs
  compress(data, selectors)      - Filter by selectors
  dropwhile(predicate, iterable) - Drop while true
  takewhile(predicate, iterable) - Take while true
  filterfalse(predicate, iterable) - Opposite of filter

Combinatoric iterators:
  product(*iterables, repeat)       - Cartesian product
  permutations(iterable, r)         - Order matters
  combinations(iterable, r)        - Order doesn't matter
  combinations_with_replacement     - With replacement

Accumulating:
  accumulate(iterable, func)       - Running totals

Grouping:
  groupby(iterable, key)           - Group consecutive
""")


# ============================================================
# Example 10: Common patterns
# ============================================================
print("\n=== Common Patterns ===")

from itertools import islice, count, cycle, product

# First N items
print("First 5:", list(islice(range(100), 5)))

# Numbered iteration
for i, item in enumerate(['a', 'b', 'c']):
    print(f"  {i}: {item}")

# Cartesian product pattern
for x, y in product([1, 2], [1, 2]):
    print(f"  ({x}, {y})")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE SUMMARY: itertools")
print("=" * 50)
print("""
itertools provides efficient iterator building blocks:

1. Infinite: count, cycle, repeat
2. Chaining: chain, islice, chain.from_iterable
3. Combinatorics: product, permutations, combinations
4. Filtering: filterfalse, takewhile, dropwhile, compress
5. Grouping: groupby
6. Pairing: pairwise, zip_longest
7. Accumulation: accumulate

Benefits:
- Memory efficient (lazy evaluation)
- Fast C-implemented
- Chainable
""")
