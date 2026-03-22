# Example117.py
# Topic: Counter and OrderedDict

# Using Counter and OrderedDict from collections.


# ============================================================
# Example 1: Counter Basics
# ============================================================
print("=== Counter Basics ===")

from collections import Counter

# From list
cnt = Counter([1, 2, 3, 2, 1, 3, 2])
print(f"Counter: {cnt}")

# From string
cnt = Counter("hello world")
print(f"String: {cnt}")

# Most common
print(f"Most common: {cnt.most_common(3)}")


# ============================================================
# Example 2: Counter Operations
# ============================================================
print("\n=== Operations ===")

a = Counter([1, 2, 3, 2, 1])
b = Counter([2, 3, 4])

print(f"a: {dict(a)}")
print(f"b: {dict(b)}")

# Addition
print(f"a + b: {dict(a + b)}")

# Subtraction
print(f"a - b: {dict(a - b)}")

# Intersection
print(f"a & b: {dict(a & b)}")

# Union
print(f"a | b: {dict(a | b)}")


# ============================================================
# Example 3: OrderedDict Basics
# ============================================================
print("\n=== OrderedDict ===")

from collections import OrderedDict

# Python 3.7+ regular dicts maintain order
# But OrderedDict still useful for compatibility

od = OrderedDict()
od['a'] = 1
od['b'] = 2
od['c'] = 3

print(f"Ordered: {od}")

# Move to end
od.move_to_end('a')
print(f"After move_to_end: {od}")

# Pop last
last = od.popitem(last=True)
print(f"Popped: {last}, Remaining: {od}")


# ============================================================
# Example 4: Counter Real World
# ============================================================
print("\n=== Counter Real World ===")

from collections import Counter

# Word frequency
text = "the quick brown fox jumps over the lazy dog the fox"
words = text.split()
word_counts = Counter(words)
print(f"Word counts: {word_counts}")
print(f"Most common: {word_counts.most_common(3)}")


# ============================================================
# Example 5: Defaultdict Deep Dive
# ============================================================
print("\n=== Defaultdict ===")

from collections import defaultdict

# int - for counting
counts = defaultdict(int)
for word in ["apple", "banana", "apple", "cherry"]:
    counts[word] += 1
print(f"Counts: {dict(counts)}")

# list - for grouping
by_first = defaultdict(list)
for word in ["apple", "banana", "apricot", "blueberry"]:
    by_first[word[0]].append(word)
print(f"By first: {dict(by_first)}")

# set - unique
by_first_set = defaultdict(set)
for word in ["apple", "apricot", "banana", "blueberry"]:
    by_first_set[word[0]].add(word)
print(f"By first (set): {dict(by_first_set)}")
