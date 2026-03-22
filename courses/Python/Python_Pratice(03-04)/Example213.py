# Example213.py
# Topic: defaultdict Advanced Patterns

# This file demonstrates advanced defaultdict patterns for grouping and indexing.


# ============================================================
# Example 1: Group by First Letter
# ============================================================
print("=== Group by First Letter ===")

from collections import defaultdict

words = ["apple", "banana", "apricot", "blueberry"]
grouped = defaultdict(list)
for word in words:
    grouped[word[0]].append(word)
print(f"Grouped: {dict(grouped)}")


# ============================================================
# Example 2: Group by Length
# ============================================================
print("\n=== Group by Length ===")

words = ["cat", "elephant", "dog", "mouse"]
grouped = defaultdict(list)
for word in words:
    grouped[len(word)].append(word)
print(f"By length: {dict(grouped)}")


# ============================================================
# Example 3: Counting with defaultdict
# ============================================================
print("\n=== Counting ===")

data = ["a", "b", "a", "c", "b", "a"]
counts = defaultdict(int)
for item in data:
    counts[item] += 1
print(f"Counts: {dict(counts)}")


# ============================================================
# Example 4: Set Grouping
# ============================================================
print("\n=== Set Grouping ===")

data = [(1, "a"), (2, "b"), (1, "c"), (2, "d")]
grouped = defaultdict(set)
for num, char in data:
    grouped[num].add(char)
print(f"Set groups: {dict(grouped)}")


# ============================================================
# Example 5: Nested defaultdict
# ============================================================
print("\n=== Nested ===")

d = defaultdict(lambda: defaultdict(list))
d["fruits"]["red"].append("apple")
d["vegetables"]["green"].append("broccoli")
print(f"Nested: {dict(d)}")


# ============================================================
# Example 6: Inverted Index
# ============================================================
print("\n=== Inverted Index ===")

docs = {1: "apple fruit", 2: "red apple", 3: "green fruit"}
index = defaultdict(list)
for doc_id, text in docs.items():
    for word in text.split():
        index[word].append(doc_id)
print(f"Index: {dict(index)}")


# ============================================================
# Example 7: Default Factory
# ============================================================
print("\n=== Default Factory ===")

d = defaultdict(lambda: "unknown")
print(f"Missing: {d['missing']}")


# ============================================================
# Example 8: Counter with defaultdict
# ============================================================
print("\n=== Counter Pattern ===")

from collections import defaultdict
d = defaultdict(int)
data = "abracadabra"
for char in data:
    d[char] += 1
print(f"Counter: {dict(d)}")


# ============================================================
# Example 9: Accumulator Pattern
# ============================================================
print("\n=== Accumulator ===")

d = defaultdict(list)
for i in range(5):
    d[i % 3].append(i)
print(f"Mod 3: {dict(d)}")


# ============================================================
# Example 10: Tree Structure
# ============================================================
print("\n=== Tree ===")

tree = defaultdict(lambda: defaultdict(dict))
tree["root"]["child"]["value"] = 42
print(f"Tree: {dict(tree)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DEFAULTDICT:
- defaultdict(list): Grouping
- defaultdict(int): Counting
- defaultdict(set): Unique grouping
- Nested: defaultdict(lambda: defaultdict(...))
""")
