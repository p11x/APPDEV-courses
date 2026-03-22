# Example196.py
# Topic: defaultdict Basics & Grouping

# This file demonstrates defaultdict for automatic value initialization
# and common grouping patterns.


# ============================================================
# Example 1: Basic defaultdict
# ============================================================
print("=== Basic defaultdict ===")

from collections import defaultdict

d = defaultdict(int)
d["a"] += 1
d["b"] += 2
d["a"] += 3
print(f"Dict: {dict(d)}")    # {'a': 4, 'b': 2}


# ============================================================
# Example 2: With list Factory
# ============================================================
print("\n=== With list ===")

d = defaultdict(list)
d["fruits"].append("apple")
d["fruits"].append("banana")
d["vegetables"].append("carrot")
print(f"Fruits: {d['fruits']}")    # ['apple', 'banana']


# ============================================================
# Example 3: With set Factory
# ============================================================
print("\n=== With set ===")

d = defaultdict(set)
d["odd"].add(1)
d["odd"].add(3)
d["even"].add(2)
print(f"Odd: {d['odd']}")    # {1, 3}


# ============================================================
# Example 4: Grouping Words by Length
# ============================================================
print("\n=== Group by Length ===")

words = ["apple", "banana", "cherry", "date", "elderberry"]
grouped = defaultdict(list)
for word in words:
    grouped[len(word)].append(word)
print(f"By length: {dict(grouped)}")    # {5: ['apple'], 6: ['banana', 'cherry'], ...}


# ============================================================
# Example 5: Grouping by First Letter
# ============================================================
print("\n=== Group by First Letter ===")

words = ["apple", "banana", "cherry", "apricot", "blueberry"]
grouped = defaultdict(list)
for word in words:
    grouped[word[0]].append(word)
print(f"By first letter: {dict(grouped)}")


# ============================================================
# Example 6: Counting with defaultdict
# ============================================================
print("\n=== Counting ===")

data = ["a", "b", "a", "c", "b", "a"]
counts = defaultdict(int)
for item in data:
    counts[item] += 1
print(f"Counts: {dict(counts)}")    # {'a': 3, 'b': 2, 'c': 1}


# ============================================================
# Example 7: Nested defaultdict
# ============================================================
print("\n=== Nested defaultdict ===")

d = defaultdict(lambda: defaultdict(list))
d["fruits"]["red"].append("apple")
d["fruits"]["yellow"].append("banana")
print(f"Nested: {dict(d)}")


# ============================================================
# Example 8: With lambda Factory
# ============================================================
print("\n=== Custom Factory ===")

d = defaultdict(lambda: "default")
print(f"Missing: {d['missing']}")    # 'default'


# ============================================================
# Example 9: Set Default
# ============================================================
print("\n=== Set Default Pattern ===")

d = {}
d.setdefault("a", []).append(1)
d.setdefault("a", []).append(2)
print(f"Dict: {d}")    # {'a': [1, 2]}


# ============================================================
# Example 10: Inverted Index
# ============================================================
print("\n=== Inverted Index ===")

documents = {
    1: "python is great",
    2: "python is easy",
    3: "java is different"
}

index = defaultdict(list)
for doc_id, text in documents.items():
    for word in text.split():
        index[word].append(doc_id)

print(f"Index: {dict(index)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
DEFAULTDICT:
- defaultdict(int): Default 0
- defaultdict(list): Default []
- defaultdict(set): Default set()
- defaultdict(lambda: value): Custom

USE CASES:
- Counting frequencies
- Grouping data
- Building indices
- Avoiding KeyError
""")
