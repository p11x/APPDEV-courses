# Example244: Counter - Advanced Patterns
from collections import Counter

# Basic Counter
print("Basic Counter:")
c = Counter(['red', 'blue', 'red', 'green', 'blue', 'blue'])
print(f"Counts: {dict(c)}")
print(f"Most common: {c.most_common(2)}")

# Counter operations
print("\nCounter operations:")
a = Counter([1, 2, 3, 1, 2])
b = Counter([2, 3, 4])
print(f"A: {dict(a)}")
print(f"B: {dict(b)}")
print(f"A + B: {dict(a + b)}")
print(f"A - B: {dict(a - b)}")
print(f"A & B (intersection): {dict(a & b)}")
print(f"A | B (union): {dict(a | b)}")

# Update and subtract
print("\nUpdate and subtract:")
c = Counter(['a', 'b', 'c'])
c.update(['a', 'a', 'b'])
print(f"After update: {dict(c)}")
c.subtract(['a'])
print(f"After subtract: {dict(c)}")

# Finding elements
print("\nElement iteration:")
c = Counter(a=3, b=1, c=-1)
print(f"Elements: {list(c.elements())}")

# Most common with custom
print("\nMost common:")
c = Counter('abracadabra')
print(f"Top 3: {c.most_common(3)}")

# Practical: word frequency
print("\nWord frequency:")
text = "the quick brown fox jumps over the lazy dog the fox was quick"
words = text.split()
freq = Counter(words)
print(f"Frequencies: {dict(freq)}")
print(f"Most common: {freq.most_common(2)}")

# Practical: find missing characters
print("\nFind missing characters:")
from string import ascii_lowercase
given = "the quick brown fox jumps"
missing = Counter(ascii_lowercase) - Counter(given.lower())
print(f"Missing letters: {list(missing.elements())}")

# Grouping with Counter
print("\nGroup by parity:")
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
grouped = {'even': Counter(), 'odd': Counter()}
for n in numbers:
    if n % 2 == 0:
        grouped['even'][n] = 1
    else:
        grouped['odd'][n] = 1
print(f"Even: {dict(grouped['even'])}")
print(f"Odd: {dict(grouped['odd'])}")
