# Example66.py
# Topic: itertools - chain, islice, count, cycle, repeat

# This file demonstrates basic itertools functions.


# ============================================================
# Example 1: itertools.chain()
# ============================================================
print("=== itertools.chain() ===")

from itertools import chain

# Chain multiple iterables
result = list(chain([1, 2], [3, 4], [5, 6]))
print(f"Chain lists: {result}")

# Chain with different types
result = list(chain([1, 2], (3, 4), [5, 6]))
print(f"Chain mixed: {result}")

# Chain with strings
result = list(chain('abc', 'def'))
print(f"Chain strings: {result}")

# Using chain.from_iterable
nested = [[1, 2], [3, 4], [5, 6]]
result = list(chain.from_iterable(nested))
print(f"Flatten nested: {result}")


# ============================================================
# Example 2: itertools.islice()
# ============================================================
print("\n=== itertools.islice() ===")

from itertools import islice

# Slice iterator
data = range(10)

# First 5 elements
result = list(islice(data, 5))
print(f"First 5: {result}")

# Elements from index 2 to 7
result = list(islice(data, 2, 8))
print(f"Index 2-7: {result}")

# Every 2nd element
result = list(islice(data, 0, 10, 2))
print(f"Every 2nd: {result}")

# Without end (lazy)
result = islice(data, 3)
print(f"First 3 (lazy): {list(result)}")


# ============================================================
# Example 3: itertools.count()
# ============================================================
print("\n=== itertools.count() ===")

from itertools import count

# Infinite counter
counter = count()
print(f"First 5 counts: {[next(counter) for _ in range(5)]}")

# Counter with start and step
counter = count(10, 2)
print(f"Count 10, step 2: {[next(counter) for _ in range(5)]}")

# Counter with negative step
counter = count(100, -10)
print(f"Count 100, -10: {[next(counter) for _ in range(5)]}")

# Using with zip
labels = list(zip(count(1), ['a', 'b', 'c', 'd']))
print(f"Zip with labels: {labels}")


# ============================================================
# Example 4: itertools.cycle()
# ============================================================
print("\n=== itertools.cycle() ===")

from itertools import cycle

# Infinite cycling
cycler = cycle(['A', 'B', 'C'])
print(f"Cycle 6 times: {[next(cycler) for _ in range(6)]}")

# Cycle with numbers
cycler = cycle(range(3))
print(f"Cycle range 3: {[next(cycler) for _ in range(8)]}")

# Practical use: alternating flags
flags = cycle([True, False])
data = [1, 2, 3, 4, 5, 6]
paired = list(zip(data, flags))
print(f"Alternating pairs: {paired}")


# ============================================================
# Example 5: itertools.repeat()
# ============================================================
print("\n=== itertools.repeat() ===")

from itertools import repeat

# Repeat a value
repeater = repeat('x', 5)
print(f"Repeat 'x' 5 times: {list(repeater)}")

# Infinite repeat
repeater = repeat('y')
# Be careful - infinite!
print(f"First 3 of infinite: {list(islice(repeat('y'), 3))}")

# Using with map and zip
result = list(map(pow, [2, 2, 2], repeat(3)))
print(f"2^3, 2^3, 2^3: {result}")


# ============================================================
# Example 6: Combining infinite iterators
# ============================================================
print("\n=== Combining Infinite Iterators ===")

from itertools import count, cycle, islice

# Create pattern: 1, 2, 3, 1, 2, 3, ...
pattern = islice(cycle([1, 2, 3]), 10)
print(f"Pattern: {list(pattern)}")

# Numbered list with cycling colors
colors = cycle(['red', 'green', 'blue'])
numbered = list(zip(count(1), colors))
print(f"Numbered colors: {numbered}")


# ============================================================
# Example 7: Practical use cases
# ============================================================
print("\n=== Practical Use Cases ===")

from itertools import chain, islice, count

# Pagination helper
def paginate(items, page_size):
    it = iter(items)
    while True:
        page = list(islice(it, page_size))
        if not page:
            break
        yield page

data = range(25)
pages = list(paginate(data, 7))
print(f"Paginated (7 per page): {pages}")

# Chunking data
def chunk(data, size):
    it = iter(data)
    while True:
        chunk = list(islice(it, size))
        if not chunk:
            break
        yield chunk

result = list(chunk(range(15), 4))
print(f"Chunks of 4: {result}")


# ============================================================
# Example 8: Real-world examples
# ============================================================
print("\n=== Real-world Examples ===")

from itertools import chain, count, cycle

# Round-robin scheduling
def schedule_teams(teams):
    return list(zip(count(1), cycle(teams)))

teams = ['Team A', 'Team B', 'Team C']
schedule = schedule_teams(teams)[:10]
print(f"Round-robin: {schedule}")

# Merging sorted streams
stream1 = [1, 3, 5, 7]
stream2 = [2, 4, 6, 8]
merged = sorted(chain(stream1, stream2))
print(f"Merged sorted: {merged}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: itertools Basic Functions")
print("=" * 50)
print("""
chain() - Chain iterables together
  - chain(*iterables)
  - chain.from_iterable(iterable)

islice() - Slice iterator
  - islice(iterable, start, stop, step)

count() - Infinite counter
  - count(start=0, step=1)

cycle() - Infinite repetition
  - cycle(iterable)

repeat() - Repeat value
  - repeat(value, times=None)  # None = infinite
""")
